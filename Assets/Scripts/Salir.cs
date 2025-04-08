using UnityEngine;

public class Salir : MonoBehaviour
{

   void Update () 
{ 
    // Verificar si la tecla Escape está presionada
     if (Input.GetKeyDown(KeyCode.Escape)) 
    { 
        // Llamar al método Application.Quit() para salir del juego
         Application .Quit (); 
        // Registrar un mensaje en la consola (útil para depurar en el Editor de Unity)
         Debug .Log ("Solicitud para salir del juego"); 
    } 
}
}
