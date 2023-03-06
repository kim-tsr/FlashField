using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class ChoixEquipe : MonoBehaviour
{
    public Text blueTeam;
    public Text redTeam;

    public Text name;

    public Button BlueButton;
    public Button RedButton;
    public Button SwitchButton;
    public Button ReadyButton;
    public InputField inputName;

    public Text[] blueList;
    public Text[] redList;

    public int place;
    public string team;

    public bool boolplace = false;
    public bool boolStart = false;

    public TeamManager TeamManager;
    public DontDestroy DontDestroy;

    public PhotonView View;
    private void Start()
    {
        SwitchButton.interactable = false;
    }

    public void Update()
    {
        if (name.text != "")
        {
            if (!boolplace)
            {
                BlueButton.interactable = true;
                RedButton.interactable = true;
                SwitchButton.interactable = false;
                ReadyButton.interactable = false;
            }
            else if (boolStart)
            {
                inputName.interactable = false;
                BlueButton.interactable = false;
                RedButton.interactable = false;
                SwitchButton.interactable = false;
                ReadyButton.interactable = false;
            }
            else
            {
                BlueButton.interactable = false;
                RedButton.interactable = false;
                SwitchButton.interactable = true;
                ReadyButton.interactable = true;
            }
            
            int blueDispo = 0;
            int redDispo = 0;
            foreach (var blueMember in blueList)
            {
                if (blueMember.text == "")
                {
                    blueDispo += 1;
                }
            }

            if (blueDispo == 0)
            {
                BlueButton.interactable = false;
            }
        
            foreach (var redMember in redList)
            {
                if (redMember.text == "")
                {
                    redDispo += 1;
                }
            }
        
            if (redDispo == 0)
            {
                RedButton.interactable = false;
            }
        }
        else
        {
            BlueButton.interactable = false;
            RedButton.interactable = false;
        }

        DontDestroy.name = name.text;
        DontDestroy.team = team;
        DontDestroy.place = place;
    }

    public void BlueTeam()
    {
        bool dejaPlace = false;
        for (int i = 0; i < blueList.Length; i++)
        {
            if (blueList[i].text == "" && ! dejaPlace)
            {
                boolplace = true;
                team = "blue";
                dejaPlace = true;
                place = i;
                View.RPC("GoBlueTeam" , RpcTarget.All,i,name.text);
            }
        }
    }
    
    public void RedTeam()
    {
        bool dejaPlace = false;
        
        for (int i = 0; i < redList.Length; i++)
        {
            if (redList[i].text == "" && !dejaPlace)
            {
                boolplace = true;
                team = "red";
                dejaPlace = true;
                place = i;
                View.RPC("GoRedTeam" , RpcTarget.All,i,name.text);
            }
        }
    }

    public void SwitchTeam()
    {
        View.RPC("ResetTeam",RpcTarget.All,team);
        boolplace = false;
    }

    public void StartGame()
    {
        boolStart = true;
        BlueButton.interactable = false;
        RedButton.interactable = false;
        SwitchButton.interactable = false;
        ReadyButton.interactable = false;
        View.RPC("AugmenteTeamManager",RpcTarget.All);
    }

    [PunRPC]
    public void AugmenteTeamManager()
    {
        TeamManager.nombreJoueurPret += 1;
    }
    
    [PunRPC]
    public void GoBlueTeam(int i, string name_)
    {
        blueList[i].text = name_;
    }
    
    [PunRPC]
    public void GoRedTeam(int i, string name_)
    {
        redList[i].text = name_;
    }
    
    [PunRPC]
    public void ResetTeam(string team_)
    {
        if (team_ == "blue")
        {
            blueList[place].text = "";
        }
        else
        {
            redList[place].text = "";
        }
    }
}
