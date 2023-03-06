using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class TeamManager : MonoBehaviourPunCallbacks
{
    public int nombreJoueurPret = 0;
    public bool do_one_time = false;

    void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    private void Update()
    {
        if (PhotonNetwork.CurrentRoom.PlayerCount == nombreJoueurPret)
        {
            PhotonNetwork.CurrentRoom.IsOpen = false;
            StartCoroutine(Lancement());
        }
    }

    IEnumerator Lancement()
    {
        if (!do_one_time)
        {
            do_one_time = true;
            yield return new WaitForSeconds(2);
            PhotonNetwork.LoadLevel("Game");
        }
        
    }
}
