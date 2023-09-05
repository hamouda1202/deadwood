using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Revolver : MonoBehaviour
{

    [Header("General Stats")]
    public float damage = 10f;
    public float range = 100f;
    public Camera fpsCam;
    public Animator anim;
    public float tempsEntreTirs = 3f;
    public float timerTirs;
    public bool shot = false;
    public GameObject projectilePrefab;
    public Transform spawnPoint;
    public float projectileForce = 100f;


    [Header("Laser")]
    public GameObject laser;
    public Transform positionDépart;
    public float fadeDuration = 0.3f;


    // Start is called before the first frame update
    void Start()
    {
        timerTirs = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1") && shot == false)
            {
            anim.SetTrigger("Shooting");
            Shoot();
            shot = true;
            anim.SetBool("ShotFired", true);
        }
        if (shot == true)
        {
            timerTirs += Time.deltaTime;
        }

        if (timerTirs > tempsEntreTirs)
        {
            anim.ResetTrigger("Shooting");
            anim.SetBool("ShotFired", false);
            shot = false;
            timerTirs = 0;
        }
    }


    void Shoot()

        {
        // Instantiate a new projectile at the spawn point
        GameObject projectile = Instantiate(projectilePrefab, spawnPoint.position, spawnPoint.rotation);

        // Apply a force to the projectile
        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.AddForce(spawnPoint.forward * projectileForce, ForceMode.Impulse);
        }

        RaycastHit hit;
            if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range))
            {
                Debug.Log(hit.transform.name);
                CreateLaser(fpsCam.transform.position + fpsCam.transform.forward * range);
            }
        }

    void CreateLaser(Vector3 end)
        {
            LineRenderer lr = Instantiate(laser).GetComponent<LineRenderer>();
            lr.SetPositions(new Vector3[2] { positionDépart.position, end });
            StartCoroutine(FadeLaser(lr));
        }

    IEnumerator FadeLaser(LineRenderer lr)
        {
            float alpha = 1;
            while (alpha > 0)
            {
                alpha -= Time.deltaTime / fadeDuration;
                lr.startColor = new Color(lr.startColor.r, lr.startColor.g, lr.startColor.b, alpha);
                lr.endColor = new Color(lr.endColor.r, lr.endColor.g, lr.endColor.b, alpha);
                yield return null;
            }
        }
    }
