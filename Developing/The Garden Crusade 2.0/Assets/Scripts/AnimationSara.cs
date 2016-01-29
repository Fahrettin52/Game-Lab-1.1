﻿using UnityEngine;
using System.Collections;

public class AnimationSara : MonoBehaviour
{

    public Animator animator;

    void Start()
    {
        animator.SetBool("cancelRun", true);
    }

    public void SarahRun(float i)
    {
        animator.SetBool("cancelRun", false);
        animator.SetFloat("idleToRun 0", i);
    }

    public void SaraPunch()
    {
        animator.SetTrigger("SwordAttack");
        animator.SetBool("idleCombat", true);
    }

    public void mayJump1(bool may)
    {
        animator.SetBool("mayJump1",may);
    }

    public void mayDie()
    {
        animator.SetBool("Death", true);
    }

    public void fightToIdle()
    {
        animator.SetTrigger("fightToIdle");
    }

    public void SarahTurn(float i) {
        animator.SetFloat("Turn", i);
    }

    public void Strave(float i) {
        animator.SetFloat("Strave", i);
    }
}

