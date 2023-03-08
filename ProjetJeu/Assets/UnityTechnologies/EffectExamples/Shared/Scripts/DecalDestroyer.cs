using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class DecalDestroyer : MonoBehaviourPun {

	public float lifeTime = 5.0f;

	private IEnumerator Start()
	{
		yield return new WaitForSeconds(lifeTime);
		PhotonNetwork.Destroy(gameObject);
	}
}
