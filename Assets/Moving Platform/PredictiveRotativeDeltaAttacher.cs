/*
 * 
 *  In this approach, we try to predict the changes that will be applied, so that we can add them *before* the physics Update
 *  
 *  The idea behind this is that if you calculate the delta to be applied before the physicsUpdate, your changes postPhysicsUpdate
 *  should reflect also the calculated delta, right?
 *  
 *  In my project I implemented an interface for this sort of calculation. 
 *  
 */


using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PredictiveRotativeDeltaAttacher : MonoBehaviour
{

    private List<Rigidbody2D> frameRigidbodies = new(); // Rigidbodies that are interacting

    private Rigidbody2D rb;

    public RotatingPlatform platformControl;

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

        rb.MoveRotation(0f);

        // Apply the movement delta to the objects
        UpdateInteractorsPositions(platformControl.CalculateFragmentDelta(rb.position));

        // Clean list
        frameRigidbodies.Clear();
    }

    // Update all of our transforms with the delta value
    void UpdateInteractorsPositions(Vector2 delta)
    {
        foreach (Rigidbody2D item in frameRigidbodies)
        {
            if (item.bodyType != RigidbodyType2D.Dynamic) continue;

            item.position += delta;
        }
    }

    // Register those objects we have contact with
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Rigidbody2D current = collision.rigidbody;

        if (collision.rigidbody == null) return;
        if (current.bodyType != RigidbodyType2D.Dynamic) return;
        if (GetAverageNormal(collision.contacts).y > 0.5f) return;
        if (frameRigidbodies.Contains(current)) return;

        frameRigidbodies.Add(current);
    }

    // Register those objects we have contact with
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
