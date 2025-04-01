using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement; // Importa el espacio de nombres para la gestión de escenas

public class ScreenChange : MonoBehaviour
{
    public void ChangeScene(string sceneName)
    {
        // Cambia a la escena especificada
        SceneManager.LoadScene(sceneName);
    }
}