using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEditor;
using UnityEngine;

public class PauseManager : MonoBehaviourPunCallbacks
{
    public Canvas canvasPause;
    public Canvas canvasTouches;
    public Canvas canvasCrosshair;

    public PlayerController playerController;

    public MenuManager menuManager;

    public GameObject playerPrefab;

    public void ChangerTouches() // Quand on appuye sur le bouton Changer Touche
    {
        canvasPause.enabled = false; // On ferme le menu Pause
        canvasTouches.enabled = true; // Pour ouvrir le menu pour changer less touches
        menuManager.boolPause = false;
    }

    public void Resume() // Quand on appuye sur le bouton Resume
    {
        canvasPause.enabled = false; // Ferme le menu Pause
        canvasCrosshair.enabled = true; // Remet en place le canvas du cross hair
        playerController.moov = true; // Remet en place les mouvements, l'utilisateur peut rejouer
        menuManager.GetComponent<MenuManager>().boolBoutique = true; // Autorise l'ouverture du menu boutique puisque le menu Pause est ferme
        menuManager.GetComponent<MenuManager>().boolInventaire = true; // Autorise l'ouverture de l'inventaire puisque le menu Pause est ferme
        menuManager.GetComponent<MenuManager>().boolScoreBoard = true;
        menuManager.GetComponent<MenuManager>().boolPause= true;
    }
    
    public override void OnLeftRoom()
    {
        PhotonNetwork.Destroy(playerPrefab);
        PhotonNetwork.LoadLevel("MenuPrincipal");
    }
}
