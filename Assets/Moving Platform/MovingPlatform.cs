using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public Vector3 targetPos;

    public float moveTime;
    public float restTime;

    private Vector3 ogPosition;
    private Vector3 modifiedPosition;

    private Rigidbody2D rigidBody;

    private float timer;
    private float restTimer;

    private bool atTargetPos;

    public Vector2 appliedDelta;
    private Vector2 oldPosition;

    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();

        ogPosition = transform.position;
        modifiedPosition = ogPosition + targetPos;
    }

    // This script is executed 
    void FixedUpdate()
    {
        if (restTimer > 0)
        {
            restTimer -= Time.fixedDeltaTime;

            appliedDelta = Vector2.zero;

            return;
        }

        timer += (Time.fixedDeltaTime / moveTime) * (!atTargetPos ? 1f : -1f);

        Vector2 targetPosition = Vector2.Lerp(ogPosition, modifiedPosition, timer);

        if (timer >= 1 || timer <= 0)
        {
            restTimer = restTime;
            atTargetPos = timer >= 1;
        }

        appliedDelta = targetPosition - rigidBody.position;
        oldPosition = targetPosition;

        rigidBody.MovePosition(targetPosition);
    }
}
