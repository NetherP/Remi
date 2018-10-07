using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeselectControll : MonoBehaviour {

    private InputManager manager;

	// Use this for initialization
	void Start () {
        manager = Managers.InputManager;
	}

    private void OnMouseDown()
    {
        manager.RemoveAll();
    }
}
