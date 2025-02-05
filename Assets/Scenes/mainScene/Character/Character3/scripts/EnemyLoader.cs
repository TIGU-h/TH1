using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLoader : MonoBehaviour
{
    public float LoadEnemyRadius = 10f;
    public LayerMask enemyLayer;
    private List<EnemyAI> detectedEnemes = new List<EnemyAI>();
    private void Update()
    {
        DetectEnemies();
    }



    private void DetectEnemies()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, LoadEnemyRadius + 1, enemyLayer);
        EnemyAI nearestEnemy = null;
        float nearestDistance = float.MaxValue;

        foreach (Collider collider in colliders)
        {
            EnemyAI enemy = collider.GetComponent<EnemyAI>();
            if (enemy != null)
            {
                float distance = Vector3.Distance(transform.position, enemy.transform.position);

                if (distance < LoadEnemyRadius && !detectedEnemes.Contains(enemy))
                {
                    detectedEnemes.Add(enemy);
                    enemy.loadEnemy(gameObject);
                }
                else if(distance > LoadEnemyRadius && detectedEnemes.Contains(enemy))
                {
                    enemy.unloadEnemy();
                    detectedEnemes.Remove(enemy);
                }
            }
        }

        //if (nearestEnemy != detectedEnemy)
        //{
        //    if (detectedEnemy != null)
        //    {
        //        detectedEnemy.unloadEnemy();
        //    }

        //    detectedEnemy = nearestEnemy;

        //    if (detectedEnemy != null)
        //    {
        //        detectedEnemy.loadEnemy(gameObject);
        //    }
        //}
    }
    private void OnDrawGizmosSelected()
    {

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, LoadEnemyRadius);
    }
}
