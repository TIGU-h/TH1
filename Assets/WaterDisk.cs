using UnityEngine;

public class WaterDisk : MonoBehaviour
{
    public float speed = 10f;


    private Transform target;
    private bool initialized = false;

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;

        if (target != null)
        {
            Vector3 direction = (target.position - transform.position).normalized;
            direction.y += 0.3f;
            transform.rotation = Quaternion.LookRotation(direction);
        }

        initialized = true;
    }

    private void Update()
    {
        if (!initialized) return;

        transform.position += transform.forward * speed * Time.deltaTime;
    }
}
