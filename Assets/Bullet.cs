using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

	// Use this for initialization
	void OnEnable () {
		
        //TODO: la lifetimedeberían ser Serializadas
        Invoke("DestroyMe", 3);
	}
	
	// Update is called once per frame
	void Update () {
        //TODO: la speed deberían ser Serializadas
        transform.Translate(transform.forward * 10 * Time.deltaTime);

    }

    private void OnCollisionEnter(Collision collision)
    {
        Health h = collision.gameObject.GetComponent<Health>();

        if (h!=null)
        {
            h.TakeDamage(10, 100); //TODO: estas variables deberían ser Serializadas
        }

        //Sin Pool
        //La bala se destruye al colisionar con al
        //Destroy(gameObject);

        //COn Pool, desactivamos el objeto y lo añadimos al pool, para poder ser reutilizado
        DestroyMe();
    }

    //COn Pool, desactivamos el objeto y lo añadimos al pool, para poder ser reutilizado
    void DestroyMe()
    {
        gameObject.SetActive(false);
        Shot.bulletPool.Add(gameObject);
    }
}
