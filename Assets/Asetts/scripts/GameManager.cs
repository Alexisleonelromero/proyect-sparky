using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private void Update()
    {
        // Verifica si la tecla R ha sido presionada
        if (Input.GetKeyDown(KeyCode.R))
        {
            RestartScene();
        }
    }

    private void RestartScene()
    {
        // Obtén el nombre de la escena actual
        string currentSceneName = SceneManager.GetActiveScene().name;

        // Vuelve a cargar la escena actual
        SceneManager.LoadScene(currentSceneName);
    }
}
