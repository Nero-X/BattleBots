using UnityEngine;

public class Bounds : MonoBehaviour
{
    public Action action; // Действие которое выполняется, если объект выходит за рамки арены
    public enum Action { Stop, Destroy }

    Vector2 min;
    Vector2 max;
    const int force = 1;

    // Start is called before the first frame update
    void Start()
    {
        min = Camera.current.ViewportToWorldPoint(new Vector2(0, 0));
        max = Camera.current.ViewportToWorldPoint(new Vector2(1, 1));
        //Debug.Log($"min x = {min.x}, min y = {min.y}, max x = {max.x}, max y = {max.y}");
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.x > max.x)
        {
            //Debug.Log($"x {transform.position.x} > {max.x}");
            if (action == Action.Destroy) Destroy(this.gameObject);
            else transform.position += Vector3.left * force;
        }
        if (transform.position.x < min.x)
        {
            //Debug.Log($"x {transform.position.x} < {min.x}");
            if (action == Action.Destroy) Destroy(this.gameObject);
            else transform.position += Vector3.right * force;
        }
        if (transform.position.y > max.y)
        {
            //Debug.Log($"y {transform.position.y} > {max.y}");
            if (action == Action.Destroy) Destroy(this.gameObject);
            else transform.position += Vector3.down * force;
        }
        if (transform.position.y < min.y)
        {
            //Debug.Log($"y {transform.position.y} < {min.y}");
            if (action == Action.Destroy) Destroy(this.gameObject);
            else transform.position += Vector3.up * force;
        }
    }
}
