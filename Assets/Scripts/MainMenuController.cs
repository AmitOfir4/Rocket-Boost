using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    void Update()
    {
        QuitApplication quitApp = GetComponent<QuitApplication>();
        
    }
    public void StartGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }
}
