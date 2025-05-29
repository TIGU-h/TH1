using UnityEngine;
using UnityEngine.SceneManagement;

public class LooseScreenController : MonoBehaviour
{
    public void RetryButton()
    {
        SceneManager.LoadScene(1);

    }
    public void ExitToMainMenu()
    {
        SceneManager.LoadScene(0);
        Time.timeScale = 1.0f;

    }
    public void ExitButton()
    {
        Application.Quit();
    }

}
