using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderKeywordFilter;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public List<Gem> gems = new List<Gem>();
    public List<Book> books = new List<Book>();
    public List<Weapon> weapons = new List<Weapon>();

    [SerializeField] private GameObject inventoryUI; // Панель інвентаря
    [SerializeField] Button[] ShowGemsB; // Кнопки для відображення гемів певного елемента
    [SerializeField] GameObject[] ShowGemP; // Панелі для відображення гемів
    [SerializeField] GameObject GemWinPrefab; // Префаб для гемів у UI

    [SerializeField] Button ShowWeaponsB;
    [SerializeField] GameObject ShowWeaponP;


    private void Start()
    {
        Invoke("init", 1f);
    }
    private void init()
    {
        // Прив'язуємо кнопки до відповідних методів
        for (int i = 0; i < ShowGemsB.Length; i++)
        {
            int index = i; // Локальна змінна для збереження індексу
            ShowGemsB[i].onClick.AddListener(() => ShowGemsByElement((Element)index));
        }

    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.B)) // Натискання клавіші "B"
        {
            OpenInventory();
        }
    }

    public void OpenInventory()
    {
        if (inventoryUI != null)
        {
            bool isActive = !inventoryUI.activeSelf;
            inventoryUI.SetActive(isActive);

            if (isActive && ShowGemsB.Length > 0)
            {
                ShowGemsB[0].onClick.Invoke(); // Натискання першої кнопки для відображення гемів
            }
        }
    }

    public void AddItem(Item item)
    {
        if (item is Gem gem)
        {
            Gem newGem = Instantiate(gem);
            gems.Add(newGem);
        }
        else if (item is Book book)
        {
            Book newBook = Instantiate(book);
            books.Add(newBook);
        }
        else if (item is Weapon weapon)
        {
            Weapon newWeapon = Instantiate(weapon);
            weapons.Add(newWeapon);
        }
        else
        {
            Debug.LogWarning("Невідомий тип предмета!");
        }
    }

    private void ShowGemsByElement(Element element)
    {
        int index = (int)element;

        if (index < 0 || index >= ShowGemP.Length)
        {
            Debug.LogWarning("Неправильний індекс панелі для гемів!");
            return;
        }

        // Очищуємо панель перед додаванням нових гемів
        foreach (Transform child in ShowGemP[index].transform)
        {
            Destroy(child.gameObject);
        }

        // Відображаємо лише ті геми, що відповідають вибраному елементу
        foreach (Gem gem in gems)
        {
            if (gem.element == element)
            {
                GameObject gemUI = Instantiate(GemWinPrefab, ShowGemP[index].transform);
                gemUI.GetComponent<ChooseGemUI>().init(gem); // Передаємо гем у UI-об'єкт
            }
        }
    }

    private IEnumerator InvokeWithDelay(System.Action method, float delay)
    {
        yield return new WaitForSeconds(delay);

        method?.Invoke();
    }

}
