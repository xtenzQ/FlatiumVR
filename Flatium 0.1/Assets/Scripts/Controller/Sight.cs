/**
 * Created by Ivan
 * 31.07.17
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Sight - its a class that represent all sight functionnality, for instance waiting of 0.5 second and execute "onsight" method
 * TODO I don't realy sure that Sight must extend MonoBehaviour
 */
public class Sight : MonoBehaviour {

	// TODO remove "Flatium.focus" here

	// singleton...
	public static Sight instance;

	// TODO this must be const... and static... static const
	// 		well... while this class is lonely singleton it can be just "private const"
	private const float ONSIGHT_TIME = 1.0f; // 500 ms, fucking Unity used float
	private const float MOVED_RADIUS = 0.07f; // TODO realy sensetive, and head is always shake a little bit... later, maybe use integral value

	private float stareTime = 0.0f;

	// this var true if mouse wasn't move at least ONSIGHT_TIME time
	public bool stare = false;
	// this var true if mouse movement more than MOVED_RADIUS
	public bool moved = false;

	// this var store mouse/sight movement. Just for better performance, even if you every time can use "Input.GetAxis("Mouse X/Y")"
	public Vector2 movement;

	void Awake () {
		Sight.instance = this;
	}

	void Start () {
		
	}
	
	void Update () {
		movement = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

		// WARN no sqrt operation - less precision, more performance
		updateSight (moved, Mathf.Abs(movement.x) > MOVED_RADIUS || Mathf.Abs(movement.y) > MOVED_RADIUS);
	}

	// TODO how to execute onsight when time is up?
	//		first - execute it only one time. Realy good variant I think, but...
	//		second - execute it every time, while you can. Required repeate check on the handler side
	//		third - execute it time to time, over and over again, just reset counter
	private void updateSight(bool prevState, bool currentState) {
		moved = currentState;
		if (currentState) { // if moving
			stare = false;
			Flatium.onmove ();

			stareTime = Time.time; // reset counter
		} else {			
			if (!stare && Time.time - stareTime > ONSIGHT_TIME) {
				stare = true;
				Flatium.onstare ();			
			}
			if (prevState) { // if was moved AND now isn't
				Flatium.onstop();

				stareTime = Time.time; // reset counter
			}
		}
	}

}
