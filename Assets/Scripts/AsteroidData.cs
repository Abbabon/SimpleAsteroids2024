using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AsteroidData", menuName = "AsteroidData")]
public class AsteroidData : ScriptableObject
{
    public int Size;
    public int Score;
    public float MinSpeed;
    public float MaxSpeed;
    public List<Sprite> Sprites;
}