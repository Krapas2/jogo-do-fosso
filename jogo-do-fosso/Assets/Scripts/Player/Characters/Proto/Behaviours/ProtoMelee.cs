using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

[RequireComponent(typeof(TeamBehaviour))]
public class ProtoMelee : CharacterSkill
{
    public ProtoPunch punchPrefab;

    [SyncVar]
    private ProtoPunch currentPunch;

    private CameraData cameraData;

    protected override void Start()
    {
        base.Start();

        cameraData = Camera.main.GetComponent<CameraData>();
    }

    void Update()
    {
        Vector2 relativeMousePosition = cameraData.worldMousePosition - transform.position.Vector2();

        if (currentPunch) {
            Aim(relativeMousePosition);
        }
        if (Input.GetButtonDown("Fire1") && !currentPunch && canUse) {
            CmdSpawnPunch(relativeMousePosition);
            StartCoroutine(Cooldown());
        }
    }

    [Client]
    void Aim(Vector2 direction)
    {
        currentPunch.transform.position = transform.position;
        currentPunch.transform.up = direction;
    }

    [Command]
    void CmdSpawnPunch(Vector2 direction)
    {
        ProtoPunch punchInstance = Instantiate(punchPrefab, transform.position, Quaternion.identity);
        punchInstance.transform.up = direction;

        punchInstance.owner = this;

        team.SpawnTeammate(punchInstance.GetComponent<TeamBehaviour>());

        currentPunch = punchInstance;
    }
}
