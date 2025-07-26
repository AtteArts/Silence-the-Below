using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class EnemyManager
{
    public static List<Enemy> AllEnemies = new List<Enemy>();
}

public struct Enemy
    {
        public string Name;
        public Sprite Sprite;
        public int MaxHealth;
        public int CurrentHealth;
        public int Damage;



    public Enemy(string name, string logoPath, int health, int damage)
    {
        Name = name;
        Sprite = Resources.Load<Sprite>(logoPath);
        MaxHealth = health;
        CurrentHealth = health;
        Damage = damage;
    }
    }

public class EnemyManagerScr : MonoBehaviour
{
    void Awake()
    {
        EnemyManager.AllEnemies.Add(new Enemy("imp", "Sprites/Enemies/imp", 15, 2));
        EnemyManager.AllEnemies.Add(new Enemy("demon", "Sprites/Enemies/demon", 20, 4));
        EnemyManager.AllEnemies.Add(new Enemy("succub", "Sprites/Enemies/succub", 25, 5));
        EnemyManager.AllEnemies.Add(new Enemy("maw", "Sprites/Enemies/maw", 35, 7));
        Debug.Log("EnemyManager.AllEnemies.Count: " + EnemyManager.AllEnemies.Count + " Прошло " + Time.time.ToString("F5") + " секунд с начала игры");
    }

}
