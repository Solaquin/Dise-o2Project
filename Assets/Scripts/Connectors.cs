using UnityEngine;

public enum ConnectorType { Start, Intermediate, End }

public class Connectors : MonoBehaviour
{
    public byte connectorID;
    public ConnectorType connectorType;
    public bool isConnected = false;

    [SerializeField] private ConnectorHandler connectorHandler;

    private void OnTriggerEnter(Collider other)
    {
        Connectors otherConnector = other.GetComponent<Connectors>();

        connectorHandler.RegisterConnectionAttempt(this, otherConnector);
    }

    private void OnTriggerExit(Collider other)
    {
        Connectors otherConnector = other.GetComponent<Connectors>();

        connectorHandler.UnregisterConnection(this, otherConnector);
    }
}
