using UnityEngine;

public class Animaciones : MonoBehaviour
{
    private Vector3 escalaOriginal;  // Guarda la escala original del objeto
    public float factorEscalado = 0.05f; // Cuánto aumenta la escala (5% por defecto)
    public float velocidad = 1.0f; // Velocidad de la respiración

    void Start()
    {
        escalaOriginal = transform.localScale; // Guarda la escala inicial
    }

    void Update()
    {
        float factor = Mathf.Lerp(1, 1 + factorEscalado, Mathf.PingPong(Time.time * velocidad, 1));
        transform.localScale = escalaOriginal * factor; // Escala sin bajar de la original
    }
}
