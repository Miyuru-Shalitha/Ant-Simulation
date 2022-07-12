using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ant : MonoBehaviour
{
    [SerializeField] private float speed = 10.0f;
    [SerializeField] private float turningAngle = 90.0f;

    int count = 0;

    private void Start()
    {
        InvokeRepeating("Turn", 0.0f, 1.0f);
    }

    private void Update()
    {
        RandomWalk();
    }

    private void Turn()
    {
        float randomAngle = Random.Range(-turningAngle, turningAngle);

        transform.Rotate(0.0f, 0.0f, randomAngle);

        Debug.Log($"TURN {++count}, Angle {randomAngle}");
    }

    private void RandomWalk()
    {
        transform.position = transform.position + speed * Time.deltaTime * transform.up;
    }
}
