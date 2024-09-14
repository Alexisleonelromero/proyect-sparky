using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AimAndTeleport : MonoBehaviour
{
    public Camera mainCamera;
    public Image crosshair;           // El punto de mira que debe seguir al mouse
    public Image reticle;             // Ret�cula para objetos el�ctricos
    public Image enemyReticle;        // Ret�cula para objetos enemigos
    public float maxDistance = 100f;
    public LayerMask electricObjectLayer;
    public LayerMask enemyLayer;
    public GameObject explosionPrefab; // Prefab de la explosi�n
    public float explosionOffsetY = 0.5f; // Offset para la altura de la explosi�n

    private RaycastHit hitInfo;
    private Vector3 currentTeleportTargetPosition; // Usamos un Vector3 para almacenar la posici�n de teleportaci�n
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
        playerInputs = GetComponent<PlayerInputs>(); // Aseg�rate de tener el script de inputs en el mismo GameObject
        reticle.gameObject.SetActive(false);         // Ocultamos la ret�cula al inicio
        enemyReticle.gameObject.SetActive(false);    // Ocultamos tambi�n la ret�cula de enemigos
    }

    private void Update()
    {
        HandleAiming();
        HandleTeleport();
    }

    private void HandleAiming()
    {
        // Mostrar el crosshair siempre si el jugador no est� corriendo
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

        // Raycast para detectar objetos el�ctricos
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hitInfo, maxDistance, electricObjectLayer))
        {
            isAimingAtElectricObject = true;
            currentTeleportTargetPosition = hitInfo.point; // Guardar la posici�n del objeto el�ctrico

            // Convertir la posici�n del objeto en el mundo a la posici�n de la pantalla
            Vector3 screenPosition = mainCamera.WorldToScreenPoint(hitInfo.point);

            // Ajustar la posici�n de la ret�cula en la UI para que est� fija sobre el objeto el�ctrico
            reticle.transform.position = screenPosition;

            // Mostrar la ret�cula solo si no est� corriendo
            reticle.gameObject.SetActive(!playerInputs.run);
        }
        else
        {
            isAimingAtElectricObject = false;
            reticle.gameObject.SetActive(false); // Ocultar si no est� apuntando a un objeto el�ctrico
        }

        // Raycast para detectar objetos enemigos
        if (Physics.Raycast(ray, out hitInfo, maxDistance, enemyLayer))
        {
            isAimingAtEnemy = true;
            currentEnemyTarget = hitInfo.transform;  // Guardar la referencia del enemigo objetivo

            // Convertir la posici�n del enemigo en el mundo a la posici�n de la pantalla
            Vector3 screenPosition = mainCamera.WorldToScreenPoint(hitInfo.point);

            // Ajustar la posici�n de la ret�cula en la UI para que est� fija sobre el objeto enemigo
            enemyReticle.transform.position = screenPosition;

            // Mostrar la ret�cula solo si no est� corriendo
            enemyReticle.gameObject.SetActive(!playerInputs.run);
        }
        else
        {
            isAimingAtEnemy = false;
            currentEnemyTarget = null;  // Borrar la referencia del enemigo objetivo
            enemyReticle.gameObject.SetActive(false); // Ocultar si no est� apuntando a un enemigo
        }
    }

    private void HandleTeleport()
    {
        // Verificar que se presion� el bot�n derecho, que se est� apuntando a un objeto el�ctrico o enemigo, y que el jugador no est� corriendo
        if (Input.GetMouseButtonDown(1))
        {
            if (isAimingAtElectricObject && !playerInputs.run)
            {
                // Teletransportaci�n a objeto el�ctrico
                Vector3 targetPosition = currentTeleportTargetPosition + new Vector3(0, 0.5f, -1.5f);
                transform.position = targetPosition;
            }
            else if (isAimingAtEnemy && !playerInputs.run)
            {
                // Teletransportaci�n a enemigo
                Vector3 targetPosition = hitInfo.point + new Vector3(0, 0.5f, -1.5f);
                transform.position = targetPosition;

                // Spawnear una explosi�n en el lugar del teletransporte
                SpawnExplosion(hitInfo.point);
            }
        }
    }

    private void SpawnExplosion(Vector3 position)
    {
        // Ajustar la posici�n de la explosi�n con un peque�o offset en el eje Y
        Vector3 explosionPosition = position + new Vector3(0, explosionOffsetY, 0);

        // Instanciar el prefab de la explosi�n
        Instantiate(explosionPrefab, explosionPosition, Quaternion.identity);
    }
}
