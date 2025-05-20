using UnityEngine;

public enum ConnectorType { Start, Intermediate, End }

public class Connectors : MonoBehaviour
{
    public byte connectorID;
    public ConnectorType connectorType;
    public bool isConnected = false;

    [SerializeField] private ConnectorHandler connectorHandler;
    [HideInInspector] public Connectors connectorInContact;

    private void OnTriggerEnter(Collider other)
    {
        Connectors otherConnector = other.GetComponent<Connectors>();
        if (otherConnector != null)
        {
            connectorInContact = otherConnector;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Connectors otherConnector = other.GetComponent<Connectors>();

        if (otherConnector != null && connectorInContact == otherConnector)
        {
            connectorInContact = null;
            connectorHandler.UnregisterConnection(this, otherConnector);
        }
        
    }
}
