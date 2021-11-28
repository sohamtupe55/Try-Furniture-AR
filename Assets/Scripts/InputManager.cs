using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.XR.Interaction.Toolkit.AR;

public class InputManager : ARBaseGestureInteractable
{
    [SerializeField] private Camera arCam;
    [SerializeField] private ARRaycastManager _raycastManager;
    [SerializeField] private GameObject crosshair;
    
    private List<ARRaycastHit> _hits = new List<ARRaycastHit>();

    private Touch touch;
    private Pose pose;
   

    // Start is called before the first frame update
    void Start()
    {
        UIController.Instance.objectPlaced = true;
    }

    protected override bool CanStartManipulationForGesture(TapGesture gesture)
    {
        if (gesture.targetObject == null)
            return true;
        return false;
    }

    protected override void OnEndManipulation(TapGesture gesture)
    {
        if (gesture.isCanceled)
            return;
        if (gesture.targetObject != null || IsPointerOverUI(gesture))
        {
            return;
        }

        if (GestureTransformationUtility.Raycast(gesture.startPosition, _hits, TrackableType.PlaneWithinPolygon) && !UIController.Instance.objectPlaced)
        {
            
            GameObject placedObj = Instantiate(DataHandler.Instance.GetFurniture(), pose.position, pose.rotation);
            
            var anchorObject = new GameObject("PlacementAnchor");
            anchorObject.transform.position = pose.position;
            //anchorObject.transform.rotation = pose.rotation;
            placedObj.transform.parent = anchorObject.transform;
            UIController.Instance.objectPlaced = true;
            UIController.Instance.FadeOff();
        }
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        CrosshairCalculation();
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("Home Screen");
        }
    }

    bool IsPointerOverUI(TapGesture touch)
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = new Vector2(touch.startPosition.x, touch.startPosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);
        return results.Count > 0;
    }

    void CrosshairCalculation()
    {
        crosshair.gameObject.SetActive(!UIController.Instance.objectPlaced);
        if(UIController.Instance.objectPlaced)
            return;
        Vector3 origin = arCam.ViewportToScreenPoint(new Vector3(0.5f, 0.5f, 0));
        if (GestureTransformationUtility.Raycast(origin, _hits, TrackableType.PlaneWithinPolygon))
        {
            pose = _hits[0].pose;
            crosshair.transform.position = pose.position;
            crosshair.transform.eulerAngles = new Vector3(90,0,0);
        }
    }
    
}