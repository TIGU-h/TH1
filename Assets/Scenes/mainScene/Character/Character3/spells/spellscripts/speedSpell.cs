using System.Collections;
using UnityEngine;



public class speedSpell : SpellActionBase
{
    public override void Cast(ESpell eSpell)
    {
        //gameObject.SetActive(true);
        gameObject.GetComponent<ParticleSystem>().Play();

        var animator = eSpell.playerWhoCasting.GetComponent<Animator>();
        StartCoroutine(InvokeWithDelay(() => animator.speed = 2, gameObject.GetComponent<ParticleSystem>().main.duration));
        StartCoroutine(InvokeWithDelay(() => animator.speed = 1, 5.0f));

        //gameObject.SetActive(false);
    }

    private IEnumerator InvokeWithDelay(System.Action method, float delay)
    {
        yield return new WaitForSeconds(delay);

        method?.Invoke();
    }
}