using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

[RequireComponent(typeof(TeamBehaviour))]
public class CharacterSkill : NetworkBehaviour
{

    [SyncVar]
    public float cooldown;

    [SyncVar] 
    [HideInInspector]
    public float cooldownProgress;
    [SyncVar]
    [HideInInspector]
    public bool canUse;
    
    [HideInInspector]
    public TeamBehaviour team;

    void Awake()
    {
        team = GetComponent<TeamBehaviour>();

        cooldownProgress = cooldown;
        canUse = true;
    }

    protected virtual void Start()
    {
        if (!isOwned){
            this.enabled = false;
        }
    }

    public IEnumerator Cooldown()
    {
        canUse = false;
        cooldownProgress = 0f;
        while(cooldownProgress < cooldown){
            cooldownProgress += Time.deltaTime;
            yield return null;
        }
        cooldownProgress = cooldown;
        canUse = true;
    }
}
