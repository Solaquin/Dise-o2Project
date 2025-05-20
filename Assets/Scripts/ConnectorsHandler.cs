using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ConnectorHandler : MonoBehaviour
{
    [SerializeField] private AudioClip succesConnectionAudio;
    [SerializeField] private AudioClip failConnectionAudio;
    [SerializeField] private PointsHandler pointsHandler;
    [SerializeField] private TextMeshProUGUI enlaceCorrectoText;


    private void Start()
    {
        enlaceCorrectoText.gameObject.SetActive(false);
    }
    public void RegisterConnectionAttempt(Connectors a, Connectors b)
    {
        if (a == null || b == null) return;

        bool isValid = IsConnectionValid(a, b);

        if (isValid)
        {
            SoundsController.Instance.EjecutarSonido(succesConnectionAudio);
            a.isConnected = true;
            b.isConnected = true;

            StartCoroutine(MostrarMensaje(2f));
            pointsHandler.CheckLevelCompletion();
        }
        else
        {
            SoundsController.Instance.EjecutarSonido(failConnectionAudio);
        }
    }

    public void UnregisterConnection(Connectors a, Connectors b)
    {
        if (a == null || b == null) return;

        a.isConnected = false;
        b.isConnected = false;

    }

    private bool IsConnectionValid(Connectors a, Connectors b)
    {
        int idA = a.connectorID;
        int idB = b.connectorID;

        // START
        if (a.connectorType == ConnectorType.Start)
            return b.connectorID == idA + 1;

        // END
        if (a.connectorType == ConnectorType.End)
            return b.connectorID == idA - 1;

        // INTERMEDIATE
        if (a.connectorType == ConnectorType.Intermediate)
        {
            if (b.connectorType == ConnectorType.Intermediate)
                return idA == idB; // Mismo ID entre intermedios

            // Puede conectarse con el siguiente ID
            return Mathf.Abs(idA - idB) == 1;
        }

        return false;
    }

    IEnumerator MostrarMensaje(float secs)
    {
        enlaceCorrectoText.gameObject.SetActive(true);

        // Esperar antes de empezar el fade
        yield return new WaitForSeconds(secs);

        // Guardar el color original
        Color originalColor = enlaceCorrectoText.color;

        float duration = 1f; // Tiempo del fade
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, elapsed / duration);
            enlaceCorrectoText.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            yield return null;
        }

        // Asegura que esté completamente transparente
        enlaceCorrectoText.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0f);
        enlaceCorrectoText.gameObject.SetActive(false);

        //Restaurar el color original
        enlaceCorrectoText.color = originalColor;
    }

}
