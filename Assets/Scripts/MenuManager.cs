using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour {
	private void Update() {
		if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return)) {
			SceneManager.LoadScene("GameScene");
			AudioManager.instance.Play("StartWhistle");
		}
	}
}
