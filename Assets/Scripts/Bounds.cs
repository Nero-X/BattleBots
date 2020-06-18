using UnityEngine;

public class Bounds : MonoBehaviour
{
    public Action action; // Действие которое выполняется, если объект выходит за рамки арены
    public enum Action { Stop, Destroy }

    public delegate void EventHandler();
    public event EventHandler OnCollisionWithBounds;

    Vector2 min;
    Vector2 max;
    const int force = 1;

    void Start()
    {
        min = Camera.main.ViewportToWorldPoint(new Vector2(0, 0));
        max = Camera.main.ViewportToWorldPoint(new Vector2(1, 1));
        //Debug.Log($"min x = {min.x}, min y = {min.y}, max x = {max.x}, max y = {max.y}");
    }

    void Update()
    {
        if (transform.position.x > max.x)
        {
            //Debug.Log($"x {transform.position.x} > {max.x}");
            OnCollisionWithBounds?.Invoke();
            if (action == Action.Destroy) Destroy(this.gameObject);
            else transform.position += Vector3.left * force;
        }
        if (transform.position.x < min.x)
        {
            //Debug.Log($"x {transform.position.x} < {min.x}");
            OnCollisionWithBounds?.Invoke();
            if (action == Action.Destroy) Destroy(this.gameObject);
            else transform.position += Vector3.right * force;
        }
        if (transform.position.y > max.y)
        {
            //Debug.Log($"y {transform.position.y} > {max.y}");
            OnCollisionWithBounds?.Invoke();
            if (action == Action.Destroy) Destroy(this.gameObject);
            else transform.position += Vector3.down * force;
        }
        if (transform.position.y < min.y)
        {
            //Debug.Log($"y {transform.position.y} < {min.y}");
            OnCollisionWithBounds?.Invoke();
            if (action == Action.Destroy) Destroy(this.gameObject);
            else transform.position += Vector3.up * force;
        }
    }
}
