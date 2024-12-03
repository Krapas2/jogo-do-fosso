using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProtoGUI : MonoBehaviour
{
    [HideInInspector]
    public GameObject proto;

    public Image meleeBar;
    public Image rangedBar;
    public Image teleportBar;
    public Image sentryBar;

    private ProtoMelee protoMelee;
    private ProtoRanged protoRanged;
    private ProtoTeleport protoTeleport;
    private ProtoSentrySpawner protoSentrySpawner;

    void Start()
    {
        protoMelee = proto.GetComponent<ProtoMelee>();
        protoRanged = proto.GetComponent<ProtoRanged>();
        protoTeleport = proto.GetComponent<ProtoTeleport>();
        protoSentrySpawner = proto.GetComponent<ProtoSentrySpawner>();
    }

    void Update()
    {
        meleeBar.fillAmount = protoMelee.cooldownProgress / protoMelee.cooldown;
        rangedBar.fillAmount = protoRanged.cooldownProgress / protoRanged.cooldown;
        teleportBar.fillAmount = protoTeleport.cooldownProgress / protoTeleport.cooldown;
        sentryBar.fillAmount = protoSentrySpawner.cooldownProgress / protoSentrySpawner.cooldown;
    }
}
