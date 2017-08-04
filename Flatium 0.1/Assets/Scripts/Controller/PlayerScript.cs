/**
 * Created by Ivan
 * 25.07.17
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour {

    public static GameObject instance;

	private float height = 1.5f;
    private float speed = 3.0f;
	private float rotation_sensety = 5.0f;

	void Awake () {
		instance = gameObject; // some kind of singleton?	
	}

	void Start () {
		gameObject.tag = "Player";
    }
	
	void Update () {
        updateMove ();
		updateRotation ();
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

	private void updateRotation () {		
		if (Input.GetMouseButton(0)) {
			this.transform.eulerAngles += new Vector3(-Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X"), 0) * rotation_sensety;
		}
	}

}
