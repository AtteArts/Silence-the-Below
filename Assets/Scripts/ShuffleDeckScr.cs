using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;


public class ShuffleDeckScr : MonoBehaviour
{
    public int deckSize;
    public TextMeshProUGUI playerDeckCount;
    public TextMeshProUGUI playerDiscardCount;
    public static List<Card> playerDeck = new List<Card>();
    public HandScr handScr;


    public void ShuffleDeck(int deckSize)
    {
        for (int i = 0; i < deckSize; i++)
        {
            int randomCardIndex = UnityEngine.Random.Range(0, CardManager.AllCards.Count);
            playerDeck.Add(CardManager.AllCards[randomCardIndex]);
            Debug.Log("В колоду добавлена карта " + CardManager.AllCards[randomCardIndex].Name);
        }

        playerDeckCount.text = playerDeck.Count.ToString();
    }

    public bool CheckForEmptyDeck()
    {
        if (playerDeck.Count <= 0)
        {
            return true;
        }
        else return false;
    }

    public void ReShuffleDeck(int deckSize)
    {
        for (int i = 0; i < deckSize; i++)
        {
            playerDeck.Add(handScr.playerDiscard[deckSize - i]);
            handScr.playerDiscard.Remove(handScr.playerDiscard[deckSize - i]);
        }
        playerDiscardCount.text = handScr.playerDiscard.Count.ToString();
    }

        void Update()
    {
        
    }
}
