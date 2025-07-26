using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.XR;
using System.Linq;


public class GameManagerScr : MonoBehaviour
{
    public PlayerStats playerStats;
    public EnemyInfoScr enemyInfoScr;
    public ShuffleDeckScr shuffleDeckScr;
    public HandScr handScr;
    public CardManagerScr cardManagerScr;
    public EnemyManagerScr enemyManagerScr;
    public int deckSize = 10;

    public bool isPlayerTurn = true;


    public Button endTurnButton;
    private int turnCount = 1;

    void Start()
    {
        playerStats.UpdateUI();
        UpdateButtonState();
        shuffleDeckScr.ShuffleDeck(deckSize);
        handScr.DrawCard(2);
        if (shuffleDeckScr.CheckForEmptyDeck())
        {
            shuffleDeckScr.ReShuffleDeck(deckSize);
        }
        Debug.Log("Текущий ход: " + turnCount);
        if (endTurnButton != null)
        {
            endTurnButton.onClick.AddListener(HandleButtonClick);
        }
        Debug.Log("У вас в руке карты: " + handScr.playerHand[0].Name + handScr.playerHand[1].Name);


    }


    private void HandleButtonClick()
    {
        EndTurn();
    }

    private void OnDisable()
    {
        if (endTurnButton != null)
        {
            endTurnButton.onClick.RemoveAllListeners();
        }
    }


    public void UpdateButtonState()
    {
        endTurnButton.interactable = isPlayerTurn;
    }

    public void EndTurn()
    {
        isPlayerTurn = false;
        UpdateButtonState();
        playerStats.TakeDamage(enemyInfoScr.currentEnemy.Damage);
        playerStats.UpdateUI();

        playerStats.CheckForDeath();

        StartCoroutine(WaitForPlayerTurn());
    }

    IEnumerator WaitForPlayerTurn()
    {
        if (!(playerStats.currentHealth <= 0))
        {
            yield return new WaitForSeconds(2);
            isPlayerTurn = true;
            playerStats.Armor = 0;
            turnCount += 1;
            playerStats.currentEnergy = playerStats.maxEnergy;
            playerStats.UpdateUI();
            handScr.DrawCard(2);
            Debug.Log("Текущий ход: " + turnCount);
            UpdateButtonState();
        }
        else endTurnButton.interactable = false;
    }
    

}


