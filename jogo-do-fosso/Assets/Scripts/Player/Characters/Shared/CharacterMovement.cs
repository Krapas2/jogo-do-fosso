using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

[RequireComponent(typeof(Rigidbody2D))]
public class CharacterMovement : NetworkBehaviour
{
    [SyncVar]
    public float speed = 5f;
    [SyncVar]
    public float acceleration = .1f;

    private Rigidbody2D rb;

    void Start()
    {
        if (!isOwned){
            this.enabled = false;
        }

        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        Vector2 moveInput = new Vector2(
            Input.GetAxisRaw("Horizontal"),
            Input.GetAxisRaw("Vertical")
        ).normalized;

        CmdAccelerate(moveInput);
    }

    [Command]
    void CmdAccelerate(Vector2 direction)
    {
        rb.velocity = Vector2.Lerp(
            direction * speed,
            rb.velocity,
            Mathf.Pow(.5f, acceleration * Time.deltaTime)
        );
    }
}
