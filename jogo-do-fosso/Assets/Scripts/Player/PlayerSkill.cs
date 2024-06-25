using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerSkill : NetworkBehaviour
{
    protected virtual void Start()
    {
        if (!isLocalPlayer){
            this.enabled = false;
        }
    }
}
