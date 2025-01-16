using System;
using UnityEngine;

[System.Serializable, CreateAssetMenu(fileName ="E_Spell")]

public class ESpell : ScriptableObject
{
    [SerializeField]
    private float cooldown;

    [SerializeField]
    private GameObject[] prefabs;

    [SerializeField]
    public AnimationClip newAnimationForPlayer;


    [SerializeField] public SpellActionBase spellAction;


    [HideInInspector] public GameObject playerWhoCasting;
    private float lastCastTime;


    public void CopyFrom(ESpell eSpell)
    {
        cooldown = eSpell.cooldown;
        prefabs = eSpell.prefabs ?? Array.Empty<GameObject>();
        newAnimationForPlayer = eSpell.newAnimationForPlayer;

        lastCastTime = -cooldown;
    }

    // Метод для активації навику
    public void Cast()
    {
        if (IsOnCooldown())
        {
            Debug.Log("Spell is on cooldown!");
            return;
        }
        spellAction.gameObject.SetActive(true);

        playerWhoCasting.GetComponent<Animator>().SetTrigger("e");

        lastCastTime = Time.time;
        spellAction.Cast(this);

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


    public void ResetCoolDown()
    {
        lastCastTime = -cooldown;
    }
}

