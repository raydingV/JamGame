using System.Collections;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    private EnemyManager _enemyManager;
    
    // [Header("Bools")]
    // [SerializeField] private bool BigMob = false;
    // [SerializeField] private bool Fighter = false;
    // [SerializeField] private bool Snake = false;
    // [SerializeField] private bool Projectile = false;

    [Header("EnemyObject")] 
    [SerializeField] private GameObject[] Enemys;

    // [Header("Data")] [SerializeField] private EnemyData[] _datas;

    private GameObject newEnemy;
    
    void Start()
    {
        // Debug.Log(_datas[0].ToString());
        // for (int i = 0; i < _datas.Length; i++)
        // {
        //     if (BigMob == false && _datas[0].ToString() == BigMob.ToString())
        //     {
        //         _datas.
        //     }
        // }
        StartCoroutine(spawnTimer());
    }


    IEnumerator spawnTimer()
    {
        yield return new WaitForSeconds(Random.Range(6, 12));
        
        spawnEnemy();
        
        StartCoroutine(spawnTimer());
    }

    void spawnEnemy()
    {
        newEnemy = Instantiate(Enemys[Random.Range(0,Enemys.Length)], transform.position, Quaternion.identity);
    }
}
