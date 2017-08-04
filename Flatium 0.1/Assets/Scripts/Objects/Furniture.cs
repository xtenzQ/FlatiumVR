/**
 * Created by Ivan
 * 26.07.17
 */
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
		
	public override void onfocus () {
		// TODO wireframe draw focus

		// HACK remove when wireframe drawing shader will be ready
		gameObject.GetComponent<Renderer>().material = (_focused) ? Flatium.FOCUSED_MATERIAL : material;
	}

	public override void onselect () {
		// TODO wireframe draw select
	}
		
	public override void onclick () {
		// TODO maybe call GUI of material changind, or moving GUI, or just select this
		Flatium.onclick(this);
	}
}
