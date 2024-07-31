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
    public Image specialBar;

    private ProtoMelee protoMelee;
    private ProtoRanged protoRanged;
    private ProtoDash protoDash;
    private ProtoSpecial protoSpecial;

    void Start()
    {
        protoMelee = proto.GetComponent<ProtoMelee>();
        protoRanged = proto.GetComponent<ProtoRanged>();
        protoDash = proto.GetComponent<ProtoDash>();
        protoSpecial = proto.GetComponent<ProtoSpecial>();
    }

    void Update()
    {
        meleeBar.fillAmount = protoMelee.cooldownProgress / protoMelee.cooldown;
        rangedBar.fillAmount = protoRanged.cooldownProgress / protoRanged.cooldown;
        dashBar.fillAmount = protoDash.cooldownProgress / protoDash.cooldown;
        specialBar.fillAmount = protoSpecial.cooldownProgress / protoSpecial.cooldown;
    }
}
