using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grap : MonoBehaviour
{
    private Canvas canvasInventaire;

    public void OnTriggerEnter(Collider other) // Le GameObject qui a le script a un sphere collider qui est un trigger
    {
        if (other.gameObject.layer == LayerMask.NameToLayer ("Player")) // Lorsqu'un GameObject avec le layer Player entre dans le trigger
        {
            canvasInventaire = other.gameObject.GetComponent<PlayerController>().canvasInventaire; // On cherche le Canvas canvasInventaire dans le player
            canvasInventaire.GetComponent<InventaireManager>().TakeObject(this.gameObject); // Pour appeler la fonction TakeObject pour que le joueur puisse prendre le GameObject si il le souhaite
        }
    }
}
