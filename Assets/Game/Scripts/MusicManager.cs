#pragma warning disable CS0649
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    [SerializeField] private AudioClip[] menuAudios;
    [SerializeField] private AudioClip[] gameAudios;
    private static MusicManager instance;
    private AudioSource source;

    private void Awake() {
        if (instance == null) {
            instance = this;
            source = GetComponent<AudioSource>();
            DontDestroyOnLoad(gameObject);
            ButtonHandler.mm = this;
            PlayMenuAudio();
        } else {
            Destroy(gameObject);
        }
    }

    public void PlayMenuAudio() {
        source.Stop();
        source.clip = menuAudios[Random.Range(0, menuAudios.Length)];
        source.Play();
    }

    public void PlayGameAudio() {
        source.Stop();
        source.clip = gameAudios[Random.Range(0, gameAudios.Length)];
        source.Play();
    }
}
