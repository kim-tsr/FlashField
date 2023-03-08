using System;
using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

public class ScoreBoardManager : MonoBehaviourPunCallbacks
{
    public Text[] listeNomBlue;
    public Text[] listeNomRed;
    public Text[] listeKillBlue;
    public Text[] listeKillRed;
    public Text[] listeMortBlue;
    public Text[] listeMortRed;
    
    public int[] listeIntKillBlue = new []{0,0,0,0,0};
    public int[] listeIntKillRed = new []{0,0,0,0,0};
    public int[] listeIntMortBlue = new []{0,0,0,0,0};
    public int[] listeIntMortRed = new []{0,0,0,0,0};

    public GameObject dontDestroy;

    public int place;
    public string team;
    public string name;

    public PhotonView View;

    
    public void Start()
    {
        dontDestroy = GameObject.FindGameObjectWithTag("DontDestroy");
        team = dontDestroy.GetComponent<DontDestroy>().team;
        name = dontDestroy.GetComponent<DontDestroy>().name;
        place = dontDestroy.GetComponent<DontDestroy>().place;
        photonView.RPC("MettreNom", RpcTarget.All,place,team,name);
        Refresh();
    }

    [PunRPC]
    public void MettreNom(int place_, string team_, string name_)
    {
        if (team_ == "blue")
        {
            listeNomBlue[place_].text = name_;
        }
        
        if (team_ == "red")
        {
            listeNomRed[place_].text = name_;
        }
    }

    
    public void Update()
    {
        for (int i = 0; i < 5; i++)
        {
            listeKillBlue[i].text = listeIntKillBlue[i].ToString();
            listeKillRed[i].text = listeIntKillRed[i].ToString();
            listeMortRed[i].text = listeIntMortRed[i].ToString();
            listeMortBlue[i].text = listeIntMortBlue[i].ToString();
        }
    }

    public void OnEnable()
    {
        PhotonNetwork.NetworkingClient.EventReceived += NetworkingClient_EventReceived;
    }

    private void NetworkingClient_EventReceived(EventData obj)
    {
        if (obj.Code == 3)
        {
            object[] content = (object[])obj.CustomData;
            listeIntKillBlue = (int[])content[0];
            listeIntKillRed = (int[])content[1];
            listeIntMortBlue = (int[])content[2];
            listeIntMortRed = (int[])content[3];
        }
    }
    
    public void OnDisable()
    {
        PhotonNetwork.NetworkingClient.EventReceived -= NetworkingClient_EventReceived;
    }

    public void Refresh()
    {
        object[] obj = new[] { listeIntKillBlue, listeIntKillRed, listeIntMortBlue, listeIntMortRed };
        PhotonNetwork.RaiseEvent((byte)3, obj ,new RaiseEventOptions { Receivers = ReceiverGroup.All },SendOptions.SendReliable);
    }
    
    
}
