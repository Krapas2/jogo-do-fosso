using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

[RequireComponent(typeof(Rigidbody2D))]
public class SentryProjectile : NetworkBehaviour
{
    [SyncVar]
    public float damage;
    [SyncVar]
    public float speed;
    [SyncVar]
    public float lifeTime;
    [SyncVar]
    public LayerMask ignore;
    [SyncVar]
    public LayerMask destroyOnTouch;
    
    [HideInInspector]
    public ProtoSentry owner;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        if(isOwned){
            Invoke(nameof(CmdDestroySelf), lifeTime);
        }
    }

    void Update ()
    {
        rb.velocity = transform.up * speed;
    }
    
    [ClientCallback]
    void OnTriggerEnter2D(Collider2D other) 
    {
        if(!isOwned){
            return;
        }

        bool otherIsOwner = owner && other.gameObject == owner.gameObject;
        bool otherIsIgnored = ignore.Includes(other.gameObject);
        if(otherIsOwner || otherIsIgnored){
            return;
        }

        if(other.gameObject.TryGetComponent<NetworkIdentity>(out NetworkIdentity otherIdentity)){
            Vector2 direction = transform.up;

            Damage(otherIdentity, damage);
        }
    }

    [ServerCallback]
    void OnTriggerStay2D(Collider2D other)
    {
        bool otherIsOwner = owner && other.gameObject == owner.gameObject;
        if(otherIsOwner){
            return;
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
