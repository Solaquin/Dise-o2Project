using UnityEngine;

public class Connectors : MonoBehaviour
{
    [SerializeField] private byte connectorID;
    [SerializeField] private bool isStartConnector;
    [SerializeField] private bool isStopConnector;

    private Connectors otherConnection;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public byte ConnectorID => connectorID;
    byte getNextConnectorID() { return connectorID++; }
    byte getPastConnectorID() { return connectorID--; }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Connector"))
            return;

        otherConnection = other.GetComponent<Connectors>();

        if (otherConnection == null)
        {
            Debug.LogWarning("El objeto colisionado no tiene el componente Connectors.");
            return;
        }

        // Caso para conector de inicio
        if (isStartConnector)
        {
            if ((connectorID + 1) == otherConnection.ConnectorID)
                Debug.Log($"Conexion entre {name} y {other.gameObject.name} correcta");
            else
                Debug.Log($"Conexion entre {name} y {other.gameObject.name} incorrecta");
        }
        // Caso para conector de fin
        else if (isStopConnector)
        {
            if ((connectorID - 1) == otherConnection.ConnectorID)
                Debug.Log($"Conexion entre {name} y {other.gameObject.name} correcta");
            else
                Debug.Log($"Conexion entre {name} y {other.gameObject.name} incorrecta");
        }
        // Caso neutro (sin flag de inicio o fin)
        else
        {
            if ((connectorID + 1) == otherConnection.ConnectorID || (connectorID - 1) == otherConnection.ConnectorID)
                Debug.Log($"Conexion entre {name} y {other.gameObject.name} correcta");
            else
                Debug.Log($"Conexion entre {name} y {other.gameObject.name} incorrecta");
        }
    }
}
