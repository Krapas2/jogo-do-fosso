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
    public float turningSpeed;

    [HideInInspector]
    [SyncVar]
    public Player owner;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        Move();
        Turn();
    }

    void Move()
    {
        rb.velocity = transform.up * speed;
    }

    void Turn()
    {
        Vector3 closestPlayerPosition = ClosestPlayerPosition();

        if(closestPlayerPosition.magnitude < Mathf.Infinity){
            Vector3 currentDirection = transform.up;
            Vector3 desiredDirection = closestPlayerPosition - transform.position;
            transform.up = Vector3.Lerp(currentDirection, desiredDirection, Time.deltaTime * turningSpeed);
        }
    }

    Vector3 ClosestPlayerPosition()
    {
        Player[] players = FindObjectsOfType<Player>();
        Vector3 closestPlayerPosition = Vector3.positiveInfinity;
        float mininumDistance = Mathf.Infinity;

        foreach (Player player in players)
        {
            float distance = Vector3.Distance(transform.position, player.transform.position);

            if (distance < mininumDistance && player != owner)
            {
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
        if (!otherIsPlayer || otherPlayer == owner)
        {
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
