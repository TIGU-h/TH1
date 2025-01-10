using MalbersAnimations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackAndSpellController : MonoBehaviour
{


    private bool canAttack = true;
    private Animator animator;
    private CharacterController characterController;
    private ESpell eSpell = new ESpell(3.0f, () => Debug.Log("Casting"), null);
    private float energy = 0;


    void Start()
    {
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        if (Input.GetButtonDown("Fire1") && canAttack)
            Attack();
        if(Input.GetButtonDown("eSpell"))
            eSpell.Cast();

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
}
