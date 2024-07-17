using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class ProtoPunch : NetworkBehaviour
{
    [SyncVar]
    public float damage;

    [HideInInspector]
    public Player owner;

    void OnTriggerEnter2D(Collider2D other) 
    {
        bool otherIsPlayer = other.gameObject.TryGetComponent<Player>(out Player otherPlayer);
        if(!otherIsPlayer || otherPlayer == owner){
            return;
        }

        Damage(otherPlayer);
    }

    [Command]
    void Damage(Player player)
    {
        player.TakeDamage(damage);
    }
}
