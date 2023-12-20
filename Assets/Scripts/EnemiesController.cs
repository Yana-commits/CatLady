using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemiesController : CreatureCollector<Enemy>
{
    [SerializeField] private GameObject player;
    [SerializeField] private float throwPower = 10;
    [SerializeField] private float bouncePower;
    [SerializeField] private float pushPower;
    [SerializeField] private float bounceKoef = 0.1f;
    [SerializeField] private int randomThrowNumber = 10;

    private int counter = 0;
    private List<int> enemiesForCats = new List<int>();

    public Action OnLookForCat;

    private void Update()
    {
        if (counter >= randomThrowNumber)
        {
            EnemyAttack();
            counter = 0;
        }
        else
        {
            counter++;
        }
    }
    public override void Initialise()
    {
        base.Initialise();
        for (var i = 0; i < creatures.Count; i++)
        {
            creatures[i].EnemySettings(player, throwPower, bouncePower, OnLookForCat, pushPower, bounceKoef);
            creatures[i].Id = i;
        }
    }

    public void LookingCat(Transform cat)
    {
        var enemy = creatures.Where(x => x.IsLookingCat == true).FirstOrDefault();
        enemy.ChaseCat(cat);
        enemiesForCats.Add(enemy.Id);
    }

    public void StopLookingCat(int id)
    {
        if (enemiesForCats.Contains(id))
        {
            enemiesForCats.Remove(id);
        }
        else if(enemiesForCats.Count > 0)
        {
            foreach (var number in enemiesForCats)
            {
                var enemy = creatures.Where(x => x.Id == number).FirstOrDefault();

                if (enemy != null)
                {
                    enemy.IsLookingCat = false;
                    enemy.NavRandomPoint();
                }
            }
            enemiesForCats.Clear();
        }
    }

    private void EnemyAttack()
    {
        var enemyNumber = UnityEngine.Random.Range(0, creatures.Count);
        var enemy = creatures[enemyNumber];

        var direction = new Vector3(UnityEngine.Random.Range(-10, 10), 1f, UnityEngine.Random.Range(-10, 10));
        enemy.WatchTarget(direction);
    }
    public void ShowEnemySmile(int enemyId)
    {
        var enemy = creatures.Where(x => x.Id == enemyId).FirstOrDefault();
        enemy.ShowSmile();
    }
}
