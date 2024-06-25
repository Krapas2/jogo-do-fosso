using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerAttackManager : PlayerSkill
{

    public PlayerSkill[] attacks;

    protected override void Start()
    {
        base.Start();
        SelectAttack(0);
    }

    void Update()
    {
        ListenForSelection();
    }

    void ListenForSelection()
    {
        for(int i = 0; i < attacks.Length; i++){
            if(Input.GetButton(string.Concat("SelectAttack", i+1))){
                SelectAttack(i);
            }
        }
    }

    void SelectAttack(int selected)
    {
        for(int i = 0; i < attacks.Length; i++){
            attacks[i].enabled = (i == selected);
        }
    }
}
