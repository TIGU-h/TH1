using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ESpell
{
    // Кулдаун навика
    private float cooldown;
    private float lastCastTime;

    // Лямбда-функція, що задає логіку навику
    private Action spellAction;

    // Масив префабів, які можна використовувати для навику
    private GameObject[] prefabs;

    // Конструктор для створення навику
    public ESpell(float cooldown, Action spellAction, GameObject[] prefabs = null)
    {
        this.cooldown = cooldown;
        this.spellAction = spellAction;
        this.prefabs = prefabs ?? Array.Empty<GameObject>();
        lastCastTime = -cooldown; // Дозволяє одразу використовувати навик після створення
    }

    // Метод для активації навику
    public void Cast()
    {
        if (IsOnCooldown())
        {
            Debug.Log("Spell is on cooldown!");
            return;
        }

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
}
