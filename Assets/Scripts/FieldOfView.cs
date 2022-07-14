using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    public float viewRadius;
    [Range(0f, 360f)]
    public float viewAngle;
    [SerializeField] private LayerMask foodLayer;

    [HideInInspector]
    public List<Transform> visibleTargets = new List<Transform>();

    private void Start()
    {
        InvokeRepeating("HandleTarget", 0f, 0.5f);
    }

    public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
    {
        if (!angleIsGlobal)
        {
            angleInDegrees += transform.eulerAngles.z;
        }
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), Mathf.Cos(angleInDegrees * Mathf.Deg2Rad), 0);
    }

    private void HandleTarget()
    {
        visibleTargets.Clear();
        Collider2D[] foodsInViewRadius = Physics2D.OverlapCircleAll(transform.position, viewRadius, foodLayer);

        foreach (Collider2D food in foodsInViewRadius)
        {
            Vector2 dirToFood = (food.transform.position - transform.position).normalized;
            if (Vector2.Angle(transform.up, dirToFood) < viewAngle / 2)
            {
                visibleTargets.Add(food.transform);
            }
        }
    }
}
