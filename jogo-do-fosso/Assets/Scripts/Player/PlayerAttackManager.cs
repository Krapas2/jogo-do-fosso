using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerAttackManager : NetworkBehaviour
{

    public PlayerSkill[] attacks;

    void Start()
    {
        if (!isLocalPlayer){
            this.enabled = false;
        }
        
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
