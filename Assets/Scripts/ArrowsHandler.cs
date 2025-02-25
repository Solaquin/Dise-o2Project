using Unity.VisualScripting;
using UnityEngine;

public class ArrowsHandler : MonoBehaviour
{
    [SerializeField] private float lineLength = 0.5f;

    private LineRenderer[] lines; // Array para almacenar las 6 líneas
    private GameObject[] lineColliders; // Array para almacenar los colliders de las líneas
    private Vector3[] directions; // Direcciones de las líneas
    private bool translationMode = true; // true = traslación, false = rotación
    private PiecesMovement activePiece; // Almacena la pieza actualmente seleccionada
    private bool isProcessing = false; // Indica si hay una pieza en movimiento



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        directions = new Vector3[]
        {
            Vector3.up, Vector3.down, Vector3.left, Vector3.right, Vector3.forward, Vector3.back
        };

        lines = new LineRenderer[6];
        lineColliders = new GameObject[6];
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            translationMode = true;
            Debug.Log("Modo: Traslación");
            UpdateArrows();
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            translationMode = false;
            Debug.Log("Modo: Rotación");
            UpdateArrows();
        }
    }

    public void createLines(Transform obj)
    {
        HideLines();

        // Crear las líneas y los colliders
        for (int i = 0; i < 6; i++)
        {
            GameObject lineObject = new GameObject("Line_" + i);
            lines[i] = lineObject.AddComponent<LineRenderer>();
            lines[i].positionCount = 5; // 5 puntos para la línea con punta de flecha
            lines[i].startWidth = 0.05f;
            lines[i].endWidth = 0.05f;
            lines[i].material = new Material(Shader.Find("Sprites/Default")); // Material básico
            lines[i].startColor = Color.green;
            lines[i].endColor = Color.green;
            lines[i].enabled = false; // Ocultar al inicio

            // Crear el collider para la línea
            lineColliders[i] = new GameObject("LineCollider_" + i);
            lineColliders[i].transform.SetParent(lineObject.transform);
            BoxCollider collider = lineColliders[i].AddComponent<BoxCollider>();
            collider.isTrigger = true; // No debe afectar la física
            lineColliders[i].SetActive(false);
        }
    }

    public void ShowLines(Transform obj)
    {
        if (translationMode)
        {
            // Mostrar todas las flechas de traslación (VERDES)
            for (int i = 0; i < 6; i++)
                DrawArrow(lines[i], directions[i], Color.green, obj);
        }
        else
        {
            // Mostrar solo las flechas de rotación (ROJAS)
            DrawArrow(lines[0], Vector3.up, Color.red, obj); // Rotación en Y
            DrawArrow(lines[2], Vector3.left, Color.red, obj); // Rotación en X
            DrawArrow(lines[5], Vector3.forward, Color.red, obj); // Rotación en X
        }
    }

    public void DrawArrow(LineRenderer line, Vector3 direction, Color color, Transform obj)
    {
        Vector3 startPos = obj.position;
        Vector3 endPos = obj.position + direction * lineLength;

        line.enabled = true;
        line.SetPosition(0, obj.position);
        line.SetPosition(1, obj.position + direction * lineLength);
        line.positionCount = 5; // 1 l�nea + 2 l�neas de la punta
        line.SetPosition(0, startPos);
        line.SetPosition(1, endPos);

        // Crear la base de la flecha
        Vector3 arrowHeadBase = endPos - direction * (lineLength * 0.2f); // Base de la punta

        Vector3 arrowLeft, arrowRight;

        // Determinar la rotaci�n de la punta de flecha seg�n la direcci�n
        if (direction == Vector3.up || direction == Vector3.down)
        {
            // Para flechas en el eje Y, rotamos en el plano XZ
            arrowLeft = arrowHeadBase + Quaternion.Euler(45, 0, 0) * (-direction * 0.1f);
            arrowRight = arrowHeadBase + Quaternion.Euler(-45, 0, 0) * (-direction * 0.1f);
        }
        else
        {
            // Para las dem�s direcciones, rotamos en el plano XY
            arrowLeft = arrowHeadBase + Quaternion.Euler(0, 45, 0) * (-direction * 0.1f);
            arrowRight = arrowHeadBase + Quaternion.Euler(0, -45, 0) * (-direction * 0.1f);
        }

        // Dibujar la punta de la flecha
        line.SetPosition(2, arrowLeft);
        line.SetPosition(3, endPos);
        line.SetPosition(4, arrowRight);

        // Configurar los colliders en la posici�n correcta
        GameObject lineCollider = lineColliders[System.Array.IndexOf(directions, direction)];
        lineCollider.SetActive(true);
        lineCollider.transform.position = obj.position + direction * (lineLength / 2);
        lineCollider.transform.position = obj.position + direction * (lineLength / 2);
        lineCollider.transform.rotation = Quaternion.LookRotation(direction);
        lineCollider.GetComponent<BoxCollider>().size = new Vector3(0.1f, 0.1f, lineLength);
    }

    public void HideLines()
    {
        if (lines != null)
        {
            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i] != null)
                {
                    Destroy(lines[i].gameObject);
                }
            }
            lines = new LineRenderer[6];
        }

        if (lineColliders != null)
        {
            for (int i = 0; i < lineColliders.Length; i++)
            {
                if (lineColliders[i] != null)
                {
                    Destroy(lineColliders[i]);
                }
            }
            lineColliders = new GameObject[6];
        }

    }
    public GameObject[] getLinesColliders()
    {
        return lineColliders;
    }

    public bool getTranslationMode()
    {
        return translationMode;
    }

    public Vector3 getDirection(int index)
    {
        return directions[index];
    }
    public void SetActivePiece(PiecesMovement piece)
    {
        if (isProcessing)
            return;

        // Si hay otra pieza activa, la deseleccionamos
        if (activePiece != null)
        {
            HideLines();
            activePiece.DeselectPiece(); // Opcional: limpiar estado de la pieza anterior
        }

        // Establecemos la nueva pieza como activa
        activePiece = piece;

        // Mostrar las flechas de la nueva pieza
        createLines(piece.transform);
    }

    public PiecesMovement GetActivePiece()
    {
        return activePiece;
    }

    public bool IsProcessing()
    {
        return isProcessing;
    }

    // Método para activar o desactivar el estado de movimiento
    public void SetProcessing(bool value)
    {
        isProcessing = value;
    }

    private void UpdateArrows()
    {
        if (activePiece != null) // Si hay una pieza seleccionada
        {
            HideLines(); // Ocultamos las flechas actuales

            if (lines == null || lines.Length == 0 || lines[0] == null)
            {
                createLines(activePiece.transform);
            }

            ShowLines(activePiece.transform); // 🔥 Redibujar flechas
        }
    }

}
