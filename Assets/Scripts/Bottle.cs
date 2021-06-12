using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bottle : MonoBehaviour {
	public int location { get; set; }

	private void OnTriggerEnter(Collider other) {
		if (other.tag == "Head") {
			GameManager.player.EquipBottle();
			SpawnNewBottle();
		}
	}

	private void SpawnNewBottle() {
		GameManager.bottleSpawned[location] = false;

		Vector3[] spawnLocations = GameManager.bottleSpawnLocations;
		Vector3 headPosition = GameManager.player.head.transform.position;
		Dictionary<float, int> distancesToIndex = new Dictionary<float, int>();
		for (int i = 0; i < spawnLocations.Length; ++i) {
			distancesToIndex.Add(Vector3.SqrMagnitude(spawnLocations[i] - headPosition), i); 
		}

		List<float> distances = new List<float>();
		foreach (float distance in distancesToIndex.Keys) {
			distances.Add(distance);
		}
		distances.Sort();

		for (int i = distances.Count - 1; i >= 0; --i) {
			int index = distancesToIndex[distances[i]];

			if (!GameManager.bottleSpawned[index]) {
				transform.position = spawnLocations[index];
				GameManager.bottleSpawned[index] = true;
				location = index;
				break;
			}
		}
	}
}
