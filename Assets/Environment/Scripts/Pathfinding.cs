using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nodo
{
    public Vector3 position = Vector3.zero;
    public float cost = 0;
    public float distance = 0;
    public float priority
    { get
        {
            return cost + distance;
        }
    }

    public Nodo(Vector3 p_position, float p_cost, float p_distance)
    {
        this.position = p_position;
        this.cost = p_cost;
        this.distance = p_distance;
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
	void Update () {
		
	}

    public List<Nodo> CalcularRuta(Vector3 initialPosition, Vector3 goal)
    {

        virtualAgent.transform.position = initialPosition;
        destination = goal;

        var nodoStart = new Nodo(initialPosition, 0, Vector3.Distance(destination, initialPosition));

        GetFrontiers(nodoStart);
        while (frontiers.Count > 0)
        {

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
                var nodo = new Nodo(hit.point, currentNodo.cost + 1, Vector3.Distance(hit.point, destination));
                frontiers.Add(nodo);
            }
        }
    }
}


