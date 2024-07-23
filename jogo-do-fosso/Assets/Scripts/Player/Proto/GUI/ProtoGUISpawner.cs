using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class ProtoGUISpawner : NetworkBehaviour
{
    public ProtoGUI protoGUI;

    void Start()
    {
        if(!isLocalPlayer){
            return;
        }

        Canvas canvas = FindObjectOfType<Canvas>();

        ProtoGUI spawnedProtoGUI = Instantiate(protoGUI, canvas.transform);
        spawnedProtoGUI.proto = GetComponent<Player>();
    }
}
