using System;
using System.Collections;
using System.Collections.Generic;
using System.Timers;
using ExitGames.Client.Photon;
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
    
    public int nombreMortBleuManche;
    public int nombreMortRougeManche;
    public int nombreMaxManche;
    public int currentNombreManche;
    public bool tempsFini = false;
    public int nombreMancheBleu;
    public int nombreMancheRouge;
    public int nombreMembreBleu;
    public int nombreMembreRouge;

    public GameObject canvasInterface;
    
    
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

                nombreMembreBleu += 1;
                PhotonNetwork.RaiseEvent((byte)5, new object[] {nombreMembreBleu,nombreMembreRouge},new RaiseEventOptions { Receivers = ReceiverGroup.All },SendOptions.SendReliable);
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
                nombreMembreRouge += 1;
                PhotonNetwork.RaiseEvent((byte)5, new object[] {nombreMembreBleu,nombreMembreRouge},new RaiseEventOptions { Receivers = ReceiverGroup.All },SendOptions.SendReliable);
            }
        }
    }

    public void Update()
    {
        if (tempsFini)
        {
            if (nombreMortBleuManche >= nombreMortRougeManche)
            {
                nombreMancheBleu += 1;
            }
            else
            {
                nombreMancheRouge += 1;
            }

            object[] content = new object[] { nombreMancheBleu, nombreMancheRouge };
            PhotonNetwork.RaiseEvent(4, content,new RaiseEventOptions { Receivers = ReceiverGroup.All },SendOptions.SendReliable);
            currentNombreManche += 1;
            nombreMortBleuManche = 0;
            nombreMortRougeManche = 0;
        }
        else if (nombreMortBleuManche >= nombreMembreBleu && nombreMembreBleu != 0)
        {
            
            nombreMancheRouge += 1;
            currentNombreManche += 1;
            object[] content = new object[] { nombreMancheBleu, nombreMancheRouge };
            PhotonNetwork.RaiseEvent(4, content,new RaiseEventOptions { Receivers = ReceiverGroup.All },SendOptions.SendReliable);
            nombreMortBleuManche = 0;
            nombreMortRougeManche = 0;
        }
        else if (nombreMortRougeManche >= nombreMembreRouge && nombreMembreRouge != 0)
        {
            
            nombreMancheBleu += 1;
            currentNombreManche += 1;
            object[] content = new object[] { nombreMancheBleu, nombreMancheRouge };
            PhotonNetwork.RaiseEvent(4, content,new RaiseEventOptions { Receivers = ReceiverGroup.All },SendOptions.SendReliable);
            nombreMortBleuManche = 0;
            nombreMortRougeManche = 0;
        }

        if (currentNombreManche >= nombreMaxManche)
        {
            
        }
    }
    
    public void OnEnable()
    {
        PhotonNetwork.NetworkingClient.EventReceived += NetworkingClient_EventReceived;
    }

    private void NetworkingClient_EventReceived(EventData obj)
    {
        if (obj.Code == 5)
        {
            object[] content = (object[]) obj.CustomData;
            nombreMembreBleu = (int)content[0];
            nombreMembreRouge = (int)content[1];
        }
        
        if (obj.Code == 6)
        {
            object[] content = (object[]) obj.CustomData;
            nombreMortBleuManche = (int)content[0];
            nombreMortRougeManche = (int)content[1];
        }
    }
    
    public void OnDisable()
    {
        PhotonNetwork.NetworkingClient.EventReceived -= NetworkingClient_EventReceived;
    }
    
    public void ChangeMort(string team_)
    {
        if (team_ == "blue")
        {
            nombreMortBleuManche += 1;
        }
        
        if (team_ == "red")
        {
            nombreMortRougeManche += 1;
        }
        PhotonNetwork.RaiseEvent(6, new object[] {nombreMortBleuManche,nombreMortRougeManche},new RaiseEventOptions { Receivers = ReceiverGroup.All },SendOptions.SendReliable);
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
