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

	public override void OnPointerEnter (PointerEventData eventData) {
		focused = true;
		// TODO wireframe draw focus

		// HACK remove when wireframe drawing shader will be ready
		gameObject.GetComponent<Renderer>().material = Flatium.FOCUSED_MATERIAL;
    }

	public override void OnPointerExit (PointerEventData eventData) {
		focused = false;
		// TODO wireframe draw focus

		// HACK remove when wireframe drawing shader will be ready
		gameObject.GetComponent<Renderer>().material = material;
	}
}
