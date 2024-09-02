using UnityEngine;

public class EnemySpawnerLogic : MonoBehaviour
{
    private const float OFFSET_X = 5f;
    private const float OFFSET_Y = 5f;

    public void SpawnEnemy(GameObject obj)
    {
        GameObject enemy = obj;
        Vector2 spawnPos = new(UnityEngine.Random.Range(transform.position.x - OFFSET_X, transform.position.x + OFFSET_X),
                               UnityEngine.Random.Range(transform.position.y - OFFSET_Y, transform.position.y - OFFSET_Y));
        enemy.transform.position = spawnPos;
        Instantiate(enemy);
    }
}
