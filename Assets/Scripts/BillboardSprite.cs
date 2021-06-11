using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillboardSprite : MonoBehaviour {
    [SerializeField]
    private Camera targetCamera;

    void Update() {
        transform.LookAt(targetCamera.transform.position);
    }
}
