using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

[RequireComponent(typeof(Rigidbody2D))]
public class ProtoSpecialProjectile : NetworkBehaviour
{
    [SyncVar]
    public float damage;
    [SyncVar]
    public float speed;
    [SyncVar]
    public float acceleration;
    public LayerMask collide;

    [HideInInspector]
    [SyncVar]
    public Player owner;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        rb.velocity = transform.up * speed;
    }

    void Update()
    {
        Accelerate();
        Turn();
    }

    void Accelerate()
    {
        Vector3 closestPlayerPosition = ClosestPlayerPosition();

        if(closestPlayerPosition.magnitude != Mathf.Infinity){
            rb.velocity = Vector2.Lerp(
                (closestPlayerPosition - transform.position).normalized * speed,
                rb.velocity,
                Mathf.Pow(.5f, acceleration * Time.deltaTime)
            );
        }
    }

    void Turn()
    {
        transform.up = rb.velocity.Vector3() + transform.position;
    }

    Vector3 ClosestPlayerPosition()
    {
        Player[] players = FindObjectsOfType<Player>();
        Vector3 closestPlayerPosition = Vector3.positiveInfinity;
        float mininumDistance = Mathf.Infinity;

        foreach (Player player in players){
            float distance = Vector3.Distance(transform.position, player.transform.position);

            if (distance < mininumDistance && player != owner){
                closestPlayerPosition = player.transform.position;
                mininumDistance = distance;
            }
        }

        return closestPlayerPosition;
    }

    [ServerCallback]
    void OnTriggerEnter2D(Collider2D other)
    {
        bool otherIsPlayer = other.gameObject.TryGetComponent<Player>(out Player otherPlayer);
        if (!otherIsPlayer || otherPlayer == owner){
            return;
        }

        otherPlayer.TakeDamage(damage);
        DestroySelf();
    }

    [ServerCallback]
    void OnTriggerStay2D(Collider2D other) 
    {
        if(collide.Includes(other.gameObject)){
            DestroySelf();
        }
    }

    [Server]
    void DestroySelf()
    {
        NetworkServer.Destroy(gameObject);
    }
}
