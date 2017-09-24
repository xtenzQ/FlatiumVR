/**
 * Created by Ivan
 * 25.07.17
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    public static Player instance;

	private static Vector3 height = new Vector3(0, 1.5f, 0);
    private float speed = 3.0f;

	void Awake () {
		instance = this;
	}

	void Start () {
		gameObject.tag = "Player";
    }
	
	void Update () {
        updateMove ();
    }

	public void moveTo (Vector3 point) {
		gameObject.transform.position = point + height;
	}

    private void updateMove () {
        Vector3 movement = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        if (movement.x < 0.5 && movement.x > -0.5) {
            movement.x = 0;
        }
        if (movement.z < 0.5 && movement.z > -0.5) {
            movement.z = 0;
        }
        if (movement.x != 0 || movement.z != 0) {
            instance.transform.Translate(movement.normalized * speed * Time.deltaTime);
        }
    }

}
