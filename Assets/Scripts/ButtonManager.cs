using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.UI;

public class ButtonManager : MonoBehaviour
{

    [SerializeField] private RawImage buttonImage;
    [SerializeField]private TMP_Text descriptionText;
    [SerializeField]private TMP_Text nameText;
    [SerializeField]private TMP_Text priceText;

    private String name;
    private Button btn;
    private int _itemId;
    private Texture2D _buttonTexture;
    private String description;
    private String price;

    [SerializeField]private Button arButton;
    public int ItemId
    {
        set => _itemId = value;
    }
    public Texture2D ButtonTexture 
    {
        set
        {
            _buttonTexture = value;
            buttonImage.texture = _buttonTexture;
        }
    }

    public String Description
    {
        get => description;
        set
        {
            description = value;
            descriptionText.text = description;
        }
    }

    public string Name
    {
        get => name;
        set
        {
            name = value;
            nameText.text = value;
        }
    }

    public string Price
    {
        get => price;
        set
        {
            price = value;
            priceText.text = "PRICE - Rs."+ value ;
        } 
    }

    void Start()
    {
        //btn = GetComponent<Button>();
        arButton.onClick.AddListener(SelectObject);
    }

    // Update is called once per frame
    void Update()
    {
        // if (UIManager.Instance.OnEntered(gameObject))
        // {
        //     transform.DOScale(Vector3.one * 2, 0.2f);
        // }
        // else
        // {
        //     transform.DOScale(Vector3.one, 0.2f);
        // }
    }

    void SelectObject()
    {
        UIController.Instance.objectPlaced = false;
        UIController.Instance.contentPanel.DOLocalMoveY(-2400, 0.5f);
        UIController.Instance.catelogButton.gameObject.SetActive(true);
        DataHandler.Instance.SetFurinute(_itemId);
        UIController.Instance.ShowTapToPlace();
    }
}
