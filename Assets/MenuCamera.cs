using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuCamera : MonoBehaviour {
    [SerializeField]
    private Transform route;
    [SerializeField]
    private float cameraSpeed;
    [SerializeField]
    private float rotationSpeed;

    private Transform[] checkpoints;
    private int _checkpointIndex = 0;
    private int checkpointIndex {
        get => _checkpointIndex;
        set {
            if (value >= checkpoints.Length) {
                _checkpointIndex = 0;
			} else {
                _checkpointIndex = value;
			}
		}
	}
    private bool started = false;

    void Start() {
        List<Transform> tempCheckpoints = new List<Transform>();
        foreach (Transform checkpoint in route) {
            tempCheckpoints.Add(checkpoint);
		}
        checkpoints = tempCheckpoints.ToArray();
    }

    void Update() {
        transform.position += transform.forward * cameraSpeed * Time.deltaTime;
        var targetRotation = Quaternion.LookRotation(checkpoints[checkpointIndex].position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        if (!started || Vector3.SqrMagnitude(transform.position - checkpoints[checkpointIndex].position) < 0.1f) {
            started = true;
            checkpointIndex++;
        }
    }
}
