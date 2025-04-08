using UnityEngine;

public class ConnectorHandler : MonoBehaviour
{
    private int connectorCount = 0;
    [SerializeField] private AudioClip succesConnectionAudio;

    public void RegisterConnectionAttempt(Connectors a, Connectors b)
    {
        if (a == null || b == null) return;

        bool isValid = IsConnectionValid(a, b);
        connectorCount = isValid ? connectorCount + 1 : connectorCount;

        string msg = isValid ? "✅ Conexión válida" : "❌ Conexión inválida";
        Debug.Log($"{msg} entre {a.connectorType}({a.connectorID}) y {b.connectorType}({b.connectorID}) - Conexiones correctas: {connectorCount / 2}");
        SoundsController.Instance.EjecutarSonido(succesConnectionAudio);
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

    public int ConnectorsCount
    {
        get { return connectorCount; }
        set { connectorCount = value; }
    }
}
