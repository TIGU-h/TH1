using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyLoader : MonoBehaviour
{
    public float LoadEnemyRadius = 10f;
    public LayerMask enemyLayer;
    private List<EnemyAI> detectedEnemes = new List<EnemyAI>();

    private void Start()
    {
        StartCoroutine(UpdateNearbyNpcsCoroutine());
    }

    private IEnumerator UpdateNearbyNpcsCoroutine()
    {
        while (true)
        {
            DetectEnemies();
            yield return new WaitForSeconds(0.2f);
        }
    }



    private void DetectEnemies()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, LoadEnemyRadius + 3, enemyLayer);


        foreach (Collider collider in colliders)
        {
            EnemyAI enemy = collider.GetComponent<EnemyAI>();
            if (enemy != null)
            {
                float distance = Vector3.Distance(transform.position, enemy.transform.position);

                if (distance < LoadEnemyRadius && !detectedEnemes.Contains(enemy))
                {
                    detectedEnemes.Add(enemy);
                    enemy.LoadEnemy(gameObject);
                }
                

            }
        }
        for (int i =0; i < detectedEnemes.Count;i++)
        {
            float distance = Vector3.Distance(transform.position, detectedEnemes[i].transform.position);

            if (distance > LoadEnemyRadius )
            {
                detectedEnemes[i].UnloadEnemy();
                detectedEnemes.RemoveAt(i);
                i--;
            }
        }
    }
    private void OnDrawGizmosSelected()
    {

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, LoadEnemyRadius);
    }
}
