using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Tail : MonoBehaviour {
    [SerializeField]
    private Animator lineImageAnimator;
    [SerializeField]
    private TMP_Text lineText;
    [SerializeField]
    private Image goalFlag;
    [SerializeField]
    private GameObject goal;
    [SerializeField]
    private float gracePeriodLength = 10.0f;
    [SerializeField]
    private float freeingIntervalLength = 5.0f;
    [SerializeField]
    private float[] BGMCutoffs;

    public bool lineExists { get => people.Count > 0; }

    private enum GraceState {
        PAUSED,
        COUNTING,
        FREEING,
        STOPPED
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
        Invoke("UpdateLineNumber", 0.1f);
    }

	private void Update() {
        UpdateGrace();

        if (people.Count >= GameManager.GOAL_NUM_PEOPLE) {
            ResetGrace();
            graceState = GraceState.STOPPED;

            goalFlag.color = new Color(goalFlag.color.r, goalFlag.color.g, goalFlag.color.b, 1.0f);
            lineText.color = new Color32(0, 222, 59, 255);
            GameManager.player.isThorsty = false;

            foreach (Cop cop in GameManager.cops) {
                cop.Pacify();
			}

            goal.SetActive(true);
        }
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
        UpdateBGMVolumes();
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
        UpdateBGMVolumes();
    }
    
    public void ResetGrace() {
        if (graceState == GraceState.COUNTING || graceState == GraceState.FREEING) {
            CancelInvoke();
            graceState = GraceState.COUNTING;
            graceTimer = 0.0f;
        }
	}

    private void FreeLast() {
        if (people.Last != null) {
            Person freedPerson = people.Last.Value;
            people.RemoveLast();
            freedPerson.Free();
            UpdateLineNumber();
            UpdateBGMVolumes();
        }
        
        if (people.Count == 0) {
            graceState = GraceState.PAUSED;
            CancelInvoke();
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
        lineText.text = string.Format("{0}/{1}", people.Count.ToString(), GameManager.GOAL_NUM_PEOPLE);
	}

    private void UpdateBGMVolumes() {
        float currentLevel = (float) people.Count / GameManager.GOAL_NUM_PEOPLE;
        AudioManager audioManager = AudioManager.instance;
        if (currentLevel == 1) {
            audioManager.Play("DoneTrumpet");
		}

        int BGMIndex = -1;
        for (int i = 0; i < BGMCutoffs.Length; ++i) {
            if (currentLevel > BGMCutoffs[i]) {
                BGMIndex = i;
			}
		}
        BGMIndex++;
        
        string[] BGMNames = new string[] { "LowBGM", "BaseBGM", "MidBGM", "TopBGM" };
        for (int i = 0; i < BGMNames.Length; ++i) {
            if (i == BGMIndex) {
                audioManager.FadeSoundVolume(BGMNames[i], 0.5f, AudioManager.FADE_TIME);
			} else {
                audioManager.FadeSoundVolume(BGMNames[i], 0.0f, AudioManager.FADE_TIME);
            }
		}
	}

}
