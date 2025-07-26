using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using System;
using UnityEngine.XR;
using Unity.VisualScripting;

public enum Target
{
    NONE,
    ENEMY,
    PLAYER,
    HAND
}

public class DropPlaceScr : MonoBehaviour, IDropHandler, IEndDragHandler
{
    public CardManagerScr cardManagerScr;
    public EnemyManagerScr enemyManagerScr;
    public CardMovementScript cardMovementScript;
    public ShuffleDeckScr shuffleDeckScr;
    public PlayerStats playerStats;
    public HandScr handScr;
    public Target targetInfo;
    private GameObject playerTarget;
    public int cardIndex;
    public CardMovementScript card;
    
    public void OnDrop(PointerEventData eventData)
    {
        card = eventData.pointerDrag.GetComponent<CardMovementScript>();
        int cardIndex = card.defaultSibIndex;

        Debug.Log("cardIndex = " + cardIndex);
        GameObject attackCard = eventData.pointerDrag;
        GameObject targetEnemy = eventData.pointerEnter;
        if (targetEnemy.GetComponent<DropPlaceScr>() != null)
            targetInfo = targetEnemy.GetComponent<DropPlaceScr>().targetInfo;
        Debug.Log("target enemy - " + targetEnemy);
        Debug.Log("target - " + targetInfo);
        if (targetEnemy.GetComponent<DropPlaceScr>() != null)
        {

            if (card != null)
            {
                if (targetInfo == Target.ENEMY)
                {
                    Debug.Log("It's enemy " + targetEnemy.GetComponent<EnemyInfoScr>().Name.text);
                    //int currentEnemyIndex = targetEnemy.GetComponent<EnemyInfoScr>().currentEnemyIndex;
                    //Enemy enemy = EnemyManager.AllEnemies[currentEnemyIndex];
                    //targetEnemy.GetComponent<EnemyInfoScr>().TakeDamage(enemy, );

                    Debug.Log("Это " + cardIndex + " карта");
                    cardManagerScr.PlayCard(playerStats, targetEnemy, handScr.playerHand[cardIndex]);
                    playerTarget = playerStats.transform.gameObject;
                    playerTarget.GetComponent<Animation>().Play("AttackAnim");
                    cardMovementScript.BlocksRaycasts(card, true);
                    Destroy(card.gameObject);
                }
                else
                if (targetInfo == Target.PLAYER || targetInfo == Target.NONE)
                {
                    Debug.Log("It's no enemy");
                    playerTarget = playerStats.transform.gameObject;
                    playerTarget.GetComponent<Animation>().Play("AttackAnim");
                    cardManagerScr.PlayCard(playerStats, playerTarget, handScr.playerHand[cardIndex]);
                    cardMovementScript.BlocksRaycasts(card, true);
                    Destroy(card.gameObject);


                }
                else
                if (targetInfo == Target.HAND)
                {
                    Debug.Log("It's hand");
                    return;
                }
            }
            else return;
        }
    }


    public IEnumerator DestroyCardWithAnimation(GameObject card)
    {
        var destroyDuration = 0.5f;
        // Анимация уменьшения
        var originalScale = card.transform.localScale;
        for (float t = 0; t < destroyDuration; t += Time.deltaTime)
        {
            card.transform.localScale = Vector3.Lerp(originalScale, Vector3.zero, t / destroyDuration);
            yield return null;
        }
        // Финальное удаление
        Destroy(card);
    }

    public void DiscardCard(int cardIndex)
    {
        handScr.playerDiscard.Add(handScr.playerHand[cardIndex]);
        Debug.Log("Вы сбросили карту из руки в колоду: " + handScr.playerHand[cardIndex].Name);
        handScr.playerHand.Remove(handScr.playerHand[cardIndex]);
        shuffleDeckScr.playerDiscardCount.text = handScr.playerDiscard.Count.ToString();
        
    }

    public void OnEndDrag(PointerEventData eventData)
    {

    }
}
