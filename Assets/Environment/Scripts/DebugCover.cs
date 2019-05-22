using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugCover : MonoBehaviour {

    //Script de las coverturas

    public void Start()
    {
        Unkown();
    }
    public void Covered()
    {
        GetComponent<MeshRenderer>().material.color = Color.green;
    }

    public void NoCovered()
    {
        GetComponent<MeshRenderer>().material.color = Color.red;
    }

    public void Unkown()
    {
        GetComponent<MeshRenderer>().material.color = Color.gray;
    }
}
