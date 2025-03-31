using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    [SerializeField] private GameObject tabBar;
    public bool activeMouse = false;
    public bool draggingMouse = false;
    void Update()
    {

        if (Input.GetButton("changeSkillType"))
        {
            tabBar.SetActive(true);
            draggingMouse = true;
            activeMouse = false;
            
        }
        else if (Input.GetButtonDown("Enable cursor"))
        {

            draggingMouse = true;
            activeMouse = true;
        }
        else if(Input.GetButtonUp("Enable cursor"))
        {

            draggingMouse = false;
            activeMouse = false;
        }

        if (Input.GetButtonUp("changeSkillType"))
        {
            tabBar.SetActive(false);
            draggingMouse = false;
            activeMouse = false;
        }

        Cursor.visible = activeMouse;
        Cursor.lockState = draggingMouse?CursorLockMode.Confined : CursorLockMode.Locked;




    }
}
