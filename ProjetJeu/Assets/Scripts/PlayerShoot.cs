using System;
using System.Collections;
using System.Net.Mime;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(WeaponManager))]
public class PlayerShoot : MonoBehaviourPunCallbacks
{

    public Camera cam;

    public float[] currentWeapon;
    public float currentRange;
    public float currentDamage;
    public float currentFireRate;
    public float tailleChargeur;
    public float munRestante;
    public float styleMun;

    public LayerMask mask;

    public WeaponManager weaponManager;

    public PlayerWeapon playerWeapon;
    public ParticleSystem flareSmokePistolet;
    public ParticleSystem flareSmokeMitraillette;
    public ParticleSystem flareSmokeFA;
    public ParticleSystem flareSmokeSniper;

    public GameObject impactBalle;

    public PlayerController playerController;

    public BoutiqueManager boutiqueManager;

    public Text texteMunRestante;
    public Text texteTailleChargeur;

    public ChangeTouches changeTouches;
    public KeyCode keyReload;

    public bool boolCut = false;

    public InventaireManager inventaireManager;


    public MunitionArme muniPistolet;
    public MunitionArme muniSniper;
    public MunitionArme muniMitraillette;
    public MunitionArme muniFA;

    public PhotonView photonView;

    public Canvas canvasScoreBoard;
    public GameObject dontDestroy;


    
    void Start()
    {
        dontDestroy = GameObject.FindGameObjectWithTag("DontDestroy");
        photonView = PhotonView.Get(this);
        if (cam == null) // S'assure qu'on est une cam et qu'on peut donc tirer
        {
            Debug.LogError("Pas de cam pour le système de tir");
            this.enabled = false;
        }
        
        currentWeapon = weaponManager.GetCurrentWeapon(); // Initialise les valeurs de l'arme avec WeaponManager
        currentRange = currentWeapon[1];
        currentDamage = currentWeapon[0];
        currentFireRate = currentWeapon[2];
        tailleChargeur = currentWeapon[3];
        styleMun = currentWeapon[4];
        munRestante = tailleChargeur;

        texteMunRestante.text = munRestante.ToString(); // Affiche sur le texte MunRestante les balles qu'il reste dans le chargeur
        texteTailleChargeur.text = tailleChargeur.ToString(); // Affiche sur le texte TailleChargeur les balles qu'il y a dans un chargeur
    }

    private void Update()
    {
        if (boolCut)
        {
            munRestante = tailleChargeur;
        }

        if (currentWeapon == playerWeapon.Pistolet)
        {
            munRestante = muniPistolet.munRestante;
        }
        
        if (currentWeapon == playerWeapon.Mitraillette)
        {
            munRestante = muniMitraillette.munRestante;
        }
        
        if (currentWeapon == playerWeapon.FusilAssault)
        {
            munRestante = muniFA.munRestante;
        }
        
        if (currentWeapon == playerWeapon.Sniper)
        {
            munRestante = muniSniper.munRestante;
        }
        
        currentWeapon = weaponManager.GetCurrentWeapon(); // Met a jour en permanence les valeurs des armes 
        currentRange = currentWeapon[1];
        currentDamage = currentWeapon[0];
        currentFireRate = currentWeapon[2];
        tailleChargeur = currentWeapon[3];
        styleMun = currentWeapon[4];
        
        texteMunRestante.text = munRestante.ToString();
        texteTailleChargeur.text = tailleChargeur.ToString();

        keyReload = changeTouches.keyReload; // Recupere la touche de Reload dans le script ChangeTouches
        

        if (Input.GetKeyDown(keyReload)) // Si l'utilisateur appuye sur la touche de Reload
        {
            Reload(); // Appelle la fonction Reload plus bas
        }


        if (currentFireRate<= 0 || boolCut)
        {
            if (Input.GetButtonDown("Fire1")) // Si il appuye sur la touche pour tirer (deja defini dans Unity)
            {
                PreparationShoot();
            }
        }
        else
        {
            if (Input.GetButtonDown("Fire1")) // Si il appuye sur la touche pour tirer (deja defini dans Unity)
            {
                InvokeRepeating("PreparationShoot", 0f,1f/currentFireRate);
            }
            else if (Input.GetButtonUp("Fire1"))
            {
                CancelInvoke("PreparationShoot");
            }
        }
        
        
    }

    public void PreparationShoot()
    {
        if (playerController.moov) // On s'assure que le joueur a le droit de se deplacer et de tirer
        {
            if (boolCut)
            {
                photonView.RPC("Cut", RpcTarget.All);
            }
                
            else
            {
                if (munRestante>0) // Si il reste des balles dans le chargeur
                {
                    photonView.RPC("Shoot", RpcTarget.All, currentDamage,currentRange); // Appelle la fonction de tir plus bas
                    if (currentWeapon == playerWeapon.Pistolet)
                    {
                        munRestante = muniPistolet.munRestante -= 1;
                    }
        
                    if (currentWeapon == playerWeapon.Mitraillette)
                    {
                        munRestante = muniMitraillette.munRestante -=1;
                    }
        
                    if (currentWeapon == playerWeapon.FusilAssault)
                    {
                        munRestante = muniFA.munRestante -= 1;
                    }
        
                    if (currentWeapon == playerWeapon.Sniper)
                    {
                        muniSniper.munRestante -= 1;
                    } // Enleve une balle dans le chargeur
                }
            }
        }
    }

    [PunRPC]
    private void Shoot(float dammage, float range)
    {
        RaycastHit hit; // Defini un point de collision

        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, range, mask)) // Si il y a bien une collision sur l'objet souhaite
        {
            Debug.Log("Objet touché : "+hit.collider.name); // Affiche dans la Console la collision

            hit.transform.gameObject.GetComponent<PlayerLife>().life -= dammage; // Enleve la vie du GameObject qui a recu le hit
            string team = dontDestroy.GetComponent<DontDestroy>().team;
            int place = dontDestroy.GetComponent<DontDestroy>().place;
            hit.transform.gameObject.GetComponent<PlayerLife>().photonView.RPC("ATouche",RpcTarget.All,team,place);
            // La vie est recuperer grace au script PlayerLife. On lui enleve les degats que fait l'arme que possede le joueur qui a tire
            
            Vector3 pos = hit.point; // On recupere le point de collision sous forme de vecteur 
            Vector3 normal = hit.normal; // On normalise le vecteur du point d'impact
            GameObject hitEffect = Instantiate(impactBalle, pos, Quaternion.LookRotation(normal)); // Pour pouvoir faire spawn l'effet de hit a l'endroit voulu et dans le sens voulu
            Destroy(hitEffect,2f); // On detruit l'effet de hit apres deux secondes
        }

        if (currentWeapon == playerWeapon.Pistolet)
        {
            flareSmokePistolet.Play();
        }
        if (currentWeapon == playerWeapon.Mitraillette)
        {
            flareSmokeMitraillette.Play();
        }
        if (currentWeapon == playerWeapon.FusilAssault)
        {
            flareSmokeFA.Play();
        }
        if (currentWeapon == playerWeapon.Sniper)
        {
            flareSmokeSniper.Play();
        }
    }

    public void Reload()
    {
        if (styleMun == 1) // Regarde qu'elle style de mun doit etre utilise
        {
            if (currentWeapon == playerWeapon.Pistolet)
            {
                muniPistolet.Reload(4);
            }
            else
            {
                muniMitraillette.Reload(4);
            }
        }

        if (styleMun == 2)
        {
            muniFA.Reload(5);
        }

        if (styleMun == 3)
        {
            muniSniper.Reload(6);
        }
    }

    [PunRPC]
    private void Cut()
    {
        Debug.Log("Le personnage Cut");
        RaycastHit hit; // Defini un point de collision

        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, currentRange, mask)) // Si il y a bien une collision sur l'objet souhaite
        {
            Debug.Log("Objet touché : "+hit.collider.name); // Affiche dans la Console la collision

            hit.transform.gameObject.GetComponent<PlayerLife>().life -= currentDamage; // Enleve la vie du GameObject qui a recu le hit
            // La vie est recuperer grace au script PlayerLife. On lui enleve les degats que fait l'arme que possede le joueur qui a tire
        }
    }
}
