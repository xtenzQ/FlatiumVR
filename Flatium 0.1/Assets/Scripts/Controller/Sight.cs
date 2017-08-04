/**
 * Created by Ivan
 * 31.07.17
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Sight - its a class that represent all sight functionnality, for instance waiting of 0.5 second and execute "onsight" method
 */
public class Sight {

	private Sight () {}

	// this must be const... and static... static const
	// well... while this class is static it can be just "private static"
	private static float ONSIGHT_TIME = 1.0f; // 1000 ms, fucking Unity used float
	private static float MOVED_RADIUS = 0.07f; // TODO realy sensetive, and head is always shake a little bit... later, maybe use integral value

	// this var true if mouse wasn't move at least ONSIGHT_TIME time
	public static bool stare { get; private set; }

	// this var true if mouse movement more than MOVED_RADIUS
	public static bool moved { get; private set; }

	// this var store mouse/sight movement. Just for better performance, even if you every time can use "Input.GetAxis("Mouse X/Y")"
	public static Vector2 movement { get; private set; }

	// this var contain Object, that lies under the mouse (usualy it's underlined in some way)
	private static FlatiumObject _focusObject;
	public static FlatiumObject focusObject {
		set {
			if (_focusObject != null) {
				_focusObject.focused = false;
			}
			_focusObject = value;
			if (_focusObject != null) {
				_focusObject.focused = true;
			}
		}
		get {
			return _focusObject;
		}
	}

	// in this var you can find most information about where cursor lies, such as: position (point), distance to focusObject (distance), normal and other
	// this var represent sight functionality. It's some kind of mouse
	public static RaycastHit focus;

	// just a variable that store stare time, used in "updateSight"
	private static float stareTime = 0.0f;

	/**
	 * This function update such variables as "movement", "moved" and "stare"
	 */
	public static void updateSight () {
		movement = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

		bool prevState = moved;
		// WARN no sqrt operation - less precision, more performance
		moved = Mathf.Abs(movement.x) > MOVED_RADIUS || Mathf.Abs(movement.y) > MOVED_RADIUS;
		if (!moved) {
			if (!stare && Time.time - stareTime > ONSIGHT_TIME) {
				stare = true;
				Flatium.onstare ();			
			}
			if (prevState) { // if was moved AND now isn't
				Flatium.onstop();
				stareTime = Time.time; // reset counter
			}
		} else { // if moving
			stare = false;
			Flatium.onmove ();

			stareTime = Time.time; // reset counter
		}
	}

	/**
	 * This function update such variables as "focusObject" and "focus", should be used after "updateSight"
	 */
	public static void updateFocus () {
		if (moved) {
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			// TODO maybe shuold create some variable like TYPE or ID, or use built-in vars like TAG... or something else (!!!)
			// 		should raycast be separate for walls and floors? Is this faster way? At least this thing determine what exactly was collided Wall or Floor
			Physics.Raycast(ray, out focus, Mathf.Infinity, LayerMask.GetMask("Walls and Furniture"));
			if (focus.collider != null) {
				// walls and furniture proccesing
				// TODO potential error place. Why? I get FlatiumObject component... I'm shoked just because it's work and I have a big doubts about it
				// 		imagine if there was two FlatiumObject binded to gameObject. Test it later
				FlatiumObject fo = focus.collider.gameObject.GetComponent<FlatiumObject>();
				focusObject = fo;
			} else {
				Physics.Raycast(ray, out focus, Mathf.Infinity, LayerMask.GetMask("Floor"));
				if (focus.collider != null) {
					// floors proccesing
					FlatiumObject fo = focus.collider.gameObject.GetComponent<FlatiumObject>();
					focusObject = fo;
				} else {
					focusObject = null;
				}
			}            
		}
	}

}
