using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class HealthBar : MonoBehaviour
{
    public Character character;
    
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        Animate();
    }

    void Animate()
    {
        float healthPercentage = character.currentHealth / character.maxHealth;
        float normalizedHealthPercentage = healthPercentage * animator.GetCurrentAnimatorClipInfo(0)[0].clip.length;
        animator.SetFloat("fillAmount", healthPercentage);
    }
}
