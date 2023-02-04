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

    #endregion

    #region Unity events

        private void Start()
        {
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
            spriteRenderer.color = Color.red;
            Invoke(nameof(ResetColor) , flashDuration);
        }

    #endregion

    #region Private Methods

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
            if (Input.GetKeyDown(KeyCode.Space)) rb.AddForce(new Vector2(rb.velocity.x , jumpForce) , ForceMode2D.Impulse);
        }

        private void HandleMove()
        {
            Turn(GetHorizontalAxis());
            rb.velocity = new Vector2(direction.x * moveSpeed * 300 * Time.deltaTime , rb.velocity.y);
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