using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.InputSystem.LowLevel.InputStateHistory;

public class DefoltPuppet : EnemyAIBase
{
    [SerializeField] private AnimationClip idleClip;

    [SerializeField] private AnimationClip normalAttack;
    [SerializeField] private AnimationClip heavyAttack;
    [SerializeField] private AnimationClip fightingIdle;
    [SerializeField] private AnimationClip[] randomMoves;
    [SerializeField] private GameObject weapon;

    private bool isAttacking = false;
    private int choosedAttack = -1;
    private bool window = false;

    protected override void Start()
    {
        base.Start();

        changespellanim("enemy_idle", idleClip);
        changespellanim("enemy_attack", normalAttack);
        changespellanim("enemy_fight_idle", fightingIdle);

        weapon.GetComponent<DamageDiller>().ActorStats = Stats;
    }
    protected override void ChangeIdleToDefolt()
    {
        changespellanim("enemy_idle", idleClip);
    }

    protected override void FightLogic(float distance)
    {
        if (isInFight == false)
        {
            ResetAnimatorParameters();
            changespellanim("enemy_idle", fightingIdle);
            isInFight = true;

        }

        animator.SetBool("goBack", distance < minFavoriteRange);


        if (choosedAttack == -1)
            choosedAttack = EnemyLoader.random.Next(0, 2);
        print(choosedAttack);



        switch (choosedAttack)
        {
            case 0:

                if (!isAttacking)
                {
                    animator.ResetTrigger("start window");
                    changespellanim("enemy_attack", normalAttack);



                    if (distance > normalAttackRange)
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
                            randomMoveIn();



                            choosedAttack = -1;
                        }, normalWindowDuriotion));
                    }, animator.GetCurrentAnimatorClipInfo(0)[0].clip.length - 0.5f));

                }
                else if (window)
                {
                    agent.SetDestination(target.position);
                    if (distance > maxFavoriteRange)
                    {
                        animator.SetTrigger("start run");
                        animator.SetBool("run", true);
                    }
                    else if (distance < minFavoriteRange)
                    {
                        animator.SetTrigger("start window");
                        animator.SetBool("run", false);

                    }
                }

                break;

            case 1:
                if (!isAttacking)
                {
                    animator.ResetTrigger("start window");
                    //çàì³íþºìî àí³ìêó
                    changespellanim("enemy combo attack", heavyAttack);


                    //ï³äá³ãàºìî ÿêùî äàëåêî
                    if (distance > heavyAttackRange)
                    {
                        if (!animator.GetBool("run"))
                            animator.SetTrigger("start run");

                        agent.SetDestination(target.position);
                        animator.SetBool("run", true);
                        return;
                    }

                    isAttacking = true;
                    animator.SetBool("run", false);
                    animator.SetTrigger("heavy attack");
                    transform.LookAt(target);
                    StartCoroutine(InvokeWithDelay(() =>
                    {
                        window = true;
                        animator.SetTrigger("start window");

                        StartCoroutine(InvokeWithDelay(() =>
                        {
                            isAttacking = false;
                            window = false;
                            randomMoveIn();

                            choosedAttack = -1;
                        }, heavyWindowDuriotion));
                    }, animator.GetCurrentAnimatorClipInfo(0)[0].clip.length - 0.5f));

                }
                else if (window)
                {
                    agent.SetDestination(target.position);
                    if (distance > maxFavoriteRange)
                    {
                        animator.SetTrigger("start run");
                        animator.SetBool("run", true);
                    }
                    else if (distance < minFavoriteRange)
                    {
                        animator.SetTrigger("start window");
                        animator.SetBool("run", false);

                    }
                }
                //choosedAttack = -1;
                break;
        }
    }

    private void randomMoveIn(float second=0)
    {
        AnimationClip anim =null;
        if (randomMoves.Length > 0)
        {
            anim = randomMoves[Random.Range(0, randomMoves.Length)];
            changespellanim("enemyStrafing", anim);
        }

        StartCoroutine(InvokeWithDelay(() => animator.SetTrigger("random move"), second));
        
    }





    public void WearponTrailOn(float AttackScale)
    {
        weapon.GetComponent<DamageDiller>().AttackScale = AttackScale;
        weapon.GetComponentInChildren<TrailRenderer>().emitting = true;
    }
    public void WearponTrailOFF()
    {
        weapon.GetComponentInChildren<TrailRenderer>().emitting = false;
    }

    private IEnumerator InvokeWithDelay(System.Action method, float delay)
    {
        yield return new WaitForSeconds(delay);

        method?.Invoke();
    }
}