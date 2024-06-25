using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

[RequireComponent(typeof(Rigidbody2D))]
public class ProtoDash : PlayerDash
{
    public float speedBoost;
    public float dashLength;
    public float accelerationWhileDashing;

    public float cooldown;

    private bool canDash;

    private PlayerMovement playerMovement;
    private Rigidbody2D rb;

    private CameraData cameraData;

    protected override void Start()
    {
        base.Start();

        playerMovement = GetComponent<PlayerMovement>();
        rb = GetComponent<Rigidbody2D>();
        cameraData = FindObjectOfType<CameraData>();

        canDash = true;
    }

    void Update()
    {
        if(Input.GetButton("Fire2") && canDash){
            Dash();
            StartCoroutine(Cooldown());
            StartCoroutine(Slide());
        }
    }

    void Dash()
    {
        bool running = rb.velocity.magnitude > 1f;
        Vector2 runDirection = rb.velocity;
        Vector2 lookDirection = cameraData.worldMousePosition - transform.position.Vector2();
        rb.velocity += (running ? runDirection : lookDirection).normalized * speedBoost;
    }

    IEnumerator Cooldown()
    {
        canDash = false;
        yield return new WaitForSeconds(cooldown);
        canDash = true;
    }

    IEnumerator Slide()
    {
        float originalAcceleration = playerMovement.acceleration;
        playerMovement.acceleration = accelerationWhileDashing;
        yield return new WaitForSeconds(dashLength);
        playerMovement.acceleration = originalAcceleration;
    }
}
