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
    public Character owner;
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
        Character[] characters = FindObjectsOfType<Character>();
        Vector3 closestCharacterPosition = Vector3.positiveInfinity;
        float mininumDistance = Mathf.Infinity;

        foreach (Character character in characters){
            float distance = Vector3.Distance(transform.position, character.transform.position);

            if (distance < mininumDistance && character != owner){
                closestCharacterPosition = character.transform.position;
                mininumDistance = distance;
            }
        }

        return closestCharacterPosition;
    }

    [ServerCallback]
    void OnTriggerEnter2D(Collider2D other)
    {
        bool otherIsCharacter = other.gameObject.TryGetComponent<Character>(out Character otherCharacter);
        if (!otherIsCharacter || otherCharacter == owner){
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

    [Server]
    void DestroySelf()
    {
        NetworkServer.Destroy(gameObject);
    }
}
