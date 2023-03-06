using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MunitionArme : MonoBehaviour
{
    public float munRestante;
    public float tailleChargeur;
    public GameObject canvasInventaire;
    public GameObject player;


    public void Update()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        munRestante = player.GetComponent<PlayerShoot>().munRestante;
    }

    public void Reload(int pos)
    {
        canvasInventaire = GameObject.FindWithTag("Inventaire");

        if (munRestante<tailleChargeur)
        {
            if (munRestante == 0)
            {
                if (canvasInventaire.GetComponent<InventaireManager>().listeNbrInventaire[pos] < tailleChargeur)
                {
                    Debug.Log("Reload : munRestante = 0 et listeInventaire < taille chargeur");
                    munRestante = canvasInventaire.GetComponent<InventaireManager>().listeNbrInventaire[pos];
                    canvasInventaire.GetComponent<InventaireManager>().listeNbrInventaire[pos] = 0;
                }
                else
                {
                    Debug.Log("Reload : munRestante = 0 et listeInventaire >= taille chargeur");
                    munRestante = tailleChargeur;
                    canvasInventaire.GetComponent<InventaireManager>().listeNbrInventaire[pos] -= tailleChargeur;
                }
            }
            else
            {
                if ((munRestante + canvasInventaire.GetComponent<InventaireManager>().listeNbrInventaire[pos]) <= tailleChargeur)
                {
                    Debug.Log("Reload : munRestante + ListeInventaire <= taille chargeur");
                    munRestante += canvasInventaire.GetComponent<InventaireManager>().listeNbrInventaire[pos];
                    canvasInventaire.GetComponent<InventaireManager>().listeNbrInventaire[pos] = 0;
                }
                else
                {
                    Debug.Log("Reload : munRestante + ListeInventaire > taille chargeur");
                    float tmp = tailleChargeur - munRestante;
                    munRestante = tailleChargeur;
                    canvasInventaire.GetComponent<InventaireManager>().listeNbrInventaire[pos] -= tmp;
                }
            }
        }
    }
}
