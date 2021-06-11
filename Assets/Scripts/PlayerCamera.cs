using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour {
	[SerializeField]
	private Transform _cameraTarget;
	public Transform cameraTarget { get => _cameraTarget; set => _cameraTarget = value; }

	[SerializeField]
	private Vector3 _cameraTranslation;
	public Vector3 cameraTranslation { get => _cameraTranslation; set => _cameraTranslation = value; }

	[SerializeField]
	private float cameraSmoothTime = 0.1f;

	private Vector3 currentVelocity = Vector3.zero;

	private void Start() {
		transform.position = cameraTarget.position;
	}

	private void Update() {
		// Update camera position.
		transform.position = Vector3.SmoothDamp(transform.position, cameraTarget.position + cameraTranslation, ref currentVelocity, cameraSmoothTime);
	}
}
