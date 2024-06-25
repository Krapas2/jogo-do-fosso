using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class ProtoMelee : PlayerSkill
{
    protected override void Start()
    {
        base.Start();
    }

    void Update()
    {
        if(Input.GetButtonDown("Fire1")){
            Debug.Log("melee attack");
        }
    }
}
