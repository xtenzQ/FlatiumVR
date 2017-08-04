/**
 * Created by Ivan
 * 26.07.17
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Wall simple object, it has two materials (each for each side)
 * TODO think about mesh organization - mesh with two different materials
 * TODO maybe should create OutsideWall and InsideWall classes for better performance
 * TODO or maybe create Wall - just single face. So inside wall - it's two faces, outside - just one. This is
 *      fast way, but what to do with edges?
 * TODO think about wall side edges, maybe they should use default concree material, use some mix
 *      of fmaterial and smaterial, or maybe they must be editable
 */
[RequireComponent(typeof(MeshCollider), typeof(MeshFilter))]
public class Wall : FlatiumObject {

	// array of some furniture (as picture or mirrow) or internal objects (as door and windows)
	// TODO mb should use ArrayList
	// TODO look for operators overloading
    private Furniture[] objects;

	// method that add furniture
    public void addFurniture (Furniture obj) {

    }

	// method that delete furniture
    public void delFurniture (Furniture obj) {

    }

    void Start () {
        gameObject.tag = "Wall";
        gameObject.layer = LayerMask.NameToLayer("Walls and Furniture");

		Transform wall = GetComponent<Transform> ();
		objects = new Furniture[wall.childCount];
		for (int i = 0; i < wall.childCount; i++) {
			objects[i] = wall.GetChild (i).gameObject.AddComponent<Furniture> ();
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
		// TODO maybe call material GUI, or place here Furniture
		Flatium.onclick(this);
	}
}
