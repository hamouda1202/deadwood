using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public GameObject Gun;
    public bool PickedUpGun;


    // Start is called before the first frame update
    void Start()
    {
        PickedUpGun = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void activation()
    {
        PickedUpGun = true;
        Gun.SetActive(true);
    }
}
