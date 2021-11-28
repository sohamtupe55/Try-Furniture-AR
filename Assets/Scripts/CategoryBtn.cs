using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class CategoryBtn : MonoBehaviour
{
    public GameObject contentBox;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(onClickCategoryBtn);
    }

    private void onClickCategoryBtn()
    {
        transform.parent.gameObject.SetActive(false);
        contentBox.transform.DOLocalMoveY(-1150, 0.5f);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
