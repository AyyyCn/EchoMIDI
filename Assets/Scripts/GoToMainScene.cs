using UnityEngine.SceneManagement;
using UnityEngine;
public class GoToMainScene : MonoBehaviour
{
    public void LoadMainScene()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
