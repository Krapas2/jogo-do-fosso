using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

[RequireComponent(typeof(Rigidbody2D))]
public class ProtoDash : CharacterSkill
{
    public float speedBoost;
    public float dashLength;
    public float accelerationWhileDashing;

    private CharacterMovement characterMovement;
    private Rigidbody2D rb;

    private CameraData cameraData;

    protected override void Start()
    {
        base.Start();
        
        characterMovement = GetComponent<CharacterMovement>();
        rb = GetComponent<Rigidbody2D>();
        
        cameraData = FindObjectOfType<CameraData>();
    }

    void Update()
    {
        if(Input.GetButton("Fire2") && canUse){
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

    IEnumerator Slide()
    {
        float originalAcceleration = characterMovement.acceleration;
        characterMovement.acceleration = accelerationWhileDashing;
        yield return new WaitForSeconds(dashLength);
        characterMovement.acceleration = originalAcceleration;
    }
}
