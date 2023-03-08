using System;
using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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

    public int matchLength = 180;
    public Text textTimer;
    public int currentMatchTime;
    private Coroutine timerCoroutine;
    
    
    void Start()
    {
        Debug.Log(PhotonNetwork.IsMasterClient);
        dontDestroy = GameObject.FindGameObjectWithTag("DontDestroy");
        team = dontDestroy.GetComponent<DontDestroy>().team;
        place = dontDestroy.GetComponent<DontDestroy>().place;
        name = dontDestroy.GetComponent<DontDestroy>().name;
        StartCoroutine("SecondStart");
    }
    
    /*
    public enum EventCodes : byte
    {
        RefreshTimer
    }

    private void InitializeTimer()
    {
        currentMatchTime = matchLength;
        RefreshTimer();

        if (PhotonNetwork.IsMasterClient)
        {
            timerCoroutine = StartCoroutine(Timer());
        }
    }
    
    private void RefreshTimer()
    {
        string minutes = (currentMatchTime / 60).ToString("00");
        string secondes = (currentMatchTime % 60).ToString("00");
        textTimer.text = $"{minutes}:{secondes}";
    }

    public void EndGame()
    {
        if (timerCoroutine != null) StopCoroutine(timerCoroutine);
        currentMatchTime = 0;
        RefreshTimer();
    }

    private IEnumerator Timer()
    {
        yield return new WaitForSeconds(1);

        currentMatchTime -= 1;
        if (currentMatchTime <= 0)
        {
            timerCoroutine = null;
        }
        else
        {
            RefreshTimer_S();
            timerCoroutine = StartCoroutine(Timer());
        }
    }
    
    */

    IEnumerator SecondStart()
    {
        if (!do_one_time)
        {
            canvasTouches = GameObject.FindGameObjectWithTag("Touches");
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
