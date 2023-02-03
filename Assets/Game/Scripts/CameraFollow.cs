using UnityEngine;

namespace Game.Scripts
{
    public class CameraFollow : MonoBehaviour
    {
    #region Private Variables

        [SerializeField]
        private Transform followTransform;

    #endregion

    #region Unity events

        private void Update()
        {
            transform.position = new Vector3(followTransform.position.x , transform.position.y , transform.position.z);
        }

    #endregion
    }
}