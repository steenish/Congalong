using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class HelperFunctions {
    private const float PERSON_TARGET_DISTANCE = 1.5f;
    public static Vector3 FindPersonTarget(Transform person) {
        return person.position - person.forward * PERSON_TARGET_DISTANCE;
    }
}
