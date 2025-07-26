using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using Unity.VisualScripting;

public class HandScr : MonoBehaviour
{

    public int drawPerTurn = 1;
    public int handSize = 5;
    public List<Card> playerHand = new List<Card>();
    
    public List<Card> playerDiscard = new List<Card>();

    public GameObject cardPrefab;
    public Transform parentContainer;
    public CardScr cardScr;
    public ShuffleDeckScr shuffleDeckScr;

    public void DrawCard(int drawPerTurn)
    {
        for (int i = 0; i < drawPerTurn; i++)
        {
            if (playerHand.Count < handSize & ShuffleDeckScr.playerDeck != null)
            {
                if (ShuffleDeckScr.playerDeck.Count >0)
                {
                    Card cardToDraw = ShuffleDeckScr.playerDeck[0];
                    playerHand.Add(cardToDraw);
                    ShuffleDeckScr.playerDeck.Remove(cardToDraw);
                    Debug.Log("Вы взяли карту из колоды: " + cardToDraw.Name);
                    AddCardToHand(playerHand[playerHand.Count - 1]);
                    shuffleDeckScr.playerDeckCount.text = ShuffleDeckScr.playerDeck.Count.ToString();
                } else Debug.LogError("Колода пуста!");

            }
        }
    
    }

    private void AddCardToHand(Card cardToDraw)
    {

        if (cardPrefab == null)
        {
            Debug.LogError("Префаб не назначен!");
            return;
        }

        if (parentContainer == null)
        {
            Debug.LogError("Родительский контейнер не назначен!");
            return;
        }



        GameObject newCard = Instantiate(cardPrefab, parentContainer, false);
        newCard.GetComponent<CardScr>().Name.text = cardToDraw.Name;
        newCard.GetComponent<CardScr>().Description.text = cardToDraw.Description;
        newCard.GetComponent<CardScr>().EnergyCost.text = cardToDraw.EnergyCost.ToString();
        newCard.GetComponent<CardScr>().Logo.sprite = cardToDraw.Sprite;
        newCard.GetComponent<CardScr>().Logo.preserveAspect = true;
    }


}
