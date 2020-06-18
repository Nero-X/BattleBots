using UnityEngine;

public class CameraRotate : MonoBehaviour
{
    void FixedUpdate()
    {
        Quaternion rotationY = Quaternion.AngleAxis(0.01f, new Vector3(0, 1, 0));
        transform.rotation *= rotationY;
    }
}
