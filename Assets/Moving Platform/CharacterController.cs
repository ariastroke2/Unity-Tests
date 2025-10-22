using UnityEngine;

public class CharacterController : MonoBehaviour
{
    public float jumpImpulse = 5f;
    public float moveSpeed = 3f;

    private bool OnGround => rigidBody.IsTouching(groundFilter);

    private Rigidbody2D rigidBody;

    private ContactFilter2D groundFilter = new()
    {
        useNormalAngle = true,
        minNormalAngle = 90f - 15f,
        maxNormalAngle = 90f + 15f,

    };

    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
    }

    // Capture input
    void Update()
    {
        Vector2 velocity = rigidBody.velocity;

        velocity *= new Vector2(0.5f, 1f);

        if (Input.GetKey(KeyCode.A))
        {
            velocity.x = -moveSpeed;
        }
        if (Input.GetKey(KeyCode.D))
        {
            velocity.x = moveSpeed;
        }
        if (Input.GetKeyDown(KeyCode.W) && OnGround)
        {
            velocity += new Vector2(0, jumpImpulse);
        }

        rigidBody.velocity = velocity;
    }
}
