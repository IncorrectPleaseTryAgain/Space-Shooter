using System;
using System.Threading;
using UnityEngine;

public class GravityPointLogic : MonoBehaviour
{
    // Properties
    [Serializable]
    struct Properties
    {
        public float mass;
        public float radius;
        public float gravityScaleMultiplpier;
        public float gravityScale;
    }
    [SerializeField]
    Properties properties;

    private void Awake()
    {
        if (properties.gravityScaleMultiplpier <= 0) { properties.gravityScaleMultiplpier = 1f; }
        if (properties.radius <= 0) { properties.radius = 1f; }

        properties.gravityScale = MathF.Abs((properties.mass / properties.radius) * properties.gravityScaleMultiplpier);
    }

    public float GetGravityScale() { return properties.gravityScale; }
}
