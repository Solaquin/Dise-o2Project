using UnityEngine;

public class PiecesMovement : MonoBehaviour
{
    public float moveSpeed = 5f; // Velocidad de movimiento de la bola
    [SerializeField] private float lineLenght = 0.5f;
    private LineRenderer[] lines; // Array para almacenar las 6 líneas
    private GameObject[] lineColliders; // Array para almacenar los colliders de las líneas
    private Vector3[] directions; // Direcciones de las líneas
    private bool linesVisible = false; // Indica si las líneas están visibles
    private int selectedDirection = -1; // Dirección seleccionada (-1 = ninguna)
    private bool isMoving = false; // Indica si la bola está en movimiento

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
            lines[i].positionCount = 2;
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

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject == this.gameObject)
                {
                    ShowLines();
                }
                else
                {
                    for (int i = 0; i < 6; i++)
                    {
                        if (hit.collider.gameObject == lineColliders[i])
                        {
                            selectedDirection = i;
                            isMoving = true;
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
            HideLines();
            MoveBall();
        }
    }

    void ShowLines()
    {
        for (int i = 0; i < 6; i++)
        {
            Vector3 startPos = transform.position;
            Vector3 endPos = transform.position + directions[i] * lineLenght;

            lines[i].enabled = true;
            lines[i].positionCount = 5; // 1 línea + 2 líneas de la punta
            lines[i].SetPosition(0, startPos);
            lines[i].SetPosition(1, endPos);

            // Crear la base de la flecha
            Vector3 arrowHeadBase = endPos - directions[i] * (lineLenght * 0.2f); // Base de la punta

            Vector3 arrowLeft, arrowRight;

            // Determinar la rotación de la punta de flecha según la dirección
            if (directions[i] == Vector3.up || directions[i] == Vector3.down)
            {
                // Para flechas en el eje Y, rotamos en el plano XZ
                arrowLeft = arrowHeadBase + Quaternion.Euler(45, 0, 0) * (-directions[i] * 0.1f);
                arrowRight = arrowHeadBase + Quaternion.Euler(-45, 0, 0) * (-directions[i] * 0.1f);
            }
            else
            {
                // Para las demás direcciones, rotamos en el plano XY
                arrowLeft = arrowHeadBase + Quaternion.Euler(0, 45, 0) * (-directions[i] * 0.1f);
                arrowRight = arrowHeadBase + Quaternion.Euler(0, -45, 0) * (-directions[i] * 0.1f);
            }

            // Dibujar la punta de la flecha
            lines[i].SetPosition(2, arrowLeft);
            lines[i].SetPosition(3, endPos);
            lines[i].SetPosition(4, arrowRight);

            // Configurar los colliders en la posición correcta
            lineColliders[i].SetActive(true);
            lineColliders[i].transform.position = transform.position + directions[i] * (lineLenght / 2);
            lineColliders[i].transform.rotation = Quaternion.LookRotation(directions[i]);
            lineColliders[i].GetComponent<BoxCollider>().size = new Vector3(0.1f, 0.1f, lineLenght);
        }
        linesVisible = true;
    }


    void HideLines()
    {
        for (int i = 0; i < 6; i++)
        {
            lines[i].enabled = false;
            lineColliders[i].SetActive(false);
        }
    }

    void MoveBall()
    {
        if (selectedDirection != -1)
        {
            Vector3 movement = directions[selectedDirection] * moveSpeed * Time.deltaTime;
            transform.position += movement;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        isMoving = false; // Detener el movimiento al chocar con otro objeto
        selectedDirection = -1; // Reiniciar la dirección
    }
}
