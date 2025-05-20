using System.Collections;
using UnityEngine;

public class PointsHandler : MonoBehaviour
{
    [SerializeField] private int points = 0;
    [SerializeField] private Canvas succesCanva;
    private ConnectorHandler connectorHandler;
    [SerializeField] private AudioClip levelPassed;
    [SerializeField] private bool soundIsPlaying = false;

    private bool alreadyTriggered = false;

    void Start()
    {
        connectorHandler = GetComponent<ConnectorHandler>();
        succesCanva.gameObject.SetActive(false);
    }


    public void CheckLevelCompletion()
    {
        if (alreadyTriggered) return;

        GameObject[] allConnectors = GameObject.FindGameObjectsWithTag("Connector");
        foreach (var connector in allConnectors)
        {
            Connectors connectorScript = connector.GetComponent<Connectors>();
            if (connectorScript == null || !connectorScript.isConnected)
                return;
        }

        alreadyTriggered = true;
        StartCoroutine(EsperarYReproducirSonido(3f));
    }


    IEnumerator EsperarYReproducirSonido(float secs)
    {
        yield return new WaitForSeconds(secs);
        if (!soundIsPlaying)
        {
            SoundsController.Instance.EjecutarSonido(levelPassed);
            soundIsPlaying = true;
        }
        succesCanva.gameObject.SetActive(true);
    }
}
