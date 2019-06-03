using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent (typeof(CowboyDecider))]
public class CowboyAttack : MonoBehaviour {

    #region Inicialización de términos
    /// <summary>
    /// Distancia a la que comenzamos a atacar al enemigo
    /// </summary>
    public float attackDistance = 2;
    public float attackDamage   = 5;

    /// <summary>
    /// Probabilidad de acierto al atacar
    /// </summary>
    [Range(0f, 10f)]
    public int chance = 1;

    /// <summary>
    /// Cadencia de disparo de los enemigos
    /// </summary>
    [SerializeField]
    float cadency = 0f;

    /// <summary>
    /// Referencia al script Health del player
    /// </summary>
    [SerializeField]
    Health playerHealth;

    /// <summary>
    /// Referencia al script CowboyDecider
    /// </summary>
    CowboyDecider decider = null;

    /// <summary>
    /// Sound effect
    /// </summary>
    private AudioSource fireSound;

    #endregion

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

    /// <summary>
    /// Función para atacar al player
    /// Retornamos una llamada al método TakeDamage del script playerHealth
    /// </summary>
    /// <returns></returns>
    public bool DoAttack()
    {
        return playerHealth.TakeDamage(attackDamage, chance);        
    }

    /// <summary>
    /// Corrutina para atacar con una cadencia específica
    /// </summary>
    /// <returns></returns>
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

    /// <summary>
    /// Visualizar distancia de ataque
    /// </summary>
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, transform.forward * attackDistance);
        //Gizmos.DrawLine(transform.position, transform.position + (Vector3.forward * attackDistance));
        //Gizmos.color = Color.blue;
    }
}
