using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PromptController : MonoBehaviour {

    [SerializeField] private Image prompt;
    [SerializeField] private Text prompText;

    private void OnEnable()
    {
        Messenger<string>.AddListener(GameEvent.SHOW_PROMPT, ShowPrompt);
    }

    private void OnDisable()
    {
        Messenger<string>.RemoveListener(GameEvent.SHOW_PROMPT, ShowPrompt);

    }

    public void ClosePrompt()
    {
        prompt.gameObject.SetActive(false);
        Managers.InputManager.IsPromptEnabled = false;
    }

    public void ShowPrompt(string text)
    {
        Managers.InputManager.IsPromptEnabled = true;
        prompText.text = text;
        prompt.gameObject.SetActive(true);
    }
}
