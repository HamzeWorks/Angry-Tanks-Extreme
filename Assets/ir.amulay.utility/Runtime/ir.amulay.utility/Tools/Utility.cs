using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Amulay.Utility
{
    public static class Utility
    {
        public static TSource Random<TSource>(this TSource[] source) => source[UnityEngine.Random.Range(0, source.Length)];

        public static void AddExplosionForce(this Rigidbody2D rb, float explosionForce, Vector2 explosionPosition, float explosionRadius, float upwardsModifier = 0.0F, ForceMode2D mode = ForceMode2D.Force)
        {
            var explosionDir = rb.position - explosionPosition;
            var explosionDistance = explosionDir.magnitude;

            // Normalize without computing magnitude again
            if (upwardsModifier == 0)
                explosionDir /= explosionDistance;
            else
            {
                // From Rigidbody.AddExplosionForce doc:
                // If you pass a non-zero value for the upwardsModifier parameter, the direction
                // will be modified by subtracting that value from the Y component of the centre point.
                explosionDir.y += upwardsModifier;
                explosionDir.Normalize();
            }
            //var factor = (explosionRadius - explosionDistance);
            var factor = (explosionRadius * explosionRadius - explosionDistance * explosionDistance);

            rb.AddForce(Mathf.Lerp(0, explosionForce, factor) * explosionDir, mode);
        }

        public static void AddExplosionForceLinear(this Rigidbody2D rb, float explosionForce, Vector2 explosionPosition, float explosionRadius, float upwardsModifier = 0.0F, ForceMode2D mode = ForceMode2D.Force)
        {
            var explosionDir = rb.position - explosionPosition;
            var explosionDistance = explosionDir.magnitude;

            // Normalize without computing magnitude again
            if (upwardsModifier == 0)
                explosionDir /= explosionDistance;
            else
            {
                // From Rigidbody.AddExplosionForce doc:
                // If you pass a non-zero value for the upwardsModifier parameter, the direction
                // will be modified by subtracting that value from the Y component of the centre point.
                explosionDir.y += upwardsModifier;
                explosionDir.Normalize();
            }
            var factor = (explosionRadius - explosionDistance);
            //var factor = (explosionRadius * explosionRadius - explosionDistance * explosionDistance);

            rb.AddForce(Mathf.Lerp(0, explosionForce, factor) * explosionDir, mode);
        }
    }
}