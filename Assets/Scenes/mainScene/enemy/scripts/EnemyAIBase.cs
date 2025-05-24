using UnityEngine;
using System.Collections;
using UnityEngine.AI;
using Unity.VisualScripting;
using System.Collections.Generic;

public class EnemyAIBase : MonoBehaviour
{
    [SerializeField] private Transform[] patrolPoints; // Масив точок патрулювання
    [SerializeField] protected float lookRadius = 10f;
    [SerializeField] private float rotspeed = 1f;
    [SerializeField] private float timeForDie = 2f;


    public Stats Stats;



    public bool isLoaded = false;

    protected Transform target;
    public Transform getTarget()
    {
        return target;
    }
    protected NavMeshAgent agent;

    [SerializeField] protected int normalAttackRange;
    [SerializeField] protected int heavyAttackRange;
    [SerializeField] protected int minFavoriteRange;
    [SerializeField] protected int maxFavoriteRange;
    [SerializeField] protected int normalWindowDuriotion;
    [SerializeField] protected int heavyWindowDuriotion;

    private int currentPatrolIndex = 0;
    private bool isChasing = false;

    private Coroutine enemyIdleCoroutine;



    protected Animator animator;
    protected bool isInFight = false;
    private bool isDead = false;
    


    protected virtual void Start()
    {
        Stats.ScaleStatsByLevel();
        agent = GetComponent<NavMeshAgent>();

        animator = GetComponent<Animator>();

        if (animator == null)
        {
            Debug.LogError("На об'єкті немає Animator!");
            return;
        }
        GetComponent<Health>().SetStats(Stats);
        GetComponent<Health>().OnDeath += Die;



    }
    
    protected virtual void Die()
    {
        isDead = true;
        target.GetComponent<CharacterStats>().GainExperience(Stats.level*10);
        StopAllCoroutines();
        ResetAllAnimatorParameters(animator);
        animator.SetTrigger("die");
        Destroy(gameObject, timeForDie);
    }

    private void ResetAllAnimatorParameters(Animator animator)
    {
        if (animator == null) return;

        foreach (AnimatorControllerParameter param in animator.parameters)
        {
            switch (param.type)
            {
                case AnimatorControllerParameterType.Bool:
                    animator.SetBool(param.name, false);
                    break;

                case AnimatorControllerParameterType.Trigger:
                    animator.ResetTrigger(param.name);
                    break;
            }
        }
    }

    private void OnDestroy()
    {
        GetComponent<Health>().OnDeath -= Die;
    }



    public void LoadEnemy(GameObject player)
    {
        isLoaded = true;
        target = target == null ? player.transform : target;

        if (enemyIdleCoroutine == null)
            enemyIdleCoroutine = StartCoroutine(EnemyIdleBehevior());

        isInFight = false;
    }

    public void UnloadEnemy()
    {
        target = null;

        if (enemyIdleCoroutine != null)
        {
            StopCoroutine(enemyIdleCoroutine);
            enemyIdleCoroutine = null;
        }
        StopAllCoroutines();
        isLoaded = false;
    }

    private IEnumerator EnemyIdleBehevior()
    {
        float delay = 1f;
        yield return new WaitForSeconds(0.1f);
        while (true)
        {

            if (isDead)
            {
                ResetAllAnimatorParameters(animator);
                yield break;
            }
            float distance = Vector3.Distance(target.position, transform.position);
            if (distance <= lookRadius)
            {
                FightLogic(distance);
                delay = 0.3f;
            }
            if (distance > lookRadius)
            {
                if (isInFight)
                {
                    isInFight = false;
                    ChangeIdleToDefolt();
                }
                isChasing = false;
                Patrol();
                delay = 1f;
            }

            yield return new WaitForSeconds(delay);
        }
    }
    protected virtual void ChangeIdleToDefolt()
    {

    }
    protected virtual void FightLogic(float distance)
    {

        isChasing = true;
        if (agent.remainingDistance < 3)
        {
            animator.SetTrigger("attack");
        }
        else
        {
            agent.SetDestination(target.position);
        }
    }


    private void Patrol()
    {

        if (patrolPoints.Length == 0 || isChasing)
        {
            animator.SetBool("walk", false);
            return;
        }
        if (!agent.pathPending && agent.remainingDistance < 1.5f)
        {
            animator.SetBool("run", false);
            if (animator.GetCurrentAnimatorStateInfo(0).tagHash != Animator.StringToHash("walk"))
                animator.SetTrigger("start walk");
            animator.SetBool("walk", true);

            agent.SetDestination(patrolPoints[currentPatrolIndex].position);
            currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
        }
    }


    // Клас повинен мати цей список
    private List<KeyValuePair<AnimationClip, AnimationClip>> allOverrides = new List<KeyValuePair<AnimationClip, AnimationClip>>();

    protected void changespellanim(string name, AnimationClip newAnimation)
    {
        if (newAnimation == null) return;
        string stateName = name;

        // Отримуємо поточний контролер
        var runtimeAnimatorController = animator.runtimeAnimatorController;

        // Створюємо OverrideController
        var overrideController = new AnimatorOverrideController(runtimeAnimatorController);

        // Отримуємо поточні оверрайди
        var currentOverrides = new List<KeyValuePair<AnimationClip, AnimationClip>>(overrideController.overridesCount);
        overrideController.GetOverrides(currentOverrides);

        // Оновлюємо глобальний список, якщо він ще порожній
        if (allOverrides.Count == 0)
        {
            allOverrides = new List<KeyValuePair<AnimationClip, AnimationClip>>(currentOverrides);
        }

        int replaced = 0;

        for (int i = 0; i < allOverrides.Count; i++)
        {
            if (allOverrides[i].Key.name == stateName)
            {
                allOverrides[i] = new KeyValuePair<AnimationClip, AnimationClip>(allOverrides[i].Key, newAnimation);
                replaced++;
            }
        }

        overrideController.ApplyOverrides(allOverrides);

        if (replaced > 0)
        {
            animator.runtimeAnimatorController = overrideController;
        }
        else
        {
            Debug.LogError($"State '{stateName}' not found in the controller!");
        }
    }



    protected void LookAt(Transform lookAtTarget)
    {
        Vector3 direction = (lookAtTarget.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotspeed);
    }

    private void OnDrawGizmosSelected()
    {

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, lookRadius);

        Gizmos.color = Color.blue;
        for (int i = 0; i < patrolPoints.Length; i++)
        {
            Gizmos.DrawWireSphere(patrolPoints[i].position, 0.3f);
            if (i < patrolPoints.Length - 1)
            {
                Gizmos.DrawLine(patrolPoints[i].position, patrolPoints[i + 1].position);
            }
        }
    }
    public void ResetAnimatorParameters()
    {
        if (animator == null)
        {
            Debug.LogWarning("Animator is not assigned.");
            return;
        }

        for (int i = 0; i < animator.parameterCount; i++)
        {
            AnimatorControllerParameter parameter = animator.GetParameter(i);
            switch (parameter.type)
            {
                case AnimatorControllerParameterType.Trigger:
                    animator.ResetTrigger(parameter.name);
                    break;
                case AnimatorControllerParameterType.Bool:
                    animator.SetBool(parameter.name, false);
                    break;
            }
        }
    }
}
