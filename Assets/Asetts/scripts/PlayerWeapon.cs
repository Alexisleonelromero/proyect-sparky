using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    public GameObject bulletPrefab;     // Prefab de la bala
    public Transform shootPoint;        // Punto de disparo

    private AimAndTeleport aimAndTeleport; // Referencia al script AimAndTeleport

    private void Start()
    {
        aimAndTeleport = GetComponent<AimAndTeleport>(); // Asegúrate de tener el script AimAndTeleport en el mismo GameObject
        if (aimAndTeleport == null)
        {
            Debug.LogError("AimAndTeleport no encontrado en el GameObject");
        }

        if (bulletPrefab == null)
        {
            Debug.LogError("bulletPrefab no está asignado en el Inspector");
        }

        if (shootPoint == null)
        {
            Debug.LogError("shootPoint no está asignado en el Inspector");
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Botón izquierdo del ratón
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        if (aimAndTeleport != null && aimAndTeleport.CurrentEnemyTarget != null && bulletPrefab != null && shootPoint != null)
        {
            // Instanciar la bala
            GameObject bullet = Instantiate(bulletPrefab, shootPoint.position, Quaternion.identity);

            // Obtener la posición del enemigo y configurar la dirección de la bala
            Vector3 enemyPosition = aimAndTeleport.CurrentEnemyTarget.position;
            PlayerBullet bulletScript = bullet.GetComponent<PlayerBullet>();
            if (bulletScript != null)
            {
                Vector3 direction = (enemyPosition - shootPoint.position).normalized;
                bulletScript.SetDirection(direction); // Solo pasa la dirección
            }
            else
            {
                Debug.LogError("El prefab de la bala no tiene el componente PlayerBullet");
            }
        }
    }
}
