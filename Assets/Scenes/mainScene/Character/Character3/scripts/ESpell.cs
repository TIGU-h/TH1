using System;
using UnityEngine;

[System.Serializable, CreateAssetMenu(fileName = "E_Spell")]

public class ESpell : ScriptableObject
{
    [SerializeField] int id;
    //[SerializeField] string name;
    [SerializeField] string description;
    [SerializeField] Element element;
    public Sprite icon;

    [SerializeField]
    private float cooldown;

    [SerializeField]
    private GameObject[] prefabs;


    public AudioClip[] sounds;

    [SerializeField]
    public AnimationClip newAnimationForPlayer;


    [SerializeField] public SpellActionBase spellAction;


    [HideInInspector] public GameObject playerWhoCasting;
    private float lastCastTime;


    public void CopyFrom(ESpell eSpell)
    {

        id = eSpell.id;
        name = eSpell.name;
        description = eSpell.description;
        element = eSpell.element;
        icon = eSpell.icon;

        cooldown = eSpell.cooldown;
        prefabs = eSpell.prefabs ?? Array.Empty<GameObject>();
        newAnimationForPlayer = eSpell.newAnimationForPlayer;
        sounds = eSpell.sounds ?? Array.Empty<AudioClip>();

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
        playerWhoCasting.GetComponent<Animator>().SetTrigger("e");

        spellAction.gameObject.SetActive(true);

        lastCastTime = Time.time;
        spellAction?.Cast(this);
        

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
    public float GetTimeForCoolDown()
    {
        return Time.time - lastCastTime;
    }
    public float GetCoolDown()
    {
        return cooldown;
    }
}

public enum Element
{
    Wather,
    Earth,
    Fire,
    Air
}

