using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitController : MonoBehaviour {

    [SerializeField] private GameObject exitWindow;

    public void ExitGame()
    {
        Application.Quit();
    }

	// Update is called once per frame
	void Update () {
		if (Application.platform == RuntimePlatform.Android)
        {
            if (Input.GetKey(KeyCode.Escape))
            {
                exitWindow.SetActive(true);
            }
        }
	}
}
