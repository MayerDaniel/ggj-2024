using UnityEngine;
using UnityEngine.SceneManagement;

public class LogoBounce : MonoBehaviour
{
    [SerializeField] public float bounceSpeed = 5.0f;
    [SerializeField] public float bounceHeight = 1.0f;

    private Vector3 originalPosition;


    private void Start()
    {
        originalPosition = transform.position;
    }

    private void Update()
    {
        // Calculate the vertical movement using Mathf.Sin to create a bouncing effect
        float verticalMovement = Mathf.Sin(Time.time * bounceSpeed) * bounceHeight;

        // Update the object's position with the vertical movement
        transform.position = new Vector3(originalPosition.x, originalPosition.y + verticalMovement, originalPosition.z);
    }
}


