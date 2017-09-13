/**
 * Created by Ivan
 * 26.07.17
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/**
 * FlatiumObject - base abstract class for all objects in Flatium. All object can be selected and 
 * selected, each one editable.
 * TODO Floor and Wall similar with their ability to add Furniture, maybe should create some 
 *      ContainerObject and put it like a fild (preferable), or create abstract ContainerObject that will 
 *      inherit FlatiumObject and will be expanded with them? // in progres Ivan
 */
[RequireComponent(typeof(EventTrigger))]
public abstract class FlatiumObject : MonoBehaviour {

	protected EventTrigger trigger;

	// HACK remove when wireframe drawing shader will be ready
	protected Material material;

	// just bool vars that shows object _focus/_selected state
	protected bool _selected = false;
    protected bool _focused = false;

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

	void Awake () {
		trigger = gameObject.GetComponent<EventTrigger> ();

		EventTrigger.Entry entry = new EventTrigger.Entry ();
		entry.eventID = EventTriggerType.PointerDown;
		entry.callback.AddListener ((data) => Yeaaaa());

		trigger.triggers.Add (entry);
	}

	public void Yeaaaa () {
		Debug.Log ("Triggggeeeeer!");
	}

	// simple event system
    public abstract void onselect();
    public abstract void onfocus();
	public abstract void onclick();

}
