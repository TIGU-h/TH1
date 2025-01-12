using System;
using UnityEngine;

[System.Serializable]
public class ESpell
{
    [SerializeField]
    private float cooldown;

    [SerializeField]
    private GameObject[] prefabs;

    [SerializeField]
    public AnimationClip newAnimation;

    private float lastCastTime;
    private Action spellAction;


    public ESpell(float cooldown, Action spellAction, GameObject[] prefabs = null)
    {
        this.cooldown = cooldown;
        this.spellAction = spellAction;
        this.prefabs = prefabs ?? Array.Empty<GameObject>();
        lastCastTime = -cooldown;
    }

    // Метод для активації навику
    public void Cast(Animator animator)
    {
        if (IsOnCooldown())
        {
            Debug.Log("Spell is on cooldown!");
            return;
        }
        animator.SetTrigger("e");

        lastCastTime = Time.time;
        spellAction?.Invoke();

        Debug.Log("Spell cast successfully!");
    }

    // Перевірка, чи знаходиться навик на кулдауні
    public bool IsOnCooldown()
    {
        return Time.time - lastCastTime < cooldown;
    }

    // Метод для отримання масиву префабів
    public GameObject[] GetPrefabs()
    {
        return prefabs;
    }

    // Публічний метод для установки дії (Action) через код
    public void SetAction(Action newAction)
    {
        spellAction = newAction;
    }
    public void ResetCoolDown()
    {
        lastCastTime = -cooldown;
    }
}

