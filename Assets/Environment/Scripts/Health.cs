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

    /// <summary>
    /// Barra de vida
    /// </summary>
    [SerializeField]
    float dumpingLifebar = 55f;

    /// <summary>
    /// Sound effects
    /// </summary>
    public AudioSource takeDamage;
    public AudioSource lowHealthSound;

    /// <summary>
    /// Sistema de partículas para visualizar la sangre
    /// </summary>
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
        /// Bajar la barra de vida pregresivamente
        healthBar.value = Mathf.Lerp(healthBar.value,  currentHealth, Time.time * dumpingLifebar);
    }


    /// <summary>
    /// Función para recibir daño cada chace segundos.
    /// </summary>
    /// <param name="damage"></param>
    /// Probabilidad de acierto
    /// <param name="chance"></param>
    /// <returns></returns>
    public bool TakeDamage(float damage, int chance)
    {
        /// Random para recibir daño o no por parte del enemigo
        int r = UnityEngine.Random.Range(0, 10);

        /// Forma de falsear que los enemigos a veces fallan cuando disparan
        if (r <= chance)
        {
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

    /// <summary>
    /// Regeneración de vida a lo largo de tiempo
    /// </summary>
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
