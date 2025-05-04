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
            // ÷ентруЇмо ц≥ль Ч додаЇмо зсув по Y
            Vector3 targetPosition = target.position + Vector3.up * 1.2f; 
            Vector3 direction = (targetPosition - transform.position).normalized;
            transform.rotation = Quaternion.LookRotation(direction);
        }

        initialized = true;
        Destroy(gameObject, 2f);
    }


    private void Update()
    {
        if (!initialized) return;

        transform.position += transform.forward * speed * Time.deltaTime;
    }
}
