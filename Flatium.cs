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
 *  TODO WTF, this is singleton or static class? (!)
 *  TODO Sight class // in progress Ivan
 */
public class Flatium : MonoBehaviour {

    // TODO WARN this is debug vars. Remove on release
    //      should be replaced by wireframe shader drawing
    public static Material STANDARD_MATERIAL;
    public static Material SELECTED_MATERIAL; 
    public static Material FOCUSED_MATERIAL;

	// such var used in singleton patterns... think about it
    public static Flatium instance;

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
		if (Flatium.focus) {
			Flatium.focus.onclick ();
		}

		// WARN remove it on release
		Debug.Log("What are you staring?");
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
		Debug.Log("Click");
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

	// this var represent sight functionality. It's some kind of mouse
	public static Sight sight;
	public static RaycastHit sight_focus; // TODO remove to Sight // TODO rename to "focus"

	// this var is a reference to player
	public static GameObject player;

	// this var contain Object, that lies under the mouse (usualy it's underlined in some way)
	private static FlatiumObject _focus; // TODO remove to Sight? // TODO rename to "focusObject"
	// get-set for _focus
	public static FlatiumObject focus { // maybe I'm too much abuse "get-set" functionality?
		set {
			if(Flatium._focus != null) {
				Flatium._focus.focused = false;

				//WARN delete on release, or when wireframe selection will be ready
				Flatium._focus.gameObject.GetComponent<Renderer>().material = STANDARD_MATERIAL; 
			}
			Flatium._focus = value;
			if (Flatium._focus != null) {
				Flatium._focus.focused = true;

				//WARN delete on release, or when wireframe selection will be ready
				Flatium._focus.gameObject.GetComponent<Renderer>().material = SELECTED_MATERIAL;
			}
		}
		get {
			return Flatium._focus;
		}
	}

	// this var contain all selected objects
    public static ArrayList sellected;

    void Awake() {
        Flatium.instance = this; // This is... some kind of singleton?
    }

    void Start () {
        // WARN remove on release, just debug variables
        STANDARD_MATERIAL = Resources.Load("Materials/Standard") as Material;
        SELECTED_MATERIAL = Resources.Load("Materials/Selected") as Material;
        FOCUSED_MATERIAL = Resources.Load("Materials/Focused") as Material;

		// TODO and if the player won't be created for this time, huh!?
		// 		mb create access to player only throught PlayerScript.instance?
		player = GameObject.FindGameObjectWithTag ("Player");

		// TODO open the project menu (preferably)
		// open default project
		// some kind of "new Project()"... this looks wierd... why I don't use "new Project()"... because gladiolus
		Flatium.project = Flatium.instance.gameObject.AddComponent<Project>();
        Flatium.project.open("1.0");

		Flatium.sight = Flatium.instance.gameObject.AddComponent<Sight>();

		setDefaultCallback ();

		Debug.Log ("Initialization over");
    }

	void Update () {
        updateMouse();
    }

	// TODO remove to Sight?
    void updateMouse () { 
		// TODO WARN such structure required order Sight.Update() -> Flatium.Update()
		// 		removing to Sight will solve this problem
		//		but... how in Unity determine the order of Updates?
		if (sight.moved) {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

			// TODO maybe shuold create some variable like TYPE or ID, or use built-in vars loke TAG... or something else
			// 		should raycast be separate for walls and floors? Is this faster way? At least this thing determine what exactly was collided Wall or Floor
			Physics.Raycast(ray, out sight_focus, Mathf.Infinity, LayerMask.GetMask("Walls and Furniture"));
			if (sight_focus.collider != null) {
				// walls and furniture proccesing

				// TODO potential error place. Why? I get FlatiumObject component... I'm shoked just because it's work and I have a big doubts about it
                // 		imagine if there was two FlatiumObject binded to gameObject. Test it later
				FlatiumObject fo = sight_focus.collider.gameObject.GetComponent<FlatiumObject>();

				Flatium.focus = fo;

            } else {
				Physics.Raycast(ray, out sight_focus, Mathf.Infinity, LayerMask.GetMask("Floor"));
				if (sight_focus.collider != null) {
					// floors proccesing
					FlatiumObject fo = sight_focus.collider.gameObject.GetComponent<FlatiumObject>();

					Flatium.focus = fo;


				} else {
					Flatium.focus = null;
				}
            }            
        }
    }
}
