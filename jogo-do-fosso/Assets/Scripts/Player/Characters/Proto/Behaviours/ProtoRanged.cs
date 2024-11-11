using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class ProtoRanged : CharacterSkill
{
    public ProtoProjectile projectilePrefab;
    public Transform projectileOrigin;

    private CameraData cameraData;


    protected override void Start()
    {
        base.Start();

        cameraData = Camera.main.GetComponent<CameraData>();
    }

    void Update()
    {
        Aim();

        if(Input.GetButtonDown("Fire1") && canUse){
            CmdFire();
            StartCoroutine(Cooldown());
        }
    }

    void Aim()
    {
        projectileOrigin.up = cameraData.worldMousePosition - transform.position.Vector2();
    }

    [Command]
    void CmdFire()
    {
        ProtoProjectile projectile = Instantiate(projectilePrefab, projectileOrigin.position, projectileOrigin.rotation);

        projectile.owner = this;
        team.SpawnTeammate(projectile.GetComponent<TeamBehaviour>());
    }
}
