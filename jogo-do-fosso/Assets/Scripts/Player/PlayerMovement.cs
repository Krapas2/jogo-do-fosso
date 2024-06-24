using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerMovement : NetworkBehaviour
{

    public float speed = 5f;
    public float acceleration = .1f;

    [SyncVar]
    [HideInInspector]
    public Vector2 velocity;

    void Start()
    {
        if (!isLocalPlayer)
        {
            this.enabled = false;
        }
        velocity = Vector2.zero;
    }

    void Update()
    {
        Accelerate();
        Move();
    }

    void Accelerate()
    {
        Vector2 direction = new Vector2(
            Input.GetAxisRaw("Horizontal"),
            Input.GetAxisRaw("Vertical")
        ).normalized;

        velocity = Vector2.Lerp(
            direction * speed,
            velocity,
            Mathf.Pow(.5f, acceleration * Time.deltaTime)
        );
    }

    void Move()
    {
        Vector3 translation = velocity * Time.deltaTime;
        transform.Translate(translation);
    }
}
