/**
 * Created by Ivan
 * 26.07.17
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * this class represent Flatium content. Ideally, instances of this classes must be contained in some DB
 * TODO I don't realy sure that Project must extend MonoBehaviour
 * TODO DB
 */
public class Project : MonoBehaviour {

    public Wall[] walls;            // All walls in the scene
    public Floor[] floors;          // All floors... where? Oh, in the scene of cource
    public Furniture[] furniture;   // All... I don't know... maybe furniture in the scene?

	//TODO adequate clearing... I think it's not
	public void clear () {
        walls = new Wall[0];
        floors = new Floor[0];
        furniture = new Furniture[0];
    }

    public void save(string url) {
        //TODO project saving into... somewhere. Is this function even required?
    }

	// TODO I tried to connect this with constructor but failed. In spirit of "Project p = new Project("MyResource/MyProject");". Connect with "Start"?
	// TODO this method require further developing
	// TODO every project must have "walls_root" and "floors_root" (used right now), or maybe just use checking for null?
	// TODO there can be no furniture in the room (used right now), or maybe every project must have "furniture_root" even empty...
	public void open(string url) {		
        clear();
        
		// getting the root of the project
		Transform project = (Resources.Load(url) as GameObject).GetComponent<Transform>();

		// geting "walls_root" and "floors_root" and others and checking for their existence
        Transform wallParent = project.Find("Walls");

		// TODO improve this... "error catch system"...
        if (wallParent == null) {
            Debug.LogError("Project has no Walls hierarchy object!");
            return;
        }
        Transform floorParent = project.Find("Floors");
        if (floorParent == null)
        {
            Debug.LogError("Project has no Floors hierarchy object!");
            return;
        }
        // hate duplication - I'we already get love with JS

		// TODO is there some way to add objects to the world (to the hierarchy) without copying (Instantiate method) ?!... (!!!)
		// 		seriously, why I should copy this fucking data? This is insane!
        wallParent = UnityEngine.Object.Instantiate(wallParent);
        for (int i = 0; i < wallParent.childCount; i++) {
            wallParent.GetChild(i).gameObject.AddComponent<Wall>();
        }
        floorParent = UnityEngine.Object.Instantiate(floorParent);
        for (int i = 0; i < floorParent.childCount; i++) {
            floorParent.GetChild(i).gameObject.AddComponent<Floor>();
        }
        walls = wallParent.GetComponentsInChildren<Wall>();
        floors = floorParent.GetComponentsInChildren<Floor>();
        // agrh... fucking duplicate

        Transform furnitureParent = project.Find("Furniture");
        if (furnitureParent != null) {
			// TODO this looks badly... back to the question about Instantiate excluding
            furnitureParent = UnityEngine.Object.Instantiate(furnitureParent);
            for (int i = 0; i < furnitureParent.childCount; i++) {
                furnitureParent.GetChild(i).gameObject.AddComponent<Furniture>();
            }
            furniture = furnitureParent.GetComponentsInChildren<Furniture>();
        }

		Debug.Log ("Project loaded: " + url);
    }

}

