using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProtoGUISpawner : MonoBehaviour
{
    public ProtoGUI protoGUI;

    void Start()
    {
        Canvas canvas = FindObjectOfType<Canvas>();

        ProtoGUI spawnedProtoGUI = Instantiate(protoGUI, canvas.transform);
        spawnedProtoGUI.proto = GetComponent<Character>();
    }
}
