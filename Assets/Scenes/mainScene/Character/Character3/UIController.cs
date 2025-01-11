using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    [SerializeField] private GameObject tabBar;
    void Update()
    {
        if (Input.GetButton("changeSkillType"))
        {
            tabBar.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            tabBar.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
        }

        
    }
    public void deb(char a)
    {
        Debug.Log(a);
    }
}
