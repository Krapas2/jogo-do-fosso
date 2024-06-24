using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Player : NetworkBehaviour
{
    [SyncVar]
    public float maxHealth;

    [SyncVar]
    //[HideInInspector]
    public float currentHealth;

    private PlayerMovement playerMovement;
    private PlayerRanged playerRanged;

    void Start()
    {
        currentHealth = maxHealth;
        if (isLocalPlayer){
            SetupLocalPlayer();
        } else {
            DisableNonLocalPlayer();
        }
    }

    void SetupLocalPlayer(){
        
        CameraController cameraController = FindObjectOfType<CameraController>();
        if(cameraController){
            cameraController.target = transform;
        }
    }

    void DisableNonLocalPlayer() 
    {
        if(TryGetComponent<PlayerMovement>(out playerMovement)){
            playerMovement.enabled = false;
        }
        if(TryGetComponent<PlayerRanged>(out playerRanged)){
            playerRanged.enabled = false;
        }
    }
    
    [ServerCallback]
    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        if (currentHealth = 0) {
            NetworkServer.Destroy(gameObject);
        }
    }
}
