using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField]
    private float speed = 100.0f; // Adjust the speed for a more natural movement
    [SerializeField]
    private float arrivalThreshold = 0.1f;

    private bool moving = false;
    private Vector3 travelCoordinates;

    // Update is called once per frame
    void Update()
    {
        // Handle movement
        if (moving) moveTowardsTarget();

    }

    private void moveTowardsTarget()
    {
        // Move towards the target position
        Vector3 direction = (travelCoordinates - transform.position).normalized;
        float step = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, travelCoordinates, step);

        // Check if the object has reached the target
        if (Vector3.Distance(transform.position, travelCoordinates) < arrivalThreshold)
        {
            moving = false;
        }
    }

    public void setDestination(Vector3 destination)
    {
        moving = true;
        travelCoordinates = destination;
    }
}