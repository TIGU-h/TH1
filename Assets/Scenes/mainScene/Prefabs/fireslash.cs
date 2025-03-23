using UnityEngine;

public class fireslash : MonoBehaviour
{
    [SerializeField] private float speed;
    private void Start()
    {
        Destroy(gameObject, 2);
    }

    void Update()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
    }
}
