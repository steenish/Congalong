using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class HelperFunctions {
    public static Vector3 FindPersonTarget(Transform person) {
        return person.position - person.forward * Constants.PERSON_TARGET_DISTANCE;
    }
}
