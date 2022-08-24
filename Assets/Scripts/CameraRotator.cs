using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotator : MonoBehaviour
{
    [SerializeField] float speed;

    private bool rotate = false;

    // Update is called once per frame
    void Update()
    {
        if (rotate) transform.Rotate(0, speed * Time.deltaTime, 0);
    }

    public void OnRotatorTogglePressed()
    {
        rotate = !rotate;
    }
}
