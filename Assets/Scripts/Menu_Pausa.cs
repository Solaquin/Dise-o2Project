using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Menu_Pausa : MonoBehaviour
{

    private bool pausado = false;
    [SerializeField] private GameObject canva;
    [SerializeField] private ScreenChange sceneManager;

    private Crono crono;

    void Start()
    {
        crono = GetComponent<Crono>();
        canva.SetActive(false);
    }


    public void Update()
    {

        if (Input.GetKeyDown(KeyCode.Escape) && crono.FailedLevel == false)
        {

            AlternarPausa();

        }

    }

    public void AlternarPausa()
    {

        pausado = !pausado;

        if (pausado)
        {
            canva.SetActive(true);
            Time.timeScale = 0f;

        }
        else
        {
            canva.SetActive(false);
            Time.timeScale = 1f;
        }
    }

    public void Pausar()
    {
        Debug.Log("Pausar");
    }

    public void Continuar()
    {
        Debug.Log("Continuar");
        canva.SetActive(false);
        Time.timeScale = 1f;
    }


    public void Reiniciar()
    {
        Debug.Log("Reiniciar");
        sceneManager.ChangeScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1f;
    }


    public void Salir()
    {
        canva.SetActive(false);
        Time.timeScale = 1f;
        sceneManager.ChangeScene("Menu Principal");
    }
}
