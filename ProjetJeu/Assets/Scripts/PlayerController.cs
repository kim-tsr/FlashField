using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class PlayerController : MonoBehaviourPunCallbacks
{
    public float speed = 3f;
    public float mouseSensitivity = 3f;
    public float boostSpeed = 1.5f;  

    private PlayerMotor motor;

    public ChangeTouches touches;

    private float xMov;
    private float zMov;

    public KeyCode keyAvancer;
    public KeyCode keyReculer;
    public KeyCode keyDroite;
    public KeyCode keyGauche;
    public KeyCode keyJump;
    public KeyCode keySlow;
    public KeyCode keyCut;
    public KeyCode keyArme1;
    public KeyCode keyArme2;
    public KeyCode keyDash;
    public KeyCode keyLancerGrenade;
    public KeyCode keyLancerGrenadeFum;

    public bool isJumping;
    public float jumpForce;

    public bool moov = true;

    public Canvas canvasInventaire;

    public BoutiqueManager boutiqueManager;
    public float[] currentWeapon1;
    public float[] currentWeapon2;

    public GameObject grenade;
    public GameObject grenadeFum;
    public float force;

    public bool canDash = true;

    public PlayerWeapon playerWeapon;
    public GameObject gmSniper;
    public GameObject gmFA;
    public GameObject gmMitraillette;

    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        motor = GetComponent<PlayerMotor>(); // Appelle le script PlayerMotor pour pouvoir y recuperer des elements
        
    }

    void Update()
    {
        if (moov && photonView.IsMine) // Si le mouvement est autoriser (pas dans un menu)
        {
            Deplacement();
        }
    }

    public void Deplacement()
    {
            keyAvancer = touches.keyAvancer; // Recupere les touches de deplacements
            keyReculer = touches.keyReculer;
            keyDroite = touches.keyDroite;
            keyGauche = touches.keyGauche;
            keyJump = touches.keyJump;
            keySlow = touches.keySlow;
            keyCut = touches.keyChangeCut;
            keyArme1 = touches.keyChangeArme1;
            keyArme2 = touches.keyChangeArme2;
            keyDash = touches.keyDash;
            keyLancerGrenade = touches.keyGrenade;
            keyLancerGrenadeFum = touches.keyGrenadeFumi;

            currentWeapon1 = boutiqueManager.currentWeapon1;
            currentWeapon2 = boutiqueManager.currentWeapon2;
            
            if (Input.GetKeyDown(keyCut)) // Si l'utilisateur demande a prendre son couteau
            {
                photonView.RPC("PrendreCut",RpcTarget.All);
            }

            if (Input.GetKeyDown(keyArme1) && currentWeapon1 != GetComponent<PlayerWeapon>().Vide) // Verifie en plus qu'il possede bien une arme
            {
                photonView.RPC("PrendreArme1",RpcTarget.All);
            }
            
            
            if (Input.GetKeyDown(keyArme2) && currentWeapon2 != GetComponent<PlayerWeapon>().Vide)
            {
                photonView.RPC("PrendreArme2",RpcTarget.All);
            }


            if (Input.GetKeyDown(keyJump)) // Si l'utilisateur demande a saute
            {
                isJumping = true; // Met le booleen a true, le traitement se fait plus bas
            }
            

            if (Input.GetKeyDown(keySlow)) // Si le joueur appuye pour Slow
            {
                speed = speed / 2f; // On reduit la vitesse de 2
            }
            
            if (Input.GetKeyUp(keySlow)) // Si il arrete de Slow
            {
                speed = speed * 2f; // On remet la vitesse comme avant
            }


            if (Input.GetKeyDown(keyDash) && canDash)
            {
                speed = speed * 5;
                canDash = false;
                StartCoroutine(Dash());
            }

            IEnumerator Dash()
            {
                yield return new WaitForSeconds((float) 0.75);
                StartCoroutine(CanDash());
                speed = speed / 5;
            }

            IEnumerator CanDash()
            {
                yield return new WaitForSeconds(5);
                canDash = true;
            }
            
            if (Input.GetKeyDown(keyLancerGrenade) && canvasInventaire.GetComponent<InventaireManager>().listeNbrInventaire[1] > 0)
            {
                canvasInventaire.GetComponent<InventaireManager>().listeNbrInventaire[1] -= 1;
                Vector3 gre = new Vector3(transform.position.x, transform.position.y,transform.position.z+1);
                GameObject Go = PhotonNetwork.Instantiate(grenade.name, gre, Quaternion.identity);
                Go.GetComponent<Rigidbody>().AddForce(transform.TransformDirection(Vector3.forward) * force);
            }
            
            if (Input.GetKeyDown(keyLancerGrenadeFum) && canvasInventaire.GetComponent<InventaireManager>().listeNbrInventaire[1] > 0)
            {
                canvasInventaire.GetComponent<InventaireManager>().listeNbrInventaire[1] -= 1;
                Vector3 gre = new Vector3(transform.position.x+1, transform.position.y,transform.position.z+1);
                GameObject Go = PhotonNetwork.Instantiate(grenadeFum.name, gre, Quaternion.identity);
                Go.GetComponent<Rigidbody>().AddForce(transform.TransformDirection(Vector3.forward) * force);
            }
            
            if (Input.GetKeyDown(keyAvancer)) // Deplacement sur les axes x et z
            {
                zMov = 1f;
            }
            if (Input.GetKeyUp(keyAvancer))
            {
                zMov = 0;
            }
            
            if (Input.GetKeyDown(keyReculer))
            {
                zMov = -1f;
            }
            if (Input.GetKeyUp(keyReculer))
            {
                zMov = 0;
            }
            
            if (Input.GetKeyDown(keyDroite))
            {
                xMov = 1f;
            }
            if (Input.GetKeyUp(keyDroite))
            {
                xMov = 0;
            }
            
            if (Input.GetKeyDown(keyGauche))
            {
                xMov = -1f;
            }
            if (Input.GetKeyUp(keyGauche))
            {
                xMov = 0;
            }
            
            Vector3 moveHorizontal = transform.right * xMov; // Creer un vecteur dans l'espace en focntion de la position du player et de la demande de le deplacer
            Vector3 moveVertical = transform.forward * zMov;
            
            Vector3 velocity = (moveHorizontal + moveVertical).normalized * (speed * boostSpeed); // Calcul la velocite a l'aide de la vitesse et des deux vecteurs precedents
            
            motor.Move(velocity); // Demande a faire le deplacement avec la fonction Move dans PlayerMotor
            
            float yRot = Input.GetAxisRaw("Mouse X"); // Recupere le deplacement de la souris sur l'axe y pour bouger la camera
            
            Vector3 rotation = new Vector3(0, yRot, 0) * (touches.floatSensi * 50); // Creer un vecteur a l'aide de la valeur yRot et de la sensibilité de la souris souhaité
            
            motor.Rotate(rotation); // Demande la rotation de la camera a l'aide de la fonction Rotate dans PlayerMotor

            float xRot = Input.GetAxisRaw("Mouse Y");
            
            Vector3 cameraRotation = new Vector3(xRot, 0, 0) * (touches.floatSensi * 10);
            
            motor.RotateCamera(cameraRotation);

            motor.Jump(isJumping,jumpForce); // Traitement du jump dans le script PlayerMotor
            /*isJumping = false;
            motor.Jump(isJumping,jumpForce);*/
    }

    [PunRPC]
    public void PrendreCut()
    {
        this.GetComponent<WeaponManager>().boolCut = true; // Met a jour les valeurs 
        this.GetComponent<WeaponManager>().boolArme1 = false;
        this.GetComponent<WeaponManager>().boolArme2 = false;
        boostSpeed = 1.5f;
    }

    [PunRPC]
    public void PrendreArme1()
    {
        this.GetComponent<WeaponManager>().boolCut = false;
        this.GetComponent<WeaponManager>().boolArme1 = true;
        this.GetComponent<WeaponManager>().boolArme2 = false;
                
        if (currentWeapon1 == playerWeapon.Mitraillette)
        {
            gmMitraillette.SetActive(true);
        }
        if (currentWeapon1 == playerWeapon.FusilAssault)
        {
            gmFA.SetActive(true);
        }
        if (currentWeapon1 == playerWeapon.Sniper)
        {
            gmSniper.SetActive(true);
        }
                
        boostSpeed = 1f;
    }

    [PunRPC]
    public void PrendreArme2()
    {
        this.GetComponent<WeaponManager>().boolCut = false;
        this.GetComponent<WeaponManager>().boolArme1 = false;
        this.GetComponent<WeaponManager>().boolArme2 = true;
        boostSpeed = 1f;
    }
}