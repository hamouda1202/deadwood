using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractions : MonoBehaviour
{
    public Transform cam;
    public float ActivationDistance;
    bool active = false;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        active = Physics.Raycast(cam.position, cam.TransformDirection(Vector3.forward), out hit, ActivationDistance);

        if (active && hit.transform.GetComponent<InteractibleObject>() != null)
        {
            hit.transform.GetComponent<InteractibleObject>().UItextEnabled = true;
        }

        if (Input.GetKeyDown(KeyCode.E) && active)
        {
            if (hit.transform.GetComponent<Animator>() != null)
            {
                hit.transform.GetComponent<Animator>().SetTrigger("Activate");
            }
        }
    }
}
