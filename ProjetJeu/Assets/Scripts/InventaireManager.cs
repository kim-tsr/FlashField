using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class InventaireManager : MonoBehaviourPunCallbacks
{
    public GameObject[] listeInventaire;
    public float[] listeNbrInventaire;

    public GameObject player;

    public GameObject heal;
    public GameObject munFa;
    public GameObject munMitraillette;
    public GameObject munSniper;
    public GameObject grenade;
    public GameObject arme1;
    public GameObject arme2;
    public GameObject argent;

    public Text textHeal;
    public Text textGrenade;
    public Text textArme1;
    public Text textArme2;
    public Text textMunMitraillette;
    public Text textMunFa;
    public Text textMunSniper;
    public Text textArgent;

    public PlayerLife playerLife;

    public GameObject takeObject;
    public bool canTake = false;

    public BoutiqueManager boutiqueManager;
    public bool dropArme1 = false;
    public bool dropArme2 = false;

    public GameObject Pistolet;
    public GameObject Mitraillette;
    public GameObject Fa;
    public GameObject Sniper;

    public GameObject Arme1Mi;
    public GameObject Arme1Fa;
    public GameObject Arme1Snip;

    public ChangeTouches touches;
    public KeyCode keyRamasser;
    
    public void Start()
    {
        listeInventaire = new[] {heal,grenade,arme1,arme2, munMitraillette, munFa, munSniper,argent};
        listeNbrInventaire = new[] {1f, 5f, 0f,0f, 31f, 31f , 9f,100000f};
    }

    public void Update()
    {
        textHeal.text = listeNbrInventaire[0].ToString(); // Affiche le nombre d'Object que possede le joueur 
        textGrenade.text = listeNbrInventaire[1].ToString();
        textArme1.text = listeNbrInventaire[2].ToString();
        textArme2.text = listeNbrInventaire[3].ToString();
        textMunMitraillette.text = listeNbrInventaire[4].ToString();
        textMunFa.text = listeNbrInventaire[5].ToString();
        textMunSniper.text = listeNbrInventaire[6].ToString();
        textArgent.text = listeNbrInventaire[7].ToString();

        keyRamasser = touches.keyRamasser;
        
        if (canTake) // Si le joueur est dans le trigger du GameObject (est change plus bas dans TakeObject) 
        {
            if (Input.GetKeyDown(keyRamasser)) // Si le joueur cherche a prendre l'objet
            {
                if (takeObject.layer == 8) // LayerMask Heal
                {
                    listeNbrInventaire[0] += 1; // Ajoute le nombre souhaiter dans l'inventaire
                    canTake = false; // Empeche de prendre des objets en boucle
                    Destroy(takeObject); // Detruit le GameObject
                }
        
                if (takeObject.layer == 9) // LayerMask Grenade
                {
                    listeNbrInventaire[1] += 1;
                    canTake = false;
                    Destroy(takeObject);
                }
        
                if (takeObject.layer ==  10) // LayerMask MunMitraillette
                {
                    listeNbrInventaire[4] += takeObject.GetComponent<Quantite>().quantity;
                    canTake = false;
                    PhotonNetwork.Destroy(takeObject);
                }
        
                if (takeObject.layer == 11) // LayerMask MunFa
                {
                    listeNbrInventaire[5] += takeObject.GetComponent<Quantite>().quantity;
                    canTake = false;
                    PhotonNetwork.Destroy(takeObject);
                }
        
                if (takeObject.layer == 12) // LayerMask MunSniper
                {
                    listeNbrInventaire[6] += takeObject.GetComponent<Quantite>().quantity;
                    canTake = false;
                    PhotonNetwork.Destroy(takeObject);
                }
        
                if (takeObject.layer == 14 && !boutiqueManager.GetComponent<BoutiqueManager>().PossessionArme2) // LayerMask Arme2
                {
                    listeNbrInventaire[3] = 1;
                    canTake = false;
                    boutiqueManager.BuyPistolet();
                    PhotonNetwork.Destroy(takeObject);
                }
                if (takeObject.layer == 16 && !boutiqueManager.GetComponent<BoutiqueManager>().PossessionArme1) // LayerMask Arme2
                {
                    listeNbrInventaire[2] = 1;
                    canTake = false;
                    boutiqueManager.BuyMitraillette();
                    PhotonNetwork.Destroy(takeObject);
                }
                if (takeObject.layer == 17 && !boutiqueManager.GetComponent<BoutiqueManager>().PossessionArme1) // LayerMask Arme2
                {
                    listeNbrInventaire[2] = 1;
                    canTake = false;
                    boutiqueManager.BuyFa();
                    PhotonNetwork.Destroy(takeObject);
                }
                
                if (takeObject.layer == 18 && !boutiqueManager.GetComponent<BoutiqueManager>().PossessionArme1) // LayerMask Arme2
                {
                    listeNbrInventaire[2] = 1;
                    canTake = false;
                    boutiqueManager.BuySniper();
                    PhotonNetwork.Destroy(takeObject);
                }
                
                if (takeObject.layer == 15) // LayerMask Argent
                {
                    listeNbrInventaire[7] += takeObject.GetComponent<Quantite>().quantity;
                    canTake = false;
                    PhotonNetwork.Destroy(takeObject);
                }
            }
        }
    }

    public void UtiliserHeal() // Si le joueur appuye sur le bouton dans le Canvas
    {
        if (listeNbrInventaire[0] > 0) // Si le joueur possede bien au moins un objet
        {
            playerLife.life = 100; // Remet la vie du joueur au maximum
            listeNbrInventaire[0] -= 1; // Enleve un objet de l'inventaire 
        }
    }

    public void JeterGrenade()
    {
        if (listeNbrInventaire[1] > 0)
        {
            // Prend la position du joueur en enlevant un peu de hauteur et en rajoutant un peu de distance devant lui
            Vector3 pos = new Vector3(player.transform.position.x , player.transform.position.y-0.5f,player.transform.position.z+2);
            PhotonNetwork.Instantiate(grenade.name, pos , player.transform.rotation); // Pour faire apparaitre le GameObject a la position
            listeNbrInventaire[1] -= 1;
        }
    }
    
    public void JeterArme1()
    {
        if (listeNbrInventaire[2] > 0)
        {
            Vector3 pos = new Vector3(player.transform.position.x , player.transform.position.y-0.5f,player.transform.position.z+2);
            GameObject arme =  PhotonNetwork.Instantiate(arme1.name, pos , player.transform.rotation);
            arme.GetComponent<SphereCollider>().enabled = true;
            listeNbrInventaire[2] -= 1;
            if (arme1 == Mitraillette)
            {
                Arme1Mi.SetActive(false);
            }
            
            if (arme1 == Fa)
            {
                Arme1Fa.SetActive(false);
            }
            
            if (arme1 == Sniper)
            {
                Arme1Snip.SetActive(false);
            }
            
            dropArme1 = true;
        }
    }
    
    public void JeterArme2()
    {
        if (listeNbrInventaire[3] > 0)
        {
            Vector3 pos = new Vector3(player.transform.position.x , player.transform.position.y-0.5f,player.transform.position.z+2);
            GameObject arme = PhotonNetwork.Instantiate(arme2.name, pos , player.transform.rotation);
            arme.GetComponent<SphereCollider>().enabled = true;
            listeNbrInventaire[3] -= 1;
            dropArme2 = true;
        }
    }
    
    public void JeterMunMitraillette()
    {
        if (listeNbrInventaire[4] > 0)
        {
            Vector3 pos = new Vector3(player.transform.position.x , player.transform.position.y-0.5f,player.transform.position.z+2);
            GameObject munMitraillette__ = munMitraillette;
            if (listeNbrInventaire[4]>30)
            {
                munMitraillette__.GetComponent<Quantite>().quantity = 30;
                listeNbrInventaire[4] -= 30;
            }
            else
            {
                munMitraillette__.GetComponent<Quantite>().quantity = listeNbrInventaire[4];
                listeNbrInventaire[4] = 0;
            }
            PhotonNetwork.Instantiate(munMitraillette__.name, pos , player.transform.rotation);
        }
    }
    
    public void JeterMunFa()
    {
        if (listeNbrInventaire[5] > 0)
        {
            Vector3 pos = new Vector3(player.transform.position.x , player.transform.position.y-0.5f,player.transform.position.z+2);
            GameObject munFa__ = munFa;
            if (listeNbrInventaire[5]>30)
            {
                munFa__.GetComponent<Quantite>().quantity = 30;
                listeNbrInventaire[5] -= 30;
            }
            else
            {
                munFa__.GetComponent<Quantite>().quantity = listeNbrInventaire[5];
                listeNbrInventaire[5] = 0;
            }
            PhotonNetwork.Instantiate(munFa__.name, pos , player.transform.rotation);
        }
    }
    
    public void JeterMunSniper()
    {
        if (listeNbrInventaire[6] > 0)
        {
            Vector3 pos = new Vector3(player.transform.position.x , player.transform.position.y-0.5f,player.transform.position.z+2);
            GameObject munSniper__ = munSniper;
            if (listeNbrInventaire[6]>8)
            {
                munSniper__.GetComponent<Quantite>().quantity = 8;
                listeNbrInventaire[6] -= 8;
            }
            else
            {
                munSniper__.GetComponent<Quantite>().quantity = listeNbrInventaire[6];
                listeNbrInventaire[6] = 0;
            }
            PhotonNetwork.Instantiate(munSniper__.name, pos , player.transform.rotation);
        }
    }
    
    public void JeterArgent()
    {
        if (listeNbrInventaire[7] > 0)
        { 
            Vector3 pos = new Vector3(player.transform.position.x , player.transform.position.y-0.5f,player.transform.position.z+2);
            GameObject argent__ = argent;
            if (listeNbrInventaire[7]>1000)
            {
                argent__.GetComponent<Quantite>().quantity = 1000;
                listeNbrInventaire[7] -= 1000;
            }
            else
            {
                argent__.GetComponent<Quantite>().quantity = listeNbrInventaire[4];
                listeNbrInventaire[7] = 1000;
            }
            PhotonNetwork.Instantiate(argent__.name, pos , player.transform.rotation);
        }
    }

    public void TakeObject(GameObject _gameObject) // Appele dans le script Grap
    {
        takeObject = _gameObject; // Regarde l'objet qui est a proximite
        canTake = true; // Met le booleen a vrai pour pouvoir prendre l'objet
    }
}
