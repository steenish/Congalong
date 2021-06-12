using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    [SerializeField]
    private Transform[] _bottleSpawnLocations;

    public static int GOAL_NUM_PEOPLE { get; private set; }

    public static GameManager instance;
    public static Player player;
    public static Person[] people;
    public static Cop[] cops;
    public static Bottle[] bottles;
    public static Vector3[] bottleSpawnLocations;
    public static bool[] bottleSpawned;

    void Start() {
        if (instance == null) {
            instance = this;
		} else {
            Destroy(gameObject);
		}

        player = GameObject.Find("Player").GetComponent<Player>();
        people = GameObject.Find("People").GetComponentsInChildren<Person>();
        GOAL_NUM_PEOPLE = people.Length == 0 ? 10 : people.Length;
        cops = GameObject.Find("Cops").GetComponentsInChildren<Cop>();
        bottles = GameObject.Find("Bottles").GetComponentsInChildren<Bottle>();

        bottleSpawnLocations = new Vector3[_bottleSpawnLocations.Length];
        for (int i = 0; i < _bottleSpawnLocations.Length; ++i) {
            bottleSpawnLocations[i] = _bottleSpawnLocations[i].position;
		}

        bottleSpawned = new bool[bottleSpawnLocations.Length];

        Shuffle(bottleSpawnLocations);
        for (int i = 0; i < bottleSpawnLocations.Length && i < bottles.Length; ++i) {
            bottles[i].transform.position = bottleSpawnLocations[i];
            bottleSpawned[i] = true;
            bottles[i].location = i;
		}
    }

    private static void Shuffle(Vector3[] array) {
        int n = array.Length;
        while (n > 1) {
            int k = Random.Range(0, n--);
            Vector3 temp = array[n];
            array[n] = array[k];
            array[k] = temp;
        }
    }
}
