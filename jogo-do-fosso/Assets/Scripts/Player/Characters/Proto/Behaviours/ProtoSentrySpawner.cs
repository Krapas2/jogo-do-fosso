using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;

[RequireComponent(typeof(TeamBehaviour))]
public class ProtoSentrySpawner : CharacterSkill
{
    [System.Serializable]
    public struct SentryLevel {
        public ProtoSentry sentry;
        public float cooldown;
    
        public SentryLevel(ProtoSentry sentry, float cooldown) {
            this.sentry = sentry;
            this.cooldown = cooldown;
        }
    }

    public SentryLevel[] sentryLevels;

    [SyncVar]
    [HideInInspector]
    public ProtoSentry currentSentry;
    [SyncVar]
    private int sentryLevel;

    protected override void Start()
    {
        base.Start();

        ResetSentryStats();
    }

    void Update()
    {
        if(!currentSentry){
            ResetSentryStats();
        }

        if(Input.GetButtonDown("Special") && canUse){
            if(!currentSentry){
                CmdSpawnSentry();
            } else if(sentryLevel < sentryLevels.Length) {
                CmdUpgradeSentry();
            }
            StartCoroutine(Cooldown());
        }
    }

    [Command]
    void CmdSpawnSentry()
    {
        sentryLevel = 0;
        Debug.Log(sentryLevel);

        ProtoSentry sentryToSpawn = sentryLevels[sentryLevel].sentry;
        Vector3 positionToSpawn = transform.position;

        ProtoSentry spawnedSentry = Instantiate(sentryToSpawn, positionToSpawn, Quaternion.identity);
        spawnedSentry.owner = this;

        team.SpawnTeammate(spawnedSentry.GetComponent<TeamBehaviour>());

        currentSentry = spawnedSentry;
        cooldown = sentryLevels[sentryLevel].cooldown;
    }

    [Command]
    void CmdUpgradeSentry()
    {
        Debug.Log(sentryLevel);
        sentryLevel = Mathf.Clamp(sentryLevel+1,0,sentryLevels.Length-1);
        Debug.Log(sentryLevel);
        ProtoSentry sentryToSpawn = sentryLevels[sentryLevel].sentry;
        Vector3 positionToSpawn = currentSentry.transform.position;

        ProtoSentry spawnedSentry = Instantiate(sentryToSpawn, positionToSpawn, Quaternion.identity);
        spawnedSentry.owner = this;

        NetworkServer.Destroy(currentSentry.gameObject);
        team.SpawnTeammate(spawnedSentry.GetComponent<TeamBehaviour>());

        currentSentry = spawnedSentry;
        cooldown = sentryLevels[sentryLevel].cooldown;
    }

    void ResetSentryStats(){
        cooldown = sentryLevels[0].cooldown;
        sentryLevel = -1;
    }
}
