using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EditorMenu : MonoBehaviour {

	public static EditorMenu instance;

	// Канва, затемняющая фон при вызове меню
	public Image fadeScreen;
	// Цвет максимального затемнения
	private Color FADE_COLOR = new Color(0.0f, 0.0f, 0.0f, 0.5f);

	public float rotationSpeed = 0.5f;
	public GameObject RTTObjectRoot;

	public void switchState () {
		gameObject.SetActive (!gameObject.activeSelf);
		if (gameObject.activeSelf) {
			fadeScreen.color = FADE_COLOR;
			// TODO копия кода из FloorMenu! После появления класса общей логики перенести его туда!
			var playerRotationY = Quaternion.AngleAxis (Player.instance.transform.eulerAngles.y, Vector3.up);
			var playerPosition = Player.instance.transform.position +
			                     playerRotationY *
			                     Vector3.forward * 3.0f;
			gameObject.transform.rotation = playerRotationY;
			gameObject.transform.position = new Vector3 (playerPosition.x, 1.8f, playerPosition.z);			
		} else {
			fadeScreen.color = new Color(0.0f, 0.0f, 0.0f, 0.0f);
		}
	}		

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
				Material material = Resources.Load ("Models/Materials/" + componentFromList.url) as Material;
				if (material) {
					material = UnityEngine.Object.Instantiate (material);
				}
				go = Resources.Load ("Models/" + componentFromList.url) as GameObject;
				if (go) {
					GameObject cgo = UnityEngine.Object.Instantiate (go);
					cgo.name = "RTTObject";
					cgo.transform.SetParent (RTTObjectRoot.transform, false);
					cgo.layer = RTTObjectRoot.layer;
					if (material) {
						cgo.GetComponent<Renderer>().material = material;	
					}
					// Если объект один, то одной смены layer достаточно. Но сейчас модели состоят из большого числа деталей,
					// для каждой из которых устанавливается layer. В идеале нужно сделать рекурсивно, но это ведь дебаго-код. 
					// REMOVE ON RELEASE
					for (int i = 0; i < cgo.transform.childCount; i++) {
						cgo.transform.GetChild (i).gameObject.layer = RTTObjectRoot.layer;
						if (material) {
							cgo.transform.GetChild (i).gameObject.GetComponent<Renderer>().material = material;
						}
					}
				}				
			}
		}
	}

	void Start () {
		EditorMenu.instance = this;
		gameObject.SetActive (false);
	}
	
	void Update () {
		RTTObjectRoot.transform.rotation *= Quaternion.AngleAxis(rotationSpeed, new Vector3(0.0f, 1.0f, 0.0f));
	}
}
