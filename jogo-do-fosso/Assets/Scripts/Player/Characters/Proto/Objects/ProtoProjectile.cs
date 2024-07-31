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
    public LayerMask collide;

    [HideInInspector]
    public Character owner;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        Invoke(nameof(DestroySelf), lifeTime);
    }

    void Update ()
    {
        rb.velocity = transform.up * speed;
    }
    
    [ServerCallback]
    void OnTriggerEnter2D(Collider2D other) 
    {
        bool otherIsCharacter = other.gameObject.TryGetComponent<Character>(out Character otherCharacter);
        if(!otherIsCharacter || otherCharacter == owner){
            return;
        }

        otherCharacter.TakeDamage(damage);
        DestroySelf();
    }
    
    [ServerCallback]
    void OnTriggerStay2D(Collider2D other) 
    {
        if(collide.Includes(other.gameObject)){
            DestroySelf();
        }
    }

    [ServerCallback]
    void DestroySelf()
    {
        NetworkServer.Destroy(gameObject);
    }
}
