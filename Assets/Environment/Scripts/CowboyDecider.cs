using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CowboyDecider : MonoBehaviour {

    #region Inicialización de términos

    /// <summary>
    /// Target
    /// </summary>
    [SerializeField]
    Transform target;

    /// <summary>
    /// Rango de visión del enemigo
    /// </summary>
    [SerializeField]
    float visionRange = 20f;

    /// <summary>
    /// Ángulo de visión del enemigo
    /// </summary>
    [SerializeField]
    float visionAngle = 10;

    /// <summary>
    /// Estado actual del enemigo.
    /// </summary>
    public stateAI currentState;

    /// <summary>
    /// Transform.
    /// </summary>
    private Transform myTransform = null;

    /// <summary>
    /// Animator.
    /// </summary>
    //private Animator animator;

    /// <summary>
    /// Vector3 que obtendrá la distancia al player
    /// </summary>
    private float distanceToPlayer;
    
    /// <summary>
    /// Referencia al script CowboyLocomotion
    /// </summary>
    CowboyLocomotion locomotion;

    /// <summary>
    /// Referencia al script CowboyAttack
    /// </summary>
    CowboyAttack cowboyAttack;

    Coroutine attacking;

    //float playerH;
    //float playerMaxH;

    
    #endregion

    private void Awake()
    {
        currentState = stateAI.patrol;
    }

    private void Start()
    {
        locomotion   = GetComponent<CowboyLocomotion>();
        myTransform  = GetComponent<Transform       >();
        cowboyAttack = GetComponent<CowboyAttack    >();
    }

    /// <summary>
    /// Log que indica cuando hemos calculado una ruta
    /// </summary>
    void RecorrerRuta()
    {
        Debug.LogWarning("RutaCalculada!");
    }

    /// <summary>
    /// Llamamos a la función Decide()
    /// </summary>
    private void Update()
    {
        // Llamamos a la función Decide();
        Decide();

        /// Obtenemos la distancia al jugador para comprobar si esta en rango de tiro.
        distanceToPlayer = Vector3.Distance(target.position, myTransform.position);
    }

    /// <summary>
    /// Función con la cual controlamos el estado de los enemigos
    /// </summary>
    void Decide()
    {
        switch (currentState)
        {
            case stateAI.patrol:
                Detector();
                locomotion.MoveToRandomCover();
                break;
            case stateAI.wander:
                Wander();
                break;
            case stateAI.runaway:
                break;
            case stateAI.attack:
                Attack();
                break;
        }
    }

    /// <summary>
    /// Función llamada en el estado "patrol"
    /// 
    /// </summary>
    void Detector()
    {
        string tag;
        //Player Detected!
        //Si elplayer está en elángulo de visión
        if (VisionAngle(out tag) < visionAngle && tag == "Player")
        {
            Debug.Log("PLayer Detected! Wander");
            currentState = stateAI.wander;

            locomotion.ResetPath();
        }

        else
        {
           // Debug.Log("Did not hit");
            currentState = stateAI.patrol;
        }
    }

    /// <summary>
    /// Persiguiendo al player
    /// </summary>
    void Wander()
    {
        string tag;

        //Calculamos la ruta hasta el player e iniciamos el movimiento.
        locomotion.MoveTo(target);

        //Distancia de ataque
        if (distanceToPlayer < cowboyAttack.attackDistance && VisionAngle(out tag) < visionAngle)
        {
            Debug.Log("PLayer In range! Attaacking");

            currentState = stateAI.attack;

            //Start Attacking
            attacking = StartCoroutine(cowboyAttack.Attacking());
        }

        if (distanceToPlayer > visionRange)
        {
            currentState = stateAI.patrol;
            locomotion.MoveToRandomCover(true);
        }
    }

    /// <summary>
    /// Atacando al player
    /// </summary>
    void Attack()
    {
        string tag;

        //Para que nose mueva mientrass ataca
        locomotion.moving = false;

        //Stop Attacking
        if (distanceToPlayer > cowboyAttack.attackDistance || VisionAngle(out tag) > visionAngle)
        {
            currentState = stateAI.wander;
            Debug.Log("PLayer Out of range! Stop Attaacking");

            StopCoroutine(attacking);                        
        }
    }

    /// <summary>
    /// Rango de visión de los enemigos
    /// </summary>
    /// <param name="tag"></param>
    /// <returns></returns>
    float VisionAngle(out string tag)
    {
        //Lanzamos un raycast hacia el player y si le immpactamos comprobamos que esté en nuestro área de visión
        RaycastHit hit;

        float angle = 0f;

        if (Physics.Raycast(myTransform.position, target.position - myTransform.position, out hit, visionRange))
        {
            //Si el ángulo entre el forward del enemigo y el raycast lanzado al player es mejor del ángulo de visión, lo hemos detectado!
            Vector3 rayVector = hit.point - myTransform.position;
            angle = Vector3.Angle(transform.TransformDirection(Vector3.forward), rayVector);
            tag = hit.collider.tag;

        }
        else
        {
            tag = "";
        }
        
        /// Retornamos el ángulo
        return angle;
    }

    //void OnDrawGizmosSelected()  //Solo dibuja el gizmo cuando el objeto está seleccionado
    void OnDrawGizmos()
    {
        float halfFOV = visionAngle;

        Quaternion leftRayRotation = Quaternion.AngleAxis(-halfFOV, Vector3.up);
        Quaternion rightRayRotation = Quaternion.AngleAxis(halfFOV, Vector3.up);

        Vector3 leftRayDirection = leftRayRotation * transform.forward;
        Vector3 rightRayDirection = rightRayRotation * transform.forward;

        Gizmos.DrawRay(transform.position, leftRayDirection * visionRange);
        Gizmos.DrawRay(transform.position, rightRayDirection * visionRange);

    }
}
