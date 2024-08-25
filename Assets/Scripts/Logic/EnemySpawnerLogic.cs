using UnityEngine;

public class EnemySpawnerLogic : MonoBehaviour
{
    public void Spawn(GameObject obj)
    {
        GameObject enemy = obj;
        enemy.transform.position = Vector3.zero;
        Instantiate(enemy);
    }
}
