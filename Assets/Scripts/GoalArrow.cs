using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalArrow : MonoBehaviour {
	[SerializeField]
	private float margin = 1.0f;
	[SerializeField]
	private float minimum = 1.0f;

	private Transform target;
	private Transform container;

	private void Start() {
		container = transform.parent;
		target = container.parent;
	}

	void Update() {
		Transform head = GameManager.player.head.transform;
		target.LookAt(head);

		container.localPosition = new Vector3(0.0f, 0.0f, Mathf.Clamp(Vector3.Magnitude(target.position - head.position) - margin, minimum, Mathf.Infinity));
    }
}
