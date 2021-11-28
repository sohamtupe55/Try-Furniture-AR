using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    private GraphicRaycaster m_Raycaster;
    PointerEventData m_PointerEventData;
    EventSystem m_EventSystem;

   // public Transform selectionPoint;
    public Transform contentPanel;
    public Transform catelogButton;
    public bool objectPlaced = true;

    public Button deleteButton;

    public ARUXAnimationManager animationManager;
    
    private static UIController instance;
    public static UIController Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<UIController>();
            }
            return instance;
        }
        
    }
    
    // Start is called before the first frame update
    void Start()
    {
        //Fetch the Raycaster from the GameObject (the Canvas)
        m_Raycaster = GetComponent<GraphicRaycaster>();

        //Fetch the Event System from the Scene
        m_EventSystem = GetComponent<EventSystem>();

        //Set up the new Pointer Event
        m_PointerEventData = new PointerEventData(m_EventSystem);

        //Set the Pointer Event Position to that of the mouse position
        // m_PointerEventData.position = selectionPoint.position;

    }

    public bool OnEntered(GameObject button)
    {
        //Create a list of Raycast Results
        List<RaycastResult> results = new List<RaycastResult>();
     
        //Raycast using the Graphics Raycaster and mouse click position
        m_Raycaster.Raycast(m_PointerEventData, results);

        //For every result returned, output the name of the GameObject on the Canvas hit by the Ray
        foreach (RaycastResult result in results)
        {
            if (result.gameObject == button)
            {
                return true;
            }
        }
        return false;
    }

    public void ScrollToButton(Button b)
    {
        
    }

    public void SlideDown()
    {
        catelogButton.gameObject.SetActive(true);
        contentPanel.DOLocalMoveY(-2400, 0.5f);
    }

    public void FadeOff()
    {
        animationManager.FadeOffCurrentUI();
    }

    public void ShowTapToPlace()
    {
        animationManager.ShowTapToPlace();
    }
}
