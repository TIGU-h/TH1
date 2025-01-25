using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    [SerializeField] private GameObject tabBar;
    void Update()
    {
        // Коли кнопка натискається
        if (Input.GetButton("changeSkillType"))
        {
            tabBar.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
        }

        // Коли кнопка відпускається
        if (Input.GetButtonUp("changeSkillType"))
        {
            tabBar.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
        }


    }
}
