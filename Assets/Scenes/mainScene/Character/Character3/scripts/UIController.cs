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
        else if (Input.GetButton("Enable cursor"))
        {
            //canRotate = false;
            //if(Cursor.lockState!=CursorLockMode.None)
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            //canRotate = true;

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        // Коли кнопка відпускається
        if (Input.GetButtonUp("changeSkillType"))
        {
            tabBar.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
        }



    }
}
