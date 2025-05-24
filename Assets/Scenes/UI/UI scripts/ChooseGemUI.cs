using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Cinemachine.DocumentationSortingAttribute;
using static UnityEngine.EventSystems.EventTrigger;

public class ChooseGemUI : MonoBehaviour
{
    static string[] elementsNames =
    {
        "Wather",
        "Earth",
        "Fire",
        "Air"
    };
    private Gem gem;
    [SerializeField] Color[] buttonColor;
    [SerializeField] Color[] BGColor;
    [SerializeField] Image bGImage;
    [SerializeField] Image gemImage;
    [SerializeField] TextMeshProUGUI gemMainBuf;
    [SerializeField] TextMeshProUGUI gemStatsBuf;

    public Button chooseButton;
    public void init(Gem gem)
    {
        if (gem == null) return;
        this.gem = gem;
        gemMainBuf.text = gem.mainbuf + "%" + elementsNames[(int)gem.element];
        gemStatsBuf.text = string.Empty;

        if (gem.statBonus.level > 0) gemStatsBuf.text += $"Level: {gem.statBonus.level}\n";
        if (gem.statBonus.MaxHP > 0) gemStatsBuf.text += $"Max HP: {gem.statBonus.MaxHP}\n";
        if (gem.statBonus.AttackPower > 0) gemStatsBuf.text += $"Attack Power: {gem.statBonus.AttackPower}\n";

        if (chooseButton != null) chooseButton.GetComponent<Image>().color = buttonColor[(int)gem.element];
        if (bGImage != null) bGImage.color = BGColor[(int)gem.element];
        if (gemImage != null) gemImage.sprite = gem.icon;

    }
}

