using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlatformDeltaAttacher : MonoBehaviour
{
    public ContactFilter2D aboveFilter = new()
    {
        useNormalAngle = true,
        minNormalAngle = 270 - 15,
        maxNormalAngle = 270 + 15,
    };

    // Deltas
    private Vector2 oldPosition;
    private Vector2 movementDelta;

    private List<Rigidbody2D> frameRigidbodies = new(); // Rigidbodies that are interacting

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

        Collider2D[] contactCols = new Collider2D[3];
        rb.OverlapCollider(aboveFilter, contactCols);

        // Adjustments

        movementDelta = rb.position - oldPosition;

        UpdateInteractorsPositions(contactCols, movementDelta);

        // Set variables

        oldPosition = rb.position;

        frameRigidbodies.Clear();

    }

    // Update all of our transforms with the delta value
    void UpdateInteractorsPositions(Collider2D[] cols, Vector2 delta)
    {
        foreach (Rigidbody2D item in frameRigidbodies)
        {
            // if (item == null) continue;
            if (item.bodyType != RigidbodyType2D.Dynamic) continue;

            item.position += delta;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Rigidbody2D current = collision.rigidbody;

        if (collision.rigidbody == null) return;
        if (current.bodyType != RigidbodyType2D.Dynamic) return;
        if (GetAverageNormal(collision.contacts).y > 0.5f) return;
        if (frameRigidbodies.Contains(current)) return;

        frameRigidbodies.Add(current);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        Rigidbody2D current = collision.rigidbody;

        if (collision.rigidbody == null) return;
        if (current.bodyType != RigidbodyType2D.Dynamic) return;
        if (GetAverageNormal(collision.contacts).y > 0.5f) return;
        if (frameRigidbodies.Contains(current)) return;

        frameRigidbodies.Add(current);
    }

    private Vector2 GetAverageNormal(ContactPoint2D[] contacts)
    {
        Vector2 sumNormal = Vector2.zero;
        int count = 0;

        foreach (var contact in contacts)
        {
            sumNormal += contact.normal;
            count++;
        }

        return (sumNormal / count).normalized;
    }
}
