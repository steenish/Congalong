using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum CopState {
    PATROLLING,
    CHASING
}

public class Cop : MonoBehaviour {
    [SerializeField]
    private Transform _patrolRoute;

    private Vector3[] patrolRoute;

    private const float CHASE_TIME = 10.0f;
    private const float COOLDOWN_TIME = 5.0f;
    private const float DISTANCE_THRESHOLD = 0.4f;

    private CopState state;
    private float chaseTimer = 0.0f;
    private float cooldownTimer = 0.0f;
    private int _currentPatrolIndex = 0;
    private int currentPatrolIndex {
        get => _currentPatrolIndex;
        set {
            if (value >= patrolRoute.Length) {
                _currentPatrolIndex = 0;
			} else {
                _currentPatrolIndex = value;
			}
		}
	}
    private NavMeshAgent agent;

    void Start() {
        List<Vector3> tempPatrolRoute = new List<Vector3>();
        foreach (Transform point in _patrolRoute) {
            tempPatrolRoute.Add(point.position);
		}
        patrolRoute = tempPatrolRoute.ToArray();

        agent = GetComponent<NavMeshAgent>();
        agent.isStopped = true;
    }

    void Update() {
        switch (state) {
            case CopState.PATROLLING:
                Patrol();

                cooldownTimer += Time.deltaTime;
                break;
            case CopState.CHASING:
                if (chaseTimer > CHASE_TIME) {
                    ResetToPatrol();
                } else {
                    agent.SetDestination(GameManager.player.head.transform.position);
				}

                chaseTimer += Time.deltaTime;
                break;
		}
    }

	private void OnTriggerEnter(Collider other) {
		if (other.tag == "Head" && state == CopState.CHASING) {
            GameManager.player.tail.ClearLine(true);
            ResetToPatrol();
		}
	}

    private void ResetToPatrol() {
        state = CopState.PATROLLING;
        cooldownTimer = 0.0f;
        agent.SetDestination(patrolRoute[currentPatrolIndex]);
    }

	public void Alert() {
        if (state == CopState.PATROLLING && cooldownTimer > COOLDOWN_TIME) {
            state = CopState.CHASING;
            chaseTimer = 0.0f;
		}
	}

    private void Patrol() {
        if (agent.isStopped || Vector3.SqrMagnitude(patrolRoute[currentPatrolIndex] - transform.position) <= DISTANCE_THRESHOLD) {
            agent.isStopped = false;
            currentPatrolIndex += 1;
            agent.SetDestination(patrolRoute[currentPatrolIndex]);
		}
	}
}
