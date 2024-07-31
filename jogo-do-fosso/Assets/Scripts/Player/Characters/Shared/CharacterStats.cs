using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Character : NetworkBehaviour
{
    [SyncVar]
    public float maxHealth;
    [SyncVar]
    public float currentHealth;

    [HideInInspector]
    [SyncVar]
    public PlayerManager manager;

    void Start()
    {
        currentHealth = maxHealth;
        if (isOwned) {
            SetupLocalPlayer();
        }
    }

    void SetupLocalPlayer(){
        CameraController cameraController = FindObjectOfType<CameraController>();
        if(cameraController){
            cameraController.target = transform;
        }

        manager.currentCharacter = this;
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
