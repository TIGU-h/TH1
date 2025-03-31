using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    public Stats BaseStats;
    public Stats Stats;

    private Gem[] equippedGems = new Gem[4];
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
            //if (equippedGems[(int)gem.element] != null)
            equippedGems[(int)gem.element] = gem;

            UpdateStats();
        }
    }
    public Gem GetEquippedGem(int element)
    {
        return equippedGems[(int)element];
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
            if (gem != null)
            {
                Stats.MaxHP += gem.statBonus.MaxHP;
                Stats.AttackPower += gem.statBonus.AttackPower;

            }

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
    public float scaleWithLevel;
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

    public void ScaleStatsByLevel()
    {
        float scale = level * scaleWithLevel;
        MaxHP = MaxHP + (int)(MaxHP * scale);
        HP = MaxHP;
        AttackPower = AttackPower + (int)(AttackPower * scale);
        energy = energy + energy * scale;
    }

}
