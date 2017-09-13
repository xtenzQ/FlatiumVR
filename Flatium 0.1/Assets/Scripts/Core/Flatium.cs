/**
 * Created by Ivan
 * 25.07.17
 */
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 *  Flatium contain main logic of program
 *  TODO WTF, this is singleton or static class? (!) working on it IVAN
 */
public class Flatium : MonoBehaviour {

    // TODO WARN this is debug vars. Remove on release
    //      should be replaced by wireframe shader drawing
    public static Material STANDARD_MATERIAL;
    public static Material SELECTED_MATERIAL; 
    public static Material FOCUSED_MATERIAL;

	// such var used in singleton patterns... think about it
    public static Flatium instance;

	// Find solution!
	// Using of C# event system
	// Flatium will be publisher and objects subscribers
	// working on it IVAN
	//===========================================EVENTS===========================================//
	// TODO it's first time when I use delegates. Delegate principle are cleare, but the system I made looks like a piece of shit..
	// delegate
	// if future this method request some args, you can use "params int[] args"
	public delegate void sightCallback();
	public delegate void clickCallback(FlatiumObject fo);

	// TODO why so difficult? Despite that it's a shit code it's allow to construct your own events system
	// 		when the whole structure of the project will be ready and clearly obvious for us, this system should be reduced and optimized
	// 		seriously, I don't like it, but for the start - deal with it

	// TODO the most difficult part is to create all variety of callbacks in all classes (like Wall, Floor and Furniture) - this is the easiest part
	// 		then you should create system that allow you fast change this callbacks
	//		and for all of that you need to focus and not to get confused...

	// TODO when our knowledge of Unity will be greater and deeper, maybe will find the better way

	private static void defaultOnStareCallback () {
		// this function allow VR use "click"
		if (Sight.instance.focusObject) {
			Sight.instance.focusObject.onclick ();
		}

		// WARN remove it on release
		//Debug.Log("What are you staring?");
	}		

	private static void defaultOnMoveCallback () {
		// just empty function
	}

	private static void defaultOnStopCallback () {
		// just empty function, maybe use only one of them?
	}

	private static void defaultOnClickCallback (FlatiumObject fo) {
		// when you use VR this function is epsent. Or not?
		// imagine a "virtual click", for instance when you stare at the point for some seconds a
		// "virtual click" happens and you call this function. And, as this function is just some kind
		// of reference you can change it on whatever you want, for example GUI appearing, material selection, 
		// apply or denay and e.t.c.

		// WARN remove it on release
		//Debug.Log("Click");
	}

	public static void setDefaultCallback () {
		onstare = defaultOnStareCallback;
		onmove = defaultOnMoveCallback;
		onstop = defaultOnStopCallback;
		onclick = defaultOnClickCallback;
	}

	// well all for this
	// a collection of callbacks
	// you can change them and in theory it will be awesome
	public static sightCallback onstare;
	public static sightCallback onmove;
	public static sightCallback onstop;
	public static clickCallback onclick;
	//===========================================EVENTS===========================================//

	// this var represent current project. Contain all FlatiumObjects, and you can get them through this refrence
    public static Project project;

	// this var is a reference to player
	public static GameObject player;

	// this var contain all selected objects
    public static ArrayList sellected;

    void Awake() {
        Flatium.instance = this; // This is... some kind of singleton?
    }

    void Start () {
		string DEFAULT_PROJECT_URL = "Projects/2.0";

        // WARN remove on release, just debug variables
        STANDARD_MATERIAL = Resources.Load("Materials/Standard") as Material;
        SELECTED_MATERIAL = Resources.Load("Materials/Selected") as Material;
        FOCUSED_MATERIAL = Resources.Load("Materials/Focused") as Material;

		// TODO and if the player won't be created for this time, huh!?
		// 		mb create access to player only throught Player.instance?
		player = GameObject.FindGameObjectWithTag ("Player");

		// TODO open the project menu (preferably)
		// open default project
		Project.open (DEFAULT_PROJECT_URL);

		setDefaultCallback (); // TODO remove it later

		Debug.Log ("Initialization over");
    }

	void Update () {
		
    }

}
