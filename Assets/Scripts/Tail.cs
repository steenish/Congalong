using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tail : MonoBehaviour {

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
	}

    public void ClearLine() {
        foreach (Person person in people) {
            person.Free();
		}

        people.Clear();
	}
}
