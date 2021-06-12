using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tail : MonoBehaviour {
    [SerializeField]
    private float gracePeriodLength = 10.0f;
    [SerializeField]
    private float freeingIntervalLength = 5.0f;

    private enum GraceState {
        PAUSED,
        COUNTING,
        FREEING
	}
    
    private float graceTimer = 0.0f;
    private GraceState graceState = GraceState.PAUSED;
    private LinkedList<Person> people;

    public static Tail instance;

    void Start() {
        if (instance == null) {
            instance = this;
		} else {
            Destroy(gameObject);
		}

        people = new LinkedList<Person>();
    }

	private void Update() {
        UpdateGrace();
    }

	public void Move(Transform headTransform) {
        Transform previousPerson = headTransform;

        foreach (Person person in people) {
            person.Move(previousPerson);
            previousPerson = person.transform;
		}
	}

    public void AddPerson(Person person) {
        person.Capture();
        people.AddLast(person);

        CancelInvoke();
        graceState = GraceState.COUNTING;
        graceTimer = 0.0f;
	}

    public void ClearLine() {
        foreach (Person person in people) {
            person.Free();
		}

        people.Clear();
        graceState = GraceState.PAUSED;
	}

    private void FreeLast() {
        Person freedPerson = people.Last.Value;
        people.RemoveLast();
        freedPerson.Free();

        if (people.Count == 0) {
            graceState = GraceState.PAUSED;
		}
	}

    private void UpdateGrace() {
        if (graceTimer > gracePeriodLength && graceState == GraceState.COUNTING) {
            InvokeRepeating("FreeLast", freeingIntervalLength, freeingIntervalLength);
            graceState = GraceState.FREEING;
        }

        if (graceState == GraceState.COUNTING) {
            graceTimer += Time.deltaTime;
        }
    }
}
