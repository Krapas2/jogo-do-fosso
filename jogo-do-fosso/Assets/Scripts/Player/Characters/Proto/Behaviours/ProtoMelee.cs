using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

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
        if(currentPunch){
            Aim();
        }

        if(Input.GetButtonDown("Fire1") && !currentPunch && canUse){
            CmdSpawnPunch();
        }
    }

    [Client]
    void Aim()
    {
        currentPunch.transform.position = transform.position;
        currentPunch.transform.up = cameraData.worldMousePosition - transform.position.Vector2();
    }

    [Command]
    void CmdSpawnPunch(){
        ProtoPunch punchInstance = Instantiate(punchPrefab, transform.position, Quaternion.identity);

        punchInstance.owner = this;

        team.SpawnTeammate(punchInstance.GetComponent<TeamBehaviour>());

        currentPunch = punchInstance;
    }
}
