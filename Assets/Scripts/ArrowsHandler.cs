using Unity.VisualScripting;
using UnityEngine;

public class ArrowsHandler : MonoBehaviour
{
    [SerializeField] private GameObject arrowPrefab;
    [SerializeField] private float arrowOffset = 0.3f; // Desfase configurable
    [SerializeField] private Material[] arrowsMaterial;
    private GameObject[] arrowInstances;

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
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            translationMode = true;
            Debug.Log($"Modo: Traslación - translationMode: {translationMode} - activePiece{activePiece}");
            UpdateArrows();
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            translationMode = false;
            Debug.Log($"Modo: Rotación - translationMode: {translationMode} - activePiece{activePiece}");
            UpdateArrows();
        }
    }

    public void createArrows(Transform obj)
    {
        HideArrows(); // Elimina las flechas anteriores

        arrowInstances = new GameObject[6];

        for (int i = 0; i < 6; i++)
        {
            Vector3 direction = directions[i];
            // Posición con desfase: se coloca a una distancia 'arrowOffset' del centro del objeto
            Vector3 position = obj.position + direction * arrowOffset;
            // Rotación para que la flecha apunte en la dirección indicada
            Quaternion rotation = Quaternion.FromToRotation(Vector3.up, direction);

            GameObject arrow = Instantiate(arrowPrefab, position, rotation);

            // Agregar o conseguir el componente ArrowButton y asignar la dirección y referencia
            ArrowButton arrowButton = arrow.GetComponent<ArrowButton>();
            if (arrowButton == null)
                arrowButton = arrow.AddComponent<ArrowButton>();

            arrowButton.direction = direction;
            arrowButton.arrowsHandler = this;

            arrowInstances[i] = arrow;
        }
    }



    public void ShowArrows(Transform obj)
    {
        if (translationMode)
        {
            for (int i = 0; i < 6; i++)
            {
                arrowInstances[i].GetComponent<MeshRenderer>().material = arrowsMaterial[0];
                arrowInstances[i].SetActive(true);
            }
        }
        else
        {
            for (int i = 0; i < 6; i++)
            {
                arrowInstances[i].GetComponent<MeshRenderer>().material = arrowsMaterial[1];
            }
            // Rotación en Y, X, Z: usar solo algunos ejes
            arrowInstances[0].SetActive(true); // up
            arrowInstances[2].SetActive(true); // left
            arrowInstances[5].SetActive(true); // forward
        }
    }

    public void HideArrows()
    {
        if (arrowInstances != null)
        {
            foreach (var arrow in arrowInstances)
            {
                if (arrow != null)
                    Destroy(arrow);
            }
        }

        arrowInstances = new GameObject[6];
    }

    public void MoveActivePiece(Vector3 direction)
    {
        if (activePiece != null)
        {
            if (translationMode)
            {
                activePiece.MoveInDirection(direction);
                HideArrows();
            }
            else
            {
                activePiece.RotateInDirection(direction);
            }
        }
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
        // Si hay una pieza activa y es distinta, la deseleccionamos
        // Se omite la verificación de isProcessing para permitir cambiar la visualización de las flechas
        if (activePiece != null && activePiece != piece)
        {
            HideArrows();
            activePiece.DeselectPiece();
        }

        activePiece = piece;
        createArrows(piece.transform);
        ShowArrows(piece.transform); // Redibujar flechas
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
            HideArrows(); // Ocultamos las flechas actuales

            // Se crean nuevamente las flechas
            createArrows(activePiece.transform);
            ShowArrows(activePiece.transform); // Redibujar flechas
        }
    }

}
