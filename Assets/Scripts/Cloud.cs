using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloud : MonoBehaviour
{
    public float moveSpeed;
    public Vector3 startPos;
    public Vector3 endPos;

    void Update ()
    {
        transform.position = Vector3.MoveTowards(transform.position, endPos, moveSpeed * Time.deltaTime);

        if(transform.position == endPos)
            transform.position = startPos;
    }
}