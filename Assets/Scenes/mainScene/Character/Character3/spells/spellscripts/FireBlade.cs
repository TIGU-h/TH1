using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBlade : SpellActionBase
{
    public override void Cast(ESpell eSpell)
    {
        transform.GetChild(0).gameObject.SetActive(true);
        StartCoroutine(InvokeWithDelay(() => transform.GetChild(0).gameObject.SetActive(false), 1));
        StartCoroutine(InvokeWithDelay(() =>
        {
            GameObject splash =  Instantiate(eSpell.GetPrefabs()[0], transform.position, transform.rotation);
            splash.GetComponent<DamageDiller>().ActorStats = GetComponentInParent<PlayerAttackAndSpellController>().Stats;
            splash.GetComponent<DamageDiller>().AttackScale = 0.8f;
        }, 0.5f));

    }

    private IEnumerator InvokeWithDelay(System.Action method, float delay)
    {
        yield return new WaitForSeconds(delay);

        method?.Invoke();
    }
}
