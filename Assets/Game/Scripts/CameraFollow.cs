#region

using UnityEngine;

#endregion

namespace Game.Scripts
{
    public class CameraFollow : MonoBehaviour
    {
    #region Private Variables

        private float  xMin , xMax , yMin , yMax;
        private float  camY , camX;
        private float  camOrthsize;
        private float  cameraRatio;
        private Camera mainCam;

        [SerializeField]
        private Transform followTransform;

        [SerializeField]
        private BoxCollider2D mapBounds;

    #endregion

    #region Unity events

        private void Start()
        {
            xMin        = mapBounds.bounds.min.x;
            xMax        = mapBounds.bounds.max.x;
            yMin        = mapBounds.bounds.min.y;
            yMax        = mapBounds.bounds.max.y;
            mainCam     = GetComponent<Camera>();
            camOrthsize = mainCam.orthographicSize;
            cameraRatio = (xMax + camOrthsize) / 2.0f;
        }

    #endregion

    #region Private Methods

        // Update is called once per frame
        private void FixedUpdate()
        {
            camY               = Mathf.Clamp(followTransform.position.y , yMin + camOrthsize , yMax - camOrthsize);
            camX               = Mathf.Clamp(followTransform.position.x , xMin + cameraRatio , xMax - cameraRatio);
            transform.position = new Vector3(camX , camY , transform.position.z);
        }

    #endregion
    }
}