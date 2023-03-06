using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoutiqueManager : MonoBehaviour
{
    public float[] currentWeapon1;
    public PlayerWeapon playerWeapon;
    public float currentRange1;
    public float currentDamage1;
    public float currentFireRate1;
    public float currentTailleChargeur1;
    
    public float[] currentWeapon2;
    public float currentRange2;
    public float currentDamage2;
    public float currentFireRate2;
    public float currentTailleChargeur2;

    public PlayerShoot playerShoot;
    
    public bool boolPistolet = false;
    public bool boolMitraillette = false;
    public bool boolFusilAssault = false;
    public bool boolSniper = false;

    public GameObject gmPistolet;
    public GameObject gmSniper;
    public GameObject gmFA;
    public GameObject gmMitraillette;

    public bool PossessionArme1 = false;
    public bool PossessionArme2 = false;

    public InventaireManager inventaireManager;

    public GameObject pistolet;
    public GameObject mitraillette;
    public GameObject Fa;
    public GameObject sniper;


    public PlayerController playerController;

    private void Start()
    {
        currentWeapon1 = playerWeapon.Vide; // Le joueur commence seulement avec un couteau donc ces autres armes sont vides (il n'en a pas)
        currentWeapon2 = playerWeapon.Vide;
    }

    void Update()
    {
        if (boolPistolet && !PossessionArme2) // Si on souhaite changer d'arme le booleen se met a true
        {
            BuyPistolet();
        }
        
        if (boolMitraillette && !PossessionArme1)
        {
            BuyMitraillette();
        }
        
        if (boolFusilAssault && !PossessionArme1)
        {
            BuyFa();
        }
        
        if (boolSniper && !PossessionArme1)
        {    
            BuySniper();
        }

        if (inventaireManager.GetComponent<InventaireManager>().dropArme1)
        {
            currentWeapon1 = playerWeapon.Vide;
            currentDamage1 = playerWeapon.Vide[0];
            currentRange1 = playerWeapon.Vide[1];
            currentFireRate1 = playerWeapon.Vide[2];
            currentTailleChargeur1 = playerWeapon.Vide[3];
            PossessionArme1 = false;
            playerController.PrendreCut();
            inventaireManager.GetComponent<InventaireManager>().dropArme1 = false;
        }
        
        if (inventaireManager.GetComponent<InventaireManager>().dropArme2)
        {
            currentWeapon2 = playerWeapon.Vide;
            currentDamage2 = playerWeapon.Vide[0];
            currentRange2 = playerWeapon.Vide[1];
            currentFireRate2 = playerWeapon.Vide[2];
            currentTailleChargeur2 = playerWeapon.Vide[3];
            PossessionArme2 = false;
            playerController.PrendreCut();
            inventaireManager.GetComponent<InventaireManager>().dropArme2 = false;
        }
    }

    public void ChangePistolet()
    {
        boolPistolet = true; // Change le booleen du Pistolet pour changer son arme actuel en pistolet et pour pouvoir faire les changements de variables
        boolMitraillette = false; // Pour s'assurer que les autres booleens sont bien a false
        boolFusilAssault = false;
        boolSniper = false;
    }
    
    public void ChangeMitraillette()
    {
        boolMitraillette = true;
        boolPistolet = false;
        boolFusilAssault = false;
        boolSniper = false;
    }
    
    public void ChangeFusilAssault()
    {
        boolFusilAssault = true;
        boolMitraillette = false;
        boolPistolet = false;
        boolSniper = false;
    }
    
    public void ChangeSniper()
    {
        boolSniper = true;
        boolMitraillette = false;
        boolFusilAssault = false;
        boolPistolet = false;
    }

    public void BuyPistolet()
    {
        gmPistolet.GetComponent<SphereCollider>().enabled = false;
        currentWeapon2 = playerWeapon.Pistolet; // Change l'arme actuel en Pistolet et toutes les valeurs sont changées
        currentDamage2 = playerWeapon.Pistolet[0];
        currentRange2 = playerWeapon.Pistolet[1];
        currentFireRate2 = playerWeapon.Pistolet[2];
        currentTailleChargeur2 = playerWeapon.Pistolet[3];
        playerShoot.GetComponent<PlayerShoot>().munRestante = currentTailleChargeur2;
        boolPistolet = false; // Remet le booleen a false pour que le changement ne se fasse pas en permanence
        PossessionArme2 = true;
        inventaireManager.GetComponent<InventaireManager>().arme2 = pistolet;
        inventaireManager.GetComponent<InventaireManager>().listeNbrInventaire[3]=1;
    }
    
    public void BuyMitraillette()
    {
        gmMitraillette.SetActive(true);
        gmMitraillette.GetComponent<SphereCollider>().enabled = false;
        currentWeapon1 = playerWeapon.Mitraillette;
        currentDamage1 = playerWeapon.Mitraillette[0];
        currentRange1 = playerWeapon.Mitraillette[1];
        currentFireRate1 = playerWeapon.Mitraillette[2];
        currentTailleChargeur1 = playerWeapon.Mitraillette[3];
        playerShoot.GetComponent<PlayerShoot>().munRestante = currentTailleChargeur1;
        boolMitraillette = false;
        PossessionArme1 = true;
        inventaireManager.GetComponent<InventaireManager>().arme1 = mitraillette;
        inventaireManager.GetComponent<InventaireManager>().listeNbrInventaire[2]=1;
    }
    
    public void BuyFa()
    {
        gmFA.SetActive(true);
        gmFA.GetComponent<SphereCollider>().enabled = false;
        currentWeapon1 = playerWeapon.FusilAssault;
        currentDamage1 = playerWeapon.FusilAssault[0];
        currentRange1 = playerWeapon.FusilAssault[1];
        currentFireRate1 = playerWeapon.FusilAssault[2];
        currentTailleChargeur1 = playerWeapon.FusilAssault[3];
        playerShoot.GetComponent<PlayerShoot>().munRestante = currentTailleChargeur1;
        boolFusilAssault = false;
        PossessionArme1 = true;
        inventaireManager.GetComponent<InventaireManager>().arme1 = Fa;
        inventaireManager.GetComponent<InventaireManager>().listeNbrInventaire[2]=1;
    }
    
    public void BuySniper()
    {
        gmSniper.SetActive(true);
        gmSniper.GetComponent<SphereCollider>().enabled = false;
        currentWeapon1 = playerWeapon.Sniper;
        currentDamage1 = playerWeapon.Sniper[0];
        currentRange1 = playerWeapon.Sniper[1];
        currentFireRate1 = playerWeapon.Sniper[2];
        currentTailleChargeur1 = playerWeapon.Sniper[3];
        playerShoot.GetComponent<PlayerShoot>().munRestante = currentTailleChargeur1;
        boolSniper = false;
        PossessionArme1 = true;
        inventaireManager.GetComponent<InventaireManager>().arme1 = sniper;
        inventaireManager.GetComponent<InventaireManager>().listeNbrInventaire[2]=1;
    }
}
