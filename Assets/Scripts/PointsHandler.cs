using System.Collections;
using TMPro;
using TMPro.Examples;
using UnityEngine;

public class PointsHandler : MonoBehaviour
{
    [SerializeField] private int points = 0;
    private float timeToReach = 0f;
    [SerializeField] private Canvas succesCanva;
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private TextMeshProUGUI pointsText;
    [SerializeField] private CameraOrbit cameraOrbit;



    private ConnectorHandler connectorHandler;
    private Crono crono;

    [SerializeField] private AudioClip levelPassed;
    [SerializeField] private bool soundIsPlaying = false;

    private bool alreadyTriggered = false;

    void Start()
    {
        connectorHandler = GetComponent<ConnectorHandler>();
        crono = GetComponent<Crono>();
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

        cameraOrbit.enabled = false;
        crono.stopTime();
        timeToReach = crono.totalTime - crono.getActualTime();
        points += Mathf.FloorToInt(crono.getActualTime());
        pointsText.text = points.ToString();
        timeText.text = crono.parseTimer(timeToReach);
        
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
