using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonController : MonoBehaviour {

    [SerializeField] private int lessThanAmount;
    [SerializeField] private int moreThanAmount;

    private Button thisButton;

    private void Start()
    {
        thisButton = GetComponent<Button>();
    }

    // Update is called once per frame
    void Update () {
		if (Managers.InputManager.selected.Count > moreThanAmount && Managers.InputManager.selected.Count < lessThanAmount)
        {
            thisButton.interactable = true;
        }
        else
        {
            thisButton.interactable = false;
        }
	}
}
