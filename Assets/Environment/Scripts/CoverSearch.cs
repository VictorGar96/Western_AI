using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CoverSearch : MonoBehaviour
{
    private GameObject[]     Covers;
    public  List<GameObject> Covered;
    public  List<GameObject> NoCovered;

    public  float LifeTime    = 2.0f;
    private float _lastUpdate = 0.0f;

    public GameObject Player;

    void Awake()
    {
        var coversGO = GetComponentsInChildren<SphereCollider>();
        Covers = new GameObject[coversGO.Length];
        for (var i=0;i< coversGO.Length;i++)
        {
            Covers[i] = coversGO[i].gameObject;
        }
    }

    void Update()
    {
        RefreshConvers(Time.time);
    }

    void RefreshConvers(float timestamp)
    {
        if (timestamp - _lastUpdate > LifeTime)
        {
            Covered   = new List<GameObject>();
            NoCovered = new List<GameObject>();
            foreach (var cover in Covers)
            {
                var hits = Physics.RaycastAll(Player.transform.position, (cover.transform.position - Player.transform.position),
                    50,LayerMask.GetMask("Covers"));
                var hitCount = hits.Length;
                if (hitCount == 0)
                {
                    Covered.Add(cover);
                    cover.GetComponent<DebugCover>().Covered();
                }
                else
                {
                    NoCovered.Add(cover);
                    cover.GetComponent<DebugCover>().NoCovered();
                }
            }
        }
    }
}
