using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CowboyLocomotion : MonoBehaviour
{
    #region

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
    bool moving;
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
            //Si hemos llegado al nodo siguiente, 
            if (Vector3.Distance(myTransform.position, path[indexNode].position) < 0.1f)
            {
                indexNode++;

                //Si llegamos al ultimo nodo, es que estamos en el destino
                if (path.Count == indexNode)
                    moving = false;
            }
        }
    }

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
        indexNode      = 0;
        path           = pathFinder.path;

    }

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