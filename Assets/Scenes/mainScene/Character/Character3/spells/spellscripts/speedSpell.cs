using System.Collections;
using UnityEngine;



public class speedSpell : SpellActionBase
{
    public override void Cast(ESpell eSpell)
    {

        if (eSpell.sounds != null)
            for (int i = 0; i < eSpell.sounds.Length; i++)
                MultiAudioSourcePlayer.PlaySound(eSpell.sounds[i]);

        gameObject.GetComponent<ParticleSystem>().Play();

        var animator = eSpell.playerWhoCasting.GetComponent<Animator>();
        StartCoroutine(InvokeWithDelay(() => animator.speed = 2 + 2*eSpell.playerWhoCasting.GetComponent<CharacterStats>().Stats.elementProcentBuff[(int)Element.Air]/100 , gameObject.GetComponent<ParticleSystem>().main.duration));
        StartCoroutine(InvokeWithDelay(() => animator.speed = 1, 5.0f));

    }

    private IEnumerator InvokeWithDelay(System.Action method, float delay)
    {
        yield return new WaitForSeconds(delay);

        method?.Invoke();
    }
}