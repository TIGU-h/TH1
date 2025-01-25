using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

public class PlayerAttackAndSpellController : MonoBehaviour
{
    private bool canAttack = true;
    private Animator animator;
    private CharacterController characterController;

    [SerializeField]
    private ESpell[] eSpellsPrefabs;

    private ESpell activeESpell;
    private ESpell[] eSpells;


    [SerializeField]
    private GameObject placeForESpell;

    [SerializeField]
    private AnimationClip curentEspellanimation;

    public GameObject test;

    //private float energy = 0;

    void Start()
    {
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();



        eSpells = new ESpell[eSpellsPrefabs.Length];

        for (int i = 0; i < eSpellsPrefabs.Length; i++)
        {
            if (eSpellsPrefabs[i] != null)
            {
                eSpells[i] = ScriptableObject.CreateInstance<ESpell>();
                eSpells[i].CopyFrom(eSpellsPrefabs[i]);

                eSpells[i].spellAction = Instantiate(eSpellsPrefabs[i].spellAction.gameObject, placeForESpell.gameObject.transform).GetComponent<SpellActionBase>();
                eSpells[i].spellAction.gameObject.SetActive(false);

                eSpells[i].ResetCoolDown();
                eSpells[i].playerWhoCasting = gameObject;

            }
        }

        activeESpell = eSpells[0];
        changespellanim(activeESpell.newAnimationForPlayer);



    }

    void Update()
    {
        if (Input.GetButtonDown("Fire1") && canAttack)
            Attack();
        if (Input.GetButtonDown("eSpell"))
        {
            activeESpell.spellAction.gameObject.SetActive(true);
            Debug.Log("active: " + activeESpell.spellAction.gameObject.name);

            activeESpell.Cast();
        }

    }

    public void Attack()
    {
        GetComponent<PlayerMovementController>().WearponOn();
        animator.SetTrigger("attack");
    }


    //CanNormalAttack
    public void SetCanAttack()
    {
        canAttack = true;
    }
    public void ResetNormal()
    {
        canAttack = false;
    }

    public void ChangeActiveESpell(int index)
    {
        index %= 4;
        activeESpell = eSpells[index];
        changespellanim(activeESpell.newAnimationForPlayer);
    }

    private void changespellanim(AnimationClip newAnimation)
    {
        string stateName = curentEspellanimation.name;

        // Отримуємо існуючий RuntimeAnimatorController
        var runtimeAnimatorController = animator.runtimeAnimatorController;

        // Створюємо AnimatorOverrideController на основі існуючого контролера
        var overrideController = new AnimatorOverrideController(runtimeAnimatorController);

        // Замінюємо потрібний стан на нову анімацію
        var overrides = new List<KeyValuePair<AnimationClip, AnimationClip>>(overrideController.overridesCount);
        overrideController.GetOverrides(overrides);

        int replaced = 0;

        for(int l = 0; l<animator.layerCount; l++)
        {
            // Отримуємо список станів для кожного шару
            var layerOverrides = new List<KeyValuePair<AnimationClip, AnimationClip>>(overrides);
            overrideController.GetOverrides(layerOverrides);

            for (int i = 0; i < layerOverrides.Count; i++)
            {
                if (layerOverrides[i].Key.name == stateName)
                {
                    layerOverrides[i] = new KeyValuePair<AnimationClip, AnimationClip>(layerOverrides[i].Key, newAnimation);
                    replaced++;
                }
            }

            // Замінюємо оригінальні оверрайди
            overrideController.ApplyOverrides(layerOverrides);
        }

        if (replaced > 0)
        {
            animator.runtimeAnimatorController = overrideController;
            Debug.Log($"Animation for state '{stateName}' replaced with '{newAnimation.name}' {replaced} times.");
        }
        else
        {
            Debug.LogError($"State '{stateName}' not found in the controller!");
        }
    }

}