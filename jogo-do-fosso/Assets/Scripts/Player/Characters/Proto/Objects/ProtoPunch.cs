using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class ProtoPunch : NetworkBehaviour
{
    [SyncVar]
    public float damage;

    [HideInInspector]
    public Character owner;

    void OnTriggerEnter2D(Collider2D other) 
    {
        bool otherIsCharacter = other.gameObject.TryGetComponent<Character>(out Character otherCharacter);
        if(!otherIsCharacter || otherCharacter == owner){
            return;
        }

        Damage(otherCharacter);
    }

    [Command]
    void Damage(Character character)
    {
        character.TakeDamage(damage);
    }
}
