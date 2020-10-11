using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlaceInfo : MonoBehaviour
{
    //[System.NonSerialized]
    public bool isEmpty = true;

    public int row, column;

    [System.NonSerialized] 
    public Renderer rend;
    public Material dragMaterial;
    public Material defaultRend;

    void Start()
    {
        rend = GetComponent<Renderer>();
    }
}
