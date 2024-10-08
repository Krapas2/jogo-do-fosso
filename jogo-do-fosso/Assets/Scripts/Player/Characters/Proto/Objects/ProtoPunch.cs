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
    public Character owner;

    private CameraData cameraData;

    void Start(){
        if (isOwned) {
            cameraData = FindObjectOfType<CameraData>();
        }
    }

    void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.gameObject == owner.gameObject){
            return;
        }


        if(other.gameObject.TryGetComponent<Character>(out Character otherCharacter)){
            CmdDamage(otherCharacter);
        }

        if(other.gameObject.TryGetComponent<NetworkIdentity>(out NetworkIdentity otherIdentity)){
            Vector2 direction = (cameraData.worldMousePosition - transform.position.Vector2()).normalized;
            if(otherIdentity.connectionToClient != connectionToClient){
                TargetKnockback(otherIdentity.connectionToClient, otherIdentity, direction * knockback);
            }else{
                OwnedKnockback(otherIdentity, direction * knockback);
            }
        }
    }

    [Command]
    void CmdDamage(Character character)
    {
        character.TakeDamage(damage);
    }

    [TargetRpc]
    void TargetKnockback(NetworkConnectionToClient target, NetworkIdentity other, Vector2 vector)
    {
        if(other.gameObject.TryGetComponent<Rigidbody2D>(out Rigidbody2D rigidbody)){
            rigidbody.velocity = vector;
        }
    }

    void OwnedKnockback(NetworkIdentity other, Vector2 vector)
    {
        if(other.gameObject.TryGetComponent<Rigidbody2D>(out Rigidbody2D rigidbody)){
            rigidbody.velocity = vector;
        }
    }
}
