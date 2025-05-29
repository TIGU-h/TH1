using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAttackAndSpellController : MonoBehaviour
{
    private bool canAttack = true;
    private Animator animator;
    private CharacterController characterController;

    [SerializeField]
    private ESpell[] eSpellsPrefabs;

    private ESpell activeESpell;
    private ESpell[] eSpells;
    private Transform focusTarget;
    [SerializeField] private float searchRadius; // Радіус пошуку цілі для фокусування
    [SerializeField] private float attackRange;
    [SerializeField] int speed;

    [SerializeField]
    private GameObject placeForESpell;

    [SerializeField]
    private AnimationClip curentEspellanimation;

    public GameObject test;

    public GameObject weapon;
    [SerializeField] private Image CDimage;
    [SerializeField] private Text CDtext;

    [SerializeField] private Button[] ElementButtons;
    [SerializeField] private AudioClip[] normalAttackSounds;





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
        weapon.GetComponent<DamageDiller>().ActorStats = GetComponent<CharacterStats>().Stats;
        GetComponent<Health>().SetStats(GetComponent<CharacterStats>().Stats);

        if (CDimage != null && CDtext != null)
        {
            CDimage.sprite = activeESpell.icon;

            if (activeESpell.IsOnCooldown())
            {
                CDtext.gameObject.SetActive(true);
                CDtext.text = ((activeESpell.GetCoolDown() - activeESpell.GetTimeForCoolDown())).ToString("F1");
                CDimage.fillAmount = activeESpell.GetTimeForCoolDown() / activeESpell.GetCoolDown();
                CDimage.GetComponent<Button>().interactable = false;
            }
            else
            {

                CDtext.gameObject.SetActive(false);
                CDimage.fillAmount = 100;
                CDimage.GetComponent<Button>().interactable = true;


            }

        }

        HandleSelection(0);







    }
    public void UseSpell()
    {
        activeESpell.spellAction.gameObject.SetActive(true);

        activeESpell.Cast();
    }

    void Update()
    {
        if (Input.GetButtonDown("Fire1") && canAttack)
            Attack();
        if (Input.GetButtonDown("eSpell"))
            UseSpell();

        if (CDimage != null && CDtext != null)
        {

            if (activeESpell.IsOnCooldown())
            {
                CDtext.gameObject.SetActive(true);
                CDtext.text = ((activeESpell.GetCoolDown() - activeESpell.GetTimeForCoolDown())).ToString("F1");
                CDimage.fillAmount = activeESpell.GetTimeForCoolDown() / activeESpell.GetCoolDown();
                CDimage.GetComponent<Button>().interactable = false;
            }
            else
            {

                CDtext.gameObject.SetActive(false);
                CDimage.fillAmount = 100;
                CDimage.GetComponent<Button>().interactable = true;


            }

        }



        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            HandleSelection(0);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            HandleSelection(1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            HandleSelection(2);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            HandleSelection(3);
        }





    }

    public void HandleSelection(int index)
    {

        for (int i = 0; i < ElementButtons.Length; i++)
        {
            float scale = (i == index) ? 1.5f : 1f;
            ElementButtons[i].transform.localScale = new Vector3(scale, scale, 1f);
        }
        ChangeActiveESpell(index+1);
    }


    private IEnumerator InvokeInLoopWithDelay(System.Action method, float delay)
    {
        while (true)
        {
            method?.Invoke();
            yield return new WaitForSeconds(delay);
        }
    }

    public void Attack()
    {
        FocusOnTarget();
        GetComponent<PlayerAttackAndSpellController>().WearponOn();
        animator.SetTrigger("attack");
    }
    public void playAttackSound()
    {
        MultiAudioSourcePlayer.PlaySound(normalAttackSounds[Random.Range(0, normalAttackSounds.Length)]);
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

    public void WearponOn()
    {
        weapon.SetActive(true);
        playAttackSound();
    }
    public void WeaponOff()
    {
        weapon.SetActive(false);
        WearponTrailOFF();

    }

    public void WearponTrailOn(float AttackScale)
    {

        weapon.GetComponent<DamageDiller>().AttackScale = AttackScale;
        weapon.GetComponentInChildren<TrailRenderer>().emitting = true;

    }
    public void WearponTrailOFF()
    {
        weapon.GetComponentInChildren<TrailRenderer>().emitting = false;


    }

    private void ChangeActiveESpell(int index)
    {
        index %= 4;
        activeESpell = eSpells[index];
        changespellanim(activeESpell.newAnimationForPlayer);

        if (CDimage != null && CDtext != null)
        {
            CDimage.sprite = activeESpell.icon;

            if (activeESpell.IsOnCooldown())
            {
                CDtext.gameObject.SetActive(true);
                CDtext.text = ((activeESpell.GetCoolDown() - activeESpell.GetTimeForCoolDown())).ToString("F1");
                CDimage.fillAmount = activeESpell.GetTimeForCoolDown() / activeESpell.GetCoolDown();
                CDimage.GetComponent<Button>().interactable = false;
            }
            else
            {

                CDtext.gameObject.SetActive(false);
                CDimage.fillAmount = 100;
                CDimage.GetComponent<Button>().interactable = true;


            }

        }


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

        for (int l = 0; l < animator.layerCount; l++)
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
        }
        else
        {
            Debug.LogError($"State '{stateName}' not found in the controller!");
        }
    }

    void FocusOnTarget()
    {
        if (focusTarget != null && Vector3.Distance(transform.position, focusTarget.position) < searchRadius / 2)
        {
            if (Input.GetAxisRaw("Horizontal") < 0.2 && Input.GetAxisRaw("Vertical") < 0.2)
            {

                Vector3 direction = focusTarget.position - transform.position; // Отримуємо напрямок до цілі
                direction.y = 0; // Ігноруємо зміну висоти (Y)
                if (direction != Vector3.zero) // Перевіряємо, щоб напрямок не був нульовим
                {
                    transform.rotation = Quaternion.LookRotation(direction);
                }

            }
            return;
        }
        int enemyLayer = LayerMask.GetMask("Enemy");
        Collider[] colliders = Physics.OverlapSphere(transform.position, searchRadius, enemyLayer);

        if (colliders.Length > 0)
        {
            Transform nearestEnemy = null;
            float minDistance = Mathf.Infinity;

            foreach (Collider collider in colliders)
            {
                float distance = Vector3.Distance(transform.position, collider.transform.position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    nearestEnemy = collider.transform;
                }
            }

            if (nearestEnemy != null)
            {
                focusTarget = nearestEnemy;
                if (Input.GetAxisRaw("Horizontal") < 0.2 && Input.GetAxisRaw("Vertical") < 0.2)
                {

                    Vector3 direction = focusTarget.position - transform.position; // Отримуємо напрямок до цілі
                    direction.y = 0; // Ігноруємо зміну висоти (Y)
                    if (direction != Vector3.zero) // Перевіряємо, щоб напрямок не був нульовим
                    {
                        transform.rotation = Quaternion.LookRotation(direction);
                    }

                }

            }
        }
    }
    public void Create(GameObject gameObject)
    {
        Instantiate(gameObject);
    }

    private IEnumerator InvokeWithDelay(System.Action method, float delay)
    {
        yield return new WaitForSeconds(delay);

        method?.Invoke();
    }

}
