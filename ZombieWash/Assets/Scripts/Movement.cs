using UnityEngine;

public class Movement : MonoBehaviour
{
    public float speed = 100.0f; // Adjust the speed for a more natural movement
    private bool moving = false;
    private Vector3 travelCoordinates;

    // Update is called once per frame
    void Update()
    {
        // Handle movement
        if (moving)
        {
            // Move towards the target position
            Vector3 direction = (travelCoordinates - transform.position).normalized;
            float step = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, travelCoordinates, step);

            // Check if the object has reached the target
            if (Vector3.Distance(transform.position, travelCoordinates) < 0.1f)
            {
                moving = false;
            }
        }

        // Check for mouse click to set new destination
        if (Input.GetMouseButtonDown(1))
        {
            moving = true;

            // Convert mouse position to world position
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                travelCoordinates = hit.point; // Set the target to the hit point on the collider
                travelCoordinates.y = transform.position.y;
            }
            else
            {
                travelCoordinates = ray.GetPoint(0); // Example: point 10 units in front of the camera
            }

            print(travelCoordinates);
        }
    }
}
