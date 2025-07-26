using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting.ReorderableList.Element_Adder_Menu;
using UnityEngine;
using TMPro;


public class PlayerStats : MonoBehaviour
{
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI armorText;
    public TextMeshProUGUI energyPool;
    public int maxHealth = 50;
    public int currentHealth;
    public int Armor;
    public int Strength;
    public int currentEnergy;
    public int maxEnergy = 3;


    public List<StatusEffect> statusEffects = new List<StatusEffect>();

    public float damageMultiplier = 1.0f;
    public float healMultiplier = 1.0f;

    public void TakeDamage(int damage)
    {
        Debug.Log("У вас " + Armor + " брони");
        var armorAfterDamage = damage;

        if (Armor > 0)
        {
            armorAfterDamage = Armor - damage;
            if (armorAfterDamage > 0)
            {
                armorText.text = armorAfterDamage.ToString();
                Debug.Log("После удара у вас " + armorAfterDamage + " брони");
                Armor = armorAfterDamage;
                damage = 0; 
            }
            else if (armorAfterDamage <= 0)
            {
                armorText.text = "0";
                damage = math.abs(armorAfterDamage);
            }
        }
            currentHealth = math.max(0, currentHealth - Mathf.RoundToInt(damage * damageMultiplier));
            Debug.Log("Вам нанесли " + Mathf.RoundToInt(damage * damageMultiplier) + " урона");
    }

    public void Heal(int amount)
    {
        currentHealth = math.min(maxHealth, currentHealth + Mathf.RoundToInt(amount * healMultiplier));
    }

    public void CheckForDeath()
    {
        if (currentHealth <= 0)
        {
            Debug.Log("-------------Вы погибли!--------------");
            return;
        }
    }

    public void UpdateUI()
    {
        healthText.text = $"{currentHealth}/{maxHealth}";
        armorText.text = Armor.ToString();
        energyPool.text = currentEnergy.ToString();
    }


    public void ApplyStatusEffect(StatusEffect effect)
    {
        int effectIndex = statusEffects.FindIndex(x => x.effectName == effect.effectName);
        if (statusEffects.Contains(effect))
        {

            statusEffects[effectIndex].duration += 1;
            Debug.Log(statusEffects[effectIndex].effectName + " наложен " + statusEffects[effectIndex].duration + "раз");
        }
        else
        {

            statusEffects.Add(effect);
            effect.Apply(this);
            statusEffects[effectIndex].duration += 1;
            Debug.Log(statusEffects[effectIndex].effectName + " наложен " + statusEffects[effectIndex].duration + "раз");
        }
    }

    public void RemoveStatusEffect(StatusEffect effect)
    {
        if (statusEffects.Contains(effect))
        {
            statusEffects.Remove(effect);
            effect.Remove(this);
            Debug.Log(effect.effectName + " снят");
        }
    }

    public void UpdateEffects()
    {
        foreach (var effect in statusEffects)
        {
            effect.Update(this);
        }
    }

    void Awake()
    {
        currentHealth = maxHealth;
        currentEnergy = maxEnergy;
    }


    void Update()
    {

    }

    public abstract class StatusEffect
    {
        public string effectName;
        public int duration;

        public abstract void Apply(PlayerStats target);
        public abstract void Update(PlayerStats target);
        public abstract void Remove(PlayerStats target);

    }

    public class BurnEffect : StatusEffect
    {
        public override void Apply(PlayerStats target)
        {
            Debug.Log($"Игрок получил эффект: {effectName}");
        }

        public override void Remove(PlayerStats target)
        {
           Debug.Log($"Эффект {effectName} снят");
        }

        public override void Update(PlayerStats target)
        {
            target.TakeDamage(5);
            duration--;
        
            if (duration <= 0)
            {
                Remove(target);
            }
        }
    }

}
