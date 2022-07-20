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

    private GameObject food;

    private void Update()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.right, rayCastDistance, blockLayer);
        if (hit)
        {
            desiredDirection = hit.normal + new Vector2(Random.Range(-0.8f, 0.8f), 0f);
        }
        else
        {
            if (fow.nextTarget)
            {
                desiredDirection = (fow.nextTarget.position - transform.position).normalized;
            }
            else
            {
                desiredDirection = (desiredDirection + Random.insideUnitCircle * wanderStrength).normalized;
            }
        }

        Walk(desiredDirection);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Food"))
        {
            fow.visibleTargets.Clear();
            
            if (!fow.hasFood)
            {
                //Destroy(collision.gameObject);
                food = collision.gameObject;
                food.tag = "Grabbed";
                food.transform.SetParent(transform, true);
                food.transform.localPosition = new Vector3(0.65f, 0f, 0f);
            }

            if (!fow.hasFood)
            {
                desiredDirection = -desiredDirection;
            }
            fow.nextTarget = null;
            fow.hasFood = true;
        }
        else if (collision.gameObject.CompareTag("Out Mark") || collision.gameObject.CompareTag("In Mark"))
        {
            fow.nextTarget = null;
        }
        else if (collision.gameObject.CompareTag("Home"))
        {
            desiredDirection = -desiredDirection;
            //acceleration = Vector2.zero;
            velocity = Vector2.zero;
            Destroy(food);
            fow.nextTarget = null;
            fow.hasFood = false;
        }
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
}
