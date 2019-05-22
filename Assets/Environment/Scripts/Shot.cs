using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shot : MonoBehaviour
{
    public GameObject bullet;

    public Transform spawnPoint;

    //POOL!
    public static List<GameObject> bulletPool = new List<GameObject>();

    // Use this for initialization
	void Start ()
    {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            DoShoot();
        }
	}

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
