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

    [HideInInspector]
    public Player owner;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update ()
    {
        rb.velocity = transform.up * speed;
    }
    
    [ServerCallback]
    void OnTriggerEnter2D(Collider2D other) 
    {
        bool otherIsPlayer = other.gameObject.TryGetComponent<Player>(out Player otherPlayer);
        if(!otherIsPlayer || otherPlayer == owner){
            return;
        }

        otherPlayer.TakeDamage(damage);
        DestroySelf();
    }

    [Server]
    void DestroySelf()
    {
        NetworkServer.Destroy(gameObject);
    }
}
