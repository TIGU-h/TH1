using Unity.VisualScripting.FullSerializer;
using UnityEngine;

[CreateAssetMenu(fileName = "NewGem", menuName = "Items/Gem")]
public class Gem : Item
{
    public Element element;
    public bool useRandomStats = false;
    [Range(1, 5)] public int gemLevel;
    public Stats statBonus;
    public Sprite[] levelElementSprites;
    public int mainbuf;

    public void InitializeRandomStats()
    {
        if (useRandomStats)
        {
            statBonus = new Stats();
            statBonus.GenerateRandomStats(gemLevel);

        }
        statBonus.HP = 0;
        statBonus.energy = 0;

        icon = levelElementSprites[gemLevel - 1];
        
        mainbuf = gemLevel * 10;
        Debug.Log("asfrg");
    }

   
}
