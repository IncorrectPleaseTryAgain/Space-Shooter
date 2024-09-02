using UnityEngine;

public class ProjectileSpawnerLogic : MonoBehaviour
{
    public void SpawnProjectile(GameObject projectile, Vector2 velocity)
    {
        GameObject obj = Instantiate(projectile, transform.position, transform.rotation);
        obj.GetComponent<ProjectileLogic>().Shoot(velocity);
    }
}
