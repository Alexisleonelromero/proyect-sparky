using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    public Transform player;                  // Referencia al transform del jugador
    public Vector3 offset;                    // Offset para la posición de la cámara
    public float sensitivity = 5f;            // Sensibilidad del mouse
    public float fastSpeedX = 10f;            // Velocidad rápida cuando el cursor está en el borde para el eje X
    public float slowSpeedX = 2f;             // Velocidad lenta cuando el cursor está en el centro para el eje X
    public float fastSpeedY = 7f;             // Velocidad rápida cuando el cursor está en el borde para el eje Y
    public float slowSpeedY = 1.5f;           // Velocidad lenta cuando el cursor está en el centro para el eje Y
    public float xBorder = 0.1f;              // Distancia al borde para el eje X
    public float yBorder = 0.05f;             // Distancia al borde para el eje Y
    public float cameraCollisionRadius = 0.5f;// Radio del "colisionador" de la cámara
    public float minDistanceToPlayer = 1.5f;  // Distancia mínima de la cámara al jugador para evitar atravesarlo
    public float cameraApproachSpeed = 5f;    // Velocidad a la que la cámara se acerca al jugador al detectar una colisión
    public float closeCameraDistance = 0.5f;  // Distancia extra cercana al detectar una pared

    private float mouseX;
    private float mouseY;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // Obtener la posición del ratón en relación a la pantalla
        float mousePosX = Input.mousePosition.x / Screen.width;
        float mousePosY = Input.mousePosition.y / Screen.height;

        // Calcular la velocidad basada en la proximidad a los bordes para el eje X
        float speedX = (mousePosX < xBorder || mousePosX > (1 - xBorder)) ? fastSpeedX : slowSpeedX;
        float speedY = (mousePosY < yBorder || mousePosY > (1 - yBorder)) ? fastSpeedY : slowSpeedY;

        float mouseMovementX = (mousePosX < xBorder || mousePosX > (1 - xBorder)) ? Input.GetAxis("Mouse X") : 0;
        float mouseMovementY = (mousePosY < yBorder || mousePosY > (1 - yBorder)) ? Input.GetAxis("Mouse Y") : 0;

        mouseX += mouseMovementX * speedX * sensitivity;
        mouseY -= mouseMovementY * speedY * sensitivity;

        // Limitar el movimiento vertical de la cámara
        mouseY = Mathf.Clamp(mouseY, -80f, 80f);

        // Rotar alrededor del jugador
        Quaternion horizontalRotation = Quaternion.Euler(0, mouseX, 0);
        Quaternion verticalRotation = Quaternion.Euler(mouseY, 0, 0);
        Quaternion combinedRotation = horizontalRotation * verticalRotation;

        // Posición objetivo de la cámara
        Vector3 desiredCameraPosition = player.position + combinedRotation * offset;

        // Realizar raycast para detectar colisiones
        RaycastHit hit;
        Vector3 directionToCamera = (desiredCameraPosition - player.position).normalized;
        float maxDistanceToCamera = offset.magnitude; // Distancia máxima de la cámara al jugador

        // Si el raycast detecta un obstáculo
        if (Physics.Raycast(player.position, directionToCamera, out hit, maxDistanceToCamera))
        {
            // Ajusta la posición de la cámara al punto de colisión menos un pequeño margen
            float hitDistance = Mathf.Clamp(hit.distance - closeCameraDistance, minDistanceToPlayer, maxDistanceToCamera);
            transform.position = Vector3.Lerp(transform.position, player.position + directionToCamera * hitDistance, cameraApproachSpeed * Time.deltaTime);
        }
        else
        {
            // Si no hay colisión, mantener la posición deseada
            transform.position = Vector3.Lerp(transform.position, desiredCameraPosition, cameraApproachSpeed * Time.deltaTime);
        }

        // Hacer que la cámara mire al jugador
        transform.LookAt(player.position + new Vector3(0, offset.y, 0));
    }
}
