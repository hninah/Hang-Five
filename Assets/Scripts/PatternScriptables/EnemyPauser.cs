using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPauser : MonoBehaviour
{
    private static EnemyPauser _instance;
    public static EnemyPauser Instance { get { return _instance; } }

    public List<Obstacle> enemies;

    void Awake()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
        } 
        else
        {
            _instance = this;
        }
    }

    void LateUpdate()
    {
        List<Obstacle> tempEnemies = new List<Obstacle>();

        foreach (Obstacle enemy in enemies)
        {
            if (!enemy)
            {
                continue;
            }

            tempEnemies.Add(enemy);
        }

        enemies = tempEnemies;
    }

    public void addObstacle(Obstacle obstacle)
    {
        enemies.Add(obstacle);
    }

    public void pauseAllObstacles()
    {
        foreach (Obstacle enemy in enemies)
        {
            if (!enemy)
            {
                continue;
            }

            enemy.enabled = false;
        }
    }

    public void unpauseAllObstacles()
    {
        foreach (Obstacle enemy in enemies)
        {
            if (!enemy)
            {
                continue;
            }

            enemy.enabled = true;
        }
    }
}
