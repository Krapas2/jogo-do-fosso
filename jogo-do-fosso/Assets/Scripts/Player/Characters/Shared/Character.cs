using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Character : NetworkBehaviour
{
    [HideInInspector]
    [SyncVar]
    public PlayerManager manager;

    void Start()
    {
        if (isOwned) {
            manager.currentCharacter = this;
        }
    }
}
