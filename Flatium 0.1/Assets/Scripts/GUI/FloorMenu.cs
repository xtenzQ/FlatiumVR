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
		syncWithPlayer ();
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
	            Player.instance.transform.position = Sight.focus.point + Player.height;
	            syncWithPlayer();
            }
	    }
	}

    /// <summary>
    /// 
    /// </summary>
    public void syncWithPlayer()
    {
        var playerRotationY = Quaternion.AngleAxis(Player.instance.transform.eulerAngles.y, Vector3.up);
        var playerPosition = Player.instance.transform.position +
                   playerRotationY *
                   Vector3.forward;
        gameObject.transform.rotation = playerRotationY * Quaternion.AngleAxis(90, Vector3.right);
        gameObject.transform.position = new Vector3(playerPosition.x, 0.01f, playerPosition.z);
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