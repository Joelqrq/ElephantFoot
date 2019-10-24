using System;
using UnityEngine;

public class PauseSystem : MonoBehaviour
{
    public GameObject pauseMenu;
    public static Action OnPause;

    private void Awake() {
        OnPause = Pause;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            Pause();
        }
    }

    public void Pause() {
        if (!pauseMenu.activeSelf) {
            Time.timeScale = 0;
            pauseMenu.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        } else {
            Time.timeScale = 1;
            pauseMenu.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}
