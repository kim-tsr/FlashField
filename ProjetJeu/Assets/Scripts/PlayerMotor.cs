using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMotor : MonoBehaviour
{
    public Camera cam;
    public PlayerController playerController;
    
    private Vector3 velocity;
    private Vector3 rotation;
    private Vector3 cameraRotation;

    private bool isJumping;
    private float jumpForce;

    public Transform groundCheck;
    public float radius;
    public LayerMask collisionLayers;

    public bool isGrounded;

    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>(); // Recupere le RigidBoby du player pour pouvoir lui appliquer des forces et le faire se deplacer
    }

    public void Move(Vector3 _velocity)
    {
        velocity = _velocity; // Recupere la velocite grace a l'appelle dans PlayerController
    }

    public void Jump(bool _isJumping , float _jumpForce)
    {
        isJumping = _isJumping; // Recupere l'autorisation de saut grace a l'appelle dans PlayerController
        jumpForce = _jumpForce; // Recupere la force du saut grace a l'appelle dans PlayerController
    }
    
    public void Rotate(Vector3 _rotation)
    {
        rotation = _rotation; // Recupere la rotation de la camera grace a l'appelle dans PlayerController
    }
    
    public void RotateCamera(Vector3 _cameraRotation)
    {
        cameraRotation = _cameraRotation;
    }

    private void FixedUpdate() // Permet de faire des deplacements plus fluide qu'avec Update
    {
        PerformMovement(); // Appelle des fonctions de deplacement, de rotation et de saut
        PerformRotation();
        PerformJump();  
    }

    private void PerformMovement()
    {
        if (velocity != Vector3.zero)
        {
            rb.MovePosition(rb.position + velocity * Time.fixedDeltaTime); // Applique le deplacement en fonction du temps
        }
    }
    
    private void PerformRotation()
    {
        rb.MoveRotation(rb.rotation * Quaternion.Euler(rotation)); // Applique la rotation
        cam.transform.Rotate(-cameraRotation);
    }

    private void PerformJump()
    {   
        if (Physics.OverlapSphere(groundCheck.position,radius,collisionLayers).Length > 0) // Regarde avec combien d'elements le cercle en dessous du joueur est en contact
        {
            isGrounded = true; // Si c'est avec plus que 0, c'est qu'il est au sol, et donc il peu saute
        }
        else
        {
            isGrounded = false; // Sinon c'est qu'il est en l'air donc impossible pout lui de saute
        }
 
        if (isJumping && isGrounded) // Si l'utilisateur demande le saut et que le joueur est au sol
        {
            rb.AddForce(new Vector3(0f,jumpForce,0f)); // On applique au joueur la force pour qu'il puisse saute
            playerController.GetComponent<PlayerController>().isJumping = false; // Remet le booleen a false pour indiquer que le saut est fait pour arreter la demande
        }
    }

    private void OnDrawGizmos() // Permet de dessiner le cercle en dessous du joueur pour savoir si il touche le sol
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundCheck.position,radius);
    }
}
