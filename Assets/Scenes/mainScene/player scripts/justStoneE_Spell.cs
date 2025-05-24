using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class justStoneE_Spell : SpellActionBase
{
    [SerializeField] float timeforcreate;
    [SerializeField] float AttackScale;
    public override void Cast(ESpell eSpell)
    {
        if (eSpell.sounds != null)
            for (int i = 0; i < eSpell.sounds.Length; i++)
                MultiAudioSourcePlayer.PlaySound(eSpell.sounds[i]);

        transform.GetChild(0).gameObject.SetActive(true);
        StartCoroutine(InvokeWithDelay(() => transform.GetChild(0).gameObject.SetActive(false), 1));
        StartCoroutine(InvokeWithDelay(() =>
        {
            GameObject stone = Instantiate(eSpell.GetPrefabs()[0], transform.position, transform.rotation);
            stone.GetComponent<DamageDiller>().ActorStats = eSpell.playerWhoCasting.GetComponent<CharacterStats>().Stats;
            stone.GetComponent<DamageDiller>().AttackScale = AttackScale;
        }, timeforcreate));
    }
    private IEnumerator InvokeWithDelay(System.Action method, float delay)
    {
        yield return new WaitForSeconds(delay);

        method?.Invoke();
    }
}
