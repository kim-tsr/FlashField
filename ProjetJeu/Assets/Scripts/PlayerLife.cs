using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class PlayerLife : MonoBehaviourPun
{
    public float life = 100f;

    public Text texteLife;

    public GameObject MainCamera;

    public PhotonView photonView;

    public Canvas canvasScoreBoard;

    public GameObject dontDestroy;
    public string namePlayerMort;
    public string teamPlayerMort;
    public int placePlayerMort;
    
    public string namePlayerQuiATouche;
    public string teamPlayerQuiATouche;
    public int placePlayerQuiATouche;
    
    public void Start()
    {
        dontDestroy = GameObject.FindGameObjectWithTag("DontDestroy");
        namePlayerMort = dontDestroy.GetComponent<DontDestroy>().name;
        teamPlayerMort = dontDestroy.GetComponent<DontDestroy>().team;
        placePlayerMort = dontDestroy.GetComponent<DontDestroy>().place;
        photonView = PhotonView.Get(this);
    }

    public void Update()
    {
        if (texteLife != null)  // Pour pouvoir faire les tests avec les cubes
        {
            texteLife.text = life.ToString(); //Affiche la vie avec l'objet texte qui se trouve dans Canvas Crosshair
        }
        
        if (life <= 0)
        {
            Debug.Log("teamPlayerQuiATouche  "+teamPlayerQuiATouche);
            Debug.Log("teamPlayerMort  "+teamPlayerMort);
            Debug.Log("placePlayerQuiATouche  "+placePlayerQuiATouche);
            Debug.Log("placePlayerMort  "+placePlayerMort );
            MainCamera = GameObject.FindGameObjectWithTag("MainCamera");
            MainCamera.SetActive(true);
            
            if (teamPlayerQuiATouche == "blue")
            {
                Debug.Log("killer blue");
                canvasScoreBoard.GetComponent<ScoreBoardManager>().listeKillBlue[placePlayerQuiATouche].text = (int.Parse(canvasScoreBoard.GetComponent<ScoreBoardManager>().listeKillBlue[placePlayerQuiATouche].text)+1).ToString();
            }
        
            if (teamPlayerQuiATouche == "red")
            {
                Debug.Log("killer red");
                canvasScoreBoard.GetComponent<ScoreBoardManager>().listeKillRed[placePlayerQuiATouche].text = (int.Parse(canvasScoreBoard.GetComponent<ScoreBoardManager>().listeKillRed[placePlayerQuiATouche].text)+1).ToString();
            }
        
            if (teamPlayerMort == "blue")
            {
                Debug.Log("mort blue");
                canvasScoreBoard.GetComponent<ScoreBoardManager>().listeMortBlue[placePlayerMort].text = (int.Parse(canvasScoreBoard.GetComponent<ScoreBoardManager>().listeMortBlue[placePlayerMort].text)+1).ToString();
            }
        
            if (teamPlayerMort == "red")
            {
                Debug.Log("mort red");
                canvasScoreBoard.GetComponent<ScoreBoardManager>().listeMortRed[placePlayerMort].text = (int.Parse(canvasScoreBoard.GetComponent<ScoreBoardManager>().listeMortRed[placePlayerMort].text)+1).ToString();
            }
            
            Test();
        }
    }

    [PunRPC]
    public void ATouche(string team_ , int place_)
    {
        teamPlayerQuiATouche = team_;
        placePlayerQuiATouche = place_;
    }
    

    [PunRPC]
    public void Kill(string[] lstrMortRed, string[] lstrMortBlue, string[] lstrKillBlue, string[] lstrKillRed)
    {
        Text[] lMortRed = canvasScoreBoard.GetComponent<ScoreBoardManager>().listeMortRed;
        Text[] lMorBlue = canvasScoreBoard.GetComponent<ScoreBoardManager>().listeMortBlue;
        Text[] lKillerBlue = canvasScoreBoard.GetComponent<ScoreBoardManager>().listeKillBlue;
        Text[] lKillerRed = canvasScoreBoard.GetComponent<ScoreBoardManager>().listeKillRed;
        
        for (int i = 0; i < 5; i++)
        {
            lKillerBlue[i].text = lstrKillBlue[i];
            lKillerRed[i].text = lstrKillRed[i];
            lMorBlue[i].text = lstrMortBlue[i];
            lMortRed[i].text = lstrMortRed[i];
        }
        
        this.gameObject.SetActive(false);
    }

    public void Test()
    {
        Text[] lMortRed = canvasScoreBoard.GetComponent<ScoreBoardManager>().listeMortRed;
        Text[] lMorBlue = canvasScoreBoard.GetComponent<ScoreBoardManager>().listeMortBlue;
        Text[] lKillerBlue = canvasScoreBoard.GetComponent<ScoreBoardManager>().listeKillBlue;
        Text[] lKillerRed = canvasScoreBoard.GetComponent<ScoreBoardManager>().listeKillRed;

        string[] lstrMortRed = new string[5];
        string[] lstrMortBlue = new string[5];
        string[] lstrKillBlue = new string[5];
        string[] lstrKillRed = new string[5];

        for (int i = 0; i < 5; i++)
        {
            lstrKillBlue[i] = lKillerBlue[i].text;
            lstrKillRed[i] = lKillerRed[i].text;
            lstrMortBlue[i] = lMorBlue[i].text;
            lstrMortRed[i] = lMortRed[i].text;
        }
        
        photonView.RPC("Kill",RpcTarget.All,lstrMortRed,lstrMortBlue , lstrKillBlue,lstrKillRed );
    }
}
