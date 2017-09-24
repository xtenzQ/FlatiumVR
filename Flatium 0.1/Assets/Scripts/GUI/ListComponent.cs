using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ListComponent : MonoBehaviour {

	private static Color DEFAULT_COLOR = new Color(0.0f, 0.0f, 0.0f, 0.0f);
	private static Color FOCUS_COLOR = new Color(1.0f, 1.0f, 1.0f, 0.1f);
	private static Color SELECT_COLOR = new Color(1.0f, 1.0f, 0.0f, 0.1f);

	public static ListComponent selectedComponent = null;

	public string url;
	public string text {
		set {
			textComponent.text = value;
		}

		get {
			return textComponent.text;
		}
	}

	private Image imageComponent;
	private Text textComponent;
	private bool selected = false;

	void Awake () {
		imageComponent = gameObject.GetComponent<Image> ();
		textComponent = gameObject.GetComponent<Text> ();
	}

	public void setFocus(bool focus) {
		if (!selected) {
			if (focus) {
				imageComponent.color = FOCUS_COLOR;
				Sight.instance.OnStareReset ();
				Sight.instance.OnStare (Sight.click);
				EditorMenu.instance.setSelectedObject (this);
			} else {
				imageComponent.color = DEFAULT_COLOR;
				Sight.instance.OnStareStop ();
				EditorMenu.instance.setSelectedObject (ListComponent.selectedComponent);
			}			 
		}			
	}		

	public void selectComponent () {		
		if (!selected) {
			if (ListComponent.selectedComponent) {
				ListComponent.selectedComponent.imageComponent.color = DEFAULT_COLOR;
				ListComponent.selectedComponent.selected = false;
			}
			ListComponent.selectedComponent = this;
			selected = true;
			imageComponent.color = SELECT_COLOR;

			EditorMenu.instance.setSelectedObject (this);	
		}
	}

}
