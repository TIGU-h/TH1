using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyLoader : MonoBehaviour
{
    [SerializeField] private float LoadEnemyRadius = 10f;
    [SerializeField] private LayerMask enemyLayer;
    private List<EnemyAIBase> detectedEnemes = new List<EnemyAIBase>();
    private Dictionary<EnemyAIBase, System.Action> enemyDeathHandlers = new();




    public static System.Random random;


    private void Start()
    {
        random = new System.Random();
        StartCoroutine(loadingEnemy());
    }

    private IEnumerator loadingEnemy()
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
            EnemyAIBase enemy = collider.GetComponent<EnemyAIBase>();
            if (enemy != null)
            {
                float distance = Vector3.Distance(transform.position, enemy.transform.position);

                if (distance < LoadEnemyRadius && !detectedEnemes.Contains(enemy))
                {
                    detectedEnemes.Add(enemy);
                    enemy.LoadEnemy(gameObject);

                    // Створюємо анонімний метод для підписки
                    System.Action deathHandler = () => RemoveEnemy(enemy);
                    enemyDeathHandlers[enemy] = deathHandler; // Зберігаємо посилання

                    enemy.GetComponent<Health>().OnDeath += deathHandler;
                }


            }
        }
        for (int i = 0; i < detectedEnemes.Count; i++)
        {
            if (detectedEnemes[i] != null)
            {

                float distance = Vector3.Distance(transform.position, detectedEnemes[i].transform.position);

                if (distance > LoadEnemyRadius)
                {
                    detectedEnemes[i].UnloadEnemy();
                    detectedEnemes.RemoveAt(i);
                    i--;
                }
            }
        }
    }

    private void RemoveEnemy(EnemyAIBase enemy)
    {
        if (enemy == null) return;

        if (detectedEnemes.Contains(enemy))
        {
            detectedEnemes.Remove(enemy);
        }

        // Відписуємося від події смерті
        if (enemyDeathHandlers.TryGetValue(enemy, out var deathHandler))
        {
            enemy.GetComponent<Health>().OnDeath -= deathHandler;
            enemyDeathHandlers.Remove(enemy);
        }
    }



    private void OnDrawGizmosSelected()
    {

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, LoadEnemyRadius);
    }
    private void OnDestroy()
    {
        foreach (var enemy in enemyDeathHandlers.Keys)
        {
            if (enemy != null)
            {
                enemy.GetComponent<Health>().OnDeath -= enemyDeathHandlers[enemy];
            }
        }

        enemyDeathHandlers.Clear();
    }

}
