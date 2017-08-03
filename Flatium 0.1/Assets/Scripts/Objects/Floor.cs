/**
 * Created by Ivan
 * 26.07.17
 */
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Floorthis class of floor... surprise
 * TODO maybe should create Level class that represent Floor and Seiling, because they so familiar,
 *      just some small difference in furniture placing (normal direction)
 */
[RequireComponent(typeof(BoxCollider), typeof(MeshFilter))]
public class Floor : FlatiumObject {

	//TODO default material. WARN if all projects will be pre-created - so there will be no need in default material, right?
    public static Material DEFAULT_FLOOR_MATERIAL;

	// array of some furniture (as bookshelfs or sofa or chair with table)
	// TODO mb should use ArrayList
    private Furniture[] objects;

	// method that add furniture
    public void addFurniture (Furniture obj) {

    }

	// method that delete furniture
    public void delFurniture (Furniture obj) {

    }

    void Start () {
        gameObject.tag = "Floor";
        gameObject.layer = LayerMask.NameToLayer("Floor");

        //gameObject.GetComponent<Renderer>().material = Floor.DEFAULT_FLOOR_MATERIAL;
    }

    void Update () {
		
	}

    public override void onfocus () {
		// TODO wireframe draw focus
    }

    public override void onselect () {
		// TODO wireframe draw select
    }

	public override void onclick () {
		// TODO maybe move person to click position, or call material GUI, or place here Furniture	
		Flatium.onclick(this);
	}

	public void OnMouseDown () {
		onclick ();

		// WARN remove on release
		if (Flatium.sight.stare) {
			Flatium.player.transform.position = Flatium.sight_focus.point + new Vector3 (0, 3, 0);
		}
	}
}
