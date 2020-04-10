using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotate : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void FixedUpdate()
    {
        Quaternion rotationY = Quaternion.AngleAxis(0.01f, new Vector3(0, 1, 0));
        transform.rotation *= rotationY;
    }
}
