using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WatherShield : SpellActionBase
{
    public override void Cast(ESpell eSpell)
    {
        StartCoroutine(InvokeWithDelay(() => gameObject.SetActive(false), 10f));
        
    }

    private IEnumerator InvokeWithDelay(System.Action method, float delay)
    {
        yield return new WaitForSeconds(delay);

        method?.Invoke();
    }
}
