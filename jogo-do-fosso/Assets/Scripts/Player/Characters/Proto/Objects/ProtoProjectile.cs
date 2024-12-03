using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

[RequireComponent(typeof(Rigidbody2D))]
public class ProtoProjectile : NetworkBehaviour
{
    [SyncVar]
    public float damage;
    [SyncVar]
    public float speed;
    [SyncVar]
    public float lifeTime;
    [SyncVar]
    public float overrideSentryTargetLength;
    [SyncVar]
    public LayerMask ignoreDamage;
    [SyncVar]
    public LayerMask ignoreOverrideSentryTarget;
    [SyncVar]
    public LayerMask destroyOnTouch;

    [HideInInspector]
    [SyncVar]
    public ProtoRanged owner;
    [HideInInspector]
    [SyncVar]
    public ProtoSentrySpawner protoSentrySpawner;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = transform.up * speed;

        if(isOwned){
            Invoke(nameof(CmdDestroySelf), lifeTime);
        }
    }

    void Update ()
    {
        transform.up = rb.velocity;
    }
    
    [ClientCallback]
    void OnTriggerEnter2D(Collider2D other) 
    {
        if(!isOwned){
            return;
        }

        bool otherIsOwner = owner && other.gameObject == owner.gameObject;
        if(otherIsOwner){
            return;
        }
        
        if(other.gameObject.TryGetComponent<NetworkIdentity>(out NetworkIdentity otherIdentity)){
            Vector2 direction = transform.up;

            bool otherIgnoresDamage = ignoreDamage.Includes(other.gameObject);
            if(!otherIgnoresDamage){
                Damage(otherIdentity, damage);
            }

            bool otherIgnoresOverrideSentryTarget = ignoreOverrideSentryTarget.Includes(other.gameObject);
            bool currentSentryExists = protoSentrySpawner.currentSentry != null;
            if(!otherIgnoresOverrideSentryTarget && currentSentryExists){
                protoSentrySpawner.currentSentry.OverrideTarget(other.transform, overrideSentryTargetLength);
            }
        }

        if(destroyOnTouch.Includes(other.gameObject)){
            Invoke(nameof(CmdDestroySelf),.01f);
        }
    }

    [Command]
    void Damage(NetworkIdentity other, float damage)
    {
        if(other && other.gameObject.TryGetComponent<CharacterHealth>(out CharacterHealth characterHealth)){
            characterHealth.TakeDamage(damage);
        }
    }

    [Command]
    void CmdDestroySelf()
    {
        NetworkServer.Destroy(gameObject);
    }
}
