using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorMenu : MonoBehaviour
{
    // Флажок для режима передвижения
    private bool movementMode;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
	{
        // Осторожно, говнокод!
        // TODO: создать делегаты
	    if (movementMode)
	    {
            //Debug.Log(Sight.focusObject.tag);
	        if (Sight.stare && Sight.focusObject.tag == "Floor")
	        {
	            movementMode = false;
	            PlayerScript.instance.transform.position = Sight.focus.point + PlayerScript.height;
	        }
	    }
	}

    public void nya(float x)
    {
	    gameObject.transform.position += Vector3.right * x;
    }

    /// <summary>
    /// Переключаем режим на противоположный
    /// </summary>
    public void onClickMoveButton()
    {
        movementMode = !movementMode;
    }
}