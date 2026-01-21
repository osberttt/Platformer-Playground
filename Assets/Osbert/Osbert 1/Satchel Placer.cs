using System.Collections.Generic;
using UnityEngine;

public class SatchelPlacer : MonoBehaviour
{
    [Header("Placement")]
    public GameObject satchelPrefab;
    public float placementRadius = 0.25f;
    public LayerMask groundLayer;

    [Header("Controls")]
    public KeyCode detonateKey = KeyCode.LeftShift;

    private List<Satchel> activeSatchels = new List<Satchel>();

    void Update()
    {
        HandlePlacement();
        HandleDetonation();
    }

    void HandlePlacement()
    {
        if (!Input.GetMouseButtonDown(0))
            return;

        Vector2 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // Check if touching ground
        bool touchingGround = Physics2D.OverlapCircle(
            worldPos,
            placementRadius,
            groundLayer
        );

        if (touchingGround)
            return;

        GameObject satchelGO = Instantiate(satchelPrefab, worldPos, Quaternion.identity);
        Satchel satchel = satchelGO.GetComponent<Satchel>();

        if (satchel != null)
            activeSatchels.Add(satchel);
    }

    void HandleDetonation()
    {
        if (!Input.GetKeyDown(detonateKey))
            return;

        for (int i = activeSatchels.Count - 1; i >= 0; i--)
        {
            if (activeSatchels[i] == null)
                continue;

            activeSatchels[i].Detonate();
        }

        activeSatchels.Clear();
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, placementRadius);
    }
}