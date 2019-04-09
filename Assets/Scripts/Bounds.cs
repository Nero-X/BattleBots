using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bounds : MonoBehaviour
{
    public Action action;
    new public Camera camera;

    public enum Action { Stop, Destroy }

    Vector2 min;
    Vector2 max;

    // Start is called before the first frame update
    void Start()
    {
        min = camera.ViewportToWorldPoint(new Vector2(0, 0));
        max = camera.ViewportToWorldPoint(new Vector2(1, 1));
        //Debug.Log($"min x = {min.x}, min y = {min.y}, max x = {max.x}, max y = {max.y}");
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.x > max.x)
        {
            Debug.Log($"x {transform.position.x} > {max.x}");
            if (action == Action.Destroy) Destroy(this.gameObject);
            else transform.position += Vector3.left;
        }
        if (transform.position.x < min.x)
        {
            Debug.Log($"x {transform.position.x} < {min.x}");
            if (action == Action.Destroy) Destroy(this.gameObject);
            else transform.position += Vector3.right;
        }
        if (transform.position.y > max.y)
        {
            Debug.Log($"y {transform.position.y} > {max.y}");
            if (action == Action.Destroy) Destroy(this.gameObject);
            else transform.position += Vector3.down;
        }
        if (transform.position.y < min.y)
        {
            Debug.Log($"y {transform.position.y} < {min.y}");
            if (action == Action.Destroy) Destroy(this.gameObject);
            else transform.position += Vector3.up;
        }
    }
}
