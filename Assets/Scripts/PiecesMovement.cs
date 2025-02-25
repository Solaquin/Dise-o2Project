using UnityEngine;

public class PiecesMovement : MonoBehaviour
{
    public float moveSpeed = 5f; // Velocidad de movimiento de la bola
    public float rotationAngle = 90f; // Ángulo de inclinación de la bola  
    private bool isMoving = false; // Indica si la bola está en movimiento
    private bool isRotating = false;
    [SerializeField] private ArrowsHandler arrowsHandler; // Referencia al script ArrowsHandler
    private int selectedDirection = -1; // Dirección seleccionada (-1 = ninguna)
    private bool isSelected = false; // Nueva variable para saber si esta pieza está seleccionada


    private CameraOrbit cameraOrbit;

    private void Awake()
    {
        arrowsHandler = GameObject.Find("ArrowsHandler").GetComponent<ArrowsHandler>(); 
    }

    void Start()
    {
        // Obtener la referencia al script CameraOrbit
        cameraOrbit = Camera.main.GetComponent<CameraOrbit>();
    }

    void Update()
    {

        if (Input.GetMouseButtonDown(0) && !isMoving)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject == this.gameObject && !isSelected && !isMoving) // Solo si se clickea esta pieza
                {
                    arrowsHandler.SetActivePiece(this);
                    isSelected = true; // Marcamos esta pieza como seleccionada
                    arrowsHandler.createLines(transform);
                    cameraOrbit.SetTarget(this.transform); // Cambiar la cámara a esta pieza
                }
                if (isSelected) // Solo la pieza seleccionada reacciona a los clics en las flechas
                {
                    arrowsHandler.ShowLines(transform);
                    GameObject[] lineColliders = arrowsHandler.getLinesColliders();
                    for (int i = 0; i < 6; i++)
                    {
                        if (hit.collider.gameObject == lineColliders[i])
                        {
                            selectedDirection = i;
                            if (arrowsHandler.getTranslationMode())
                            {
                                isMoving = true;
                            }
                            else
                            {
                                isRotating = true;
                            }
                            arrowsHandler.HideLines();
                            break;
                        }
                    }
                }
            }
        }


        if (isSelected) // Solo la pieza seleccionada se mueve
        {
            if (isMoving)
            {
                MovePiece();
            }

            if (isRotating)
            {
                RotatePiece();
            }
        }
    }

    void MovePiece()
    {
        if (selectedDirection != -1)
        {
            arrowsHandler.SetProcessing(true);
            Vector3 movement = arrowsHandler.getDirection(selectedDirection) * moveSpeed * Time.deltaTime;
            transform.position += movement;
        }
    }

    void RotatePiece()
    {
        if (selectedDirection != -1)
        {
            Vector3 direction = arrowsHandler.getDirection(selectedDirection);

            // Inclinamos la bola hacia la dirección seleccionada
            Quaternion targetRotation = Quaternion.FromToRotation(Vector3.up, direction) * transform.rotation;
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationAngle);

            if (direction == Vector3.up || direction == Vector3.down)
            {
                transform.Rotate(Vector3.up, rotationAngle, Space.World);
            }


            isRotating = false; // Detenemos la rotación después de un solo paso
            isSelected = false; // Ya no está seleccionada
        }
    }

    public void DeselectPiece()
    {
        isSelected = false; // Ya no está seleccionada
    }

    private void OnCollisionEnter(Collision collision)
    {
        isMoving = false;
        selectedDirection = -1;
        isSelected = false; // Desseleccionamos la pieza tras chocar
        arrowsHandler.SetProcessing(false);
    }
}
