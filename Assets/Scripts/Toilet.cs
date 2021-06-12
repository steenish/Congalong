using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Toilet : MonoBehaviour {
	[SerializeField]
	private float stepTime = 1.0f;
	[SerializeField]
	private Transform childBoxCollider;

	private void OnTriggerEnter(Collider other) {
		if (other.tag == "Head" && GameManager.player.isDrunk) {
			other.transform.position = new Vector3(childBoxCollider.position.x, other.transform.position.y, childBoxCollider.position.z);
			GameManager.player.isPaused = true;
			GameManager.player.head.Hide();
			InvokeRepeating("SoberUpAndCheck", stepTime, stepTime);
		}
	}

	private void SoberUpAndCheck() {
		Player player = GameManager.player;
		player.SoberIncrement();
		if (!player.isDrunk) {
			CancelInvoke();
			player.head.transform.position -= transform.right * 3.0f;
			player.isPaused = false;
			player.head.Unhide();
		}
	}
}
