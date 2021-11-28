using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class DataHandler : MonoBehaviour
{
    [SerializeField]private GameObject furniture;
    
    [SerializeField] private ButtonManager buttonPrefab;
    [SerializeField] private GameObject buttonContainer;
    [SerializeField] private List<Item> _items;
    [SerializeField] private String label;
    
    private SnapshotCamera snapshot;
    [HideInInspector]
    public Vector3 rotation = new Vector3(345.8529f, 313.8297f, 14.28433f);
    [HideInInspector]
    public Vector3 scale = new Vector3(1, 1, 1);
    
    
    public Material visualizerMat;
    
    private int id = 0;
    
    private static DataHandler instance;
    public static DataHandler Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<DataHandler>();
            }
            return instance;
        }
        
    }
    
    private void Start()
    {
        snapshot = SnapshotCamera.MakeSnapshotCamera("SnapshotLayer");
        _items = new List<Item>();
        LoadItems();
        //await Get(label);
        CreateButtons();
    }

    void LoadItems()
    {
        var items_obj =Resources.LoadAll("Items",typeof(Item));
        foreach (var item in items_obj)
        {
            _items.Add(item as Item);
        }
        
    }
    void CreateButtons()
    {
        foreach (Item i in _items)
        {
            ButtonManager b = Instantiate(buttonPrefab, buttonContainer.transform);
            b.ItemId = id;
            b.ButtonTexture = snapshot.TakePrefabSnapshot(i.itemPrefab, Color.clear, new Vector3(0,-0.5f,100), Quaternion.Euler(rotation), scale, width: 512, height: 512);
            b.Description = i.description;
            b.Name = i.name;
            b.Price = i.price.ToString();
            id++;
        }
        buttonContainer.GetComponent<UIContentFitter>().Fit();
    }

    public void SetFurinute(int id)
    {
        furniture = _items[id].itemPrefab;
    }

    public GameObject GetFurniture()
    {
        return furniture;
    }

    // public async Task Get(String label)
    // {
    //     var locations = await Addressables.LoadResourceLocationsAsync(label).Task;
    //     foreach (var location in locations)
    //     {
    //         var obj = await Addressables.LoadAssetAsync<Item>(location).Task;
    //         _items.Add(obj);
    //     }
    // }
}
