using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEditor;
using UnityEngine;

public class PlayerReseau : MonoBehaviourPunCallbacks
{
    public PhotonView View;

    public Behaviour[] componentsToDisable;

    public void Start()
    {
        if (!View.IsMine)
        {
            foreach (var component in componentsToDisable)
            {
                component.enabled = false;
            }
        }
    }
}
