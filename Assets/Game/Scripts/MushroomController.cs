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

        private bool isEnable;

        [SerializeField]
        [Min(0)]
        private float moveSpeed;

        [SerializeField]
        private SpriteRenderer spriteRenderer;

        [SerializeField]
        private Rigidbody2D rb;

    #endregion

    #region Unity events

        private void Update()
        {
            if (isEnable == false) return;

            Move();
        }

    #endregion

    #region Public Methods

        public void EnableController()
        {
            isEnable = true;
            DisableMask();
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

        private void Move()
        {
            Turn(GetHorizontalAxis());
            // rb.velocity = Vector2.zero;
            rb.velocity = new Vector2(direction.x * moveSpeed * 300 * Time.deltaTime , rb.velocity.y);
            // transform.position += (Vector3)direction * Time.deltaTime * moveSpeed;
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