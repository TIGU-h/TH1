using UnityEngine;
using System.Collections;
using UnityEngine.AI;
using Unity.VisualScripting;

public class EnemyAIBase : MonoBehaviour
{
    [SerializeField] private Transform[] patrolPoints; // Масив точок патрулювання
    [SerializeField] protected float lookRadius = 10f;
    [SerializeField] private float rotspeed = 1f;

    public bool isLoaded = false;

    protected Transform target;
    protected NavMeshAgent agent;
    private int currentPatrolIndex = 0;
    private bool isChasing = false;

    private Coroutine enemyIdleCoroutine;
    private Coroutine patrolCoroutine;

    public AnimationClip idleClip;
    public AnimationClip runClip;
    public AnimationClip normalAttackClip;
    public AnimationClip heavyAttackClip;

    protected Animator animator;
    protected AnimatorOverrideController overrideController;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        animator = GetComponent<Animator>();

        if (animator == null)
        {
            Debug.LogError("На об'єкті немає Animator!");
            return;
        }

        // Створюємо новий OverrideController на основі існуючого контролера
        overrideController = new AnimatorOverrideController(animator.runtimeAnimatorController);
        animator.runtimeAnimatorController = overrideController;

        ReplaceAnimationClip("idle", idleClip);
        ReplaceAnimationClip("run", runClip);
        ReplaceAnimationClip("normal attack", normalAttackClip);
        ReplaceAnimationClip("heavy attack", heavyAttackClip);

        //overrideController.ApplyOverrides()
    }

    private void ReplaceAnimationClip(string tag, AnimationClip newClip)
    {
        if (newClip == null) return;

        foreach (var clipPair in overrideController.animationClips)
        {
            AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

            if (stateInfo.tagHash == Animator.StringToHash(tag))
            {
                overrideController[clipPair.name] = newClip;
                Debug.Log($"Анімація для {tag} змінена на {newClip.name}");
                break;
            }
        }
    }


    public void LoadEnemy(GameObject player)
    {
        isLoaded = true;
        target = target == null ? player.transform : target;

        if (enemyIdleCoroutine == null)
            enemyIdleCoroutine = StartCoroutine(EnemyIdleBehevior());
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
        yield return new WaitForSeconds(0.1f);
        while (true)
        {

            float distance = Vector3.Distance(target.position, transform.position);
            if (distance <= lookRadius)
            {
                FightLogic(distance);
            }
            if (distance > lookRadius)
            {
                isChasing = false;
                Patrol();
            }

            yield return new WaitForSeconds(1f);
        }
    }
    protected virtual void FightLogic(float distance)
    {
        print("base");
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
            if (animator.GetCurrentAnimatorStateInfo(0).tagHash != Animator.StringToHash("walk"))
                animator.SetTrigger("start walk");
            animator.SetBool("walk", true);

            //LookAt(patrolPoints[currentPatrolIndex].transform);
            agent.SetDestination(patrolPoints[currentPatrolIndex].position);
            currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
        }
    }

    protected void LookAt(Transform lookAtTarget)
    {
        Vector3 direction = (lookAtTarget.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotspeed);
    }
    protected void LookTarget()
    {
        Vector3 direction = (target.position - transform.position).normalized;
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
}
