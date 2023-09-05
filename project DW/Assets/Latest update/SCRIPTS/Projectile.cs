using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 100f;
    public float gravity = -9.81f;
    public float damage = 20f;
    public float headshotMultiplier = 2f;
    public float trailDelay = 2f; // Delay before the trail appears
    public LayerMask ignoreLayers; // Assign the layer containing the player character to this in the Inspector
    private HitMarker hitMarker; // Reference to the HitMarkerController script
    private HitMarkerHeadshot hitMarkerHeadshot; // ref hs

    private Vector3 currentVelocity;
    private float distanceTraveled; // Distance traveled by the projectile

    private TrailRenderer trailRenderer;

    private bool hasHitEnemy = false; // Flag to track if the projectile has hit an enemy

    // Start is called before the first frame update
    void Start()
    {
        // Calculate initial velocity based on speed
        currentVelocity = transform.forward * speed;

        // Get the TrailRenderer component
        trailRenderer = GetComponent<TrailRenderer>();

        // Disable the trail initially
        trailRenderer.enabled = false;

        // Enable the trail after a certain delay
        Invoke("EnableTrail", trailDelay);

        // Find the HitMarkerController script in the scene
        hitMarker = FindObjectOfType<HitMarker>();

        // Find the HitMarkerController script in the scene
        hitMarkerHeadshot = FindObjectOfType<HitMarkerHeadshot>();
    }

    // Enable the trail
    void EnableTrail()
    {
        trailRenderer.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        // Calculate new velocity based on gravity and time
        currentVelocity.y += gravity * Time.deltaTime;
        Vector3 newPosition = transform.position + currentVelocity * Time.deltaTime;

        // Use a raycast to check if the projectile hits anything, ignoring the player layer
        RaycastHit hit;
        if (Physics.Raycast(transform.position, currentVelocity, out hit, Vector3.Distance(transform.position, newPosition), ~ignoreLayers))
        {
            // If the projectile hits an enemy, deal damage
            EnemyHealth enemy = hit.collider.GetComponentInParent<EnemyHealth>();
            if (enemy != null)
            {
                // Check if the hit was in the head
                bool hitHead = false;
                if (hit.collider.CompareTag("Head"))
                {
                    hitHead = true;
                    hitMarkerHeadshot.ShowHitMarkerHeadshot();
                    enemy.TakeDamage(damage * headshotMultiplier, true);
                }
                else
                {
                    hitMarker.ShowHitMarker();
                    enemy.TakeDamage(damage, false);
                }

                // Log the name of the enemy hit
                if (hitHead)
                {
                    Debug.Log("Hit " + hit.collider.name + " for " + damage * headshotMultiplier + " damage!");
                }
                else
                {
                    Debug.Log("Hit " + hit.collider.name + " for " + damage + " damage!");
                }

                hasHitEnemy = true; // Set the flag to indicate that the projectile has hit an enemy
            }

            // Destroy the projectile
            Destroy(gameObject);
        }

        // Move the projectile to its new position
        transform.position = newPosition;
    }
}