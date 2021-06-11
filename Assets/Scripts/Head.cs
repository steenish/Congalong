using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Head : MonoBehaviour {
    void Start() {
        
    }

    void Update() {
        
    }

	public void Move(float horizontalInput, float verticalInput) {
        Vector3 deltaMovement = Vector3.Normalize(Vector3.right * horizontalInput + Vector3.forward * verticalInput);
        Vector3 target = transform.position + deltaMovement * Constants.HEAD_TARGET_DISTANCE;
        transform.LookAt(target);
        transform.position = Vector3.MoveTowards(transform.position, target, Constants.MAX_HEAD_MOVE_DELTA * Time.deltaTime);
	}
}