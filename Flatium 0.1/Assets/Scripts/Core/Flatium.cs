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
    
	public static Flatium instance;

	// TODO WARN this is debug vars. Remove on release
    //      should be replaced by wireframe shader drawing
    public static Material STANDARD_MATERIAL;
    public static Material SELECTED_MATERIAL; 
    public static Material FOCUSED_MATERIAL;

	// this var represent current project. Contain all FlatiumObjects, and you can get them through this refrence
    public static Project project;

	// this var is a reference to player
	public static GameObject player;

	// this var contain all selected objects
    public static ArrayList sellected;

	// Flatium state machine
	public delegate void FlatiumEvent ();

	public FlatiumEvent onEnterWall { get; private set; }
	public FlatiumEvent onExitWall { get; private set; }
	public FlatiumEvent onClickWall { get; private set; }

	public FlatiumEvent onEnterFloor { get; private set; }
	public FlatiumEvent onExitFloor { get; private set; }
	public FlatiumEvent onClickFloor { get; private set; }

	public FlatiumEvent onEnterFurniture { get; private set; }
	public FlatiumEvent onExitFurniture { get; private set; }
	public FlatiumEvent onClickFurniture { get; private set; }

	private FlatiumEvent[] FlatiumState;

	public void setState (int i) {
		FlatiumState [i] ();
	}

    void Awake() {
        Flatium.instance = this; // This is... some kind of singleton?

		FlatiumState = new FlatiumEvent[2];
		// Дефолтное состояние. Вызова Fast Triangl и т.п.
		FlatiumState [0] = () => {
			FlatiumEvent empty = () => {};
			onEnterWall = onExitWall = onClickWall = empty;
			onEnterFloor = onExitFloor = onClickFloor = empty;
			onEnterFurniture = onExitFurniture = onClickFurniture = empty;
		};
		// Состояние после нажатия на кнопку "Передвижение"
		FlatiumState [1] = () => {
			FlatiumEvent empty = () => {};
			onEnterWall = onExitWall = onClickWall = empty;
			onEnterFurniture = onExitFurniture = onClickFurniture = empty;
			onEnterFloor = () => {
				Sight.instance.OnStareClick();
			};
			onExitFloor = () => {
				Sight.instance.OnStareStop();
			};
			onClickFloor = () => {
				Player.instance.moveTo (Sight.instance.rayHit.point);
				FloorMenu.instance.syncWithPlayer ();
				FloorMenu.instance.switchMovement ();
			};
		};

		setState (0);
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

		Debug.Log ("Initialization over");
    }

	void Update () {
		
    }

}
