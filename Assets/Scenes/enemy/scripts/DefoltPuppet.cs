using System.Collections;
using UnityEngine;

public class DefoltPuppet : EnemyAIBase
{
    [SerializeField] private AnimationClip idleClip;

    [SerializeField] private AnimationClip normalAttack1;
    [SerializeField] private AnimationClip normalAttack2;
    [SerializeField] private AnimationClip fightingIdle;

    private bool isAttacking = false;
    private int choosedAttack = -1;
    private bool window = false;

    protected override void Start()
    {
        base.Start();

        changespellanim("enemy|idle", idleClip);
        changespellanim("enemy|attack", normalAttack1);
    }
    protected override void ChangeIdleToDefolt()
    {
        changespellanim("enemy|idle", idleClip);
    }

    protected override void FightLogic(float distance)
    {
        if (isInFight == false)
        {
            changespellanim("enemy|idle", fightingIdle);
            isInFight = true;

        }
        animator.SetFloat("distance", distance);
        if (choosedAttack == -1)
            choosedAttack = 0;// EnemyLoader.random.Next(0, 1);


        switch (choosedAttack)
        {
            case 0:

                if (!isAttacking)
                {
                    animator.ResetTrigger("start window");
                    changespellanim("enemy|attack", normalAttack1);



                    if (distance > 3)
                    {
                        if (!animator.GetBool("run"))
                            animator.SetTrigger("start run");

                        agent.SetDestination(target.position);
                        animator.SetBool("run", true);
                        return;
                    }

                    isAttacking = true;
                    animator.SetBool("run", false);
                    animator.SetTrigger("normal attack");
                    transform.LookAt(target);
                    StartCoroutine(InvokeWithDelay(() =>
                    {
                        window = true;
                        animator.SetTrigger("start window");

                        StartCoroutine(InvokeWithDelay(() =>
                        {
                            isAttacking = false;
                            window = false;

                            choosedAttack = -1;
                        }, 5f));
                    }, animator.GetCurrentAnimatorClipInfo(0)[0].clip.length - 0.5f));

                }
                else if (window)
                {
                    agent.SetDestination(target.position);
                    if (distance > 8)
                    {
                        animator.SetTrigger("start run");
                        animator.SetBool("run", true);
                    }
                    else if (distance < 5)
                    {
                        animator.SetTrigger("start window");
                        animator.SetBool("run", false);

                    }
                }

                break;

            case 1:
                choosedAttack = -1;
                break;
        }
    }

    private IEnumerator InvokeWithDelay(System.Action method, float delay)
    {
        yield return new WaitForSeconds(delay);

        method?.Invoke();
    }
}