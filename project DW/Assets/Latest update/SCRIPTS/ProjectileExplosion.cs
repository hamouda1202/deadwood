using System.Collections;
using UnityEngine;

public class ProjectileExplosion : MonoBehaviour
{
    public float explosionRadius = 5f;

    void OnCollisionEnter(Collision collision)
    {
        // Check if the collision is within the explosion radius
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (Collider col in colliders)
        {
            // Check if the collider belongs to the player
            if (col.CompareTag("Player"))
            {

                // Modify the knockback direction and duration of the knockback script attached to the player
                AnotherMovement anotherMovement = col.GetComponent<AnotherMovement>();
                if (anotherMovement != null)
                {
                    anotherMovement.currentKnockbackTime = 0f;
                    // Calculate the horizontal knockback direction
                    Vector3 horizontalDirection = (col.transform.position - transform.position).normalized;
                    // horizontalDirection.y = 0f;
                    anotherMovement.Zvelocity.y = 0f;

                    anotherMovement.knockbackDirection = horizontalDirection;
                    anotherMovement.IsKnockback = true;
                }
            }
        }
        // Destroy the projectile
        Destroy(gameObject);
    }
}