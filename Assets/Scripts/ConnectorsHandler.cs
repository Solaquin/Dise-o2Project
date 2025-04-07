using UnityEngine;

public class ConnectorHandler : MonoBehaviour
{
    public void RegisterConnectionAttempt(Connectors a, Connectors b)
    {
        if (a == null || b == null) return;

        bool isValid = IsConnectionValid(a, b);

        string msg = isValid ? "✅ Conexión válida" : "❌ Conexión inválida";
        Debug.Log($"{msg} entre {a.connectorType}({a.connectorID}) y {b.connectorType}({b.connectorID})");
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
            // Puede conectarse con el siguiente ID
            return Mathf.Abs(idA - idB) == 1;
        }

        return false;
    }
}
