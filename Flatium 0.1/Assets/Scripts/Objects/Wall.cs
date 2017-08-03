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
[RequireComponent(typeof(BoxCollider), typeof(MeshFilter))]
public class Wall : FlatiumObject {

	// first and second material
    private Material _fm;
    private Material _sm;

	// get-set for _fm and _sm
    public Material fmaterial {
        get {
            return _fm;
        }

        set {
			_fm = value;
            //TODO set on the first face
        }
    }

    public Material smaterial {
        get {
            return _sm;
        }

        set {
			_sm = value;
            //TODO set on the second face
        }
    }

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
		// TODO maybe call material GUI, or place here Furniture
		Flatium.onclick(this);
	}
}
