using System.Collections;
using System.Collections.Generic;
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

    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();

        ogPosition = transform.position;
        modifiedPosition = ogPosition + targetPos;
    }

    void FixedUpdate()
    {
        if (restTimer > 0)
        {
            restTimer -= Time.fixedDeltaTime;

            return;
        }

        timer += (Time.fixedDeltaTime / moveTime) * (!atTargetPos ? 1f : -1f);

        Vector2 targetPosition = Vector2.Lerp(ogPosition, modifiedPosition, timer);

        if(timer >= 1 || timer <= 0)
        {
            restTimer = restTime;
            atTargetPos = timer >= 1;
        }

        rigidBody.MovePosition(targetPosition);
    }
}
