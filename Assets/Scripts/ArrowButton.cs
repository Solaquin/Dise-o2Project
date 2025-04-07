using UnityEngine;

public class ArrowButton : MonoBehaviour
{
    [HideInInspector] public Vector3 direction; // La direcci�n asignada a esta flecha
    [HideInInspector] public ArrowsHandler arrowsHandler; // Referencia al manejador de flechas

    // Se usa OnMouseDown para detectar el clic (aseg�rate de que el prefab tenga Collider)
    private void OnMouseDown()
    {
        if (arrowsHandler != null)
        {
            arrowsHandler.MoveActivePiece(direction);
        }
    }
}
