using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageDiller : MonoBehaviour
{
    [SerializeField] private LayerMask targetMask;
    [SerializeField] private GameObject hitEffectPrefab;
    [SerializeField] private GameObject canvasPrefab;
    [SerializeField] private TypeOfDamage typeOfDamage;
    [SerializeField] public bool haveToDelete = false;

    public Stats ActorStats;


    private List<GameObject> targets = new List<GameObject>();
    private float attackScale;
    public float effectScale = 0.1f;
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
    private bool wasEffect = false;


    private void OnTriggerEnter(Collider other)
    {

        if (haveToDelete)
        {

            Vector3 hitPosition = other.ClosestPoint(transform.position);
            if (hitEffectPrefab != null)
            {
                GameObject effect = Instantiate(hitEffectPrefab, hitPosition, Quaternion.identity);
                effect.transform.localScale *= effectScale;
                Destroy(effect, 1f);
                wasEffect = true;

            }
        }

        if (((1 << other.gameObject.layer) & targetMask) != 0 && !targets.Contains(other.gameObject) && GetComponentInChildren<TrailRenderer>().emitting)
        {
            Vector3 hitPosition = other.ClosestPoint(transform.position);

            var hp = other.GetComponent<Health>();
            if (hp != null)
            {
                if (hp.statsRef.HP <= 0)
                {
                    return;
                }
                int damage = (int)(attackScale * ActorStats.AttackPower);
                if (typeOfDamage != TypeOfDamage.Phisical)
                    damage  = (int)((float)damage * (1f + (ActorStats.elementProcentBuff[(int)typeOfDamage - 1]/100f)));
                damage++;


                hp.TakeDamage(damage);
                if (canvasPrefab != null)
                {


                    GameObject canvas = Instantiate(canvasPrefab, hitPosition, Quaternion.identity);
                    canvas.GetComponent<DamageText>().Setup(damage, typeOfDamage);
                }
            }

            targets.Add(other.gameObject);
            if (hitEffectPrefab != null && !wasEffect)
            {

                GameObject effect = Instantiate(hitEffectPrefab, hitPosition, Quaternion.identity);
                effect.transform.localScale *= effectScale;
                Destroy(effect, 1f);


            }
        }
        if (haveToDelete)
        {


            gameObject.SetActive(false);
        }
    }

}
