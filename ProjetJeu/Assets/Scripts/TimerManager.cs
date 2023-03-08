using System;
using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

public class TimerManager : MonoBehaviourPunCallbacks
{
    public Text textTimer;

    public int dureeManche;
    public int currentMatchTimer;

    public bool mancheFini = false;
    public bool start = true;


    private void Start()
    {
        StartCoroutine(Timer());
    }

    public void OnEnable()
    {
        PhotonNetwork.NetworkingClient.EventReceived += NetworkingClient_EventReceived;
    }

    private void NetworkingClient_EventReceived(EventData obj)
    {
        if (obj.Code == 2)
        {
            object content = obj.CustomData;
            string minutes = ((int)content / 60).ToString("00");
            string secondes = ((int)content % 60).ToString("00");
            textTimer.text = minutes + ":" + secondes;
        }
    }
    
    public void OnDisable()
    {
        PhotonNetwork.NetworkingClient.EventReceived -= NetworkingClient_EventReceived;
    }
    

    /*
    public void Update()
    {
        Debug.Log("C'est dans le Update");
        if (PhotonNetwork.IsMasterClient && start)
        {
            Debug.Log("C'est passe");
            start = false;
            Timer();
        }
    }*/


    IEnumerator Timer()
    {
        yield return new WaitForSeconds(1);
        currentMatchTimer -= 1;
        if (currentMatchTimer <= 0)
        {
            mancheFini = true;
        }
        else
        {
            PhotonNetwork.RaiseEvent((byte)2, currentMatchTimer ,new RaiseEventOptions { Receivers = ReceiverGroup.All },SendOptions.SendReliable);
            StartCoroutine(Timer());
        }
    }

}
