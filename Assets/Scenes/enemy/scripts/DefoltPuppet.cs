using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;

public class DefoltPuppet : EnemyAIBase
{
    [SerializeField] private AnimationClip normalAttack1;
    [SerializeField] private AnimationClip normalAttack2;


    [SerializeField] private AnimationClip fightingIdle;

    private bool isAttacking = false;
    private int choosedAttack = -1;
    private bool window = false;



    Vector3 newPoint;
    Vector3 ret;
    private float angle;


    protected override void FightLogic(float distance)
    {

        print(animator.GetCurrentAnimatorClipInfo(0)[0].clip.name);
        if (choosedAttack == -1)
            choosedAttack = EnemyLoader.random.Next(0, 1);


        switch (choosedAttack)
        {
            case 0:

                if (!isAttacking)
                {


                    if (distance > 3 && !animator.GetBool("run"))
                        animator.SetTrigger("start run");
                    if (distance > 3)
                    {
                        agent.SetDestination(target.position);
                        animator.SetBool("run", true);
                        agent.speed = 3;
                        return;
                    }

                    isAttacking = true;
                    agent.SetDestination(transform.position);
                    animator.SetBool("run", false);
                    agent.speed = 1;
                    animator.SetTrigger("normal attack");
                    StartCoroutine(InvokeWithDelay(() =>
                    {

                        window = true;

                    }, animator.GetCurrentAnimatorClipInfo(0)[0].clip.length));

                    StartCoroutine(InvokeWithDelay(() =>
                    {
                        isAttacking = false;
                        window = false;
                        choosedAttack = -1;
                    }, animator.GetCurrentAnimatorClipInfo(0)[0].clip.length + 5f));
                }
                else if (window)
                {
                    if (distance < 5f)
                    {
                        Vector3 directionAway = (transform.position - target.position).normalized;

                        Vector3 newPosition = transform.position + directionAway * 2;

                        agent.SetDestination(newPosition);

                    }
                    else
                    {
                        // Якщо дистанція більша за 5 одиниць, рухаємось вліво або вправо
                        agent.updateRotation = true;  // Включаємо повертання на ціль

                        // Вибір випадкового напрямку (ліво або право від цілі)
                        Vector3 directionToTarget = target.position - transform.position;
                        Vector3 right = Vector3.Cross(directionToTarget, Vector3.up);  // Напрямок вправо
                        Vector3 left = -right;  // Напрямок вліво

                        // Випадковий вибір напрямку
                        Vector3 moveDirection = (UnityEngine.Random.value > 0.5f) ? right : left;

                        // Рухаємо агента в одному з напрямків
                        Vector3 newPosition = transform.position + moveDirection * 2;
                        agent.SetDestination(newPosition);
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