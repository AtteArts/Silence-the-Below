using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.PlayerLoop;
using System;
using System.Linq;
using TMPro;

public static class CardManager
{
    public static List<Card> AllCards = new List<Card>();
}

[System.Serializable]
public struct Card
{
    public string Name;
    public string Description;
    public Sprite Sprite;
    public int EnergyCost;
    public List<CardProperty> Properties;
    public enum Target {PLAYER, ENEMY}
    public Target IsTarget;
    public bool isCanDiscard;

    public Card(string name, string description, string logoPath, int energyCost, List<CardProperty> property, Target isTarget)
    {
        Name = name;
        Description = description;
        Sprite = Resources.Load<Sprite>(logoPath);
        EnergyCost = energyCost;
        Properties = new List<CardProperty>();
        for (int i = 0; i < property.Count(); i++)
        {
            Properties.Add(property[i]);
        }
        IsTarget = isTarget;
        isCanDiscard = false;
    }
    public void ApplyCardLogic(GameObject target)
    {
        if (target != null)
            foreach (var prop in Properties)
            {
                prop.Apply(target);
            }
    }
}

public abstract class CardProperty
{
    public abstract void Apply(GameObject target);

}

public class AttackProperty : CardProperty
{
    public int Damage { get; private set; }

    public AttackProperty(int damage)
    {
        Damage = damage;
    }

    public override void Apply(GameObject target)
    {
        if (target.GetComponent<EnemyInfoScr>() != null)
        {
            var targetInfo = target.GetComponent<DropPlaceScr>();
            var targetStats = target.GetComponent<EnemyInfoScr>();
            if (targetInfo.targetInfo == Target.ENEMY)
            {
                Debug.Log(targetStats.currentEnemy.CurrentHealth.ToString());
                targetStats.TakeDamage(Damage);
                Debug.Log($"Нанесено {Damage} урона!" + "Текущее здоровье: " + targetStats.currentEnemy.CurrentHealth.ToString());
            }
        }
        else
        {
            var nearEnemy = target.transform.parent.GetComponentInChildren<EnemyInfoScr>();
            nearEnemy.TakeDamage(Damage);
            Debug.Log($"Противника нет, атака прошла по ближайшему противнику");
        }
    }
}

public class DefenseProperty : CardProperty
{
    public int Armor { get; private set; }

    public DefenseProperty(int armor)
    {
        Armor = armor;
    }

    public override void Apply(GameObject target)
    {
        
        var targetInfo = target.GetComponent<DropPlaceScr>();
        
        Debug.Log($"Карта брони отпущена на {targetInfo}!");
        var player = target.transform.parent.GetComponentInChildren<PlayerStats>();
        Debug.Log($"Карта брони Использована на {player}!");
        if (player != null)
        {
            player.Armor += Armor;
            Debug.Log($"Получено {Armor} брони!");
            player.UpdateUI();
        }
        
    }
}

public class BuffProperty : CardProperty
{
    public int Strength { get; private set; }

    public BuffProperty(int strength)
    {
        Strength = strength;
    }

    public override void Apply(GameObject target)
    {

        var targetInfo = target.GetComponent<DropPlaceScr>();
        Debug.Log($"Карта силы отпущена на {targetInfo}!");
        var player = target.transform.parent.GetComponentInChildren<PlayerStats>();
        Debug.Log($"Карта силы Использована на {player}!");
        if (player != null)
        {
            player.Strength += Strength;
            Debug.Log($"Получено усиление силы на {Strength}!");
            Debug.Log($"Текущая сила {player.Strength}!");
        }

    }
}

public class CardManagerScr : MonoBehaviour
{
    DropPlaceScr dropPlaceScr;
    public PlayerStats playerStats;
    public HandScr handScr;
    
    public void PlayCard(PlayerStats playerStats, GameObject target, Card card)
    {
        if (CheckCost(playerStats, card.EnergyCost))
        {
            var targetInfo = target.GetComponent<DropPlaceScr>();
            if (targetInfo.targetInfo == Target.ENEMY)
            {
                card.ApplyCardLogic(target);
                UpdateEnergyPool(playerStats, card.EnergyCost);

                card.isCanDiscard = true;
                Debug.Log("Можно ли сбросить карту " + handScr.playerHand[dropPlaceScr.cardIndex].isCanDiscard);
                if (handScr.playerHand[dropPlaceScr.cardIndex].isCanDiscard)
                {
                    dropPlaceScr.DiscardCard(dropPlaceScr.cardIndex);
                    Destroy(dropPlaceScr.card.currentCard);
                }
            }
            else if (targetInfo.targetInfo == Target.NONE || targetInfo.targetInfo == Target.PLAYER)
            {
                card.ApplyCardLogic(target);
                UpdateEnergyPool(playerStats, card.EnergyCost);
                card.isCanDiscard = true;
            }
            else if (targetInfo.targetInfo == Target.HAND)
            {
                Debug.Log($"Карта возвращена в руку!");
                return;
            }

            // Дополнительные действия
            // Звуковые эффекты
        }
        else
        {
            Debug.Log("Недостаточно ресурсов!");
        }
    }
    

    private bool CheckCost(PlayerStats player, int cost)
    {
        if (player.currentEnergy >= 0 & player.currentEnergy >= cost)
        {
            return true;
        }
        else return false;
    }

    private void UpdateEnergyPool(PlayerStats player, int cost)
    {
        player.currentEnergy -= cost;
        player.energyPool.text = player.currentEnergy.ToString();
        Debug.Log("Потрачено " + cost + " маны, осталось " + player.currentEnergy + " маны");
        
    }
    void Awake()
    {
        InitializeCards();
    }

    private void InitializeCards()
    {
        var Properties = new List<CardProperty>();
        CardManager.AllCards.Add(new Card("strike", "Deal damage", "Sprites/Cards/strike", 1, Properties = new List<CardProperty> { new AttackProperty(10) }, Card.Target.ENEMY));
        
        CardManager.AllCards.Add(new Card("def", "Give defence", "Sprites/Cards/def", 1, Properties = new List<CardProperty> { new DefenseProperty(10) }, Card.Target.PLAYER));

        CardManager.AllCards.Add(new Card("dash", "Deal damage, Give defence", "Sprites/Cards/dash", 2, Properties = new List<CardProperty> { new AttackProperty(12), new DefenseProperty(12) }, Card.Target.ENEMY));
       
        CardManager.AllCards.Add(new Card("strength", "Give strength", "Sprites/Cards/strength", 2, Properties = new List<CardProperty> { new BuffProperty(2) }, Card.Target.PLAYER));
    }
}

