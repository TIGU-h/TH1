using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField] private GameObject tabBar;
    [SerializeField] private GameObject StatsPanel;
    [SerializeField] private GameObject StatsCamera;
    [SerializeField] private PauseController PausePanel;
    [SerializeField] private GameObject playerParent;
    public bool activeMouse = false;
    public bool draggingMouse = false;

    void Update()
    {
        Cursor.visible = activeMouse;
        Cursor.lockState = draggingMouse ? CursorLockMode.Confined : CursorLockMode.Locked;

        if (playerParent.GetComponentInChildren<CinemachineBrain>() != null)
        {
            playerParent.GetComponentInChildren<CinemachineBrain>().enabled = !(activeMouse || draggingMouse);
            //playerParent.GetComponentInChildren<PlayerAttackAndSpellController>().enabled = !(activeMouse || draggingMouse);
        }
        if (playerParent.GetComponentInChildren<PlayerMovementController>()!=null)
        {
            if((activeMouse || draggingMouse) && playerParent.GetComponentInChildren<PlayerMovementController>().enabled)
                playerParent.GetComponentInChildren<PlayerMovementController>().ResetAnimatorParameters();
            playerParent.GetComponentInChildren<PlayerMovementController>().enabled = !(activeMouse || draggingMouse);

        }

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
        else if (Input.GetButtonUp("Enable cursor"))
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


        if (Input.GetButtonUp("ShowStats"))
        {
            StatsPanel.SetActive(true);
            StatsCamera.SetActive(true);
            StatsPanel.GetComponent<StatsUI_Controller>().statsButton.onClick.Invoke();

            activeMouse = true;
            draggingMouse = true;
        }
        if (Input.GetButtonUp("Cancel"))
        {
            Pause();
        }



    }
    public void Pause()
    {
        draggingMouse = true;
        activeMouse = true;
        PausePanel.gameObject.SetActive(true);
        Time.timeScale = 0;
    }
}
