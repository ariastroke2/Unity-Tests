/*
 * 
 *  This script aims to record the change in position from one physicsUpdate to another, and apply this
 *  to the objects that are above it.
 *  
 *  This script applies some of the suggestions you gave, already.
 *  
 *  Though, it seems to lose contact easily upon platform descent.
 *  
 */


using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class DeltaAttacher2 : MonoBehaviour
{
    public ContactFilter2D contactFilter = new()
    {
        useNormalAngle = true,
        minNormalAngle = 270 - 15f,
        maxNormalAngle = 270 + 15f,
    };

    // Deltas
    private Vector2 oldPosition;
    private Vector2 movementDelta;

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnValidate()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.isKinematic = true;
    }

    void FixedUpdate()
    {
        // Adjustments

        Collider2D[] results = new Collider2D[3];
        rb.OverlapCollider(contactFilter, results);

        // Calculate delta through position difference
        movementDelta = rb.position - oldPosition;

        // Apply the movement delta to the objects
        UpdateInteractorsPositions(results, movementDelta);

        // Update the old rigidbody position
        oldPosition = rb.position;
    }

    // Update all of our transforms with the delta value
    void UpdateInteractorsPositions(Collider2D[] cols, Vector2 delta)
    {
        foreach (Collider2D item in cols)
        {
            if (item == null) continue;
            if (item.attachedRigidbody.bodyType != RigidbodyType2D.Dynamic) continue;

            item.attachedRigidbody.position += delta;
        }
    }
}
