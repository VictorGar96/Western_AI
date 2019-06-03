using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CowboyLocomotion : MonoBehaviour
{
    #region Inicialización de términos

    /// <summary>
    /// Varibles
    /// </summary>
    private NavMeshAgent _navAgent;
    private Transform myTransform = null;
    public float rotVel = 90f;
    public float movVel = 20f;

    /// <summary>
    /// Lista de tipo nodo
    /// </summary>
    List<Nodo> path;

    /// <summary>
    /// Variables booleanas por las cuales comprobamos si nos estamos moviendo y si estamos calculando una ruta
    /// </summary>
    public bool moving;
    bool calculandoRuta;

    /// <summary>
    /// posición de los nodos a recorrer por enemy
    /// </summary>
    int indexNode;

    /// <summary>
    /// Referencia al script Pathfinder
    /// </summary>
    Pathfinding pathFinder;

    int i = 0;

    public DebugCover[] patrolPoints;

    #endregion

    void Awake()
    {
        _navAgent    = GetComponentInParent         <NavMeshAgent>();
        myTransform  = transform.parent.GetComponent<Transform   >();
        patrolPoints = FindObjectsOfType            <DebugCover  >();
        pathFinder   = GetComponent                 <Pathfinding >();

        //_navAgent.stoppingDistance = GetComponent<CowboyAttack>().attackDistance;
        _navAgent.angularSpeed = rotVel;
    }

    void Update()
    {
        if (moving)
        {
            //if (nohemosllegado al ultimo punto de la ruta)
            myTransform.position = Vector3.MoveTowards(myTransform.position, path[indexNode].position, movVel * Time.deltaTime);

            //Mira siempre hacia el destino
            myTransform.LookAt(path[indexNode].position);
            //Si hemos llegado al siguiente nodo, 
            if (Vector3.Distance(myTransform.position, path[indexNode].position) < 0.1f)
            {
                indexNode++;

                //Si llegamos al ultimo nodo, es que estamos en el destino
                if (path.Count == indexNode)
                    moving = false;
            }
        }
    }

    /// <summary>
    /// Método que dado un target se mueve hasta ese target
    /// </summary>
    /// <param name="target"></param>
    public void MoveTo(Transform target)
    {
        if (!calculandoRuta)
        {
            pathFinder.CalcularRuta(myTransform.position, target.position, Comenzar);
            calculandoRuta = true;
        }

        //Old System
        //_navAgent.destination = target.position;
    }

    //Cuando la ruta está calculada comienza el movimiento
    void Comenzar()
    {
        moving         = true;
        calculandoRuta = false;
        path           = pathFinder.path;
        indexNode      = NearetNode();

    }

    /// <summary>
    /// Reseteamos los valores para volver a calcular la ruta
    /// </summary>
    public void ResetPath()
    {
        moving = false;
        calculandoRuta = false;
    }

    /// <summary>
    /// Partimos del nodo más cercanos cuando recalculamos la ruta
    /// Esto está implmentado para mejorar el algoritmo cuando un enemigo ha detectado al player y se mueve hacia él
    /// </summary>
    /// <returns></returns>
    private int NearetNode()
    {
        for(int i = 0; i < path.Count; i++)
        {
            if(Vector3.Distance(path[i].position, myTransform.position) < 1f)
                return i;
        }
        return 0;
    }

    /// <summary>
    /// Mover al enemigo a una cover random cada vez
    /// </summary>
    /// <param name="reset"></param>
    public void MoveToRandomCover(bool reset = false)
    {
        //Si no nos estamos moviendo, llamamomos al MoveTo hacia una rando cover.
        //if (_navAgent.velocity == Vector3.zero || reset)
        if (!moving)
        {
            int rand = Random.Range(0, patrolPoints.Length);
            MoveTo(patrolPoints[rand].transform);
        }
    }
}