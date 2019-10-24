using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonHandler : MonoBehaviour
{
    public static MusicManager mm;

    public void StartButton()
    {
        StartCoroutine(StartButtonTimer());
    }

    public void ExitButton()
    {
        StartCoroutine(ExitButtonTimer());
    }

    public void BackButton()
    {
        mm.PlayMenuAudio();
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }

    public void RestartButton()
    {
        mm.PlayGameAudio();
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false; 
        SceneManager.LoadScene(1);
    }

    IEnumerator StartButtonTimer()
    {
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene(1);
        mm.PlayGameAudio();
    }

    IEnumerator ExitButtonTimer()
    {
        yield return new WaitForSeconds(2);
        Application.Quit();
    }
}
