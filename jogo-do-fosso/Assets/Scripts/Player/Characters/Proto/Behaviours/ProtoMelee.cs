using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class ProtoMelee : CharacterSkill
{
    public ProtoPunch punchPrefab;
    [SyncVar]
    public float punchLength;

    [SyncVar]
    private ProtoPunch currentPunch;

    private CameraData cameraData;

    protected override void Start()
    {
        base.Start();
        
        cameraData = FindObjectOfType<CameraData>();
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

    void Aim()
    {
        currentPunch.transform.position = transform.position;
        currentPunch.transform.up = cameraData.worldMousePosition - transform.position.Vector2();
    }

    [Command]
    void CmdSpawnPunch(){
        ProtoPunch punchInstance = Instantiate(punchPrefab, transform.position, Quaternion.identity);

        punchInstance.owner = this;

        NetworkServer.Spawn(punchInstance.gameObject, connectionToClient);

        currentPunch = punchInstance;
    }
}
