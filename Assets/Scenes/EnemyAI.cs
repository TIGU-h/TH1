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

    private Transform target;
    private NavMeshAgent agent;
    private int currentPatrolIndex = 0;
    private bool isChasing = false;


    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }



    public void loadEnemy(GameObject player)
    {
        target = target == null ? player.transform : target;
        StartCoroutine(EnemyIdleBehevior());
        StartCoroutine(PatrolRoutine());
    }

    public void unloadEnemy()
    {
        target = null;
        StopCoroutine(EnemyIdleBehevior());
        StopCoroutine(PatrolRoutine());
        //StopAllCoroutines();

    }
    private IEnumerator EnemyIdleBehevior()
    {
        while (true)
        {

            float distance = Vector3.Distance(target.position, transform.position);

            //if (distance <= LookRadius)
            //{

            //    agent.SetDestination(transform.position);

            //    if (distance <= agent.stoppingDistance)
            //    {

            //        LookTarget();

            //    }

            //}


            // гпт
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
            //Debug.Log("patrolPoints.Length = " + patrolPoints.Length);
            LookAt(patrolPoints[currentPatrolIndex].transform);
        }
    }

    void LookAt(Transform lookAtTarget)
    {
        Vector3 direction = (lookAtTarget.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }
    void LookTarget()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
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
