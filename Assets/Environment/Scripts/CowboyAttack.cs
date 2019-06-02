using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent (typeof(CowboyDecider))]
public class CowboyAttack : MonoBehaviour {

    /// <summary>
    /// Distancia a la que comenzamos a atacar al enemigo
    /// </summary>
    public float attackDistance = 2;
    public float attackDamage   = 5;

    /// <summary>
    /// Probabilidad de acierto
    /// </summary>
    [Range(0f, 10f)]
    public int chance = 1;

    [SerializeField]
    float cadency = 0f;

    /// <summary>
    /// Referencia al script Health del player
    /// </summary>
    [SerializeField]
    Health playerHealth;

    CowboyDecider decider = null;

    private AudioSource fireSound;

    // Use this for initialization
    void Start ()
    {
        decider   = GetComponent<CowboyDecider>();
        fireSound = GetComponent<AudioSource  >();
    }
	
	// Update is called once per frame
	void Update ()
    {

	}

    public bool DoAttack()
    {
        return playerHealth.TakeDamage(attackDamage, chance);        
    }

    public IEnumerator Attacking()
    {
        //Mientras el player esté vivo le atacamos.
        while (DoAttack())
        {
            fireSound.Play();
            yield return new WaitForSeconds(cadency);
        }

        //Cuando el player muere volvemos a patrol
        decider.currentState = stateAI.patrol;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, transform.forward * attackDistance);
        //Gizmos.DrawLine(transform.position, transform.position + (Vector3.forward * attackDistance));
        //Gizmos.color = Color.blue;
    }
}
