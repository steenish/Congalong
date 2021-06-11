using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    [SerializeField]
    private Head head;
    [SerializeField]
    private Tail tail;

    void Start() {
        
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.T)) {
            tail.ClearLine();
		}

        HeadMovement();
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
}
