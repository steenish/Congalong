using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

    public Head head { get; private set; }
    public Tail tail { get; private set; }

    private bool _bottleEquipped = false;
    private bool bottleEquipped {
        get => _bottleEquipped;
        set {
            bottleSlotAnimator.SetBool("Filled", value);
            _bottleEquipped = value;
		}
	}

    public bool isPaused { get; set; }

    private int _drunkenness = 0;
    private int drunkenness {
        get => _drunkenness;
        set {
            if (_drunkenness < value) {
                if (value > Constants.MAX_DRUNKENNESS) {
                    PissSelf();
                    _drunkenness = 0;
                } else {
                    PositiveBottle();
                    _drunkenness = value;
                }
            } else {
                _drunkenness = value;
			}
            
            WCSlider.value = (float)_drunkenness / Constants.MAX_DRUNKENNESS;
        }
	}

    public bool isDrunk { get => drunkenness > 0; }
    public void SoberIncrement() { drunkenness--; }

	private void Awake() {
        head = _head;
        tail = _tail;
	}

	void Update() {
        if (!isPaused) {
            if (Input.GetKeyDown(KeyCode.Space)) {
                DoBottle();
            }

            HeadMovement();
        }

        TailMovement();
    }

    private void HeadMovement() {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");

        head.Move(horizontalInput, verticalInput);
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
		}
	}

    private void PissSelf() {
        tail.ClearLine(true);
	}

    private void PositiveBottle() {
        foreach (Person person in GameManager.people) {
            person.Attract();
		}
        tail.ResetGrace();
	}
}
