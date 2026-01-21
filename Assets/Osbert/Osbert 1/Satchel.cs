using UnityEngine;

public class Satchel : MonoBehaviour
{
    [Header("Explosion")]
    public float explosionRadius = 4f;
    public float maxForce = 15f;
    public LayerMask affectedLayers;
    public AnimationCurve forceFalloff = AnimationCurve.EaseInOut(0, 1, 1, 0);

    [Header("Player Boost")]
    public float playerForceMultiplier = 1.2f;
    public string playerTag = "Player";

    public void Detonate()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(
            transform.position,
            explosionRadius,
            affectedLayers
        );

        foreach (Collider2D hit in hits)
        {
            Rigidbody2D rb = hit.attachedRigidbody;
            if (rb == null)
                continue;

            Vector2 dir = (rb.position - (Vector2)transform.position);
            float distance = dir.magnitude;

            if (distance < 0.1f)
                dir = Vector2.up;
            else
                dir /= distance;

            float t = Mathf.Clamp01(distance / explosionRadius);
            float force = maxForce * forceFalloff.Evaluate(1f - t);

            // Player tuning
            if (hit.CompareTag(playerTag))
            {
                rb.linearVelocity = Vector2.zero; // arcade control, optional
                rb.AddForce(dir * force * playerForceMultiplier, ForceMode2D.Impulse);
            }
            else
            {
                rb.AddForce(dir * force, ForceMode2D.Impulse);
            }
        }

        Destroy(gameObject);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}