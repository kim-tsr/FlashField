using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using Vector3 = System.Numerics.Vector3;

public class GrenadeScript : MonoBehaviourPunCallbacks
{
    public float sec = 2f;
    public bool boom = false;
    public GameObject particules;
    public bool do_one_time = false;
    public bool isSmoke;

    private void OnCollisionEnter(Collision collision)
    {
        StartCoroutine(Explositon());
    }

    IEnumerator Explositon()
    {
        yield return new WaitForSeconds(sec);
        boom = true;
    }

    private void OnTriggerStay(Collider other)
    {
        if (!boom)
        {
            return;   
        }

        if (other.CompareTag("Mur") && !isSmoke)
        {
            other.GetComponent<PlayerLife>().life -= 80;
        }

        if (particules != null && !do_one_time)
        {
            do_one_time = true;
            UnityEngine.Vector3 pos = new UnityEngine.Vector3();
            if (isSmoke)
            {
                pos = new UnityEngine.Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y,
                    this.gameObject.transform.position.z-10);
            }
            else
            {
                pos = new UnityEngine.Vector3(gameObject.transform.position.x,gameObject.transform.position.y,gameObject.transform.position.z);
            }
            GameObject effet =  PhotonNetwork.Instantiate(particules.name, pos, Quaternion.identity);
            
            StartCoroutine(Effet(effet));
        }

        
    }

    IEnumerator Effet(GameObject effet)
    {
        if (isSmoke)
        {
            yield return new WaitForSeconds(20);
        }
        else
        {
            yield return new WaitForSeconds(3);
        }
        PhotonNetwork.Destroy(effet);
        PhotonNetwork.Destroy(gameObject);
    }
}
