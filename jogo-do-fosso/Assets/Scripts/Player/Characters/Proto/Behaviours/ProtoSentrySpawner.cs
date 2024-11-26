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
    private int nextSentryLevel;

    protected override void Start()
    {
        base.Start();

        ResetSentryStats();
    }

    void Update()
    {
        if(Input.GetButtonDown("Special") && canUse){
            CmdPlaceSentry(connectionToClient);
            StartCoroutine(ListenForSentryDeath());
            StartCoroutine(Cooldown());
        }
    }

    [Command]
    void CmdPlaceSentry(NetworkConnectionToClient sender)
    {
        if(nextSentryLevel >= sentryLevels.Length){
            return;
        }

        ProtoSentry sentryToSpawn = sentryLevels[nextSentryLevel].sentry;
        cooldown = sentryLevels[nextSentryLevel].cooldown;
        nextSentryLevel++;
        
        Vector3 positionToSpawn;

        if(!currentSentry){
            positionToSpawn = transform.position;
        }else{
            positionToSpawn = currentSentry.transform.position;
            NetworkServer.Destroy(currentSentry.gameObject);
        }

        ProtoSentry spawnedSentry = Instantiate(sentryToSpawn, positionToSpawn, Quaternion.identity);
        spawnedSentry.owner = this;

        team.SpawnTeammate(spawnedSentry.GetComponent<TeamBehaviour>());

        currentSentry = spawnedSentry;
    }

    IEnumerator ListenForSentryDeath()
    {
        yield return new WaitUntil(() => !currentSentry);

        ResetSentryStats();
    }

    void ResetSentryStats(){
        cooldown = sentryLevels[0].cooldown;
        nextSentryLevel = 0;
    }
}
