using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProtoGUI : MonoBehaviour
{
    [HideInInspector]
    public Character proto;

    public Image meleeBar;
    public Image rangedBar;
    public Image dashBar;
    public Image sentryBar;

    private ProtoMelee protoMelee;
    private ProtoRanged protoRanged;
    private ProtoDash protoDash;
    private ProtoSentrySpawner protoSentrySpawner;

    void Start()
    {
        protoMelee = proto.GetComponent<ProtoMelee>();
        protoRanged = proto.GetComponent<ProtoRanged>();
        protoDash = proto.GetComponent<ProtoDash>();
        protoSentrySpawner = proto.GetComponent<ProtoSentrySpawner>();
    }

    void Update()
    {
        meleeBar.fillAmount = protoMelee.cooldownProgress / protoMelee.cooldown;
        rangedBar.fillAmount = protoRanged.cooldownProgress / protoRanged.cooldown;
        dashBar.fillAmount = protoDash.cooldownProgress / protoDash.cooldown;
        sentryBar.fillAmount = protoSentrySpawner.cooldownProgress / protoSentrySpawner.cooldown;
    }
}
