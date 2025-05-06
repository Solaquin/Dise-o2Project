using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement; // Importa el espacio de nombres para la gestión de escenas

public class ScreenChange : MonoBehaviour
{
    public string[] sceneNames;

    public void ChangeScene(string sceneName)
    {
        // Cambia a la escena especificada
        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    }

    public void ChangeSceneRandom(int level)
    {
        int randomIndexScene = Random.Range(0, 2);
        if (level == 1)
        {
            if (randomIndexScene == 0)
                SceneManager.LoadScene(sceneNames[0], LoadSceneMode.Single);
            else
                SceneManager.LoadScene(sceneNames[1], LoadSceneMode.Single);
        }
    }
}