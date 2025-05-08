using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WatherShield : SpellActionBase
{
    public override void Cast(ESpell eSpell)
    {
        if (eSpell.sounds != null)
            for (int i = 0; i < eSpell.sounds.Length; i++)
                MultiAudioSourcePlayer.PlaySound(eSpell.sounds[i]);

        transform.GetChild(0).gameObject.SetActive(true);
        StartCoroutine(InvokeWithDelay(() => transform.GetChild(0).gameObject.SetActive(false), 10f));
        
        eSpell.playerWhoCasting.GetComponent<Health>().HealProcent(eSpell.playerWhoCasting.GetComponent<CharacterStats>().Stats.elementProcentBuff[(int)Element.Wather] + 10);
        
    }

    private IEnumerator InvokeWithDelay(System.Action method, float delay)
    {
        yield return new WaitForSeconds(delay);

        method?.Invoke();
    }
}
