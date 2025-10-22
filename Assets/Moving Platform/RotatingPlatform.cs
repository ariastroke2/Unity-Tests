using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class RotatingPlatform : MonoBehaviour
{
    [Header("Rotation Settings")]
    [SerializeField] private float degreesPerSecond = 45f;

    [SerializeField] private bool focusOnRotation = false;

    private Rigidbody2D rb;

    private float appliedDeltaRotation;

    private void Start()
    {
        rb.GetComponent<Rigidbody2D>();
    }

    private void OnValidate()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Static;
        rb.isKinematic = true;
    }

    // Due to execution order shenanigans, we gotta do this in fixedupdate
    void FixedUpdate()
    {
        float previousRotation = rb.rotation;
        float newRotation = previousRotation + (degreesPerSecond * Time.fixedDeltaTime);

        appliedDeltaRotation = newRotation - previousRotation;

        rb.MoveRotation(newRotation);
    }

    public Vector2 CalculateFragmentDelta(Vector3 platformPosition)
    {
        Vector3 localPos = transform.InverseTransformPoint(platformPosition);

        Quaternion rotationDelta = Quaternion.Euler(0, 0, appliedDeltaRotation);

        Vector3 rotatedGlobalPos = transform.TransformPoint(rotationDelta * localPos);

        Vector2 calculatedDelta = (rotatedGlobalPos - platformPosition);

        return calculatedDelta;
    }
}
