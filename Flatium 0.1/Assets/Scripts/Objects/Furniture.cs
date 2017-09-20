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
 * Furniture is a simple mesh somewhere in the flat. It can be anything, begining from fridge and ending with slippers
 */
[RequireComponent(typeof(BoxCollider), typeof(MeshFilter))]
public class Furniture : FlatiumObject {

    void Start () {
        gameObject.tag = "Furniture";
        gameObject.layer = LayerMask.NameToLayer("Walls and Furniture");

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
