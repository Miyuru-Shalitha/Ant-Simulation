using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    public float viewRadius;
    [Range(0f, 360f)]
    public float viewAngle;
    [SerializeField] private LayerMask targetLayer;
    [SerializeField] private Transform home;

    [HideInInspector] public List<Transform> visibleTargets = new List<Transform>();

    public Transform nextTarget = null;
    //public string targetType = null;
    public bool hasFood = false;

    private List<Transform> outMarks = new List<Transform>();
    private List<Transform> inMarks = new List<Transform>();

    private void Start()
    {
        InvokeRepeating("HandleTarget", 0f, 0.2f);
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
        Collider2D[] targetsInViewRadius = Physics2D.OverlapCircleAll(transform.position, viewRadius, targetLayer);

        foreach (Collider2D target in targetsInViewRadius)
        {
            Vector2 dirToTarget = (target.transform.position - transform.position).normalized;
            if (Vector2.Angle(transform.up, dirToTarget) < viewAngle / 2)
            {
                //if (target.CompareTag("Food"))
                //{
                //    visibleTargets.Add(target.transform);
                //}
                visibleTargets.Add(target.transform);

                if (!nextTarget)
                {
                    if (hasFood)
                    {
                        if (target.CompareTag("Home"))
                        {
                            nextTarget = target.transform;
                            //targetType = target.tag;
                        }
                        else if (target.CompareTag("Out Mark"))
                        {
                            //nextTarget = target.transform;
                            //targetType = target.tag;

                            outMarks.Add(target.transform);
                        }
                    }
                    else
                    {
                        if (target.CompareTag("Food"))
                        {
                            nextTarget = target.transform;
                            //targetType = target.tag;
                        }
                        else if (target.CompareTag("In Mark"))
                        {
                            //nextTarget = target.transform;
                            //targetType = target.tag;

                            inMarks.Add(target.transform);
                        }
                    }
                }
            }
        }

        if (hasFood)
        {
            float minDistance = float.PositiveInfinity;

            foreach (Transform outMark in outMarks)
            {
                float distOutMarkToHome = Vector2.Distance(home.position, outMark.position);

                if (distOutMarkToHome < minDistance)
                {
                    minDistance = distOutMarkToHome;
                    nextTarget = outMark;
                }
            }

            outMarks.Clear();
        }
        else
        {
            float maxDistance = float.NegativeInfinity;

            foreach (Transform inMark in inMarks)
            {
                float distInMarkToHome = Vector2.Distance(home.position, inMark.position);

                if (distInMarkToHome > maxDistance)
                {
                    maxDistance = distInMarkToHome;
                    nextTarget = inMark;
                }
            }

            inMarks.Clear();
        }
    }
}
