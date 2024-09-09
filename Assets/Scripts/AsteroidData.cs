using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AsteroidData", menuName = "AsteroidData")]
public class AsteroidData : ScriptableObject
{
    public int Level;
    public int Score;
    public float InitialThrustForce;
    public List<Sprite> Sprites;
}