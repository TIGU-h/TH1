using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DefoltPuppet : EnemyAIBase
{
    [SerializeField] private AnimationClip normalAttack1;
    [SerializeField] private AnimationClip normalAttack2;


    [SerializeField] private AnimationClip fightingIdle;

    private bool isAttacking = false;
    private int choosedAttack = -1;


    protected override void FightLogic(float distance)
    {
        print("normal");
        print(animator.GetCurrentAnimatorClipInfo(0)[0].clip.name);
        if (choosedAttack == -1)
            choosedAttack = EnemyLoader.random.Next(0, 1);


        switch (choosedAttack)
        {
            case 0:
                if (distance > 3 && !animator.GetBool("run"))
                    animator.SetTrigger("start run");
                if (distance > 3)
                {
                    agent.SetDestination(target.position);
                    animator.SetBool("run", true);
                }
                else
                {
                    if (!isAttacking)
                    {
                        isAttacking = true;
                        agent.SetDestination(transform.position);
                        animator.SetBool("run", false);
                        LookTarget();
                        animator.SetTrigger("normal attack");
                        StartCoroutine(InvokeWithDelay(() =>
                        {
                            Vector3 randomPoint = target.position + (UnityEngine.Random.insideUnitSphere *Vector3.Distance(transform.position, target.position));
                            randomPoint.y = target.position.y;
                            agent.SetDestination(randomPoint);
                            animator.SetTrigger("walk right");
                        }, animator.GetCurrentAnimatorClipInfo(0)[0].clip.length));

                        StartCoroutine(InvokeWithDelay(() =>
                        {
                            isAttacking = false;
                            choosedAttack = -1;
                        }, animator.GetCurrentAnimatorClipInfo(0)[0].clip.length + 2f));
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