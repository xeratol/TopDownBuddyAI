using UnityEngine;
using UnityEngine.AI;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    public Transform _enemyPrefab = null;

    [SerializeField]
    public float _spawnDistance = 4.0f;

    void Start()
    {
        Debug.Assert(_enemyPrefab, "Enemy Prefab not set", this);
    }

    private void OnTriggerEnter(Collider other)
    {
        var angle = Random.Range(0, 2 * Mathf.PI);
        var pos = transform.position;
        pos.x += _spawnDistance * Mathf.Cos(angle);
        pos.z += _spawnDistance * Mathf.Sin(angle);

        NavMeshHit hit;
        if (NavMesh.SamplePosition(pos, out hit, 2.0f, NavMesh.AllAreas))
        {
            pos = hit.position;
        }

        var enemy = Instantiate(_enemyPrefab, pos, Quaternion.identity);
        var healthBehavior = enemy.GetComponent<HealthBehavior>();
        healthBehavior.OnDeathListener += OnEnemyDeath;

        gameObject.SetActive(false);
    }

    private void OnEnemyDeath()
    {
        gameObject.SetActive(true);
    }
}
