using System.Collections;
using UnityEngine;

public class PointsHandler : MonoBehaviour
{
    [SerializeField] private int points = 0; // Points variable to keep track the conections
    [SerializeField] private Canvas succesCanva;
    private ConnectorHandler connectorHandler;
    [SerializeField] AudioClip levelPassed;


    void Start()
    {
        connectorHandler = GetComponent<ConnectorHandler>();
        succesCanva.gameObject.SetActive(false); // Hide the canvas at the start
    }

    // Update is called once per frame
    void Update()
    {
        if (connectorHandler.ConnectorsCount >= 3)
        {
            StartCoroutine(EsperarYReproducirSonido(3f)); // Wait 0.5 seconds before playing the sound
        }
    }

    IEnumerator EsperarYReproducirSonido(float secs)
    {
        yield return new WaitForSeconds(secs);
        SoundsController.Instance.EjecutarSonido(levelPassed);
        succesCanva.gameObject.SetActive(true);
    }

}
