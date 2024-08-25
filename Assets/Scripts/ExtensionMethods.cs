using UnityEngine;

public static class ExtensionMethods
{
    public static void HaltRigidbody(this Rigidbody2D rigidbody2D)
    {
        rigidbody2D.velocity = Vector2.zero;
        rigidbody2D.angularVelocity = 0f;
    }      
}