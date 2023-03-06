using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public float[] primaryWeapon;

    private float[] currentWeapon1;
    private float[] currentWeapon2;
    private PlayerWeapon currentGraphics;

    public PlayerWeapon cut;

    public BoutiqueManager boutiqueManager;

    public Transform weaponHolder;

    public string weaponLayerName = "Weapon";

    public bool boolCut = true;
    public bool boolArme1 = false;
    public bool boolArme2 = false;
    public GameObject objectArme1;
    public GameObject objectArme2;
    public GameObject objectCut;

    private void Start()
    {
        EquipWeapon(primaryWeapon);
        currentWeapon1 = boutiqueManager.currentWeapon1;
        currentWeapon2 = boutiqueManager.currentWeapon2;
        boolCut = true;
    }

    public float[] GetCurrentWeapon()
    {
        if (boolCut)
        {
            objectArme1.SetActive(false);
            objectArme2.SetActive(false);
            objectCut.SetActive(true);
            GetComponent<PlayerShoot>().boolCut = true;
            return cut.Cut;
        }

        if (boolArme1 && boutiqueManager.currentWeapon1 != GetComponent<PlayerWeapon>().Vide)
        {
            objectArme1.SetActive(true);
            objectArme2.SetActive(false);
            objectCut.SetActive(false);
            GetComponent<PlayerShoot>().boolCut = false;
            return boutiqueManager.currentWeapon1;
        }

        if (boolArme2 && boutiqueManager.currentWeapon2 != GetComponent<PlayerWeapon>().Vide)
        {
            objectArme1.SetActive(false);
            objectArme2.SetActive(true);
            objectCut.SetActive(false);
            GetComponent<PlayerShoot>().boolCut = false;
            return boutiqueManager.currentWeapon2;
        }
        return cut.Cut;

    }
    
    public PlayerWeapon GetCurrentGraphics()
    {
        return currentGraphics;
    }

    void EquipWeapon(float[] _weapon)
    {
        currentWeapon1 = _weapon;
        
        /*

        GameObject weaponIns = Instantiate(_weapon.graphics, weaponHolder.position, weaponHolder.rotation);
        weaponIns.transform.SetParent(weaponHolder);

        currentGraphics = weaponIns.GetComponent<WeaponGraphics>();

        if (currentGraphics == null)
        {
            Debug.LogError("Pas de script WeaponGraphics sur l'arme : "+weaponIns.name);
        }*/
    }
}



/*"com.unity.package-manager-ui": "2.1.2",*/
