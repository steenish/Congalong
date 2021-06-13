using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Head : MonoBehaviour {
    [SerializeField]
    private float swayAmplitude = 0.02f;
    [SerializeField]
    private float swayPeriod = 0.3f;

    private const float HEAD_TARGET_DISTANCE = 1.5f;
    private const float MAX_HEAD_MOVE_DELTA = 5.0f;

    private Rigidbody rb;

	private void Start() {
        rb = GetComponent<Rigidbody>();
	}

	private void Update() {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
	}

	public void Move(float horizontalInput, float verticalInput, float swayAmount) {
        Vector3 deltaMovement = Vector3.Normalize(Vector3.right * horizontalInput + Vector3.forward * verticalInput);
        float sway = Mathf.Sin(Time.time * swayPeriod) * swayAmplitude * Mathf.Clamp01(Mathf.Abs(horizontalInput) + Mathf.Abs(verticalInput));
        Vector3 target = transform.position + deltaMovement * HEAD_TARGET_DISTANCE + transform.right * sway * swayAmount;
        Debug.DrawRay(transform.position, (target - transform.position) * 10, Color.red, Time.deltaTime);
        
        if (deltaMovement != Vector3.zero) {
            Quaternion targetRotation = Quaternion.LookRotation(target - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 10 * Time.deltaTime);
        }
        
        //transform.LookAt(target);
        transform.position = Vector3.MoveTowards(transform.position, target, MAX_HEAD_MOVE_DELTA * Time.deltaTime);
	}

    public void Hide() {
        GetComponentInChildren<SpriteRenderer>().enabled = false;
	}

    public void Unhide() {
        GetComponentInChildren<SpriteRenderer>().enabled = true;
    }
}
