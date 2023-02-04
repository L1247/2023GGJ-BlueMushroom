#region

using UnityEngine;

#endregion

namespace Game.Scripts
{
    public class MushroomController : MonoBehaviour
    {
    #region Private Variables

        private int horizontalAxis;

        private Vector2 direction;

        private bool onGround;

        private int currentHealthAmount;

        private bool isDead;

        [SerializeField]
        [Min(0)]
        private float moveSpeed;

        [SerializeField]
        private SpriteRenderer spriteRenderer;

        [SerializeField]
        private Rigidbody2D rb;

        [SerializeField]
        private bool movable;

        [SerializeField]
        private int jumpForce = 15;

        [SerializeField]
        private float flashDuration = 0.3f;

        [SerializeField]
        private int healthAmount = 6;

        [SerializeField]
        private HealthBar healthBar;

    #endregion

    #region Unity events

        private void Start()
        {
            currentHealthAmount = healthAmount;
            if (movable) EnableController();
            else spriteRenderer.maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
        }

        private void Update()
        {
            if (movable == false) return;
            HandleMove();
            HandleJump();
        }

    #endregion

    #region Public Methods

        public void EnableController()
        {
            movable = true;
            DisableMask();
        }

        [ContextMenu("TakeDamage")]
        public void TakeDamage()
        {
            if (isDead) return;
            spriteRenderer.color = Color.red;
            Invoke(nameof(ResetColor) , flashDuration);
            currentHealthAmount -= 1;
            var healthPercent = currentHealthAmount / (float)healthAmount;
            healthBar.SetFillAmount(healthPercent);
            if (currentHealthAmount == 0) Die();
        }

    #endregion

    #region Private Methods

        [ContextMenu("Die")]
        private void Die()
        {
            isDead         = true;
            movable        = false;
            rb.velocity    = Vector2.zero;
            rb.constraints = RigidbodyConstraints2D.None;
            var angularVelocity              = onGround ? 400 : 100;
            var angularVelocityWithDirection = IsFacingLeft() ? -angularVelocity : angularVelocity;
            rb.angularVelocity = angularVelocityWithDirection;
        }

        private void DisableMask()
        {
            spriteRenderer.maskInteraction = SpriteMaskInteraction.None;
        }

        private int GetHorizontalAxis()
        {
            if (horizontalAxis != 0) return horizontalAxis;
            return (int)Input.GetAxisRaw("Horizontal");
        }

        private void HandleJump()
        {
            var spaceKeyDown = Input.GetKeyDown(KeyCode.Space);
            var canJump      = spaceKeyDown && onGround;
            if (canJump) rb.AddForce(new Vector2(rb.velocity.x , jumpForce) , ForceMode2D.Impulse);
        }

        private void HandleMove()
        {
            var horizontal = GetHorizontalAxis();
            Turn(horizontal);
            rb.velocity = new Vector2(direction.x * moveSpeed * 300 * Time.deltaTime , rb.velocity.y);
        }

        private bool IsFacingLeft()
        {
            return spriteRenderer.flipX;
        }

        private void OnCollisionEnter2D(Collision2D col)
        {
            var isGroundObject           = col.gameObject.name is "Ground" or "One way platform";
            if (isGroundObject) onGround = true;
        }

        private void OnCollisionExit2D(Collision2D col)
        {
            var isGroundObject           = col.gameObject.name is "Ground" or "One way platform";
            if (isGroundObject) onGround = false;
        }

        private void ResetColor()
        {
            spriteRenderer.color = Color.white;
        }

        private void Turn(int horizontalAxis)
        {
            direction = horizontalAxis switch
            {
                    1 => Vector2.right , -1 => Vector2.left , 0 => Vector2.zero , _ => direction
            };
            spriteRenderer.flipX = horizontalAxis switch
            {
                    1 => false , -1 => true , _ => spriteRenderer.flipX
            };
        }

    #endregion
    }
}