using UnityEngine;

public class TutorialHandler : MonoBehaviour
{
    [SerializeField] private Canvas[] tutorialCanvas;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Desactiva todos los canvas de tutorial al inicio
        foreach (Canvas canvas in tutorialCanvas)
        {
            canvas.gameObject.SetActive(false);
        }

        // Activa el primer canvas de tutorial
        if (tutorialCanvas.Length > 0)
        {
            tutorialCanvas[0].gameObject.SetActive(true);
        }

    }

    public void NextTutorial(int currentIndex)
    {
        // Desactiva el canvas actual
        if (currentIndex < tutorialCanvas.Length)
        {
            tutorialCanvas[currentIndex].gameObject.SetActive(false);
        }
        // Activa el siguiente canvas de tutorial
        if (currentIndex + 1 < tutorialCanvas.Length)
        {
            tutorialCanvas[currentIndex + 1].gameObject.SetActive(true);
        }
    }

    public void PreviousTutorial(int currentIndex)
    {
        // Desactiva el canvas actual
        if (currentIndex < tutorialCanvas.Length)
        {
            tutorialCanvas[currentIndex].gameObject.SetActive(false);
        }
        // Activa el canvas anterior de tutorial
        if (currentIndex - 1 >= 0)
        {
            tutorialCanvas[currentIndex - 1].gameObject.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {

        
    }
}
