using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyReticleTracker : MonoBehaviour
{
    public Transform enemy; // El enemigo a rastrear
    public Transform reticle; // La ret�cula que debe seguir al enemigo
    public float trackingSpeed = 5f; // Velocidad de seguimiento de la ret�cula

    private void Update()
    {
        if (enemy != null)
        {
            // Actualizar la posici�n de la ret�cula para que siga al enemigo
            Vector3 screenPosition = Camera.main.WorldToScreenPoint(enemy.position);
            reticle.position = Vector3.Lerp(reticle.position, screenPosition, trackingSpeed * Time.deltaTime);
        }
    }

    public void SetTarget(Transform newEnemy)
    {
        enemy = newEnemy;
    }

    public void ClearTarget()
    {
        enemy = null;
    }
}
