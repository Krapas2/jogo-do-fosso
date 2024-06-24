using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public abstract class PlayerRanged : NetworkBehaviour {
	
    protected virtual void Start()
    {
        if (!isLocalPlayer) {
            this.enabled = false;
        }
	}
}
