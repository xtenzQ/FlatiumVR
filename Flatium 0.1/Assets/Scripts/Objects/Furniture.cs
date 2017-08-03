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

	//TODO default material. WARN if all projects will be pre-created - so there will be no need in default material, right?
	public static Material DEFAULT_FURNITURE_MATERIAL; 

    void Start () {
        gameObject.tag = "Furniture";
        gameObject.layer = LayerMask.NameToLayer("Walls and Furniture");

        //gameObject.GetComponent<Renderer>().material = Furniture.DEFAULT_FURNITURE_MATERIAL;
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
		// TODO maybe call GUI of material changind, or moving GUI, or just select this
		Flatium.onclick(this);
	}
}
