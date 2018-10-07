using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugDealer : MonoBehaviour {

    [SerializeField] private HandsRender toRender;
    [SerializeField] private Hands toSwap;
    public List<int> cards;

    public void getListReference()
    {
        cards = toSwap.cards;
    }

    public void Rerender()
    {
        toSwap.Push(Random.Range(0, 51));
        toRender.UnrenderCards();
        toRender.RenderCards();
    }
}
