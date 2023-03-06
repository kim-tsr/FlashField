using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    public float[] Vide = new float[5];
    public float[] Pistolet = new[] {10f, 100f, 2f, 10f, 1f}; // Liste des valeurs d'une arme 
    public float[] Mitraillette = new[] {20f, 250f, 5f, 20f,1f}; 
    public float[] FusilAssault = new[] {40f, 500f, 10f, 30f,2f}; 
    public float[] Sniper = new[] {100f, 1000f, 1f, 5f,3f};
    public float[] Cut = new[] {34f,5f,0f,1f,0f};

    /*
     * arme[0] = damage
     * arme[1] = range
     * arme[2] = fireRate
     * arme[3] = tailleChargeur
     * arme[4] = style de balle (0 = pas de balle , 1 = mitraillette, 2 = fusil d'assault , 3 = sniper)
     * arme[5] = 
     * arme[6] =
     * arme[7] =
     */
}
