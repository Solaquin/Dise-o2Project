using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    public float velocidad = 5f;
    public float sensibilidadMouse = 2f;
    public float alturaCamara = 1.75f; // Altura promedio de la cámara
    public float gravedad = 9.81f;
    public float multiplicadorSalto = 2f; // Salto es el doble de la altura de la cámara
    public float duracionSprint = 10f; // Duración máxima del sprint
    public float tiempoRecargaSprint = 20f; // Tiempo de espera para volver a usar el sprint
    public Transform camara;

    private CharacterController controller;
    private float rotacionX = 0f;
    private Vector3 velocidadJugador;
    private bool estaEnSuelo;
    private bool puedeCorrer = true;
    private float tiempoSprintRestante;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        tiempoSprintRestante = duracionSprint;
    }

    void Update()
    {
        // Detección de si está en el suelo
        estaEnSuelo = controller.isGrounded;

        // Manejo del sprint (Shift)
        float velocidadActual = velocidad;
        if (Input.GetKey(KeyCode.LeftShift) && puedeCorrer && tiempoSprintRestante > 0)
        {
            velocidadActual *= 2f; // Duplica la velocidad
            tiempoSprintRestante -= Time.deltaTime;
            if (tiempoSprintRestante <= 0)
            {
                puedeCorrer = false;
                StartCoroutine(RecargarSprint());
            }
        }

        // Movimiento con WASD
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector3 movimiento = transform.right * horizontal + transform.forward * vertical;
        controller.Move(movimiento * velocidadActual * Time.deltaTime);

        // Aplicar gravedad
        if (estaEnSuelo && velocidadJugador.y < 0)
        {
            velocidadJugador.y = -2f; // Mantener al personaje pegado al suelo
        }

        // Salto ajustado (el doble de la altura de la cámara)
        if (Input.GetKeyDown(KeyCode.Space) && estaEnSuelo)
        {
            velocidadJugador.y = Mathf.Sqrt(2 * gravedad * alturaCamara * multiplicadorSalto);
        }

        // Aplicar gravedad constante
        velocidadJugador.y -= gravedad * Time.deltaTime;
        controller.Move(velocidadJugador * Time.deltaTime);

        // Rotación con el mouse
        float mouseX = Input.GetAxis("Mouse X") * sensibilidadMouse;
        float mouseY = Input.GetAxis("Mouse Y") * sensibilidadMouse;

        transform.Rotate(Vector3.up * mouseX); // Rotar el personaje en el eje Y (izquierda/derecha)

        // Rotar la cámara en el eje X (mirar arriba/abajo)
        rotacionX -= mouseY;
        rotacionX = Mathf.Clamp(rotacionX, -90f, 90f); // Limita la rotación vertical
        camara.localRotation = Quaternion.Euler(rotacionX, 0f, 0f);
    }

    IEnumerator RecargarSprint()
    {
        yield return new WaitForSeconds(tiempoRecargaSprint);
        tiempoSprintRestante = duracionSprint;
        puedeCorrer = true;
    }
}
