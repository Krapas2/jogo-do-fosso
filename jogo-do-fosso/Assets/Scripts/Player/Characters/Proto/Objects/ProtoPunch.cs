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
            Debug.Log(otherIdentity.connectionToClient);

            Vector2 direction = transform.up;

            if(!otherIdentity.isServer){
                TargetKnockback(otherIdentity.connectionToClient, otherIdentity, direction * knockback);
            }else{
                Knockback(otherIdentity, direction * knockback);
            }
            
            Damage(otherIdentity, damage);
        }
    }

    [Server]
    void Damage(NetworkIdentity other, float damage)
    {
        if(other.gameObject.TryGetComponent<Character>(out Character character)){
            character.TakeDamage(damage);
        }
    }

    //Client has authority over its rigidbody, so the function must be called there
    [TargetRpc]
    void TargetKnockback(NetworkConnectionToClient target, NetworkIdentity other, Vector2 vector)
    {
        Debug.Log("madeitknockOut");
        if(other.gameObject.TryGetComponent<Rigidbody2D>(out Rigidbody2D rigidbody)){
            rigidbody.velocity = vector;
        }
    }

    [Server]
    void Knockback(NetworkIdentity other, Vector2 vector)
    {
        Debug.Log("madeitknockIn");
        if(other.gameObject.TryGetComponent<Rigidbody2D>(out Rigidbody2D rigidbody)){
            rigidbody.velocity = vector;
        }
    }
}
