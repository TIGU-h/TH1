using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    public Stats BaseStats;
    public Stats Stats;

    private List<Gem> equippedGems = new List<Gem>();
    private Weapon equippedWeapon;

    private void Awake()
    {
        Stats = new Stats();
        UpdateStats();
    }

    public void EquipGem(Gem gem)
    {
        if (gem != null)
        {
            equippedGems.Add(gem);
            UpdateStats();
        }
    }

    public void EquipWeapon(Weapon weapon)
    {
        equippedWeapon = weapon;
        UpdateStats();
    }

    private void UpdateStats()
    {
        Stats.level = BaseStats.level;
        Stats.MaxHP = BaseStats.MaxHP;
        Stats.HP = BaseStats.HP;
        Stats.AttackPower = BaseStats.AttackPower;
        Stats.energy = BaseStats.energy;

        // Додаємо бонуси від каменів
        foreach (var gem in equippedGems)
        {
            Stats.MaxHP += gem.statBonus.MaxHP;
            Stats.AttackPower += gem.statBonus.AttackPower;
        }

        // Додаємо бонуси від зброї
        if (equippedWeapon != null)
        {
            Stats.AttackPower += equippedWeapon.baseAttackPower;
            Stats.MaxHP += equippedWeapon.bonusStats.MaxHP;
        }
    }
}
[System.Serializable]
public class Stats
{
    public int level;
    public int MaxHP;
    public int HP;
    public int AttackPower;
    public float energy;

    public void GenerateRandomStats(int itemLevel)
    {
        level = itemLevel;
        MaxHP = Random.Range(5 * itemLevel, 10 * itemLevel);
        HP = MaxHP;
        AttackPower = Random.Range(2 * itemLevel, 5 * itemLevel);
        energy = Random.Range(1f * itemLevel, 3f * itemLevel);
    }
}
