using UnityEngine;
using static UnityEngine.GridBrushBase;

public class PiecesMovement : MonoBehaviour
{
    public float moveSpeed = 5f; // Velocidad de movimiento de la bola
    public float rotationAngle = 90f; // Ángulo de inclinación de la bola  
    private bool isMoving = false; // Indica si la bola está en movimiento
    private bool isRotating = false; // Indica si la bola está rotando
    [SerializeField] private ArrowsHandler arrowsHandler; // Referencia al script ArrowsHandler
    private bool isSelected = false; // Nueva variable para saber si esta pieza está seleccionada

    private Rigidbody rb;
    private Vector3 movementDirection;
    private CameraOrbit cameraOrbit;

    private void Awake()
    {
        arrowsHandler = GameObject.Find("ArrowsHandler").GetComponent<ArrowsHandler>(); 
    }

    void Start()
    {
        // Obtener la referencia al script CameraOrbit
        cameraOrbit = Camera.main.GetComponent<CameraOrbit>();
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {

        if (Input.GetMouseButtonDown(0) && !isMoving && !arrowsHandler.IsProcessing())
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject == this.gameObject && !isSelected)
                {
                    arrowsHandler.SetActivePiece(this);
                    isSelected = true;
                    cameraOrbit.SetTarget(this.transform);
                }
                if(isSelected)
                {
                    arrowsHandler.SetActivePiece(this);
                    isSelected = true;
                }
            }
        }


        if (isSelected) // Solo la pieza seleccionada se mueve
        {
            if (isMoving)
            {
                MovePiece();
            }
        }
    }
    public void MoveInDirection(Vector3 direction)
    {
        if (!isMoving)
        {
            arrowsHandler.SetProcessing(true);
            isMoving = true;
            movementDirection = direction;
        }
    }

    void MovePiece()
    {
        transform.position += movementDirection * moveSpeed * Time.deltaTime;
    }

    public void RotateInDirection(Vector3 direction)
    {
        
        // Calculamos la rotación objetivo combinando la rotación actual y la deseada
        Quaternion targetRotation = Quaternion.FromToRotation(Vector3.up, direction) * transform.rotation;
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationAngle);

        // En caso de necesitar ajustar la rotación extra en Y para flechas verticales
        if (direction == Vector3.up || direction == Vector3.down)
        {
            transform.Rotate(Vector3.up, rotationAngle, Space.World);
        }
            
    }

    public void DeselectPiece()
    {
        isSelected = false; // Ya no está seleccionada
    }

    private void OnCollisionEnter(Collision collision)
    {
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        isMoving = false;
        arrowsHandler.SetProcessing(false);

        // Redondear la posición de la pieza
        transform.position = new Vector3(
            Mathf.Round(transform.position.x),
            Mathf.Round(transform.position.y),
            Mathf.Round(transform.position.z)
        );
    }
}
