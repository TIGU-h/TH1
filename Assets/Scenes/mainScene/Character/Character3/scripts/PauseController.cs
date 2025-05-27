using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

public class PauseController : MonoBehaviour
{
    public void ContinueButton()
    {
        Time.timeScale = 1.0f;
        GetComponent<Animator>().SetTrigger("exitpause");
    }
    public void ExitPause()
    {
        gameObject.SetActive(false);

    }
    public void ExitToMainMenu()
    {
        SceneManager.LoadScene(0);
    }
    public void ExitButton()
    {
        Application.Quit();
    }

    private IEnumerator InvokeWithDelay(System.Action method, float delay)
    {
        yield return new WaitForSeconds(delay);

        method?.Invoke();
    }
}
