using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public GameObject canvasPause;
    public GameObject canvasCrosshair;
    public GameObject canvasBoutique;
    public GameObject canvasInventaire;
    public GameObject canvasScoreBoard;
    public PlayerController playerController;

    public bool boolPause = true;
    public bool boolBoutique = true;
    public bool boolInventaire = true;
    public bool boolScoreBoard = true;

    public ChangeTouches changeTouches;
    public KeyCode keyBoutique = KeyCode.B;
    public KeyCode keyInventaire = KeyCode.CapsLock;
    public KeyCode keyScoreBoard = KeyCode.Tab;
    public KeyCode keyPause = KeyCode.Escape;

    void Update()
    {
        keyBoutique = changeTouches.keyBoutique; // Recupere la touche prevu pour ouvrir la boutique dans le script ChangeTouches
        keyInventaire = changeTouches.keyInventaire; // Recupere la touche prevu pour ouvrir l'inventaire dans le script ChangeTouches
        keyPause = changeTouches.keyPause;
        keyScoreBoard = changeTouches.keyScoreBoard;
        
        
        if (Input.GetKeyDown(keyPause)) // Lorsque l'utilisateur appuye sur Echap
        {
            if (boolPause) // Si on peut ouvrir le menu, c'est a dire si aucun autre menu n'est ouvert
            {
                canvasCrosshair.GetComponent<Canvas>().enabled = false; // On ferme le canvas du crosshair
                canvasPause.GetComponent<Canvas>().enabled = true; // Pour ouvrir celui du menu Pause
                playerController.moov = false; // On arrete tout deplacement du joueur
                boolBoutique = false; // On empeche l'ouverture du menu de boutique
                boolInventaire = false; // On empeche l'ouverture de l'inventaire
                boolScoreBoard = false;
            }
        }

        if (Input.GetKeyDown(keyScoreBoard))
        {
            if (boolScoreBoard)
            {
                canvasCrosshair.GetComponent<Canvas>().enabled = false;
                canvasScoreBoard.GetComponent<Canvas>().enabled = true;
                boolBoutique = false;
                boolInventaire = false;
                boolPause = false;
            }
        }
        else if (Input.GetKeyUp(keyScoreBoard))
        {
            if (boolScoreBoard)
            {
                canvasCrosshair.GetComponent<Canvas>().enabled = true;
                canvasScoreBoard.GetComponent<Canvas>().enabled = false;
                boolBoutique = true;
                boolInventaire = true;
                boolScoreBoard = true;
                boolPause = true;
            }
        }
        
        if (Input.GetKeyDown(keyBoutique))
        {
            if (boolBoutique)
            {
                if (canvasBoutique.GetComponent<Canvas>().enabled) // Si le canvas est deja ouvert alors on le ferme quand l'utilisateur re-appuye sur la touche
                {
                    canvasCrosshair.GetComponent<Canvas>().enabled = true; // Ouvre le Canvas du Crosshair 
                    canvasBoutique.GetComponent<Canvas>().enabled = false; // Ferme el Canvas de la boutique
                    playerController.moov = true; // Reaoutorise le controle
                    boolPause = true; // Autorise l'ouverture du menu Pause 
                    boolInventaire = true; // Autorise l'ouverture du menu de l'Inventaire
                    boolScoreBoard = true;
                }
                else // Sinon on l'ouvre
                {
                    canvasCrosshair.GetComponent<Canvas>().enabled = false;
                    canvasBoutique.GetComponent<Canvas>().enabled = true;
                    playerController.moov = false;
                    boolPause = false;
                    boolInventaire = false;
                    boolScoreBoard = false;
                }
            }
        }
        
        if (Input.GetKeyDown(keyInventaire))
        {
            if (boolInventaire)
            {
                if (canvasInventaire.GetComponent<Canvas>().enabled) // Si le canvas est deja ouvert alors on le feme quand l'utilisateur re-appuye sur la touche
                {
                    canvasCrosshair.GetComponent<Canvas>().enabled = true;
                    canvasInventaire.GetComponent<Canvas>().enabled = false;
                    playerController.moov = true;
                    boolPause = true;
                    boolBoutique = true;
                    boolScoreBoard = true;
                }
                else // Sinon on l'ouvre
                {
                    canvasCrosshair.GetComponent<Canvas>().enabled = false;
                    canvasInventaire.GetComponent<Canvas>().enabled = true;
                    playerController.moov = false;
                    boolPause = false;
                    boolBoutique = false;
                    boolScoreBoard = false;
                }
            }
        }
    }
    
    
}
