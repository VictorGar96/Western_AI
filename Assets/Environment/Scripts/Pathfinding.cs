using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[Serializable]
public class Nodo
{
    public const float MIN_DISTANCE = 0.5F; /// TODO: aumentar

    public Vector3 position = Vector3.zero;
    public float cost       = 0;
    public float distance   = 0;

    public float priority
    {
        get { return cost + distance; }
    }

    public Nodo padre;

    public Nodo(Vector3 p_position, float p_cost, float p_distance, Nodo p_padre)
    {
        this.position = p_position;
        this.cost     =     p_cost;
        this.distance = p_distance;
        this.padre    =    p_padre;
    }

    /// <summary>
    /// Calcular que un nodo es meta o si es igual a otro.
    /// </summary>
    /// <param name="goal"></param>
    /// <returns></returns>
    public bool EsIgual(Vector3 goal, float minDistance = MIN_DISTANCE)
    {
        if (Vector3.Distance(position, goal) < minDistance)//TODO: distance
        {
            return true;
        }
        return false;
    }

    /// <summary>
    ///  Calcular el coste mediante la posición actual y el goal 
    /// </summary>
    /// <param name="_distance"></param>
    /// <returns></returns>
    public float FStar(float _distance)
    {
        var hStar = distance;

        return cost + hStar;
        
    }
}

public class Pathfinding : MonoBehaviour {

    [SerializeField]
    float cost = 0.7f;

    Vector3 destination         = Vector3.zero;
    List<Nodo> frontiers = new List<Nodo>();
    public List<Nodo> path      = new List<Nodo>();

    GameObject virtualAgent;

    // Use this for initialization
    void Start ()
    {
        virtualAgent = GameObject.CreatePrimitive(PrimitiveType.Sphere);
    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public List<Nodo> CalcularRuta(Vector3 initialPosition, Vector3 goal, Action callBack)
    {
        frontiers.Clear();
        
        StartCoroutine(Calculate(initialPosition, goal, callBack));

        return null;
    }

    //Como el calculo de ruta se realiza en una corrutina, la ruta calculada la devolvemos mediante CallBack
    IEnumerator Calculate(Vector3 initialPosition, Vector3 goal, Action callBack)
    {
        /// Inicializamos la posición inicial a la posición del agente 
        virtualAgent.transform.position = initialPosition;
        destination = goal;

        /// Creamos una variable tipo nodo temporal al cual le asignamos initial position
        /// padre de nodoStart = null
        Nodo nodoStart = new Nodo(initialPosition, 0, Vector3.Distance(destination, initialPosition), null);

        /// Llamamos a la función GetFrontiers() pasandole nodoStart
        /// Desde GetFrontiers() lanzamos los raycast llamando a la función ThrowRayCast()
        GetFrontiers(nodoStart);

        while (frontiers.Count > 0)
        {
            /// Extraemos la primera posición
            Nodo nodo = frontiers[0];
            frontiers.RemoveAt(0);

            //Agente para debug
            virtualAgent.transform.position = nodo.position;
  
            //La ruta hecha hasta ahora.
            path = RecorrerPadres(nodo);

            /// Comprobamos si es nodo META!! 3m
            if (nodo.EsIgual(destination, 3))
            {
                //Devolvemos la lista de los padres que forman la ruta. 
                callBack();
                break;
            }

            /// Buscamos las nuevas fronteras del nodo actual
            /// 
            GetFrontiers(nodo);

            Debug.Log("Nodos: " + frontiers.Count);

          //  Debug.Break();

            yield return null;
        }
        yield return null;
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

        /// Ordenamos la lista por su priority (coste + distancia)
        frontiers = frontiers.OrderBy(nodo => nodo.priority).ToList();
    }


    /// <summary>
    /// Lanza los rayos y si chocan con el suelo los añade a la lista frontiers
    /// </summary>
    /// <param name="direction"></param>
    /// <param name="currentNodo"></param>
    private void ThrowRayCast(Vector3 direction, Nodo currentNodo)
    {
        RaycastHit hit;

        Debug.DrawRay(currentNodo.position + (Vector3.up * 1f), direction*5, Color.blue);

        if (Physics.Raycast(currentNodo.position + (Vector3.up * 1f), direction, out hit))
        {
            if (hit.collider.tag == "Ground" || hit.collider.tag == "Player")
            {
                //Creamos cada uno de los 8 posibles nodos de dirección
                Nodo nodo = new Nodo(hit.point, currentNodo.cost + cost, Vector3.Distance(hit.point, destination), currentNodo);

                ///TODO:
                ///1) Aumentar la distancia de llegada y comprobar si se lanza el log de "llegado!"
                ///2) Crear un sistem de comprobación, que solo añada el nodo si no está en frontiers ni en el path
                ///3) Quitar la corrutina y devolver la ruta 
                ///4) sustituir el sistema antiguo por este. 
                
                /// Si el nodo actual es el primerono o el nuevo nodo es distinto al nodo padre, lo añadimos como nuevo camino posible
                if (currentNodo.padre == null || !nodo.EsIgual(currentNodo.padre.position) && IsValidNode(nodo))
                {
                    frontiers.Add(nodo);
                }
            }
        }
    }

    /*
    public void ordInsertar(Nodo n, List<Nodo> a, float coste)
    {

        //frontiers.OrderBy(nodo => nodo.priority);

        //for (var i = 0; i < frontiers.Count; i++)
        //{
        //    if (frontiers[i].priority > coste)
        //    {
        //        frontiers.Insert(i, n);
        //        break;
        //    }
        //}
        //if (frontiers.Count == 0) frontiers.Add(n);
    }
    */

    /// <summary>
    /// Recorrer los padres para calcular la ruta.
    /// Hacer un reverse de la lista.
    /// </summary>
    /// <param name="last"></param>
    /// <returns></returns>
    public List<Nodo> RecorrerPadres(Nodo last)
    {
        var comeFrom = new List<Nodo>();

        while (last.padre != null)
        {
            comeFrom.Add(last);
            /// Nodo del que viene
            last = last.padre;
        }

        comeFrom.Reverse();

        return comeFrom;
    }

    public bool IsValidNode(Nodo nodo)
    {
        foreach (Nodo n in frontiers)
        {
            if (n.EsIgual(nodo.position))
                return false;               
        }

        foreach (Nodo p in path)
        {
            if (p.EsIgual(nodo.position))
                return false;
        }

        return true;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        foreach (Nodo n in frontiers)
        {
            Gizmos.DrawWireCube(n.position, Vector3.one * 0.5f);
        }
        Gizmos.color = Color.white;
        foreach (Nodo n in path)
        {
            Gizmos.DrawWireCube(n.position, Vector3.one * 0.5f);
        }
    }
}


