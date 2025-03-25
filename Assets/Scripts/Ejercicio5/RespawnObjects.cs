using System.Collections;
using UnityEngine;

public class AutoRespawn : MonoBehaviour
{
    public Transform respawnPoint; 
    public float respawnTime = 6f;

    void Start()
    {
        StartCoroutine(RespawnLoop());
    }

    IEnumerator RespawnLoop()
    {
        while (true) 
        {
            yield return new WaitForSeconds(respawnTime);

            if (respawnPoint != null)
            {
                Debug.Log($"{gameObject.name} ha respawneado después de {respawnTime} segundos.");
                transform.position = respawnPoint.position;
                GetComponent<Rigidbody>().linearVelocity = Vector3.zero; //reinicia velocidad
            }
        }
    }
}
