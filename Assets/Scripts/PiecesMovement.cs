using UnityEngine;

public class PiecesMovement : MonoBehaviour
{
    public float moveSpeed = 5f; // Velocidad de movimiento de la bola
    public float rotationAngle = 90f; // Ángulo de inclinación de la bola  
    [SerializeField] private float lineLength = 0.5f;

    private LineRenderer[] lines; // Array para almacenar las 6 líneas
    private GameObject[] lineColliders; // Array para almacenar los colliders de las líneas
    private Vector3[] directions; // Direcciones de las líneas
    private bool linesVisible = false; // Indica si las líneas están visibles
    private int selectedDirection = -1; // Dirección seleccionada (-1 = ninguna)
    private bool isMoving = false; // Indica si la bola está en movimiento
    private bool isRotating = false;
    private bool translationMode = true; // true = traslación, false = rotación

    private CameraOrbit cameraOrbit;

    void Start()
    {
        // Inicializar las direcciones de las líneas
        directions = new Vector3[]
        {
            Vector3.up, Vector3.down, Vector3.left, Vector3.right, Vector3.forward, Vector3.back
        };

        // Inicializar el array de líneas y colliders
        lines = new LineRenderer[6];
        lineColliders = new GameObject[6];

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

        // Obtener la referencia al script CameraOrbit
        cameraOrbit = Camera.main.GetComponent<CameraOrbit>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            translationMode = true;
            Debug.Log("Modo: Traslación");
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            translationMode = false;
            Debug.Log("Modo: Rotación");
        }

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject == this.gameObject)
                {
                    ShowLines();
                    cameraOrbit.SetTarget(this.transform); // Cambiar el objetivo de la cámara a esta pieza
                }
                else
                {
                    for (int i = 0; i < 6; i++)
                    {
                        if (hit.collider.gameObject == lineColliders[i])
                        {
                            selectedDirection = i;
                            if (translationMode)
                            {
                                isMoving = true;
                            }
                            else
                            {
                                isRotating = true;
                            }
                            linesVisible = false;
                            HideLines();
                            break;
                        }
                    }
                }
            }
        }

        if (isMoving)
        {
            MovePiece();
        }

        if (isRotating)
        {
            RotatePiece();
        }
    }

    void ShowLines()
    {
        HideLines();

        if (translationMode)
        {
            // Mostrar todas las flechas de traslación (VERDES)
            for (int i = 0; i < 6; i++)
                DrawArrow(lines[i], directions[i], Color.green);
        }
        else
        {
            // Mostrar solo las flechas de rotación (ROJAS)
            DrawArrow(lines[0], Vector3.up, Color.red); // Rotación en Y (arriba/abajo)
            DrawArrow(lines[2], Vector3.left, Color.red); // Rotación en X (izquierda/derecha)
            DrawArrow(lines[4], Vector3.forward, Color.red); // Rotación en Z (adelante/atrás)
        }

        linesVisible = true;
    }

    void DrawArrow(LineRenderer line, Vector3 direction, Color color)
    {
        Vector3 startPos = transform.position;
        Vector3 endPos = transform.position + direction * lineLength;

        line.enabled = true;
        line.positionCount = 5;
        line.startColor = color;
        line.endColor = color;

        line.SetPosition(0, startPos);
        line.SetPosition(1, endPos);

        Vector3 arrowHeadBase = endPos - direction * (lineLength * 0.2f);
        Vector3 arrowLeft = arrowHeadBase + Quaternion.Euler(0, 45, 0) * (-direction * 0.1f);
        Vector3 arrowRight = arrowHeadBase + Quaternion.Euler(0, -45, 0) * (-direction * 0.1f);

        line.SetPosition(2, arrowLeft);
        line.SetPosition(3, endPos);
        line.SetPosition(4, arrowRight);

        GameObject lineCollider = lineColliders[System.Array.IndexOf(directions, direction)];
        lineCollider.SetActive(true);
        lineCollider.transform.position = transform.position + direction * (lineLength / 2);
        lineCollider.transform.rotation = Quaternion.LookRotation(direction);
        lineCollider.GetComponent<BoxCollider>().size = new Vector3(0.1f, 0.1f, lineLength);
    }

    void HideLines()
    {
        for (int i = 0; i < 6; i++)
        {
            lines[i].enabled = false;
            lineColliders[i].SetActive(false);
        }
    }

    void MovePiece()
    {
        if (selectedDirection != -1)
        {
            Vector3 movement = directions[selectedDirection] * moveSpeed * Time.deltaTime;
            transform.position += movement;
        }
    }

    void RotatePiece()
    {
        if (selectedDirection != -1)
        {
            Vector3 direction = directions[selectedDirection];

            // Inclinamos la bola hacia la dirección seleccionada
            Quaternion targetRotation = Quaternion.FromToRotation(Vector3.up, direction) * transform.rotation;
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationAngle);

            isRotating = false; // Detenemos la rotación después de un solo paso
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        isMoving = false;
        isRotating = false;
        selectedDirection = -1;
    }
}
