using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : PlayerSkill
{

    public float speed = 5f;
    public float acceleration = .1f;

    private Rigidbody2D rb;

    protected override void Start()
    {
        base.Start();

        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        Accelerate();
    }

    void Accelerate()
    {
        Vector2 direction = new Vector2(
            Input.GetAxisRaw("Horizontal"),
            Input.GetAxisRaw("Vertical")
        ).normalized;

        rb.velocity = Vector2.Lerp(
            direction * speed,
            rb.velocity,
            Mathf.Pow(.5f, acceleration * Time.deltaTime)
        );
    }
}
