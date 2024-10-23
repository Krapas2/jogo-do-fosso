using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class CharacterHealth : NetworkBehaviour
{
    [SyncVar]
    public float maxHealth;
    [SyncVar]
    [HideInInspector]
    public float currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
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
