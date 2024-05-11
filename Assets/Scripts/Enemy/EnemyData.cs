using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

[CreateAssetMenu(fileName = "NewEnemy", menuName = "Enemy")]
public class EnemyData : ScriptableObject
{
    [Header("String")]
    public string enemyName;

    [Header("ArtWorks")]
    public Sprite Art;
    public AnimatorController Animation;
    
    [Header("Values")]
    public float Health;
    public float Speed;
    public float Distance;
    public Vector3 Scale;
    
}
