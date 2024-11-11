using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class CharacterAttackManager : NetworkBehaviour
{

    public CharacterSkill[] attacks;

    private int lastUsedSkillIndex;

    void Start()
    {
        if (!isOwned){
            this.enabled = false;
        }
        
        SelectSkill(0);
    }

    void Update()
    {
        ListenForSelection();
    }

    void ListenForSelection()
    {
        for(int i = 0; i < attacks.Length; i++){
            if(Input.GetButton(string.Concat("SelectAttack", i+1))){
                SelectSkill(i);
            }
        }
    }

    void SelectSkill(int selected)
    {
        for(int i = 0; i < attacks.Length; i++){
            attacks[i].enabled = (i == selected);
        }
        lastUsedSkillIndex = selected;
    }

    public void DisableAllSkills()
    {
        for(int i = 0; i < attacks.Length; i++){
            attacks[i].enabled = false;
        }
    }

    public void EnableLastUsedSkill()
    {
        for(int i = 0; i < attacks.Length; i++){
            attacks[i].enabled = (i == lastUsedSkillIndex);
        }
    }
}
