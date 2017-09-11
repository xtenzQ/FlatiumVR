using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EditorMenu : MonoBehaviour {

	public static EditorMenu instance;

	public float rotationSpeed = 0.5f;
	public GameObject RTTObjectRoot;

	public void setSelectedObject (ListComponent componentFromList) {
		if (RTTObjectRoot != null) {
			GameObject go;
			// Удаление старого RTT объекта
			if (RTTObjectRoot.transform.childCount > 0) {
				go = RTTObjectRoot.transform.GetChild (0).gameObject;
				RTTObjectRoot.transform.DetachChildren ();
				GameObject.Destroy(go);
				// Странно, но на прямую без DetachChildren не работает. Что поделать, пусть будет так
			}
			// Добавление нового RTT объекта
			if (componentFromList != null) {
				go = Resources.Load ("Models/" + componentFromList.url) as GameObject;
				if (go) {
					GameObject cgo = UnityEngine.Object.Instantiate (go);
					cgo.name = "RTTObject";
					cgo.transform.SetParent (RTTObjectRoot.transform, false);
					cgo.layer = RTTObjectRoot.layer;
					// Если объект один, то одной смены layer достаточно. Но сейчас модели состоят из большого числа деталей,
					// для каждой из которых устанавливается layer. В идеале нужно сделать рекурсивно, но это ведь дебаго-код. 
					// REMOVE ON RELEASE
					for (int i = 0; i < cgo.transform.childCount; i++) {
						cgo.transform.GetChild (i).gameObject.layer = RTTObjectRoot.layer;
					}
				}				
			}
		}
	}

	void Start () {
		EditorMenu.instance = this;
	}
	
	void Update () {
		RTTObjectRoot.transform.rotation *= Quaternion.AngleAxis(rotationSpeed, new Vector3(0.0f, 1.0f, 0.0f));
	}
}
