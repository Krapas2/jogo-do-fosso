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

    [ServerCallback]
    void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.gameObject == owner.gameObject || previouslyHit.Contains(other)){
            return;
        }

        if(other.gameObject.TryGetComponent<NetworkIdentity>(out NetworkIdentity otherIdentity)){
            Vector2 direction = transform.up;

            Knockback(otherIdentity, direction * knockback);
            Damage(otherIdentity, damage);
            previouslyHit.Add(other);
        }
    }

    void Damage(NetworkIdentity other, float damage)
    {
        if(other.gameObject.TryGetComponent<CharacterHealth>(out CharacterHealth characterHealth)){
            characterHealth.TakeDamage(damage);
        }
    }

    [Server]
    void Knockback(NetworkIdentity other, Vector2 vector)
    {
        if(other.gameObject.TryGetComponent<Rigidbody2D>(out Rigidbody2D rigidbody)){
            rigidbody.velocity = vector;
        }
    }
}
