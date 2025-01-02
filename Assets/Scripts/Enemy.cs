using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Transform player; // Assign the player's transform in the Inspector
    public float moveSpeed = 3f; // Speed of movement towards the player
    public float oscillationSpeed = 2f; // Speed of oscillation
    public float oscillationAmplitude = 1f; // Amplitude of oscillation

    private Vector3 startingPosition;

    void Start()
    {
        startingPosition = transform.position;
    }

    void Update()
    {
        if (player != null)
        {
            // Move towards the player
            Vector3 direction = (player.position - transform.position).normalized;
            transform.position += direction * moveSpeed * Time.deltaTime;

            // Oscillate left-right or up-down
            Vector3 oscillation = new Vector3(
                Mathf.Sin(Time.time * oscillationSpeed) * oscillationAmplitude,
                Mathf.Cos(Time.time * oscillationSpeed) * oscillationAmplitude,
                0f
            );

            transform.position += oscillation * Time.deltaTime;
        }
    }
}
