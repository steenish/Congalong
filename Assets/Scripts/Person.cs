using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum PersonState {
    FREE,
    DANCING
}

public class Person : MonoBehaviour {
    [SerializeField]
    private float destinationSwitchMinTime = 1.0f;
    [SerializeField]
    private float destinationSwitchMaxTime = 5.0f;
    [SerializeField]
    private float destionationSearchRadius = 10.0f;

    private float destinationSwitchThreshold;
    private float destinationSwitchTimer;
    private NavMeshAgent agent;
    private PersonState state = PersonState.FREE;
#pragma warning disable
    private SphereCollider collider;
#pragma warning restore
    private Vector3 destination;

    void Start() {
        collider = GetComponent<SphereCollider>();
        agent = GetComponent<NavMeshAgent>();
        UpdateDestination();
    }

    void Update() {
        if (state == PersonState.FREE) {
            if (destinationSwitchTimer > destinationSwitchThreshold || Vector3.SqrMagnitude(destination - transform.position) <= 0.3f) {
                UpdateDestination();
			}
            
            destinationSwitchTimer += Time.deltaTime;
		}
    }

	private void OnTriggerEnter(Collider other) {
		if (other.tag == "Head") {
            Tail.instance.AddPerson(this);
		}
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

    private void UpdateDestination() {
        destinationSwitchTimer = 0;
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
