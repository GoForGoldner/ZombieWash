using System.Collections;
using UnityEngine;

public class Movement : MonoBehaviour {
    [SerializeField] private Animator animator;

    [SerializeField]
    private float _speed = 100.0f;

    [SerializeField]
    private float _rotationSpeed = 5.0f;

    [SerializeField] private Vector3 _deskLocation;

    private const float _arrivalThreshold = 0.1f;

    private bool _moving = false;
    private Vector3 _travelCoordinates;

    void Update() {
        if (_moving) {
            animator.SetBool("Moving", true);
            MoveTowardsTarget();
        } else {
            animator.SetBool("Moving", false);
        }
    }

    private void MoveTowardsTarget() {
        // Move towards the target position
        Vector3 direction = (_travelCoordinates - transform.position).normalized;
        float step = _speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, _travelCoordinates, step);

        // Rotate towards the direction of movement
        if (direction != Vector3.zero) {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);
        }

        // Check if the object has reached the target
        if (Vector3.Distance(transform.position, _travelCoordinates) < _arrivalThreshold) {
            _moving = false;
            animator.SetBool("Moving", false); // Stop the animation when reached
            StartCoroutine(ReturnToDesk());

            if (Vector3.Distance(transform.position, _deskLocation) < _arrivalThreshold) {
                transform.rotation = Quaternion.Euler(0, 180, 0);
            }
        }
    }

    private IEnumerator ReturnToDesk() {
        // Wait for 0.25 seconds
        yield return new WaitForSeconds(0.125f);

        // Only set the destination if not already at the desk
        if (Vector3.Distance(transform.position, _deskLocation) > _arrivalThreshold) {
            SetDestination(_deskLocation);
        }
    }

    public void SetDestination(Vector3 newPosition) {
        _moving = true;
        _travelCoordinates = newPosition;
    }
}