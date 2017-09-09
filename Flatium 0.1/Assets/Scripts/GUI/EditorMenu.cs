using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EditorMenu : MonoBehaviour {

	private static GameObject SelectedObject = null;
	private static Vector3 SELECTED_OBJECT_POSITION = new Vector3 (-20.0f, -11.0f, 0.0f);

	public float rotationSpeed = 0.5f;

	public ScrollRect sr;

	public static void setSelectedObject (ListComponent c) {
		if (!c) return;
		GameObject go = Resources.Load (c.url) as GameObject;
		if (go != null) {
			EditorMenu.SelectedObject = UnityEngine.Object.Instantiate(go);
			EditorMenu.SelectedObject.transform.position = SELECTED_OBJECT_POSITION;	
		}
	}		
	
	void Update () {		
		if (EditorMenu.SelectedObject != null) {
			EditorMenu.SelectedObject.transform.rotation *= Quaternion.AngleAxis(rotationSpeed, new Vector3(0.0f, 1.0f, 0.0f));
		}
	}
}
