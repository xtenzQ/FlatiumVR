/**
 * Created by Ivan
 * 31.07.17
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/**
 * Sight - its a class that represent all sight functionnality, for instance waiting of 0.5 second and execute "onsight" method
 */
public class Sight : MonoBehaviour {

	public static Sight instance;

	public Image sightCircle;
	public Image loadCircle;

	// this must be const... and static... static const
	// well... while this class is static it can be just "private static"
	public float stareTime = 1.0f; // 1000 ms, fucking Unity used float
	public float movedRadius = 0.07f; // TODO realy sensetive, and head is always shake a little bit... later, maybe use integral value
	public float rotationSensety = 5.0f;

	// this var true if mouse movement more than MOVED_RADIUS
	public bool moved { get; private set; }

	// this var store mouse/sight movement. Just for better performance, even if you every time can use "Input.GetAxis("Mouse X/Y")"
	public Vector3 movement { get; private set; }

	public RaycastResult focus;

	// this var contain Object, that lies under the mouse (usualy it's underlined in some way)
	private GameObject _focusObject;
	public GameObject focusObject {
		set {			
			if (_focusObject != value) {				
				var pointer = new PointerEventData(EventSystem.current);
				pointer.position = new Vector2(Screen.width / 2.0f, Screen.height / 2.0f);
				if (_focusObject != null) {
					ExecuteEvents.Execute(_focusObject, pointer, ExecuteEvents.pointerExitHandler);	
				}					
				_focusObject = value;
				if (_focusObject != null) {
					ExecuteEvents.Execute(_focusObject, pointer, ExecuteEvents.pointerEnterHandler);	
				}
				if (!_sightCircleCourutine) {
					StartCoroutine("sightCircleAnimate");
				}
				clickOnStare ();
			}
		}
		get {
			return _focusObject;
		}
	}

	public void clickOnStare () {
		loadCircleState = 0.0f;
		if (!_loadCircleCourutine) {
			StartCoroutine("loadCircleCourutine");
		}
	}

	private float loadCircleState = 0.0f;
	private bool _loadCircleCourutine = false;
	private IEnumerator loadCircleCourutine () {
		_loadCircleCourutine = true;
		while (_loadCircleCourutine && loadCircleState < 1.0f) {
			loadCircleState += (_focusObject != null) ? Time.deltaTime / (stareTime * (10.0f * (movement.z > 0.5f ? movement.z : 0.0f) + 1.0f)) : -0.15f;
			if (loadCircleState < 1.0f) {
				loadCircle.fillAmount = loadCircleState;
				yield return new WaitForSeconds (0.01f);
			}
		}
		if (loadCircleState >= 1.0f) {
			// TODO onclick
			loadCircleState = 0.0f;
			loadCircle.fillAmount = 0.0f;

			/*var pointer = new PointerEventData(EventSystem.current); // pointer event for Execute
			//pointer.position = Input.mousePosition;
			pointer.position = new Vector2(Screen.width / 2, Screen.height / 2);
			List<RaycastResult> results = new List<RaycastResult>();
			EventSystem.current.RaycastAll(pointer, results);
			if (results.Count > 0) {
				ExecuteEvents.Execute(results[0].gameObject, pointer, ExecuteEvents.pointerClickHandler);
			}*/
		}
		_loadCircleCourutine = false;
	}

	private float sightCircleState = 0.0f;
	private bool _sightCircleCourutine = false;
	private IEnumerator sightCircleAnimate () {		
		_sightCircleCourutine = true;
		Button bt = sightCircle.GetComponent<Button> ();
		do {
			sightCircleState += (_focusObject != null) ? 0.1f : -0.1f;
			bt.interactable = sightCircleState > 0.2f;
			if (sightCircleState > 1.0f) {
				sightCircleState = 1.0f;
				sightCircle.transform.localScale = new Vector3 (5.0f, 5.0f, 1.0f);
			} else if (sightCircleState < 0.0f) {
				sightCircleState = 0.0f;
				sightCircle.transform.localScale = new Vector3 (1.0f, 1.0f, 1.0f);
			} else {
				sightCircle.transform.localScale = new Vector3 (1.0f + sightCircleState * 4.0f, 1.0f + sightCircleState * 4.0f, 1.0f);
				yield return new WaitForSeconds (0.01f);
			}
		} while (_sightCircleCourutine && sightCircleState > 0.0f && sightCircleState < 1.0f);
		_sightCircleCourutine = false;
	}
		
	void Awake () {
		Sight.instance = this;
	}

	void Update () {
		updateSight ();
		updateFocus ();
	}

	/**
	 * This function update such variables as "movement", "moved" and "stare"
	 */
	public void updateSight () {
		Vector2 mouse = new Vector2 (Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
		// Velocity for mouse
		/*movement = new Vector3(mouse.x, mouse.y, Mathf.Sqrt (mouse.x * mouse.x + mouse.y * mouse.y));
		moved = movement.z > movedRadius;*/

		// Velocity for centre of the screeen
		if (Input.GetMouseButton(0) || Input.GetKey(KeyCode.LeftAlt)) {
			movement = new Vector3(mouse.x, mouse.y, Mathf.Sqrt (mouse.x * mouse.x + mouse.y * mouse.y));
			moved = movement.z > movedRadius;
			Vector3 mouseMovement = new Vector3(-mouse.y, mouse.x, 0) * rotationSensety + this.transform.eulerAngles;
			// Говнокод! Исправить!
			if (mouseMovement.x > 180) {
				if (mouseMovement.x < 270) {
					mouseMovement.x = 270;
				}
			} else {
				if (mouseMovement.x > 90) {
					mouseMovement.x = 90;
				}
			}
			this.transform.eulerAngles = mouseMovement;
		}
	}

	/**
	 * This function update such variables as "focusObject" and "focus", should be used after "updateSight"
	 */
	public void updateFocus () {		
		if (moved) {
			Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2.0f, Screen.height / 2.0f, 0.0f));
			//Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit = new RaycastHit ();
			focusObject = (
				Physics.Raycast (ray, out hit, Mathf.Infinity, 
					(1 << LayerMask.NameToLayer("Floor")) |
					(1 << LayerMask.NameToLayer("Walls and Furniture")) )
			) ? hit.collider.gameObject : null;

			/*PointerEventData pointerData = new PointerEventData(EventSystem.current);
			//pointerData.position = Input.mousePosition;
			pointerData.position = new Vector2(Screen.width / 2, Screen.height / 2);
			List<RaycastResult> results = new List<RaycastResult>();
			EventSystem.current.RaycastAll(pointerData, results);
			if (results.Count > 0) {
				Debug.Log (results[0]);
				focusObject = results [0].gameObject;
				focus = results [0];
			} else {
				focusObject = null;
			}*/
		}
	}

}
