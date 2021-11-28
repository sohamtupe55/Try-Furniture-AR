using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSnap : MonoBehaviour
{
    public SnapshotCamera cam;

    public GameObject prefab;
    // Start is called before the first frame update
    void Start()
    {
        cam.TakePrefabSnapshot(prefab);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
