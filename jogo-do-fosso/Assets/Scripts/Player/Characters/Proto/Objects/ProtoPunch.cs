using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Mirror.Experimental;

public class ProtoPunch : NetworkBehaviour
{
    [SyncVar]
    public float damage;
    [SyncVar]
    public float knockback;

    [HideInInspector]
    [SyncVar]
    public Character owner;

    private CameraData cameraData;

    void Update()
    {
        CheckOwner();
    }

    [ServerCallback]
    void CheckOwner()
    {
        if (!owner) {
            NetworkServer.Destroy(gameObject);
        }
    }

    [ServerCallback]
    void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.gameObject == owner.gameObject){
            return;
        }


        if(other.gameObject.TryGetComponent<NetworkIdentity>(out NetworkIdentity otherIdentity)){
            Vector2 direction = transform.up;
            TargetKnockback(otherIdentity.connectionToClient, otherIdentity, direction * knockback);

            TargetDamage(otherIdentity.connectionToClient, otherIdentity, damage);
        }
        if(other.gameObject.TryGetComponent<Character>(out Character otherCharacter)){
        }
    }

    [TargetRpc]
    void TargetDamage(NetworkConnectionToClient target, NetworkIdentity other, float damage)
    {
        if(other.gameObject.TryGetComponent<Character>(out Character character)){
            character.TakeDamage(damage);
        }
    }

    [TargetRpc]
    void TargetKnockback(NetworkConnectionToClient target, NetworkIdentity other, Vector2 vector)
    {
        if(other.gameObject.TryGetComponent<Rigidbody2D>(out Rigidbody2D rigidbody)){
            rigidbody.velocity = vector;
        }
    }
}
