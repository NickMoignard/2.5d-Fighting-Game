using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitbox : MonoBehaviour {
    private HitCollider hitCollider;

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("hitbox was hit!");
    }

}
