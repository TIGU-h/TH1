using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageDiller : MonoBehaviour
{
    [SerializeField] private LayerMask targetMask;
    [SerializeField] private GameObject hitEffectPrefab;


    private List<GameObject> targets = new List<GameObject>();
    private float attackScale;
    public float afsadfasefasef = 0.1f;
    public float AttackScale
    {
        get => attackScale;
        set
        {
            if (value < 0)
                value = 0;
            attackScale = value;

            targets = new List<GameObject>();


        }
    }



    private void OnTriggerEnter(Collider other)
    {
        if (((1 << other.gameObject.layer) & targetMask) != 0 && !targets.Contains(other.gameObject))
        {
            print("damage to " + other.gameObject.name + " : " + attackScale);
            targets.Add(other.gameObject);
            if (hitEffectPrefab != null)
            {

                Vector3 hitPosition = other.ClosestPoint(transform.position);
                GameObject effect = Instantiate(hitEffectPrefab, hitPosition, Quaternion.identity);
                effect.transform.localScale *= afsadfasefasef;
                Destroy(effect, 1f);
            }
        }
    }
}
