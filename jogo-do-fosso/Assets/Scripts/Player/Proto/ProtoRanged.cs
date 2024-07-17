using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class ProtoRanged : PlayerSkill
{
    public ProtoProjectile projectilePrefab;
    public Transform projectileOrigin;
    [SyncVar]
    public float cooldown;

    [SyncVar]
    private bool canFire;

    private Player player;

    private CameraData cameraData;

    protected override void Start()
    {
        base.Start();

        player = GetComponent<Player>();

        cameraData = FindObjectOfType<CameraData>();

        canFire = true;
    }

    void Update()
    {
        Aim();

        if(Input.GetButtonDown("Fire1") && canFire){
            Fire();
            StartCoroutine(CoolDown());
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

        projectile.owner = player;
        NetworkServer.Spawn(projectile.gameObject);
    }

    IEnumerator CoolDown()
    {
        canFire = false;
        yield return new WaitForSeconds(cooldown);
        canFire = true;
    }
}
