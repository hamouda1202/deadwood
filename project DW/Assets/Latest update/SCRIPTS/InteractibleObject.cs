using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractibleObject : MonoBehaviour
{
    public GameObject UItext;
    public bool UItextEnabled = false;
    public float UItextTimer;
    public float UItextShowingTime = 10f;

    // Start is called before the first frame update
    void Start()
    {
        UItextTimer = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (UItextEnabled == true && UItextTimer < UItextShowingTime)
        {
            UItext.SetActive(true);
            UItextTimer += Time.deltaTime;
        }
        if (UItextTimer > UItextShowingTime)
        {
            UItextEnabled = false;
            UItext.SetActive(false);
            UItextTimer = 0;
        }
    }
}
