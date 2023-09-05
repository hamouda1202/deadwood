using UnityEngine;
using UnityEngine.UI;

public class SpeedDisplay : MonoBehaviour
{
    public GameObject playerCharacter;
    private AnotherMovement anotherMovement;
    private Text speedText;

    private void Start()
    {
        if (playerCharacter != null)
        {
            anotherMovement = playerCharacter.GetComponent<AnotherMovement>();
        }
        speedText = GetComponent<Text>();
    }

    private void Update()
    {
        if (anotherMovement != null)
        {
            speedText.text = "Speed: " + anotherMovement.speed.ToString();
        }
    }
}