using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingBlock : MonoBehaviour
{
    private float speed = 2f;   // Movement speed
    private float distance = 3f; // Maximum movement distance
    private Vector3 startPos;
    private int direction = 1;  // 1 = right, -1 = left

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        // Move block back and forth
        transform.position += Vector3.right * speed * direction * Time.deltaTime;

        // Reverse direction if it reaches max distance
        if (Mathf.Abs(transform.position.x - startPos.x) >= distance)
        {
            direction *= -1;
        }
    }

    public void SetMovementParams(float newSpeed, float newDistance)
    {
        speed = newSpeed;
        distance = newDistance;
    }
}

