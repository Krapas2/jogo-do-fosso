using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class ProtoRanged : CharacterSkill
{
    public ProtoProjectile projectilePrefab;
    public Transform projectileOrigin;
    
    private Character character;

    private CameraData cameraData;


    protected override void Start()
    {
        base.Start();

        character = GetComponent<Character>();

        cameraData = FindObjectOfType<CameraData>();
    }

    void Update()
    {
        Aim();

        if(Input.GetButtonDown("Fire1") && canUse){
            Fire();
            StartCoroutine(Cooldown());
        }
    }

    void Aim()
    {
        projectileOrigin.up = cameraData.worldMousePosition - transform.position.Vector2();
    }

    [Command]
    void Fire()
    {
        ProtoProjectile projectile = Instantiate(projectilePrefab, projectileOrigin.position, projectileOrigin.rotation);

        projectile.owner = character;
        NetworkServer.Spawn(projectile.gameObject);
    }
}
