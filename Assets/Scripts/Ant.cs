using System.Collections.Generic;
using UnityEngine;

public class Ant : MonoBehaviour
{
    [SerializeField] private float maxSpeed = 2.0f;
    [SerializeField] private float steerStrength = 2.0f;
    [SerializeField] private float wanderStrength = 1.0f;
    [SerializeField] private Animator animator;
    [SerializeField] private Transform head;
    [SerializeField] private FieldOfView fow;
    [SerializeField] private LayerMask blockLayer;
    [SerializeField] private float rayCastDistance = 2.0f;

    private Vector2 position;
    private Vector2 velocity;
    private Vector2 desiredDirection;
    private Vector2 acceleration;

    private bool isTargetLocated = false;
    private Transform foodTarget;

    private void Update()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.right, rayCastDistance, blockLayer);
        if (hit)
        {
            desiredDirection = hit.normal + new Vector2(Random.Range(-0.8f, 0.8f), 0f);
        }
        else
        {
            if (fow.visibleTargets.Count > 0)
            {
                if (!isTargetLocated)
                {
                    foodTarget = fow.visibleTargets[0];
                    desiredDirection = (foodTarget.position - transform.position).normalized;
                    isTargetLocated = true;
                    Debug.Log("LOCATED!");
                }
            }
            else
            {
                desiredDirection = (desiredDirection + Random.insideUnitCircle * wanderStrength).normalized;
            }
        }

        Walk(desiredDirection);
    }

    private void Walk(Vector2 desiredDirection)
    {
        Vector2 desiredVelocity = desiredDirection * maxSpeed;
        Vector2 desiredSteeringForce = (desiredVelocity - velocity) * steerStrength;
        acceleration = Vector2.ClampMagnitude(desiredSteeringForce, steerStrength) / 1;

        velocity = Vector2.ClampMagnitude(velocity + acceleration * Time.deltaTime, maxSpeed);
        animator.SetFloat("Blend", velocity.magnitude);
        position += velocity * Time.deltaTime;

        float angle = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg;
        transform.SetPositionAndRotation(position, Quaternion.Euler(0, 0, angle));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Food"))
        {
            fow.visibleTargets.Clear();
            Destroy(collision.gameObject);
            isTargetLocated = false;
        }
    }
}
