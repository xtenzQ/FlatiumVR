/**
 * Created by Ivan
 * 26.07.17
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * FlatiumObject - base abstract class for all objects in Flatium. All object can be selected and 
 * selected, each one editable.
 * TODO Floor and Wall similar with their ability to add Furniture, maybe should create some 
 *      ContainerObject and put it like a fild (preferable), or create abstract ContainerObject that will 
 *      inherit FlatiumObject and will be expanded with them? // in progres Ivan
 */
public abstract class FlatiumObject : MonoBehaviour {

	// just bool vars that shows object _focus/_selected state
    private bool _selected = false;
    private bool _focused = false;

    // get-set for _focus/_selected vars
    public bool selected {
        get {
            return _selected;
        }
        set {
            this._selected = value;
            onselect();
        }
    }

    public bool focused {
        get {
            return _focused;
        }
        set {
            this._focused = value;
            onfocus();
        }
    }

	// simple event system
    public abstract void onselect();
    public abstract void onfocus();
	public abstract void onclick();

}
