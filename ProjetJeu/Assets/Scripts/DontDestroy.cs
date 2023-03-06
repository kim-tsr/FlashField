using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class DontDestroy : MonoBehaviour
{
    public string name;
    public string team;
    public int place;
    
    private void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }
}
