using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ant : MonoBehaviour
{
    private bool isStop = true;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            isStop = !isStop;
        }

        if (!isStop)
        {
            transform.position = transform.position + new Vector3(0.0f, 0.5f, 0.0f) * Time.deltaTime;
        }
    }
}
