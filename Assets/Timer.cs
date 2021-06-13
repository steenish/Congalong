using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour {
	[SerializeField]
	private TMP_Text timerText;

	public bool isPaused { get; set; } = false;

	private float timeValue = 0.0f;
	
	private void Update() {
		if (!isPaused) {
			timeValue += Time.deltaTime;
			timeValue = Mathf.Clamp(timeValue, 0.0f, 5999.0f);
			//timerText.text = Mathf.FloorToInt(timeValue).ToString();
			timerText.text = FormatTime();
		}
	}

	private string FormatTime() {
		int value = Mathf.FloorToInt(timeValue);
		return string.Format("{0}:{1}", Mathf.FloorToInt((float)value / 60.0f).ToString("D2"), (value % 60).ToString("D2"));
	}
}
