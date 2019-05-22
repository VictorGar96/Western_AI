using System;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour {

    #region Inicialización de términos
    /// <summary>
    /// Vida máxima del jugdor
    /// </summary>
    [SerializeField]
    float maxHealth = 1f;

    /// <summary>
    /// Imagen para representar la vida del juegador.
    /// </summary>
    [SerializeField]
    Slider healthBar;

    /// <summary>
    /// Vida actual del jugador
    /// </summary>
    private float currentHealth;

    /// <summary>
    /// Referencia al objeto. Este se utiliza para ser destruido.
    /// </summary>
    GameObject player;

    [SerializeField]
    float dumpingLifebar = 55f;

    public AudioSource takeDamage;
    public AudioSource lowHealthSound;

    public GameObject bloodParticle;
    #endregion

    // Use this for initialization
    void Start ()
    {
        //player = GetComponent<GameObject>();
        currentHealth = maxHealth;

        InvokeRepeating("UpdateHealth", 5, 1f);
	}
	
	// Update is called once per frame
	void Update ()
    {
        healthBar.value = Mathf.Lerp(healthBar.value,  currentHealth, Time.time * dumpingLifebar);
    }


    /// <summary>
    /// Función para recibir daño cada 2 segundos.
    /// </summary>
    /// <param name="damage"></param>
    /// Probabilidad de acierto
    /// <param name="chance"></param>
    /// <returns></returns>
    public bool TakeDamage(float damage, int chance)
    {
        /// Random para recibir daño por parte del enemigo
        int r = UnityEngine.Random.Range(0, 10);

        if (r <= chance)
        {
            /// TODO: añadir sonido de daño recibido.
            /// Paricle System rojo (gotas de sangre).
            //Debug.Log(currentHealth);

            currentHealth -= damage;

            if(takeDamage != null)
                takeDamage.Play();

            GameObject blood = Instantiate(bloodParticle, transform.position, Quaternion.identity);
            Destroy(blood, 3.0f);

            Debug.Log("Hit¡¡");
        }
        else
        {
            Debug.Log("Oh fuck I missed");
        }

        if (currentHealth < 0.4f && lowHealthSound != null )
            lowHealthSound.Play();

        if (currentHealth <= 0)
        {
            /// TODO: añadir sonido de muerte.

            gameObject.SetActive(false);
            healthBar.gameObject.SetActive(false);

            if (lowHealthSound != null)
                lowHealthSound.Stop();

            return false;
        }
        
        return true;
    }

    void UpdateHealth()
    {
        if (currentHealth < maxHealth)
            currentHealth += 0.01f;
    }

    /// <summary>
    /// Devuelve la vida actual del jugador.
    /// </summary>
    /// <returns></returns>
    public float getCurrentHealth()
    {
        return currentHealth;
    }
}
