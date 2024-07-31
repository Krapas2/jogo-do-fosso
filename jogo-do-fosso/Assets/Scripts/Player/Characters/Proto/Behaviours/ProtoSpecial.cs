using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class ProtoSpecial : PlayerSkill
{
    public ProtoSpecialProjectile projectilePrefab;
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

        if(Input.GetButtonDown("Special") && canUse){
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
        ProtoSpecialProjectile projectile = Instantiate(projectilePrefab, projectileOrigin.position, projectileOrigin.rotation);

        projectile.owner = character;
        NetworkServer.Spawn(projectile.gameObject, connectionToClient);
    }
}
