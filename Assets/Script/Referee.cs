using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Referee : MonoBehaviour {

	public bool CheckCards(List<int> cards)
    {
        
        if(cards.Count < 3)
        {
            Debug.Log("less than 3 cards");
            return false;
        }
        else if(IsTwoJoker(cards))
        {
            Debug.Log("two joker");
            return false;
        }
        else
        {
            bool isPairs = false;
            if (OfAKind(cards, Managers.TurnManager.CurrentPlayer.HaveCapital()) || Straight(cards))
                isPairs = true;
            return isPairs;
        }
    }

    public bool PullCheck(List<int> cards)
    //used for checking joker, change this to enable joker pulling
    {
        if (CheckCards(cards))
        {
            foreach (int i in cards)
            {
                if (IsJoker(i))
                    return false;
            }
            return true;
        }
        else
            return false;
    }

    public bool IsJoker(int card)
    {
        if (card == 52 || card == 53)
            return true;
        else
            return false;
    }

    public int CardScore(int card)
    {
        if (card == 52 || card == 53)
        {
            return 250;
        }
        int cardType = card % 13;
        switch (cardType)
        {
            case 0:             //AS
                return 150;
            case 1:             // two
            case 2:             // three
            case 3:             // four
            case 4:             // five
            case 5:             // six
            case 6:             // seven
            case 7:             // eight
            case 8:             // nine
            case 9:             // ten
                return 50;
            case 10:            // jack
            case 11:            // queen
            case 12:            // king
                return 100;
            default:
                Debug.LogError("card scorer goes through default case: " + cardType);
                return 0;
        }
    }

    private bool IsTwoJoker(List<int> cards)
    {
        int jokerCount = 0;
        foreach (int i in cards)
        {
            if (IsJoker(i))
                jokerCount++;
        }
        if (jokerCount > 1)
            return true;
        else
            return false;
    }

    private bool OfAKind(List<int> cards, bool haveCapital)
    {
        int value;
        if (IsJoker(cards[0]))                          //if first card is joker, use second card for initial
            value = cards[1] % 13;
        else
            value = cards[0] % 13;
        if (value != 0 && !haveCapital)
        // card not AS or don't have capital
        {
            Debug.Log("not AS or don't have capital");
            return false;
        }
        else if (value == 0 && !haveCapital && cards.Count != 4)
        // if AS but don't have capital and NOT four card
        {
            Debug.Log("AS but not four cards/don't have capital");
            return false;
        }
        foreach (int i in cards)
        {
            int temp = i % 13;
            if (IsJoker(i))                             //if joker change it to value
            {
                temp = value;
            }
            if (temp != 0 && temp < 10)
                return false;
            if (temp != value)
                return false;
        }
        return true;
    }

    private bool Straight(List<int> cards)
    {
        int leaf = -1;                                      //take the leaf
        bool isRoyalty = false;                             //take the card kind to disable overflow to/from royalty
        int initializer = IsJoker(cards[0]) ? 1 : 0;
        leaf = cards[initializer] / 13;
        if ((cards[initializer] % 13) < 10)
            isRoyalty = false;
        else
            isRoyalty = true;
        List<int> temp = new List<int>();
        foreach(int i in cards)
        {
            //check royalty consistency
            if (isRoyalty)
            {
                if (!IsJoker(i) && (i % 13) < 10)
                {
                    Debug.Log("unconsistent royalty status, royalty: " + isRoyalty);
                    return false;
                }
            }
            else
            {
                if (!IsJoker(i) && (i % 13) > 9)
                {
                    Debug.Log("unconsistent royalty status, royalty: " + isRoyalty);
                    return false;
                }
            }
            if (!IsJoker(i) && i % 13 == 0)                                            //check for AS
            {
                Debug.Log("AS cannot be straight");
                return false;
            }
            if (i / 13 != leaf && !IsJoker(i))                          //check for same leaf or joker
                return false;
            if (i == 52 || i == 53)
                temp.Add(-1);                                           //-1 for joker
            else
                temp.Add(i % 13);
        }
        temp.Sort();
        if (temp[0] == -1)                                              //if there is joker
        {
            int count = temp[temp.Count - 1] - temp[1] + 1;             // the last card minus the first card plus 1 equal to the amount of card
            if (count == temp.Count)
                return true;
            else if (count == temp.Count - 1)                           //last or first card can be joker
                return true;
            else
                return false;
        }
        else
        {
            int count = temp[temp.Count - 1] - temp[0] + 1;
            if (count == temp.Count)
                return true;
            else
                return false;
        }
    }
}
