using MalbersAnimations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerAttackAndSpellController : MonoBehaviour
{


    private bool canAttack = true;
    private Animator animator;
    private CharacterController characterController;
    [SerializeField]
    private ESpell eSpell;
    private float energy = 0;


    void Start()
    {
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();

        eSpell.SetAction(()=>
        {
            animator.SetTrigger("e");
            eSpell.GetPrefabs()[0].SetActive(true);
            eSpell.GetPrefabs()[0].GetComponent<ParticleSystem>().Play();
            StartCoroutine(changeAnimatorSpeed(5.0f));



        }); 
    }
    private IEnumerator changeAnimatorSpeed(float delay)
    {
        yield return new WaitForSeconds(eSpell.GetPrefabs()[0].GetComponent<ParticleSystem>().main.duration);
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

            eSpell.Cast();

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
}
