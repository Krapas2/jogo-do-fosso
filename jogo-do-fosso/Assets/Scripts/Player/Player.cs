using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Player : NetworkBehaviour
{
    [SyncVar]
    public float maxHealth;
    [SyncVar]
    public float currentHealth;

    private PlayerMovement playerMovement;
    private PlayerRanged playerRanged;

    void Start()
    {
        currentHealth = maxHealth;
        if (isLocalPlayer) {
            SetupLocalPlayer();
        }
    }

    void SetupLocalPlayer(){
        
        CameraController cameraController = FindObjectOfType<CameraController>();
        if(cameraController){
            cameraController.target = transform;
        }
    }

    [ServerCallback]
    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0) {
            NetworkServer.Destroy(gameObject);
        }
    }
}
