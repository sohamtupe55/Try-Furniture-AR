using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.AR;

public class TranslationManager : ARBaseGestureInteractable
{
    [SerializeField]
    [Tooltip("Controls whether the object will be constrained vertically, horizontally, or free to move in all axis.")]
    GestureTransformationUtility.GestureTranslationMode m_ObjectGestureTranslationMode;

    /// <summary>
    /// Controls whether the object will be constrained vertically, horizontally, or free to move in all axis.
    /// </summary>
    public GestureTransformationUtility.GestureTranslationMode objectGestureTranslationMode
    {
        get => m_ObjectGestureTranslationMode;
        set => m_ObjectGestureTranslationMode = value;
    }

    [SerializeField] [Tooltip("The maximum translation distance of this object.")]
    float m_MaxTranslationDistance = 10f;

    /// <summary>
    /// The maximum translation distance of this object.
    /// </summary>
    public float maxTranslationDistance
    {
        get => m_MaxTranslationDistance;
        set => m_MaxTranslationDistance = value;
    }

    const float k_PositionSpeed = 12f;
    const float k_DiffThreshold = 0.0001f;

    bool m_IsActive;

    Vector3 m_DesiredLocalPosition;
    float m_GroundingPlaneHeight;
    Vector3 m_DesiredAnchorPosition;
    Quaternion m_DesiredRotation;
    GestureTransformationUtility.Placement m_LastPlacement;

    protected void Update()
    {
        UpdatePosition();
    }

    /// <inheritdoc />
    protected override bool CanStartManipulationForGesture(DragGesture gesture)
    {
        if (gesture.targetObject == null)
        {
            return false;
        }

        // If the gesture isn't targeting this item, don't start manipulating.
        if (gesture.targetObject != gameObject)
        {
            return false;
        }

        return true;
    }

    /// <inheritdoc />
    protected override void OnStartManipulation(DragGesture gesture)
    {
        m_GroundingPlaneHeight = transform.parent.position.y;
    }

    /// <inheritdoc />
    protected override void OnContinueManipulation(DragGesture gesture)
    {
        Debug.Assert(transform.parent != null, "Translate interactable needs a parent object.");
        m_IsActive = true;

        var desiredPlacement =
            GestureTransformationUtility.GetBestPlacementPosition(
                transform.parent.position, gesture.position, m_GroundingPlaneHeight, 0.03f,
                maxTranslationDistance, objectGestureTranslationMode);

        if (desiredPlacement.hasHoveringPosition && desiredPlacement.hasPlacementPosition)
        {
            // If desired position is lower than current position, don't drop it until it's finished.
            m_DesiredLocalPosition = transform.parent.InverseTransformPoint(desiredPlacement.hoveringPosition);
            m_DesiredAnchorPosition = desiredPlacement.placementPosition;

            m_GroundingPlaneHeight = desiredPlacement.updatedGroundingPlaneHeight;

            // Rotate if the plane direction has changed.
            if (((desiredPlacement.placementRotation * Vector3.up) - transform.up).magnitude > k_DiffThreshold)
                m_DesiredRotation = desiredPlacement.placementRotation;
            else
                m_DesiredRotation = transform.rotation;

            if (desiredPlacement.hasPlane)
                m_LastPlacement = desiredPlacement;
        }
    }

    /// <inheritdoc />
    protected override void OnEndManipulation(DragGesture gesture)
    {
        if (!m_LastPlacement.hasPlacementPosition)
            return;

        var oldAnchor = transform.parent.gameObject;
        var desiredPose = new Pose(m_DesiredAnchorPosition, m_LastPlacement.placementRotation);

        var desiredLocalPosition = transform.parent.InverseTransformPoint(desiredPose.position);

        if (desiredLocalPosition.magnitude > maxTranslationDistance)
            desiredLocalPosition = desiredLocalPosition.normalized * maxTranslationDistance;
        desiredPose.position = transform.parent.TransformPoint(desiredLocalPosition);

        var anchorGO = new GameObject("PlacementAnchor");
        anchorGO.transform.position = m_LastPlacement.placementPosition;
        anchorGO.transform.rotation = m_LastPlacement.placementRotation;
        transform.parent = anchorGO.transform;

        Destroy(oldAnchor);

        m_DesiredLocalPosition = Vector3.zero;

        // Rotate if the plane direction has changed.
        if (((desiredPose.rotation * Vector3.up) - transform.up).magnitude > k_DiffThreshold)
            m_DesiredRotation = desiredPose.rotation;
        else
            m_DesiredRotation = transform.rotation;

        // Make sure position is updated one last time.
        m_IsActive = true;
    }

    void UpdatePosition()
    {
        if (!m_IsActive)
            return;

        // Lerp position.
        var oldLocalPosition = transform.localPosition;
        var newLocalPosition = Vector3.Lerp(
            oldLocalPosition, m_DesiredLocalPosition, Time.deltaTime * k_PositionSpeed);

        var diffLength = (m_DesiredLocalPosition - newLocalPosition).magnitude;
        if (diffLength < k_DiffThreshold)
        {
            newLocalPosition = m_DesiredLocalPosition;
            m_IsActive = false;
        }

        transform.localPosition = newLocalPosition;

        // Lerp rotation.
        var oldRotation = transform.rotation;
        var newRotation =
            Quaternion.Lerp(oldRotation, m_DesiredRotation, Time.deltaTime * k_PositionSpeed);
        transform.rotation = newRotation;
    }
}