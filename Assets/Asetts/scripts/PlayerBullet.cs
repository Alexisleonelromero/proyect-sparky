using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    private Vector3 direction;
    private float speed = 100f;

    public void SetDirection(Vector3 direction)
    {
        this.direction = direction.normalized;
        Destroy(gameObject, 5f); // Destruye la bala después de 5 segundos (opcional)
    }

    private void Update()
    {
        transform.position += direction * speed * Time.deltaTime;
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Destruir la bala al colisionar
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        // Destruir la bala al colisionar con un trigger
        Destroy(gameObject);
    }
}
