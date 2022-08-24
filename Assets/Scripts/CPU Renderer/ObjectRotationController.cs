using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectRotationController : MonoBehaviour
{
    [SerializeField] int rotSpeed;

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, rotSpeed, 0, Space.World);
    }
}
