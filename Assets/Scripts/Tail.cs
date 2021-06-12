using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Tail : MonoBehaviour {
    [SerializeField]
    private Animator lineImageAnimator;
    [SerializeField]
    private TMP_Text lineText;
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
    private GraceState _graceState = GraceState.PAUSED;
    private GraceState graceState {
        get => _graceState;
        set {
            switch (value) {
                case GraceState.COUNTING:
                case GraceState.PAUSED:
                    lineImageAnimator.SetBool("Vibrating", false);
                    lineText.color = new Color32(0, 0, 0, 255);
                    break;
                case GraceState.FREEING:
                    lineImageAnimator.SetBool("Vibrating", true);
                    lineText.color = new Color32(255, 120, 0, 255);
                    break;
			}
            _graceState = value;
		}
	}
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

        graceState = GraceState.COUNTING;
        ResetGrace();
        UpdateLineNumber();
	}

    public void ClearLine(bool flee) {
        foreach (Person person in people) {
            person.Free();

            if (flee) {
                person.Flee();
			}
		}

        people.Clear();
        graceState = GraceState.PAUSED;
        UpdateLineNumber();
    }
    
    public void ResetGrace() {
        if (graceState == GraceState.COUNTING || graceState == GraceState.FREEING) {
            CancelInvoke();
            graceState = GraceState.COUNTING;
            graceTimer = 0.0f;
        }
	}

    private void FreeLast() {
        Person freedPerson = people.Last.Value;
        people.RemoveLast();
        freedPerson.Free();
        UpdateLineNumber();

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

    private void UpdateLineNumber() {
        lineText.text = people.Count.ToString();
	}
}
