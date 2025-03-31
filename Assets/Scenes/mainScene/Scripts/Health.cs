using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;


public class Health : MonoBehaviour
{
    [SerializeField] private Slider healthSlider;
    [SerializeField] private bool rotate;
    [SerializeField] private Text text;
    [SerializeField] private TextMeshProUGUI level;

    public Stats statsRef { get; private set; } // Посилання на Stats


    public event Action OnDeath;



    private void Awake()
    {
        init();
    }
    private void init()
    {
        if (healthSlider != null && statsRef != null)
        {
            healthSlider.maxValue = statsRef.MaxHP;
            healthSlider.value = statsRef.HP;
            if (rotate)
                StartCoroutine(FaceCamera());

            if (text != null)
            {
                text.text = statsRef.HP + "/" + statsRef.MaxHP;
            }
            if (level != null)
            {
                level.text = "Lv. " + statsRef.level;
            }
        }
    }
    public void SetStats(Stats stats)
    {
        statsRef = stats;
        init();
    }
    private IEnumerator FaceCamera()
    {
        while (true)
        {
            if (healthSlider != null && Camera.main != null)
            {
                healthSlider.transform.LookAt(Camera.main.transform);
            }
            yield return new WaitForSeconds(0.1f);
        }
    }
    public void SetMaxHP(int newMaxHP, bool updateCurrentHP = true)
    {
        statsRef.MaxHP = Mathf.Max(1, newMaxHP);
        if (updateCurrentHP)
        {
            statsRef.HP = statsRef.MaxHP;
        }
        if (healthSlider != null)
        {
            healthSlider.maxValue = statsRef.MaxHP;
            healthSlider.value = statsRef.HP;
        }
    }

    public void SetCurrentHP(int newHP)
    {
        statsRef.HP = Mathf.Clamp(newHP, 0, statsRef.MaxHP);
        if (healthSlider != null)
        {
            healthSlider.value = statsRef.HP;
        }
        if (statsRef.HP == 0)
        {
            Die();
        }
    }

    public void TakeDamage(int damage)
    {
        SetCurrentHP(statsRef.HP - damage);
        if (text != null)
        {
            text.text = statsRef.HP + "/" + statsRef.MaxHP;
        }
    }

    public void Heal(int healAmount)
    {
        SetCurrentHP(statsRef.HP + healAmount);
        if (text != null)
        {
            text.text = statsRef.HP + "/" + statsRef.MaxHP;
        }
    }

    private void Die()
    {
        OnDeath?.Invoke();
        Debug.Log($"{gameObject.name} помер!");
        // Додаткові дії при смерті (наприклад, вимкнення об'єкта)
    }
}
