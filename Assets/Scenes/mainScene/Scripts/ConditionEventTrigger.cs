using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ConditionEventTrigger : MonoBehaviour
{
    [Header("Список об'єктів з Health, які мають померти")]
    public List<Health> targets = new List<Health>();

    [Header("Події, які викликаються після смерті всіх")]
    public UnityEvent onAllDead;

    private HashSet<Health> _deadTargets = new HashSet<Health>();

    private void Start()
    {
        foreach (var target in targets)
        {
            if (target == null)
            {
                Debug.LogWarning("Один з об'єктів у списку targets не заданий!", this);
                continue;
            }

            target.OnDeath += () => OnTargetDied(target);
        }
    }

    private void OnTargetDied(Health target)
    {
        if (_deadTargets.Contains(target)) return;

        _deadTargets.Add(target);
        Debug.Log($"Об'єкт {target.name} помер. {_deadTargets.Count} з {targets.Count}");

        if (_deadTargets.Count >= targets.Count)
        {
            Debug.Log("Всі вороги мертві. Викликаємо події.");
            onAllDead?.Invoke();
        }
    }
}
