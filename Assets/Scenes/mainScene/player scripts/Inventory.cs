using System.Collections;
using System.Collections.Generic;
using Unity.Android.Gradle;
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
    [SerializeField] ChooseGemUI[] CurentGems;

    [SerializeField] Button ShowWeaponsB;
    [SerializeField] GameObject ShowWeaponP;
    CharacterStats ch;


    //********     LOG
    public GameObject gemLogPrefab;
    [SerializeField] private Sprite expLogItemSprite;

    public Transform logContainer;
    [SerializeField] private float logInterval = 0.3f;
    [SerializeField] private float DestroyLogTime = 1f;

    private Queue<System.Action> logQueue = new Queue<System.Action>();
    private bool isLogging = false;



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
            ch = GetComponent<CharacterStats>();
            ShowGemsB[i].onClick.AddListener(() =>
            {
                if (ch.GetEquippedGem(index) != null)
                    CurentGems[index].init(ch.GetEquippedGem(index));
                ShowGemsByElement((Element)index);

            });
        }

    }
    /*
     
     
    public void Log(Gem gem)
    {
        var logItem = Instantiate(gemLogPrefab, logContainer);
        logItem.GetComponent<InventaryLogItem>().SetData(gem);
        Destroy(logItem, 0.5f);

    }
    public void Log(int exp)
    {
        var logItem = Instantiate(gemLogPrefab, logContainer);
        logItem.GetComponent<InventaryLogItem>().SetData(expLogItemSprite, "+"+exp);
        Destroy(logItem, 0.5f);

    }
    
     
     */



    public void Log(Gem gem)
    {
        logQueue.Enqueue(() => CreateGemLog(gem));
        TryStartLogging();
    }

    public void Log(int exp)
    {
        logQueue.Enqueue(() => CreateExpLog(exp));
        TryStartLogging();
    }

    private void TryStartLogging()
    {
        if (!isLogging)
        {
            StartCoroutine(ProcessLogQueue());
        }
    }

    private IEnumerator ProcessLogQueue()
    {
        isLogging = true;

        while (logQueue.Count > 0)
        {
            var logAction = logQueue.Dequeue();
            logAction.Invoke();
            yield return new WaitForSeconds(logInterval);
        }

        isLogging = false;
    }

    private void CreateGemLog(Gem gem)
    {
        var logItem = Instantiate(gemLogPrefab, logContainer);
        logItem.GetComponent<InventaryLogItem>().SetData(gem);
        Destroy(logItem, DestroyLogTime);
    }

    private void CreateExpLog(int exp)
    {
        var logItem = Instantiate(gemLogPrefab, logContainer);
        logItem.GetComponent<InventaryLogItem>().SetData(expLogItemSprite, "+" + exp);
        Destroy(logItem, DestroyLogTime);
    }

    //***************    LOG
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.B)) // Натискання клавіші "B"
        {
            OpenInventory();
        }
        if (inventoryUI.activeSelf)
        {
            inventoryUI.GetComponentInParent<UIController>().activeMouse = true;
            inventoryUI.GetComponentInParent<UIController>().draggingMouse = true;

        }

    }

    public void OpenInventory()
    {
        if (inventoryUI != null)
        {
            bool isActive = !inventoryUI.activeSelf;

            inventoryUI.GetComponentInParent<UIController>().activeMouse = isActive;
            inventoryUI.GetComponentInParent<UIController>().draggingMouse = isActive;

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
            Log(gem);
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
                gemUI.GetComponent<ChooseGemUI>().chooseButton.onClick.AddListener(() =>
                {
                    GetComponent<CharacterStats>().EquipGem(gem);
                    CurentGems[(int)element].init(GetComponent<CharacterStats>().GetEquippedGem((int)element));
                });



            }
        }
    }

    private IEnumerator InvokeWithDelay(System.Action method, float delay)
    {
        yield return new WaitForSeconds(delay);

        method?.Invoke();
    }

}
