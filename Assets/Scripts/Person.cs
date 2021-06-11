using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PersonState {
    FREE,
    DANCING
}

public class Person : MonoBehaviour {
    private SphereCollider collider;
    private PersonState state = PersonState.FREE;

    void Start() {
        collider = GetComponent<SphereCollider>();
    }

    void Update() {
        
    }

	private void OnTriggerEnter(Collider other) {
		if (other.tag == "Head" || other.tag == "Tail") {
            Tail.instance.AddPerson(this);
		}
	}

    public void Free() {
        state = PersonState.FREE;
        collider.isTrigger = true;
	}

    public void Capture() {
        state = PersonState.DANCING;
        collider.isTrigger = false;
	}

	public void Move(Transform previousPerson) {
        transform.LookAt(previousPerson.position);
        transform.position = Vector3.MoveTowards(transform.position, HelperFunctions.FindPersonTarget(previousPerson), Constants.MAX_PERSON_MOVE_DELTA * Time.deltaTime);
	}
}
