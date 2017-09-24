using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FloorMenu : MonoBehaviour
{

	public static FloorMenu instance;

    private bool movementMode;

	public Button transitionButton;
	public Button editorButton;
	public Button menuButton;

	void Start () {
		instance = this;
		syncWithPlayer ();
	}
	
	void Update () {
		/*if (movementMode) {
			if (Sight.instance.focusObject.tag == "Floor") {
				Sight.instance.OnStare (() => {
					Player.instance.moveTo (Sight.instance.rayHit.point);
					syncWithPlayer ();
					switchMovement ();
				});
			} else {
				Sight.instance.OnStareStop ();
			}
		}*/
	}

    public void syncWithPlayer() {
        var playerRotationY = Quaternion.AngleAxis(Player.instance.transform.eulerAngles.y, Vector3.up);
        var playerPosition = Player.instance.transform.position +
                   playerRotationY *
                   Vector3.forward;
        gameObject.transform.rotation = playerRotationY * Quaternion.AngleAxis(90, Vector3.right);
        gameObject.transform.position = new Vector3(playerPosition.x, 0.01f, playerPosition.z);
    }
		    
	public void switchMovement() {
        movementMode = !movementMode;
		Flatium.instance.setState ( (movementMode) ? 1 : 0 ); // TODO state stack
    }
}