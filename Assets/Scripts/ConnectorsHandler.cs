using System.Collections.Generic;
using UnityEngine;

public class ConnectorHandler : MonoBehaviour
{
    [SerializeField] private AudioClip succesConnectionAudio;
    [SerializeField] private AudioClip failConnectionAudio;
    [SerializeField] private PointsHandler pointsHandler;

    private List<(Connectors, Connectors)> activeConnections = new();

    public void RegisterConnectionAttempt(Connectors a, Connectors b)
    {
        if (a == null || b == null) return;

        bool isValid = IsConnectionValid(a, b);

        if (isValid)
        {
            SoundsController.Instance.EjecutarSonido(succesConnectionAudio);
            a.isConnected = true;
            b.isConnected = true;

            activeConnections.Add((a, b));
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
}
