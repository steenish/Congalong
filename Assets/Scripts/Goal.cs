using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class Goal : MonoBehaviour {
	[SerializeField]
	private float walkSpeed = 1.0f;
	[SerializeField]
	private float waitTime = 10.0f;
	[SerializeField]
	private GameObject winText;
	[SerializeField]
	private Transform walkTarget;
	[SerializeField]
	private Collider townHallCollider;
	[SerializeField]
	private GameObject goalArrow;

	private bool moving = false;
	private float waitTimer = 0.0f;
	private CapsuleCollider triggerZone;

	private void Start() {
		triggerZone = GetComponent<CapsuleCollider>();
	}

	private void OnTriggerEnter(Collider other) {
		if (other.tag == "Head") {
			moving = true;
			GameManager.player.isPaused = true;
			winText.SetActive(true);
			GameManager.player.head.transform.LookAt(walkTarget);
			townHallCollider.enabled = false;
			Camera.main.GetComponent<PlayerCamera>().isHeld = true;
			goalArrow.SetActive(false);
			AudioManager.instance.Play("WinTrumpet");
			AudioManager.instance.Play("CheeringNoise");

			foreach (Person person in GameManager.people) {
				person.GetComponent<NavMeshAgent>().enabled = false;
			}
		}
	}

	private void Update() {
		if (moving) {
			GameManager.player.head.transform.position += GameManager.player.head.transform.forward * walkSpeed * Time.deltaTime;
			waitTimer += Time.deltaTime;
		}

		if (waitTimer > waitTime) {
			SceneManager.LoadScene("MainMenuScene");
		}
	}
}
