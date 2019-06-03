using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[Serializable]
public class Nodo
{
    #region Inicialización de términos
    public const float MIN_DISTANCE = 0.5F; /// TODO: aumentar

    public Vector3 position = Vector3.zero;
    public float cost       = 0;
    public float distance   = 0;

    public float priority
    {
        get { return cost + distance; }
    }

    public Nodo padre;

    #endregion

    /// <summary>
    /// Constructor de la clase Nodo
    /// </summary>
    /// <param name="p_position"></param>
    /// <param name="p_cost"></param>
    /// <param name="p_distance"></param>
    /// <param name="p_padre"></param>
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
}

public class Pathfinding : MonoBehaviour {

    #region Inicialización de métodos
    [SerializeField]
    float cost = 0.7f;

    /// <summary>
    /// Destination, lista de fronteras y lista que contendrá el camino a seguir
    /// </summary>
    Vector3 destination         = Vector3.zero;
           List<Nodo> frontiers = new List<Nodo>();
    public List<Nodo> path      = new List<Nodo>();

    GameObject virtualAgent;

    #endregion
    // Use this for initialization
    void Start ()
    {
        virtualAgent = GameObject.CreatePrimitive(PrimitiveType.Sphere);
    }
	
    /// <summary>
    /// Método que llamaremos en el Script CowboyLocomotion para calcular la ruta y mover a los enemies
    /// </summary>
    /// <param name="initialPosition"></param>
    /// <param name="goal"></param>
    /// <param name="callBack"></param>
    /// <returns></returns>
    public List<Nodo> CalcularRuta(Vector3 initialPosition, Vector3 goal, Action callBack)
    {
        frontiers.Clear();
        
        StartCoroutine(Calculate(initialPosition, goal, callBack));

        return null;
    }

    /// Como el calculo de ruta se realiza en una corrutina, la ruta calculada la devolvemos mediante CallBack
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
            GetFrontiers(nodo);

            Debug.Log("Nodos: " + frontiers.Count);

          //  Debug.Break();

            yield return null;
        }
        yield return null;
    }

    /// <summary>
    /// Lanzamos un rayo en 8 direcciones
    /// </summary>
    /// <param name="current"></param>
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
                /// Creamos cada uno de los 8 posibles nodos de dirección
                Nodo nodo = new Nodo(hit.point, currentNodo.cost + cost, Vector3.Distance(hit.point, destination), currentNodo);

                /// Si el nodo actual es el primerono o el nuevo nodo es distinto al nodo padre, lo añadimos como nuevo camino posible
                if (currentNodo.padre == null || !nodo.EsIgual(currentNodo.padre.position) && IsValidNode(nodo))
                {
                    frontiers.Add(nodo);
                }
            }
        }
    }

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

    /// <summary>
    /// Comprobamos que es un nodo váilido comprobando si no es el nodo de destiono o 
    /// hay otro nodo con la misma posición.
    /// </summary>
    /// <param name="nodo"></param>
    /// <returns></returns>
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

    /// <summary>
    /// Visualizar las rayos lanzados y los nodos de ambas listas
    /// </summary>
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

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(destination, 1f);
    }
}


