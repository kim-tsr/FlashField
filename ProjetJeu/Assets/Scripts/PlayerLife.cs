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
    public GameObject GameManager;

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
            
            if (teamPlayerQuiATouche == "red")
            {
                 canvasScoreBoard.GetComponent<ScoreBoardManager>().listeIntKillBlue[placePlayerQuiATouche] += 1;
                 canvasScoreBoard.GetComponent<ScoreBoardManager>().Refresh();
                 canvasScoreBoard.GetComponent<ScoreBoardManager>().listeIntMortRed[placePlayerMort] += 1;
                 canvasScoreBoard.GetComponent<ScoreBoardManager>().Refresh();
                 GameManager.GetComponent<GameManager>().ChangeMort("red");
            }
        
            if (teamPlayerQuiATouche == "blue")
            {
                canvasScoreBoard.GetComponent<ScoreBoardManager>().listeIntKillRed[placePlayerQuiATouche] += 1;
                canvasScoreBoard.GetComponent<ScoreBoardManager>().Refresh();
                canvasScoreBoard.GetComponent<ScoreBoardManager>().listeIntMortBlue[placePlayerMort] += 1;
                canvasScoreBoard.GetComponent<ScoreBoardManager>().Refresh();
                GameManager.GetComponent<GameManager>().ChangeMort("blue");
            }
            
            this.gameObject.SetActive(false);
        }
    }

    [PunRPC]
    public void Desact(GameObject player)
    {
        player.SetActive(false);
    }
    
    public void ATouche(string team_ , int place_)
    {
        teamPlayerQuiATouche = team_;
        placePlayerQuiATouche = place_;
    }
    
    /*
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
        
        Kill(lstrMortRed,lstrMortBlue , lstrKillBlue,lstrKillRed );
    }
    
    */
}
