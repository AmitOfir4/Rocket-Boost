using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour
{
    [SerializeField] float levelLoadDelay = 2f;

    [SerializeField] AudioClip CrashSound;
    [SerializeField] AudioClip SuccessSound;
    [SerializeField] ParticleSystem successParticles;
    [SerializeField] ParticleSystem crashParticles;
    AudioSource audioSource;
    bool isControllable = true;
    bool isCollidable = true;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        RespondToDebugKeys();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (!isControllable || !isCollidable)
        {
            Debug.Log("C key is press, collision is disabled.");
            return;
        }

        switch (collision.gameObject.tag)
        {
            case "Friendly":
                Debug.Log("Friendly collision detected!");
                break;
            case "Fuel":
                HandleFuelCollision(collision);
                break;
            case "Finish":
                StartSucessSequence();
                break;
            default:
                StartCrashSequence();
                break;
        }
    }

    void HandleFuelCollision(Collision collision)
    {
        Debug.Log("Fuel collected!");
        Destroy(collision.gameObject); // Remove the fuel object from the scene
    }

    void ReloadLevel()
    {
        int currentScene = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentScene);
    }

    void LoadNextLevel()
    {
        GetComponent<Movement>().enabled = false;
        int currentScene = SceneManager.GetActiveScene().buildIndex;

        int nextScene = currentScene + 1;

        if (nextScene == SceneManager.sceneCountInBuildSettings)
        {
            nextScene = 0;
        }

        SceneManager.LoadScene(nextScene);
    }

    void LoadPreviousLevel()
    {
        GetComponent<Movement>().enabled = false;
        int currentScene = SceneManager.GetActiveScene().buildIndex;

        int previousScene = currentScene - 1;

        if (previousScene < 1)
        {
            previousScene = 1;
        }

        SceneManager.LoadScene(previousScene);
    }

    void StartCrashSequence()
    {
        isControllable = false;
        audioSource.Stop();
        PlayParticles(crashParticles);
        PlaySound(CrashSound);
        DisableMovement();
        Invoke("ReloadLevel", levelLoadDelay);
    }

    void StartSucessSequence()
    {
        isControllable = false;
        audioSource.Stop();
        PlayParticles(successParticles);
        PlaySound(SuccessSound);
        DisableMovement();
        Invoke("LoadNextLevel", levelLoadDelay);
    }

    void DisableMovement()
    {
        GetComponent<Movement>().enabled = false;
    }

    private void PlaySound(AudioClip soundToPlay)
    {
        audioSource.PlayOneShot(soundToPlay);
    }

    private void PlayParticles(ParticleSystem particles)
    {
        if (particles != null)
        {
            particles.Play();
        }
    }

    private void RespondToDebugKeys()
    {
        if (Keyboard.current.lKey.wasPressedThisFrame)
        {
            LoadNextLevel();
        }
        else if (Keyboard.current.rKey.wasPressedThisFrame)
        {
            ReloadLevel();
        }
        else if (Keyboard.current.cKey.wasPressedThisFrame)
        {
            isCollidable = !isCollidable;
        }
        else if (Keyboard.current.pKey.wasPressedThisFrame)
        {
            LoadPreviousLevel();
        }
    }
}
