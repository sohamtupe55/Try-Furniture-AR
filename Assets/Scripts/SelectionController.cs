using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.AR;

public class SelectionController : ARBaseGestureInteractable
{
    [SerializeField, Tooltip("The visualization GameObject that will become active when the object is selected.")]
        GameObject m_SelectionVisualization;
        /// <summary>
        /// The visualization <see cref="GameObject"/> that will become active when the object is selected.
        /// </summary>
        public GameObject selectionVisualization
        {
            get => m_SelectionVisualization;
            set => m_SelectionVisualization = value;
        }
        
        [SerializeField, Tooltip("The visualization GameObject that will become active when the object is selected.")]
        Button m_DeleteButtonVisualize;
        /// <summary>
        /// The visualization <see cref="GameObject"/> that will become active when the object is selected.
        /// </summary>
        public Button DeleteButtonVisualize
        {
            get => m_DeleteButtonVisualize;
            set => m_DeleteButtonVisualize = value;
        }

        bool m_GestureSelected;

        private void Start()
        {
            DeleteButtonVisualize.onClick.AddListener(OnClickDelete);
        }

        private void OnClickDelete()
        {
            Destroy(gameObject);
        }

        /// <inheritdoc />
        public override bool IsSelectableBy(XRBaseInteractor interactor)
        {
            if (!(interactor is ARGestureInteractor))
                return false;

            return m_GestureSelected;
        }

        /// <inheritdoc />
        protected override bool CanStartManipulationForGesture(TapGesture gesture) => true;

        /// <inheritdoc />
        protected override void OnEndManipulation(TapGesture gesture)
        {
            base.OnEndManipulation(gesture);

            if (gesture.isCanceled)
                return;
            if (gestureInteractor == null)
                return;

            if (gesture.targetObject == gameObject)
            {
                // Toggle selection
                m_GestureSelected = !m_GestureSelected;
            }
            else
                m_GestureSelected = false;
        }

        /// <inheritdoc />
        protected override void OnSelectEntering(XRBaseInteractor interactor)
        {
            base.OnSelectEntering(interactor);
            if (m_SelectionVisualization != null)
            {
                m_SelectionVisualization.SetActive(true);
                m_DeleteButtonVisualize.gameObject.SetActive(true);
            }
            
                
        }

        /// <inheritdoc />
        protected override void OnSelectExiting(XRBaseInteractor interactor)
        {
            base.OnSelectExiting(interactor);

            if (m_SelectionVisualization != null)
            {
                m_SelectionVisualization.SetActive(false);
                m_DeleteButtonVisualize.gameObject.SetActive(false);
            }
                
        }

        /// <inheritdoc />
        protected override void OnSelectCanceling(XRBaseInteractor interactor)
        {
            base.OnSelectCanceling(interactor);
            if (m_SelectionVisualization != null)
            {
                m_SelectionVisualization.SetActive(false);
                m_DeleteButtonVisualize.gameObject.SetActive(false);
            }
               
        }
}
