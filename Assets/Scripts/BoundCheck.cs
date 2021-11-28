using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BoundCheck : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Bounds b = GetMaxBounds(gameObject);
        gameObject.AddComponent<BoxCollider>();
        BoxCollider col = GetComponent<BoxCollider>();
        //g.transform.SetParent(gameObject.transform);
        col.size = b.extents*2;

        SelectionController sc = gameObject.AddComponent<SelectionController>();
        TranslationManager tm = gameObject.AddComponent<TranslationManager>();
        
        GameObject g = GameObject.CreatePrimitive(PrimitiveType.Cube);
        var offset = transform.position - b.center;
        g.transform.localScale = b.extents * 2;
        g.transform.position = transform.position - offset;
        g.transform.SetParent(transform);
        col.center = g.transform.localPosition;
        sc.selectionVisualization = g;
        sc.DeleteButtonVisualize = UIController.Instance.deleteButton;
        g.GetComponent<MeshRenderer>().material = DataHandler.Instance.visualizerMat;
    }

    Bounds GetMaxBounds(GameObject g) {
        var b = new Bounds(g.transform.position, Vector3.zero);
        foreach (Renderer r in g.GetComponentsInChildren<Renderer>()) {
            b.Encapsulate(r.bounds);
        }
        return b;
    }
}
