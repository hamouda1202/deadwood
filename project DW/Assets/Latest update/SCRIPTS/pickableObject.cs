using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pickableObject : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (other.GetComponent<WeaponManager>().PickedUpGun == false)
            {
                other.gameObject.GetComponent<WeaponManager>().activation();
            }
            this.gameObject.SetActive(false);
        }
    }
}
