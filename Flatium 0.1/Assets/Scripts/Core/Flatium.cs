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

		Debug.Log ("Initialization over");
    }

	void Update () {
		
    }

}
