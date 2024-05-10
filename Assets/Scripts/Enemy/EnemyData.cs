using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

[CreateAssetMenu(fileName = "NewEnemy", menuName = "Enemy")]
public class EnemyData : ScriptableObject
{
    public string enemyName;

    public Sprite Art;
    public AnimatorController Animation;
    
    public float Health;
    public float Speed;
}
