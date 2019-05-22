using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CowboyLocomotion : MonoBehaviour
{

    private NavMeshAgent _navAgent;
    private Transform myTransform = null;
    public float rotVel = 90f;

    int i = 0;

    public DebugCover[] patrolPoints;
 
    void Awake()
    {
        _navAgent    = GetComponentInParent<NavMeshAgent>();
        myTransform  = GetComponent<Transform>();
        patrolPoints = FindObjectsOfType<DebugCover>();
        //_navAgent.stoppingDistance = GetComponent<CowboyAttack>().attackDistance;
        _navAgent.angularSpeed = rotVel;
    }

    void Update()
    {
       
    }

    public void MoveTo(Transform target)
    {
        //Quaternion root = Quaternion.LookRotation(target.position - myTransform.position, Vector3.up);
        //myTransform.rotation  = Quaternion.RotateTowards(myTransform.rotation, target.rotation, rotVel * Time.deltaTime);
        _navAgent.destination = target.position;
       // _navAgent.gameObject.transform.LookAt(_navAgent.destination);
        
    }

    public void MoveToRandomCover(bool reset = false)
    {
        //Si no nos estamos moviendo, llamamomos al MoveTo hacia una rando cover.
        if (_navAgent.velocity == Vector3.zero || reset)
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
