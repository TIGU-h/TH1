using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    [Header("Налаштування сундука")]
    public int chestLevel = 1; // Рівень предметів, що випадають
    public int gemCount = 2; // Кількість гемів, що випадають
    public int bookCount = 1; // Кількість книжок, що випадають
    public Weapon guaranteedWeapon; // Гарантована або випадкова зброя (може бути null)

    [Header("Можливі предмети")]
    public List<Gem> possibleGems;
    public List<Book> possibleBooks;
    public List<Weapon> possibleWeapons; // Використовується, якщо гарантованого меча немає


    public void OpenChest(Inventory playerInventory)
    {
        

        // Додаємо випадкові геми
        for (int i = 0; i < gemCount; i++)
        {
            if (possibleGems.Count > 0)
            {
                Gem randomGem = Instantiate(possibleGems[Random.Range(0, possibleGems.Count)]);
                randomGem.gemLevel = chestLevel;
                randomGem.InitializeRandomStats();
                playerInventory.AddItem(randomGem);
                Debug.Log($"В інвентар додано гем: {randomGem.itemName}");
            }
        }

        // Додаємо випадкові книжки
        for (int i = 0; i < bookCount; i++)
        {
            if (possibleBooks.Count > 0)
            {
                Book randomBook = Instantiate(possibleBooks[Random.Range(0, possibleBooks.Count)]);
                playerInventory.AddItem(randomBook);
                Debug.Log($"В інвентар додано книгу: {randomBook.itemName}");
            }
        }

        // Додаємо меч: або гарантований, або випадковий
        if (guaranteedWeapon != null)
        {
            playerInventory.AddItem(guaranteedWeapon);
            Debug.Log($"В інвентар додано гарантовану зброю: {guaranteedWeapon.itemName}");
        }
        else if (possibleWeapons.Count > 0)
        {
            Weapon randomWeapon = Instantiate(possibleWeapons[Random.Range(0, possibleWeapons.Count)]);
            playerInventory.AddItem(randomWeapon);
            Debug.Log($"В інвентар додано випадкову зброю: {randomWeapon.itemName}");
        }
    }
}
