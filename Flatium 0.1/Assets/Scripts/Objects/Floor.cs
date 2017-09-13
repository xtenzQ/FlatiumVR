/**
 * Created by Ivan
 * 26.07.17
 */
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/**
 * Floorthis class of floor... surprise
 * TODO maybe should create Level class that represent Floor and Seiling, because they so familiar,
 *      just some small difference in furniture placing (normal direction)
 */
[RequireComponent(typeof(BoxCollider), typeof(MeshFilter))]
public class Floor : FlatiumObject {

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

		Transform floor = GetComponent<Transform> ();
		objects = new Furniture[floor.childCount];
		for (int i = 0; i < floor.childCount; i++) {
			objects[i] = floor.GetChild (i).gameObject.AddComponent<Furniture> ();
		}

		// HACK remove when wireframe drawing shader will be ready
		material = gameObject.GetComponent<Renderer>().material;
    }

    void Update () {
		
	}

    public override void onfocus () {
		// TODO wireframe draw focus

		// HACK remove when wireframe drawing shader will be ready
		gameObject.GetComponent<Renderer>().material = (_focused) ? Flatium.FOCUSED_MATERIAL : material;
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
		if (Sight.instance.stare) {
			Flatium.player.transform.position = Sight.instance.focus.point + new Vector3 (0, 1.5f, 0);
		}
	}
}
