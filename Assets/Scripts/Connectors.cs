using UnityEngine;

public enum ConnectorType { Start, Intermediate, End }

public class Connectors : MonoBehaviour
{
    public byte connectorID;
    public ConnectorType connectorType;

    [SerializeField] private ConnectorHandler connectorHandler;

    private void OnTriggerEnter(Collider other)
    {
        Connectors otherConnector = other.GetComponent<Connectors>();

        connectorHandler.RegisterConnectionAttempt(this, otherConnector);
    }
}
