using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CowboyLocomotion : MonoBehaviour
{

    private NavMeshAgent _navAgent;
    private Transform myTransform = null;
    //public float rotVel = 90f;
    public float movVel = 20f;

    List<Nodo> path;

    bool moving;
    bool calculandoRuta;

    int indexNode;

    Pathfinding pathFinder;

    int i = 0;

    public DebugCover[] patrolPoints;
 
    void Awake()
    {
        _navAgent    = GetComponentInParent<NavMeshAgent>();
        myTransform  = transform.parent.GetComponent<Transform>();
        patrolPoints = FindObjectsOfType<DebugCover>();
        pathFinder = GetComponent<Pathfinding>();

        //_navAgent.stoppingDistance = GetComponent<CowboyAttack>().attackDistance;
        //_navAgent.angularSpeed = rotVel;
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
            pathFinder.CalcularRuta(myTransform.position, target.position, Comenar);
            calculandoRuta = true;
        }

        //Old System
        //_navAgent.destination = target.position;

        
        
    }

    //Cuando la ruta está calculada comienza el movimiento
    void Comenar ()
    {
        moving = true;
        calculandoRuta = false;
        indexNode = 0;
        path = pathFinder.path;

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

    //public IEnumerator CoversPath(DebugCover[] covers)
    //{
    //    Transform playerenemy = transform;
    //    do
    //    {
    //        MoveTo(covers[i].transform);

    //        //Hemos llegado al punto de patruya
    //        if (Vector3.Distance(myTransform.position, covers[i].transform.position) < 1)
    //        {
    //            yield return new WaitForSeconds(2);
    //            i++;
    //        }
    //    } while (i < covers.Length);

    //}

    ////Activamos la corrutina con un método público para llamarla desde CowboyDecider
    //public void StartC()
    //{
    //    StartCoroutine(CoversPath(patrolPoints));
    //}

    //public void StopC()
    //{
    //    StopCoroutine(CoversPath(patrolPoints));
    //}
}
