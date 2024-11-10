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
    [SyncVar]
    public float lifeTime;

    [SyncVar]
    public LayerMask ignoreDamage;
    [SyncVar]
    public LayerMask ignoreKnockback;

    [HideInInspector]
    [SyncVar]
    public ProtoMelee owner;

    private CameraData cameraData;

    private List<Collider2D> previouslyHit = new List<Collider2D>();

    void Start()
    {
        // isOwned refers to the client this is running in having authority
        // if isOwned is true, owner is also owned and ProtoMelee might be enabled
        // otherwise checkOwner will always run DestroySelf
        if(isOwned){
            StartCoroutine(WaitForOwnerDeath());
            Invoke(nameof(CmdDestroySelf), lifeTime);
        }
    }

    [Client]
    IEnumerator WaitForOwnerDeath()
    {
        yield return new WaitUntil(CheckOwner);
        CmdDestroySelf(); 
    }

    bool CheckOwner()
    {
        return !owner || !owner.enabled;
    }

    [Command]
    public void CmdDestroySelf()
    {
        if(gameObject){
            NetworkServer.Destroy(gameObject);
        }
    }

    [ClientCallback]
    void OnTriggerEnter2D(Collider2D other) 
    {
        if(!isOwned){
            return;
        }
        
        bool otherIsOwner = other.gameObject == owner.gameObject || previouslyHit.Contains(other);
        bool otherHasBeenHit = previouslyHit.Contains(other);
        if(otherIsOwner || otherHasBeenHit){
            return;
        }

        if(other.gameObject.TryGetComponent<NetworkIdentity>(out NetworkIdentity otherIdentity)){
            Vector2 direction = transform.up;

            if(!ignoreKnockback.Includes(other.gameObject)){
                Knockback(otherIdentity, direction * knockback);
            }
            if(!ignoreDamage.Includes(other.gameObject)){
                Damage(otherIdentity, damage);
            }
            previouslyHit.Add(other);
        }
    }

    [Command]
    void Damage(NetworkIdentity other, float damage)
    {
        if(other.gameObject.TryGetComponent<CharacterHealth>(out CharacterHealth characterHealth)){
            characterHealth.TakeDamage(damage);
        }
    }

    [Command]
    void Knockback(NetworkIdentity other, Vector2 vector)
    {
        if(other.gameObject.TryGetComponent<Rigidbody2D>(out Rigidbody2D rigidbody)){
            rigidbody.velocity = vector;
        }
    }
}
