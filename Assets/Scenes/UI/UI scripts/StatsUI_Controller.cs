using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class StatsUI_Controller : MonoBehaviour
{
    public Button statsButton;
    public Button talismansButton;
    [SerializeField] private TextMeshProUGUI HpText;
    [SerializeField] private TextMeshProUGUI APText;
    [SerializeField] private TextMeshProUGUI[] statsElements;
    [SerializeField] private Image[] talismans;
    [SerializeField] private Animator characterAnimator;
    [SerializeField] private GameObject StatsCamera;


    [HideInInspector] public CharacterStats StatsPlayer;



    public void ShowStats()
    {
        EventSystem.current.SetSelectedGameObject(statsButton.gameObject);

        if (characterAnimator != null)
            characterAnimator.SetTrigger("to idle");

        HpText.text = StatsPlayer.Stats.MaxHP.ToString();
        APText.text = StatsPlayer.Stats.AttackPower.ToString();

        for (int i = 0; i < 4; i++)
        {
            statsElements[i].text = StatsPlayer.Stats.elementProcentBuff[i] + "%";
        }
    }

    public void ShowTalismans()
    {


        if (characterAnimator != null)
            characterAnimator.SetTrigger("to talisman");

        for (int i = 0; i < 4; i++)
        {
            Gem tal = StatsPlayer.GetEquippedGem(i);
            if (tal != null)
                talismans[i].sprite = tal.icon;

        }
    }

    public void Exit()
    {
        gameObject.SetActive(false);
        StatsCamera.SetActive(false);

        GetComponentInParent<UIController>().activeMouse = false;
        GetComponentInParent<UIController>().draggingMouse = false;
    }
}