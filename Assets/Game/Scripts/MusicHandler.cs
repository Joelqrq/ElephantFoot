using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicHandler : MonoBehaviour
{
    public GameObject music;
    void Update()
    {
        DontDestroyOnLoad(music);
        
        if(SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Map"))
        {
            GameObject character = GameObject.FindGameObjectWithTag("Player");
            music.transform.position = character.transform.position;
        }
    }
}
