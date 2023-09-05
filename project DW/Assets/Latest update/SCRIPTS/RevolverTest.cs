using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RevolverTest : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform spawnPoint;
    public AudioSource Gunshot;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Gunshot.Play();
            GameObject bullet = Instantiate(bulletPrefab, spawnPoint.position, spawnPoint.rotation);
        }
    }
}