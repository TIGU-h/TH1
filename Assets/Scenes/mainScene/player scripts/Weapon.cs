using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

[CreateAssetMenu(fileName = "NewWeapon", menuName = "Items/Weapon")]
public class Weapon : Item
{
    public Mesh weaponMesh;
    public int baseAttackPower;
    public Stats bonusStats;
}
