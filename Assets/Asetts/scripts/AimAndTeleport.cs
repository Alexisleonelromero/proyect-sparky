using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AimAndTeleport : MonoBehaviour
{
    public Camera mainCamera;
    public Image crosshair;           // El punto de mira que debe seguir al mouse
    public Image reticle;             // Retícula para objetos eléctricos
    public Image enemyReticle;        // Retícula para objetos enemigos
    public float maxDistance = 100f;
    public LayerMask electricObjectLayer;
    public LayerMask enemyLayer;
    public GameObject explosionPrefab; // Prefab de la explosión
    public float explosionOffsetY = 0.5f; // Offset para la altura de la explosión

    private RaycastHit hitInfo;
    private Vector3 currentTeleportTargetPosition; // Usamos un Vector3 para almacenar la posición de teleportación
    private PlayerInputs playerInputs;             // Referencia al script de inputs
    private bool isAimingAtElectricObject;
    private bool isAimingAtEnemy;
    private Transform currentEnemyTarget;           // Referencia al enemigo objetivo

    public Transform CurrentEnemyTarget
    {
        get { return currentEnemyTarget; }
    }

    private void Start()
    {
        playerInputs = GetComponent<PlayerInputs>(); // Asegúrate de tener el script de inputs en el mismo GameObject
        reticle.gameObject.SetActive(false);         // Ocultamos la retícula al inicio
        enemyReticle.gameObject.SetActive(false);    // Ocultamos también la retícula de enemigos
    }

    private void Update()
    {
        HandleAiming();
        HandleTeleport();
    }

    private void HandleAiming()
    {
        // Mostrar el crosshair siempre si el jugador no está corriendo
        if (!playerInputs.run)
        {
            Vector2 mousePosition = Input.mousePosition;
            crosshair.transform.position = mousePosition;
            crosshair.gameObject.SetActive(true);
        }
        else
        {
            crosshair.gameObject.SetActive(false);
        }

        // Raycast para detectar objetos eléctricos
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hitInfo, maxDistance, electricObjectLayer))
        {
            isAimingAtElectricObject = true;
            currentTeleportTargetPosition = hitInfo.point; // Guardar la posición del objeto eléctrico

            // Convertir la posición del objeto en el mundo a la posición de la pantalla
            Vector3 screenPosition = mainCamera.WorldToScreenPoint(hitInfo.point);

            // Ajustar la posición de la retícula en la UI para que esté fija sobre el objeto eléctrico
            reticle.transform.position = screenPosition;

            // Mostrar la retícula solo si no está corriendo
            reticle.gameObject.SetActive(!playerInputs.run);
        }
        else
        {
            isAimingAtElectricObject = false;
            reticle.gameObject.SetActive(false); // Ocultar si no está apuntando a un objeto eléctrico
        }

        // Raycast para detectar objetos enemigos
        if (Physics.Raycast(ray, out hitInfo, maxDistance, enemyLayer))
        {
            isAimingAtEnemy = true;
            currentEnemyTarget = hitInfo.transform;  // Guardar la referencia del enemigo objetivo

            // Convertir la posición del enemigo en el mundo a la posición de la pantalla
            Vector3 screenPosition = mainCamera.WorldToScreenPoint(hitInfo.point);

            // Ajustar la posición de la retícula en la UI para que esté fija sobre el objeto enemigo
            enemyReticle.transform.position = screenPosition;

            // Mostrar la retícula solo si no está corriendo
            enemyReticle.gameObject.SetActive(!playerInputs.run);
        }
        else
        {
            isAimingAtEnemy = false;
            currentEnemyTarget = null;  // Borrar la referencia del enemigo objetivo
            enemyReticle.gameObject.SetActive(false); // Ocultar si no está apuntando a un enemigo
        }
    }

    private void HandleTeleport()
    {
        // Verificar que se presionó el botón derecho, que se está apuntando a un objeto eléctrico o enemigo, y que el jugador no está corriendo
        if (Input.GetMouseButtonDown(1))
        {
            if (isAimingAtElectricObject && !playerInputs.run)
            {
                // Teletransportación a objeto eléctrico
                Vector3 targetPosition = currentTeleportTargetPosition + new Vector3(0, 0.5f, -1.5f);
                transform.position = targetPosition;
            }
            else if (isAimingAtEnemy && !playerInputs.run)
            {
                // Teletransportación a enemigo
                Vector3 targetPosition = hitInfo.point + new Vector3(0, 0.5f, -1.5f);
                transform.position = targetPosition;

                // Spawnear una explosión en el lugar del teletransporte
                SpawnExplosion(hitInfo.point);
            }
        }
    }

    private void SpawnExplosion(Vector3 position)
    {
        // Ajustar la posición de la explosión con un pequeño offset en el eje Y
        Vector3 explosionPosition = position + new Vector3(0, explosionOffsetY, 0);

        // Instanciar el prefab de la explosión
        Instantiate(explosionPrefab, explosionPosition, Quaternion.identity);
    }
}
