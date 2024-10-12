using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class ProtoSentry : NetworkBehaviour
{
    private CameraData cameraData;

    [SyncVar]
    [HideInInspector]
    public ProtoSentrySpawner owner;

    void Start()
    {
        if (isOwned) {
            cameraData = FindObjectOfType<CameraData>();
        }
    }

    void Update()
    {
        
    }
}
