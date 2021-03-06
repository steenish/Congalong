using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MonoBehaviour {
    [SerializeField]
    private Head _head;
    [SerializeField]
    private Tail _tail;
    [SerializeField]
    private Slider WCSlider;
    [SerializeField]
    private Animator bottleSlotAnimator;
    [SerializeField]
    private Animator pissPuddleAnimator;

    public Head head { get; private set; }
    public Tail tail { get; private set; }

    private const int MAX_DRUNKENNESS = 3;

    private bool _bottleEquipped = false;
    private bool bottleEquipped {
        get => _bottleEquipped;
        set {
            bottleSlotAnimator.SetBool("Filled", value);
            _bottleEquipped = value;
		}
	}

    public bool isPaused { get; set; }
    public bool isThorsty { get; set; } = true;

    private int _drunkenness = 0;
    private int drunkenness {
        get => _drunkenness;
        set {
            if (_drunkenness < value) {
                if (value > MAX_DRUNKENNESS) {
                    PissSelf();
                    _drunkenness = 0;
                } else {
                    PositiveBottle();
                    _drunkenness = value;
                }
            } else {
                _drunkenness = value;
			}
            
            WCSlider.value = (float)_drunkenness / MAX_DRUNKENNESS;
        }
	}

    public bool isDrunk { get => drunkenness > 0; }
    public void SoberIncrement() { drunkenness--; }

	private void Awake() {
        head = _head;
        tail = _tail;
	}

	void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            SceneManager.LoadScene("MainMenuScene");
		}

        if (!isPaused) {
            if (Input.GetKeyDown(KeyCode.Space) && isThorsty) {
                DoBottle();
            }

            HeadMovement();
        }

        TailMovement();
    }

    private void HeadMovement() {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");

        head.Move(horizontalInput, verticalInput, (float)drunkenness / MAX_DRUNKENNESS);
	}

    private void TailMovement() {
        tail.Move(head.transform);
	}

    public void EquipBottle() {
        bottleEquipped = true;
	}

    private void DoBottle() {
        if (bottleEquipped) {
            bottleEquipped = false;
            drunkenness++;
            AudioManager.instance.Play("BottlePop");
            AudioManager.instance.Play("PartyHorn");
        }
	}

    private void PissSelf() {
        tail.ClearLine(true);
        AudioManager.instance.Play("PeeingGround");
        pissPuddleAnimator.SetTrigger("ShowPuddle");
        pissPuddleAnimator.transform.position = new Vector3(head.transform.position.x, pissPuddleAnimator.transform.position.y, head.transform.position.z);
	}

    private void PositiveBottle() {
        foreach (Person person in GameManager.people) {
            person.Attract();
		}
        head.spriteAnimator.SetTrigger("PopBottle");
        tail.ResetGrace();
	}
}
