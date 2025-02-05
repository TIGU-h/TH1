using UnityEngine;
using System.Collections;
using UnityEngine.AI;
using Unity.VisualScripting;

public class EnemyAI : MonoBehaviour
{
    public Transform[] patrolPoints; // Масив точок патрулювання
    public float lookRadius = 10f;
    public float waitTime = 2f;
    public float speed = 3.5f;
    public bool isLoaded = false;

    private Transform target;
    private NavMeshAgent agent;
    private int currentPatrolIndex = 0;
    private bool isChasing = false;

    private Coroutine enemyIdleCoroutine;
    private Coroutine patrolCoroutine;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }


    public void LoadEnemy(GameObject player)
    {
        isLoaded = true;
        target = target == null ? player.transform : target;

        if (enemyIdleCoroutine == null)
            enemyIdleCoroutine = StartCoroutine(EnemyIdleBehevior());

        if (patrolCoroutine == null)
            patrolCoroutine = StartCoroutine(PatrolRoutine());
    }

    public void UnloadEnemy()
    {
        target = null;

        if (enemyIdleCoroutine != null)
        {
            StopCoroutine(enemyIdleCoroutine);
            enemyIdleCoroutine = null;
        }

        if (patrolCoroutine != null)
        {
            StopCoroutine(patrolCoroutine);
            patrolCoroutine = null;
        }

        StopAllCoroutines();
        isLoaded = false;
    }
    private IEnumerator EnemyIdleBehevior()
    {
        while (true)
        {

            float distance = Vector3.Distance(target.position, transform.position);
            if (distance <= lookRadius)
            {
                isChasing = true;
                agent.SetDestination(target.position);
                LookTarget();
            }
            else
            {
                isChasing = false;
                Patrol();
            }

            yield return new WaitForSeconds(1f);
        }
    }
    private IEnumerator PatrolRoutine()
    {
        while (true)
        {
            Patrol();
            yield return new WaitForSeconds(1f);
        }
    }

    private void Patrol()
    {
        if (patrolPoints.Length == 0 || isChasing) return;

        if (!agent.pathPending && agent.remainingDistance < 1.5f)
        {
            agent.SetDestination(patrolPoints[currentPatrolIndex].position);
            currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
            LookAt(patrolPoints[currentPatrolIndex].transform);
        }
    }

    void LookAt(Transform lookAtTarget)
    {
        Vector3 direction = (lookAtTarget.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime );
    }
    void LookTarget()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime );
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
