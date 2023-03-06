using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class ScoreBoardManager : MonoBehaviour
{
    public Text[] listeNomBlue;
    public Text[] listeNomRed;
    public Text[] listeKillBlue;
    public Text[] listeKillRed;
    public Text[] listeMortBlue;
    public Text[] listeMortRed;

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
        View.RPC("MettreNom",RpcTarget.All,place,team,name);
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
}
