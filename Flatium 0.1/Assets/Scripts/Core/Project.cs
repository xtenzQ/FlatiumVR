/**
 * Created by Ivan
 * 26.07.17
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * This class stores all objects in the room
 * That makes easy select all objects in the room
 */
public class Room {

	public Room (Transform root) {
		// great... So, when I export project with cinema all objects with same name
		// like "Wall" replaced by "Wall_1", "Wall_2" and e.t.c.
		// so, this fucking HACK is temporary solution. Use not a .Find(name), but getChild(i)
		// and then check it's name... agrh, TODO should think about export structure
		for (int i = 0; i < root.childCount; i++) {
			Transform roomObject = root.GetChild (i);
			if (roomObject.name.Contains("Walls")) {
				walls = new Wall[roomObject.childCount];
				for (int j = 0; j < roomObject.childCount; j++) {
					walls [j] = roomObject.GetChild (j).gameObject.AddComponent<Wall> ();
				}
			}
			if (roomObject.name.Contains("Floor")) {
				floor = roomObject.gameObject.AddComponent<Floor>();
			}
			if (roomObject.name.Contains("Plinth")) {
				plinth = roomObject;
			}
		}
		// TODO use when export structure will be fixed!
		/*
			// TODO improve this... "error catch system"...
		Transform root = roomRoot.Find ("Walls");
		if (root != null) {
			rooms[i].walls = new Wall[root.childCount];
			for (int j = 0; j < root.childCount; j++) {
				rooms [i].walls [j] = root.GetChild (j).gameObject.AddComponent<Wall> ();
			}
		}
		*/
	}

	public Wall[] walls; // all walls of the room
	public Floor floor; // floor of the room
	//public Ceiling ceilings; //TODO Ceiling class
	public Transform plinth; // think about it

}

/**
 * this static class represent Flatium content. Ideally, instances of this classes must be contained in some DB
 * TODO DB
 */
public class Project {

	public static GameObject instance; // project root in hierarchy

	public static Room[] rooms;	// All rooms in current project

	private Project () {} // It's a static class

	public static void reset () {
		GameObject.Destroy (instance);
		if ((instance = GameObject.FindGameObjectWithTag("Project")) == null) { 
			instance = new GameObject ("Project");
			instance.tag = "Project";
		}

		rooms = null;
    }

	public static void save(string url) {
        //TODO project saving into... somewhere. Is this function even required?
    }

	public static void open(string url) {
		reset (); // reset previous project

		// put project into world
		instance = UnityEngine.Object.Instantiate(Resources.Load (url) as GameObject);

		// get "Rooms" root
		Transform roomsRoot = instance.GetComponent<Transform> ().Find ("Rooms");
		// set "rooms" array length as in "Room" root
		rooms = new Room[roomsRoot.childCount];
		for (int i = 0; i < roomsRoot.childCount; i++) {
			// creating rooms
			rooms [i] = new Room (roomsRoot.GetChild (i));
		}

		// WARN remove on release
		Debug.Log ("Project loaded: " + url);
	}			
}


