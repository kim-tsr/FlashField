using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviourPunCallbacks
{
    public string[] listePlayer;

    public GameObject playerPrefabBlue;
    public GameObject playerPrefabRed;
    public bool quit = false;
    public bool leave = false;
    public bool do_one_time = false;
    public GameObject dontDestroy;

    public GameObject[] spawnPointBlue;
    public GameObject[] spawnPointRed;

    public bool do_one_time2 = false;

    public string name;
    public int place;
    public string team;

    public GameObject canvasTouches;
    
    void Start()
    {
        Debug.Log(PhotonNetwork.IsMasterClient);
        dontDestroy = GameObject.FindGameObjectWithTag("DontDestroy");
        team = dontDestroy.GetComponent<DontDestroy>().team;
        place = dontDestroy.GetComponent<DontDestroy>().place;
        name = dontDestroy.GetComponent<DontDestroy>().name;
        StartCoroutine("SecondStart");
    }

    IEnumerator SecondStart()
    {
        if (!do_one_time)
        {
            do_one_time2 = true;
            yield return new WaitForSeconds(1);
            if (team == "blue")
            {
                GameObject player = PhotonNetwork.Instantiate(this.playerPrefabBlue.name,spawnPointBlue[place].transform.position,spawnPointBlue[place].transform.rotation,0 );
                
                PhotonView view = player.GetPhotonView();
                if (!view.IsMine)
                {
                    Camera cam = player.GetComponent<Camera>();
                    cam.enabled = false;
                }
            }
            else if (team == "red")
            {
                GameObject player = PhotonNetwork.Instantiate(this.playerPrefabRed.name,spawnPointRed[place].transform.position,spawnPointRed[place].transform.rotation,0 );
                PhotonView view = player.GetPhotonView();
                if (!view.IsMine)
                {
                    Camera cam = player.GetComponent<Camera>();
                    cam.enabled = false;
                }
            }
        }
    }

    /*
    public   void Update()
    {
        if (quit)
        {
            QuitApplication();
        }
        
    }
    public void OnPlayerEnterRoom(Player other)
    {
        Debug.Log(other.NickName + "s'est connecté !");
    }
    public override void OnPlayerLeftRoom(Player other)
    {
        Debug.Log(other.NickName + "s'est déconnecté !");
    }*/

    public void QuitApplication()
    {
        Application.Quit();
    }
}
