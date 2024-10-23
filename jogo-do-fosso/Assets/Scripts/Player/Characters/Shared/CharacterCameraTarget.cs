using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class CharacterCameraTarget : NetworkBehaviour
{
    void Start()
    {
        if (isOwned) {
            CameraController cameraController = FindObjectOfType<CameraController>();
            if(cameraController){
                cameraController.target = transform;
            }
        }
    }
}
