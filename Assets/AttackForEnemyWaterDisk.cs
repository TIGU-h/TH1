using System.Collections;
using UnityEngine;

public class AttackForEnemyWaterDisk : MonoBehaviour
{
    [Header("Attack Settings")]
    public GameObject waterDiskPrefab;
    public Transform spawnPoint;





    public void FireDisk()
    {
        GameObject disk = Instantiate(waterDiskPrefab, spawnPoint.position, Quaternion.identity);

        // Передаємо ціль у снаряд (він сам її використає при старті)
        WaterDisk diskScript = disk.GetComponent<WaterDisk>();
        if (diskScript != null)
        {
            diskScript.SetTarget(GetComponent<DefoltPuppet>().getTarget());
        }
        disk.GetComponent<DamageDiller>().ActorStats = GetComponent<DefoltPuppet>().Stats;
        disk.GetComponent<DamageDiller>().AttackScale = 1;
    }
}
