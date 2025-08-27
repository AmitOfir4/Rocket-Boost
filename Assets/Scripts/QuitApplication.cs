using UnityEngine;
using UnityEngine.InputSystem;

public class QuitApplication : MonoBehaviour
{
    void Update()
    {
        if (Keyboard.current.escapeKey.isPressed)
        {
            Debug.Log("Escape key is pressed, quitting the application.");
            QuitGame();
        }
    }

    public void QuitGame()
    {
        Debug.Log("Quit Game button clicked, quitting the application.");
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // makes Quit work in editor
#endif
    }
}
