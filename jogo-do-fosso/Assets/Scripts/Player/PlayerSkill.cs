using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerSkill : NetworkBehaviour
{

    [SyncVar]
    public float cooldown;

    [SyncVar] 
    [HideInInspector]
    public float cooldownProgress;
    [SyncVar]
    [HideInInspector]
    public bool canUse;

    void Awake()
    {
        cooldownProgress = cooldown;
        canUse = true;
    }

    protected virtual void Start()
    {
        if (!isLocalPlayer){
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
