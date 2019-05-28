using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nodo
{
    public Vector3 position = Vector3.zero;
    public float cost     = 0;
    public float distance = 0;
    public float priority
    {
        get { return cost + distance; }
    }

    public Nodo padre;

    public Nodo(Vector3 p_position, float p_cost, float p_distance, Nodo p_padre)
    {
        this.position = p_position;
        this.cost     = p_cost;
        this.distance = p_distance;
        this.padre    = p_padre;
    }

    public bool Meta(Vector3 goal)
    {
        if (this.position == goal)
        {
            return true;
        }
        return false;
    }

    public float FStar(float _distance)
    {
        var hStar = distance;

        return cost + hStar;
        
    }
}

public class Pathfinding : MonoBehaviour {

    Vector3 destination  = Vector3.zero;
    List<Nodo> frontiers = new List<Nodo>();
    List<Nodo> comeFrom  = new List<Nodo>();

    GameObject virtualAgent;

    // Use this for initialization
    void Start ()
    {
        virtualAgent = new GameObject("Virtual Agent");

    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public List<Nodo> CalcularRuta(Vector3 initialPosition, Vector3 goal)
    {

        virtualAgent.transform.position = initialPosition;
        destination = goal;

        var nodoStart = new Nodo(initialPosition, 0, Vector3.Distance(destination, initialPosition), null);

        GetFrontiers(nodoStart);

        while (frontiers.Count > 0)
        {
            /// Extraemos la primera posición
            var nodo = frontiers[0];
            frontiers.RemoveAt(0);

            /// Comprobamos si es nodo meta

            if (nodo.Meta(destination))
            {
                return frontiers;
            }

            /// Comprobar si tienen padre, en caso contrario lo añadimos a la lista
            /// 
            foreach (var c in frontiers)
            {

                if (c != nodo.padre)
                {
                    ordInsertar(c, frontiers, c.FStar(c.cost));
                }
            }
        }
        return null;
    }

    private void GetFrontiers(Nodo current)
    {
        /// North node
        Vector3 rayDirection = Vector3.down + Vector3.forward;
        ThrowRayCast(rayDirection, current);

        /// North-East node
        rayDirection = Vector3.down + (Vector3.forward + Vector3.right);
        ThrowRayCast(rayDirection, current);

        /// East node
        rayDirection = Vector3.down + Vector3.right;
        ThrowRayCast(rayDirection, current);

        /// South-East node
        rayDirection = Vector3.down + (-Vector3.forward + Vector3.right);
        ThrowRayCast(rayDirection, current);

        /// South node
        rayDirection = Vector3.down + -Vector3.forward;
        ThrowRayCast(rayDirection, current);

        /// South-West node
        rayDirection = Vector3.down + (-Vector3.forward + Vector3.left);
        ThrowRayCast(rayDirection, current);

        /// West node
        rayDirection = Vector3.down + Vector3.left;
        ThrowRayCast(rayDirection, current);

        /// North-West node
        rayDirection = Vector3.down + (Vector3.forward + Vector3.left);
        ThrowRayCast(rayDirection, current);

    }

    private void ThrowRayCast(Vector3 direction, Nodo currentNodo)
    {
        RaycastHit hit;

        if (Physics.Raycast(currentNodo.position + (Vector3.up * 2f), direction, out hit))
        {
            if (hit.collider.tag == "Ground")
            {
                var nodo = new Nodo(hit.point, currentNodo.cost + 1, Vector3.Distance(hit.point, destination), null);
                frontiers.Add(nodo);
            }
        }
    }

    public void ordInsertar(Nodo n, List<Nodo> a, float coste)
    {
        for (var i = 0; i < frontiers.Count; i++)
        {
            if (frontiers[i].FStar(n.cost) > coste)
            {
                frontiers.Insert(i, n);
                break;
            }
        }
        if (frontiers.Count == 0) frontiers.Add(n);
    }

    public List<Nodo> RecorrerPadres(Nodo last)
    {
        var closed = new List<Nodo>();

        while (last.padre != null)
        {
            /// Nodo del que viene
            //closed.Add(last.)
            last = last.padre;
        }

        closed.Reverse();

        return null;
    }
}


