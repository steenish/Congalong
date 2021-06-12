using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopTriggerSphere : MonoBehaviour {
	[SerializeField]
	private Cop parentCop;
	
	private void OnTriggerStay(Collider other) {
		if (other.tag == "Head" || other.tag == "Tail") {
			parentCop.Alert();
		}
	}
}
