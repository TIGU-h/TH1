using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

public class PlayerAttackAndSpellController : MonoBehaviour
{
    private bool canAttack = true;
    private Animator animator;
    private CharacterController characterController;
    [SerializeField]
    private ESpell[] eSpells = new ESpell[4];
    private ESpell activeESpell;
    private float energy = 0;

    //public AnimationClip customAnimation; // Анімація, яку ви передаєте через інспектор


    void Start()
    {
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();


        eSpells[0].SetAction(()=>
        {
            //eanim.Play();
            eSpells[0].GetPrefabs()[0].SetActive(true);
            eSpells[0].GetPrefabs()[0].GetComponent<ParticleSystem>().Play();
            StartCoroutine(changeAnimatorSpeed(5.0f));



        }); 
        eSpells[0].ResetCoolDown();
        activeESpell = eSpells[0];
    }
    private IEnumerator changeAnimatorSpeed(float delay)
    {
        yield return new WaitForSeconds(eSpells[0].GetPrefabs()[0].GetComponent<ParticleSystem>().main.duration);
        animator.speed = 2;
        Debug.Log("Animator speed reset to 2");
        yield return new WaitForSeconds(delay);
        animator.speed = 1; // Повертаємо швидкість до 1
        Debug.Log("Animator speed reset to 1");
    }


    void Update()
    {
        if (Input.GetButtonDown("Fire1") && canAttack)
            Attack();
        if(Input.GetButtonDown("eSpell"))
        {

            eSpells[0].Cast(animator);

        }

    }

    public void Attack()
    {
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

    public void changespellanim(AnimationClip newAnimation)
    {
        string stateName = "idle";

        // Отримуємо існуючий RuntimeAnimatorController
        var runtimeAnimatorController = animator.runtimeAnimatorController;

        // Створюємо AnimatorOverrideController на основі існуючого контролера
        var overrideController = new AnimatorOverrideController(runtimeAnimatorController);

        // Замінюємо потрібний стан на нову анімацію
        var overrides = new List<KeyValuePair<AnimationClip, AnimationClip>>(overrideController.overridesCount);
        overrideController.GetOverrides(overrides);


        bool replaced = false;
        

        for (int i = 0; i < overrides.Count; i++)
        {
            //Debug.Log(runtimeAnimatorController.);

            if (overrides[i].Key.name == stateName)
            {
                overrides[i] = new KeyValuePair<AnimationClip, AnimationClip>(overrides[i].Key, newAnimation);
                replaced = true;
            }
        }

        if (replaced)
        {
            overrideController.ApplyOverrides(overrides);
            animator.runtimeAnimatorController = overrideController;
            Debug.Log($"Animation for state '{stateName}' replaced with '{newAnimation.name}'");
        }
        else
        {
            Debug.LogError($"State '{stateName}' not found in the controller!");
        }
    }
}
