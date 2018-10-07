using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputManager : MonoBehaviour, IGameManager {
    public ManagerStatus status { get; private set; }

    public List<int> selected;
    private List<CardController> cardControllers;
    private int lastSelected;
    private string promptText;
    public bool IsPromptEnabled;
    public bool IsTurnEnded;
    public List<int> EnabledLayer;

    public void Startup()
    {
        Debug.Log("Input manager starting...");
        selected = new List<int>();
        cardControllers = new List<CardController>();
        IsPromptEnabled = false;
        IsTurnEnded = true;
        EnabledLayer = new List<int>();
        status = ManagerStatus.Started;
    }

    public void AddCard(CardController cardCtrl)
    {
        int index = cardCtrl.GetComponent<CardModel>().cardIndex;
        if (!selected.Contains(index))
        {
            selected.Add(index);
            cardControllers.Add(cardCtrl);
            cardCtrl.Select();
        }
        else if (cardCtrl.gameObject.layer != 13)
        {
            RemoveCard(index);
        }
    }

    public void RemoveCard(int value)
    {
        int index = selected.IndexOf(value);
        selected.RemoveAt(index);
        cardControllers[index].Deselect();
        cardControllers.RemoveAt(index);
    }

    public void RemoveAll()
    {
        List<int> removed = new List<int>();
        foreach(CardController ctrl in cardControllers)
        {
            if (ctrl.gameObject.layer != 14)
            {
                ctrl.Deselect();
                removed.Add(ctrl.cardIndex);
            }
        }
        foreach (int i in removed)
        {
            RemoveCard(i);
        }
    }

    public int[] LockSelected()
    {
        List<int> layer = new List<int>();
        foreach(var card in cardControllers)
        {
            layer.Add(card.gameObject.layer);
            card.gameObject.layer = 14;
        }
        return layer.ToArray();
    }

    public void UnlockSelected(int[] layers)
    {
        for (int i = 0; i < layers.Length; i++)
        {
            if (i < cardControllers.Count)
            {
                cardControllers[i].gameObject.layer = layers[i];
            }
        }
    }

    private bool IsRayDisabled()
    {
        return IsPromptEnabled || IsTurnEnded;
    }

    private bool isLayerEnabled(int layer)
    {
        foreach (int i in EnabledLayer)
        {
            if (i == layer)
                return true;
        }
        return false;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && !IsRayDisabled())                                            //this is ray for card selection
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.GetRayIntersection(ray);
            if (hit.collider != null && hit.collider.gameObject.layer == 8)
                RemoveAll();
            else if (hit.collider != null && isLayerEnabled(hit.collider.gameObject.layer))
            {
                CardController card = hit.collider.GetComponent<CardController>();
                AddCard(card);
            }
            else if (hit.collider != null)
            {
                Debug.Log("this object raycast is disabled. layerNum: " + hit.transform.gameObject.layer);
            }
        }
    }

}
