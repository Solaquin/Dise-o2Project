using UnityEngine;

public class ArrowButton : MonoBehaviour
{
    [HideInInspector] public Vector3 direction; // La dirección asignada a esta flecha
    [HideInInspector] public ArrowsHandler arrowsHandler; // Referencia al manejador de flechas

    // Se usa OnMouseDown para detectar el clic (asegúrate de que el prefab tenga Collider)
    private void OnMouseDown()
    {
        if (arrowsHandler != null)
        {
            arrowsHandler.MoveActivePiece(direction);
        }
    }
}
