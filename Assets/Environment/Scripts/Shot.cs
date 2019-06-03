using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shot : MonoBehaviour
{
    #region Inicialización de términos

    /// <summary>
    /// Objeto a instanciar
    /// </summary>
    public GameObject bullet;

    /// <summary>
    /// Posición donde se instancia el objeto
    /// </summary>
    public Transform spawnPoint;

    /// <summary>
    /// POOL!
    /// </summary>
    public static List<GameObject> bulletPool = new List<GameObject>();

    #endregion

    // Update is called once per frame
    void Update ()
    {
        /// Cuando pulsamos el botón derecho del ratón disparamos
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            DoShoot();
        }
	}

    /// <summary>
    /// Función que hace un pool de balas para disparar, de esta manera mejorarmos el rendimiento
    /// </summary>
    void DoShoot()
    {
        //SI hay algún elemnto en el pool, lo usamos, si no creamos uno nuevo, que al ser 'destruido' se desactivará y alñadirá a la lista (en Bullet.cs)
        if (bulletPool.Count > 0)
        {
            bulletPool[0].SetActive(true);
            bulletPool[0].transform.position = spawnPoint.position;
            bulletPool[0].transform.rotation = spawnPoint.rotation;
            //Lo sacamos de la lista
            bulletPool.RemoveAt(0);
        }
        else
        {
            //Lo creamos y añadimos a la lista.
            GameObject bulletInstance = Instantiate(bullet, spawnPoint.position, spawnPoint.rotation);   
        }
    }
}
