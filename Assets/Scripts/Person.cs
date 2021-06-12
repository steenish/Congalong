using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum PersonState {
	FREE,
	DANCING,
	ATTRACTED,
	FLEEING
}

public class Person : MonoBehaviour {
	[SerializeField]
	private float destinationSwitchMinTime = 1.0f;
	[SerializeField]
	private float destinationSwitchMaxTime = 5.0f;
	[SerializeField]
	private float destionationSearchRadius = 10.0f;

	private float attractionTimer = 0.0f;
	private float destinationSwitchThreshold;
	private float destinationSwitchTimer = 0.0f;
	private float fleeingTimer = 0.0f;
	private NavMeshAgent agent;
	private PersonState state = PersonState.FREE;
#pragma warning disable
	private SphereCollider collider;
#pragma warning restore
	private Vector3 destination;

	void Start() {
		collider = GetComponent<SphereCollider>();
		agent = GetComponent<NavMeshAgent>();
		agent.speed = Constants.NORMAL_SPEED;
		UpdateDestination();
	}

	void Update() {
		switch (state) {
			case PersonState.FREE:
				DestinationTick();
				break;
			case PersonState.ATTRACTED:
				if (attractionTimer > Constants.ATTRACTION_TIME) {
					FreeToRoam();
				} else {
					agent.SetDestination(GameManager.player.head.transform.position);
					attractionTimer += Time.deltaTime;
				}
				break;
			case PersonState.FLEEING:
				DestinationTick();

				if (fleeingTimer > Constants.FLEEING_TIME) {
					FreeToRoam();
				} else {
					fleeingTimer += Time.deltaTime;
				}
				break;
		}
	}

	private void OnTriggerEnter(Collider other) {
		if (other.tag == "Head") {
			Tail.instance.AddPerson(this);
		}
	}

	private void FreeToRoam() {
		state = PersonState.FREE;
		UpdateDestination();
		agent.speed = Constants.NORMAL_SPEED;
	}

	private void DestinationTick() {
		if (destinationSwitchTimer > destinationSwitchThreshold || Vector3.SqrMagnitude(destination - transform.position) <= 0.3f) {
			UpdateDestination();
		}

		destinationSwitchTimer += Time.deltaTime;
	}

	public void Free() {
		state = PersonState.FREE;
		collider.isTrigger = true;
		gameObject.tag = "Person";
		agent.isStopped = false;
		UpdateDestination();
	}

	public void Capture() {
		state = PersonState.DANCING;
		collider.isTrigger = false;
		gameObject.tag = "Tail";
		agent.isStopped = true;
	}

	public void Move(Transform previousPerson) {
		transform.LookAt(previousPerson.position);
		transform.position = Vector3.MoveTowards(transform.position, HelperFunctions.FindPersonTarget(previousPerson), Constants.MAX_PERSON_MOVE_DELTA * Time.deltaTime);
	}

	public void Attract() {
		if (state == PersonState.FREE) {
			state = PersonState.ATTRACTED;
			attractionTimer = 0.0f;
			agent.speed = Constants.ATTRACTION_SPEED;
		}
	}

	public void Flee() {
		state = PersonState.FLEEING;
		fleeingTimer = 0.0f;
		agent.speed = Constants.FLEEING_SPEED;
	}

	private void UpdateDestination() {
		destinationSwitchTimer = 0.0f;
		destinationSwitchThreshold = Random.Range(destinationSwitchMinTime, destinationSwitchMaxTime);
		destination = PickNewDestination();
		agent.SetDestination(destination);
	}

	private Vector3 PickNewDestination() {
		Vector3 randomDirection = Random.insideUnitSphere * destionationSearchRadius;
		randomDirection += transform.position;
		NavMeshHit hit;
		Vector3 finalPosition = Vector3.zero;
		if (NavMesh.SamplePosition(randomDirection, out hit, destionationSearchRadius, 1)) {
			finalPosition = hit.position;
		}
		return finalPosition;
	}
}
