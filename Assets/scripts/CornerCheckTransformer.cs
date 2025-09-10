using System;
using UnityEngine;

namespace Oculus.Interaction
{
    /// <summary>
    /// A Transformer that switches between rotation and translation based on grab point.
    /// If the grab point is near the corners of the object, it enters rotation mode;
    /// otherwise, it enters translation mode.
    /// </summary>
    public class CornerCheckTransformer : MonoBehaviour, ITransformer
    {
        [Serializable]
        public class CornerCheckSettings
        {
            public float CornerThreshold = 0.1f; // Distance threshold to define a corner
        }

        [SerializeField]
        private CornerCheckSettings _settings = new CornerCheckSettings();

        private ITransformer _rotationTransformer;
        private ITransformer _translationTransformer;
        private IGrabbable _grabbable;

        public void Initialize(IGrabbable grabbable, ITransformer rotationTransformer, ITransformer translationTransformer)
        {
            _grabbable = grabbable;
            _rotationTransformer = rotationTransformer;
            _translationTransformer = translationTransformer;

            _rotationTransformer.Initialize(grabbable);
            
            _translationTransformer.Initialize(grabbable);
        }

        public void BeginTransform()
        {
            ITransformer selectedTransformer = GetSelectedTransformer();
            selectedTransformer.BeginTransform();
        }

        public void UpdateTransform()
        {
            ITransformer selectedTransformer = GetSelectedTransformer();
            selectedTransformer.UpdateTransform();
        }

        public void EndTransform()
        {
            ITransformer selectedTransformer = GetSelectedTransformer();
            selectedTransformer.EndTransform();
        }

        private ITransformer GetSelectedTransformer()
        {
            Vector3 grabPoint = _grabbable.GrabPoints[0].position;
            Transform target = _grabbable.Transform;

            // Check if grab point is near a corner
            Vector3 localGrabPoint = target.InverseTransformPoint(grabPoint);
            Vector3 localBounds = target.GetComponent<Collider>().bounds.size / 2;

            bool isCorner = Mathf.Abs(Mathf.Abs(localGrabPoint.x) - localBounds.x) < _settings.CornerThreshold &&
                            Mathf.Abs(Mathf.Abs(localGrabPoint.y) - localBounds.y) < _settings.CornerThreshold &&
                            Mathf.Abs(Mathf.Abs(localGrabPoint.z) - localBounds.z) < _settings.CornerThreshold;

            return isCorner ? _rotationTransformer : _translationTransformer;
        }

        #region Inject

        public void InjectSettings(CornerCheckSettings settings)
        {
            _settings = settings;
        }

        public void Initialize(IGrabbable grabbable)
        {
            _rotationTransformer.Initialize(grabbable);
        }

        #endregion
    }
}
