using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using System;
using JetBrains.Annotations;
using Unity.VisualScripting;
using Unity.Mathematics;

public class EnemyInfoScr : MonoBehaviour
{
    [NonSerialized] public int currentEnemyIndex;
    public Enemy selfEnemy;
    public Image Logo;
    public TextMeshProUGUI Name;
    public TextMeshProUGUI Health;
    public TextMeshProUGUI Damage;
    [SerializeField] public Enemy currentEnemy;

    public void ShowEnemyInfo(Enemy enemy)
    {
        Logo.sprite = enemy.Sprite;
        Logo.preserveAspect = true;
        Name.text = enemy.Name;
        Health.text = enemy.CurrentHealth.ToString() + "/" + enemy.MaxHealth.ToString();
        Damage.text = enemy.Damage.ToString();
    }
    public void UpdateEnemyInfo(Enemy enemy)
    {
        Health.text = enemy.CurrentHealth.ToString() + "/" + enemy.MaxHealth.ToString();
        Damage.text = enemy.Damage.ToString();
    }

    public void TakeDamage(int damage)
    {
        //enemy.CurrentHealth -= damage;
        currentEnemy.CurrentHealth = math.max(0, currentEnemy.CurrentHealth - damage);
        UpdateEnemyInfo(currentEnemy);
    }

    public void CheckEnemyForDeath()
    {
        if (currentEnemy.CurrentHealth <= 0)
        {
            Debug.Log("Враг " + currentEnemy.Name + " побежден!");
            currentEnemyIndex = UnityEngine.Random.Range(0, EnemyManager.AllEnemies.Count);
            currentEnemy = EnemyManager.AllEnemies[currentEnemyIndex];
            ShowEnemyInfo(currentEnemy);
            Debug.Log("currentEnemyIndex = " + currentEnemyIndex + ", текущий противник: " + currentEnemy.Name);
            Debug.Log("Новый Враг " + currentEnemy.Name + " прибыл!");
        }
    }

    private void Start()
    {
        currentEnemyIndex = UnityEngine.Random.Range(0, EnemyManager.AllEnemies.Count);
        currentEnemy = EnemyManager.AllEnemies[currentEnemyIndex];
        Debug.Log("currentEnemyIndex = " + currentEnemyIndex + ", текущий противник: " + currentEnemy.Name);
        //ShowEnemyInfo(currentEnemy);
        ShowEnemyInfo(currentEnemy);

        
    }

    void Update()
    {
        CheckEnemyForDeath();
    }
}
