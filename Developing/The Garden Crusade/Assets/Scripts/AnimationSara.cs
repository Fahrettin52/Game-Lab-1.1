using UnityEngine;
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
        animator.SetTrigger("Punch");
        animator.SetBool("idleCombat", true);
    }

    public void mayJump1(bool may)
    {
        animator.SetBool("mayJump1",may);
    }

    public void mayDie()
    {
        animator.SetTrigger("mayDie");
    }

    public void fightToIdle()
    {
        animator.SetTrigger("fightToIdle");
    }
}

