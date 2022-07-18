using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarkingSystem : MonoBehaviour
{
    [SerializeField] private GameObject outMark;
    [SerializeField] private GameObject inMark;
    [SerializeField] private Transform marks;
    [SerializeField] private FieldOfView fow;

    private void Start()
    {
        InvokeRepeating("Mark", 0f, 0.2f);
    }

    private void Mark()
    {
        if (fow.hasFood)
        {
            Instantiate(inMark, new Vector3(transform.position.x, transform.position.y, transform.position.z + 1), Quaternion.identity, marks);
        }
        else
        {
            Instantiate(outMark, new Vector3(transform.position.x, transform.position.y, transform.position.z + 1), Quaternion.identity, marks);
        }
    }
}
