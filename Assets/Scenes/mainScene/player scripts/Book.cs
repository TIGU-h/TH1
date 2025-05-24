using UnityEngine;

[CreateAssetMenu(fileName = "NewBook", menuName = "Items/Book")]
public class Book : Item
{
    public int experienceAmount;
    public Sprite[] levelSprites;

    public Sprite GetSprite()
    {
        int index = Mathf.Clamp(experienceAmount / 10, 0, levelSprites.Length - 1);
        return levelSprites[index];
    }
}
