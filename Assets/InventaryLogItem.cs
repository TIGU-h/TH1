using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventaryLogItem : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI text;

    public void SetData(Sprite image, string name)
    {
        if (icon != null)
        {
            icon.sprite = image;
        }
        if (text != null)
        {
            text.text = name;
        }
    }

    public void SetData(Gem gem)
    {
        if (icon != null)
        {
            icon.sprite = gem.icon;
        }
        if (text != null)
        {
            text.text = gem.itemName;
        }
    }

}
