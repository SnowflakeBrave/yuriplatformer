using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class ReverseMoveInDirection : MonoBehaviour
{
    [Tooltip("Applies a constant force in this direction.")]
    public Vector2 moveDirection;
    public bool scriptEnabled = true;
    private Rigidbody2D rigidbody2D;
    private BoxCollider2D collider2D;

    private int facingDirection = -1;
    private bool isGrounded;

    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        collider2D = GetComponent<BoxCollider2D>();
    }

    void FixedUpdate()
    {
        if(scriptEnabled)
        {
            if (isGrounded && !IsGroundEdge())
            {
                facingDirection *= -1;
            }

            rigidbody2D.AddForce(moveDirection * facingDirection);
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        foreach (ContactPoint2D contact in collision.contacts)
        {
            if (contact.normal.x * facingDirection < 0 && Mathf.Abs(contact.normal.x) > 0.9f)
            {
                facingDirection *= -1;
            }
            if (contact.normal.y > 0.5f)
            {
                isGrounded = true;
            }
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        isGrounded = false;
    }

    private bool IsGroundEdge()
    {
        Vector2 origin = new Vector2(collider2D.bounds.center.x, collider2D.bounds.min.y);

        RaycastHit2D hit = Physics2D.Raycast(
            origin,
            Vector2.down,
            collider2D.bounds.extents.y + 0.1f,
            LayerMask.GetMask("Default")
        );

        return hit.collider != null;
    }
}
