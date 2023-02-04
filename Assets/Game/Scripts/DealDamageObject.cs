#region

using UnityEngine;

#endregion

namespace Game.Scripts
{
    public class DealDamageObject : MonoBehaviour
    {
    #region Private Methods

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.gameObject.TryGetComponent<MushroomController>(out var mushroomController)) mushroomController.TakeDamage();
        }

    #endregion
    }
}