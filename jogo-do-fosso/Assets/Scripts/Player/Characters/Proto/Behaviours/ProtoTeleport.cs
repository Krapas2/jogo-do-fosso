using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

[RequireComponent(typeof(ProtoSentrySpawner))]
[RequireComponent(typeof(CharacterAttackManager))]
public class ProtoTeleport : CharacterSkill
{
    public float range;

    private bool teleporting;

    private ProtoSentrySpawner protoSentrySpawner;
    private CharacterAttackManager characterAttackManager;

    private CameraController cameraController;
    private CameraData cameraData;

    protected override void Start()
    {
        base.Start();
        
        protoSentrySpawner = GetComponent<ProtoSentrySpawner>();
        characterAttackManager = GetComponent<CharacterAttackManager>();
        
        cameraController = Camera.main.GetComponent<CameraController>();
        cameraData = Camera.main.GetComponent<CameraData>();
    }

    void Update()
    {
        if(Input.GetButtonDown("Fire2") && canUse){
            Prepare();
        }

        if(teleporting && Input.GetButtonUp("Fire2")){
            Cancel();
        }

        if(teleporting && Input.GetButtonDown("Fire1") && canUse){
            Teleport();
            StartCoroutine(Cooldown());
        }
    }

    [Client]
    void Prepare()
    {
        teleporting = true;
        cameraController.target = protoSentrySpawner.currentSentry.transform;
        characterAttackManager.DisableAllSkills();
    }
    
    [Client]
    void Cancel()
    {
        teleporting = false;
        cameraController.target = transform;
        characterAttackManager.EnableLastUsedSkill();
    }

    [Client]
    void Teleport()
    {
        Vector3 position = cameraData.worldMousePosition.Vector3();
        CmdTeleport(position);

        teleporting = false;
        cameraController.target = transform;
        characterAttackManager.EnableLastUsedSkill();
    }

    [Command]
    void CmdTeleport(Vector3 position)
    {
        Vector3 sentryPosition = protoSentrySpawner.currentSentry.transform.position;
        Vector3 vectorFromSentry = Vector3.ClampMagnitude(position - sentryPosition, range);
        transform.position = vectorFromSentry + sentryPosition;
    }
}
