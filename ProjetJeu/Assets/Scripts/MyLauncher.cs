using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using Photon.Realtime;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class MyLauncher : MonoBehaviourPunCallbacks
{
    public Button connexion;
    public byte maxPlayersPerRoom = 4;
    public Text feedbackText;

    public bool isConnecting;

    public string gameVersion = "1";

    public GameObject playerPrefab;

    public bool joinedLobby = false;
    public bool joinedRoom = false;

    public Canvas canvasTouches;
    public Canvas canvasMenu;

    
   /*
    public override void OnConnectedToMaster()
        {
            Debug.Log("Connected to master server");
            JoinRoom();
        }*/
    /*

    public override void OnCreatedRoom()
        {
            Debug.Log("Created room: " + PhotonNetwork.CurrentRoom.Name);
            JoinRoom("Game");
        }*/
/*
        public void Connect()
        {
            Debug.Log(" started");
            PhotonNetwork.ConnectUsingSettings();
        }
        
        public void JoinRoom()
        {
            Debug.Log("Join Random Room");
            RoomOptions roomOptions = new RoomOptions();
            roomOptions.MaxPlayers = maxPlayersPerRoom;
            PhotonNetwork.JoinOrCreateRoom("MyRoom",roomOptions,TypedLobby.Default);
        }

        public override void OnJoinedRoom()
        {
            PhotonNetwork.LoadLevel("Game");
        }*/

            
    /*
    private void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
        PhotonNetwork.GameVersion = this.gameVersion;
    }*/

    void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    private void Start()
    {
        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.Disconnect();
        }
    }


    public void Connect()
    {
        feedbackText.text = "";

        isConnecting = true;

        connexion.interactable = false;

        if (PhotonNetwork.IsConnected)
        {
            LogFeedback("Joining Room...");
            PhotonNetwork.JoinRandomRoom();
        }
        else
        {

            LogFeedback("Connecting...");

            PhotonNetwork.ConnectUsingSettings();
            PhotonNetwork.GameVersion = this.gameVersion;
        }

    }

    /*
    public override void OnConnectedToMaster()
    {
        RoomOptions myRoomOptions = new RoomOptions();
        myRoomOptions.MaxPlayers = maxPlayersPerRoom;
        PhotonNetwork.JoinOrCreateRoom("Game", myRoomOptions, TypedLobby.Default);
    }

    public override void OnJoinedLobby()
    {
        RoomOptions myRoomOptions = new RoomOptions();
        myRoomOptions.MaxPlayers = maxPlayersPerRoom;
        PhotonNetwork.JoinOrCreateRoom("Game", myRoomOptions, TypedLobby.Default);
    }
    */
    
    
    

    void LogFeedback(string message)
    {
        if (feedbackText == null) {
            return;
        }

        feedbackText.text += System.Environment.NewLine+message;
    }

    public override void OnConnectedToMaster()
    {
        if (isConnecting)
        {
            LogFeedback("OnConnectedToMaster: Next -> try to Join Random Room");
            Debug.Log("PUN Basics Tutorial/Launcher: OnConnectedToMaster() was called by PUN. Now this client is connected and could join a room.\n Calling: PhotonNetwork.JoinRandomRoom(); Operation will fail if no room found");
		
            PhotonNetwork.JoinRandomRoom();
        }
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        LogFeedback("<Color=Red>OnJoinRandomFailed</Color>: Next -> Create a new Room");
        Debug.Log("PUN Basics Tutorial/Launcher:OnJoinRandomFailed() was called by PUN. No random room available, so we create one.\nCalling: PhotonNetwork.CreateRoom");

        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = this.maxPlayersPerRoom});
    }

    
    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.LogError(cause);
        isConnecting = false;
        connexion.interactable = true;
    }

    
    public override void OnJoinedRoom()
    {
        LogFeedback("<Color=Green>OnJoinedRoom</Color> with "+PhotonNetwork.CurrentRoom.PlayerCount+" Player(s)");
        Debug.Log("PUN Basics Tutorial/Launcher: OnJoinedRoom() called by PUN. Now this client is in a room.\nFrom here on, your game would be running.");
		
        if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
        {
            Debug.Log("We load the 'Room for 1' ");

            PhotonNetwork.LoadLevel("ChooseTeam");

        }
    }

    public void Parametre()
    {
        canvasMenu.enabled = false;
        canvasTouches.enabled = true;
    }
}

    