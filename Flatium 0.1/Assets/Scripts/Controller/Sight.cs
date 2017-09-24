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
	public float movedRadius = 0.07f; // TODO realy sensetive, and head is always shake a little bit... later, maybe use integral value
	public float rotationSensety = 5.0f;

	// this var true if mouse movement more than MOVED_RADIUS
	public bool moved { get; private set; }

	// this var store mouse/sight movement. Just for better performance, even if you every time can use "Input.GetAxis("Mouse X/Y")"
	public Vector3 movement { get; private set; }

	public RaycastResult rayResult;
	public RaycastHit rayHit;

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
			}
		}
		get {
			return _focusObject;
		}
	}
		
	public delegate void Callback ();

	public static Callback click = () => {
		var pointer = new PointerEventData(EventSystem.current);
		pointer.position = new Vector2(Screen.width / 2.0f, Screen.height / 2.0f);
		ExecuteEvents.Execute(Sight.instance._focusObject, pointer, ExecuteEvents.pointerClickHandler);
	};

	private Callback callback;
	public float stareTime = 0.5f;
	public float loadCircleDelay = -0.2f;
	public float loadCircleDecrease = -0.035f;
	public float loadCircleMovementSensety = 0.2f;

	private float loadCircleState = -1.0f;
	private bool _loadCircleCourutine = false;
	private bool _loadCircleDirection = false;

	// Эта функция включает счетчик загрузки. Как только загрузка будет выполнена, будет вызвана функция func
	public void OnStare (Callback func = null) {
		callback = func;
		_loadCircleDirection = true;
		if (!_loadCircleCourutine) {
			StartCoroutine("loadCircleCourutine");
		}
	}

	// Эту функция позволяет вызвать OnStare из EventTrigger-а в Unity редакторе, что есть удобнее
	public void OnStareClick() {
		OnStareReset ();
		OnStare (click);
	}

	// Эта функция сбрасывает счетчик круга загрузки
	public void OnStareReset() {		
		loadCircle.fillAmount = 0.0f;
		loadCircleState = loadCircleDelay;
	}

	// Эта функция меняет направление счетчика загрузки, и он рано или поздно сбрасывается в 0
	public void OnStareStop() {
		_loadCircleDirection = false;
	}

	/*public bool OnStarePush () {
		float decrease = (movement.z > 0.5f) ? loadCircleMovementSensety * movement.z * movement.z : 0.0f;
		float increase = Time.deltaTime / stareTime - decrease;
		loadCircleState += (increase < loadCircleDecrease) ? loadCircleDecrease : increase;
		if (loadCircleState < loadCircleDelay) {
			loadCircleState = loadCircleDelay;
		}
		if (loadCircleState >= loadCircleDelay && loadCircleState < 1.0f) {
			loadCircle.fillAmount = loadCircleState;
		}
		return IsStare ();
	}

	public bool IsStare () {
		return loadCircleState == 1.0f;
	}*/

	private IEnumerator loadCircleCourutine () {
		_loadCircleCourutine = true;
		float decrease = 0.0f;
		float increase = 0.0f;
		do {
			decrease = (movement.z > 0.5f) ? loadCircleMovementSensety * movement.z * movement.z : 0.0f;
			increase = Time.deltaTime / stareTime - decrease;
			loadCircleState += (_loadCircleDirection) ? (increase < loadCircleDecrease) ? loadCircleDecrease : increase : loadCircleDecrease;
			if (loadCircleState < loadCircleDelay) {
				loadCircleState = loadCircleDelay;
			}
			if (loadCircleState >= loadCircleDelay && loadCircleState < 1.0f) {
				loadCircle.fillAmount = loadCircleState;
				yield return new WaitForSeconds (0.01f);
			}
		} while ((_loadCircleDirection && loadCircleState < 1.0f) || (!_loadCircleDirection && loadCircleState > 0.0f));
		if (loadCircleState >= 1.0f && callback != null) {
			callback ();
		}
		loadCircle.fillAmount = loadCircleState = 0.0f;
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
	 * Именно этот метод позволяет определить на что смотрит центр экорана
	 * Если его не вызывать ничего не выйдет. Увы совсем на родные raycaster-ы Unity перейти не получилось из-за проблемы с мышью
	 * TODO если выяснится что в VR мыши нет (по сути это так), то можно будет немножко подчистить код
	 * Результаты можно увидеть в переменных focusObject, rayHit и rayResult
	 */
	public void updateFocus () {		
		// До тех пор пока в flatium нет движущихся объектов это условие будет маленькой оптимизацией
		if (moved) { 
			var pointer = new PointerEventData(EventSystem.current);
			//pointer.position = Input.mousePosition;
			pointer.position = new Vector2(Screen.width / 2, Screen.height / 2);
			List<RaycastResult> results = new List<RaycastResult>();
			EventSystem.current.RaycastAll(pointer, results);
			if (results.Count > 0) {				
				focusObject = results [0].gameObject;
				rayResult = results [0];
			} else {
				Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2.0f, Screen.height / 2.0f, 0.0f));
				//Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
				rayHit = new RaycastHit ();
				focusObject = (
				    Physics.Raycast (ray, out rayHit, Mathf.Infinity, 
					    (1 << LayerMask.NameToLayer ("Floor")) |
					    (1 << LayerMask.NameToLayer ("Walls and Furniture")))
				) ? rayHit.collider.gameObject : null;
			}
		}
	}

}
