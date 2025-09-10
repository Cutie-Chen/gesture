using UnityEngine;

namespace Oculus.Interaction
{
    public class CustomGrabTransformer : MonoBehaviour, ITransformer
    {
        public OneGrabTranslateTransformer _translateTransformer;
        public OneGrabRotateTransformer _rotateTransformer;
        private IGrabbable _grabbable;
        private bool _isRotationMode = false; // 初始为平移模式

        private void Awake()
        {
            // 初始化平移和旋转变换器
            _translateTransformer = gameObject.AddComponent<OneGrabTranslateTransformer>();
            _rotateTransformer = gameObject.AddComponent<OneGrabRotateTransformer>();
        }

        public void Initialize(IGrabbable grabbable)
        {
            // 初始化两个变换器
            _translateTransformer.Initialize(grabbable);
            _rotateTransformer.Initialize(grabbable);
        }

        public void BeginTransform()
        {
            var grabPose = _grabbable.GrabPoints[0];
            var targetTransform = _grabbable.Transform;

            // 根据抓取点位置判断模式（假设我们以物体的角落为判断标准）
            _isRotationMode = IsGrabPointInCubeBounds(grabPose.position, targetTransform);

            if (_isRotationMode)
            {
                _rotateTransformer.BeginTransform(); // 进入旋转模式
            }
            else
            {
                _translateTransformer.BeginTransform(); // 进入平移模式
            }
        }

        public void UpdateTransform()
        {
            var grabPose = _grabbable.GrabPoints[0];
            var targetTransform = _grabbable.Transform;

            // 根据抓取点位置判断模式（假设我们以物体的角落为判断标准）
            _isRotationMode = IsGrabPointInCubeBounds(grabPose.position, targetTransform);

            if (_isRotationMode)
            {
                _rotateTransformer.BeginTransform(); // 进入旋转模式
                _rotateTransformer.UpdateTransform(); // 更新旋转变换
            }
            else
            {
                _translateTransformer.BeginTransform(); // 进入平移模式
                _translateTransformer.UpdateTransform(); // 更新平移变换
            }
           
        }

        public void EndTransform()
        {
            _translateTransformer.EndTransform();
            _rotateTransformer.EndTransform();
        }

        private bool IsGrabPointInCubeBounds(Vector3 grabPoint, Transform targetTransform)
        {
            // 获取物体的四个角的坐标
            Vector3[] cubeCorners = new Vector3[8];
            Vector3 halfExtents = targetTransform.localScale * 0.5f;

            cubeCorners[0] = targetTransform.position + targetTransform.rotation * new Vector3(-halfExtents.x, -halfExtents.y, -halfExtents.z);
            cubeCorners[1] = targetTransform.position + targetTransform.rotation * new Vector3(halfExtents.x, -halfExtents.y, -halfExtents.z);
            cubeCorners[2] = targetTransform.position + targetTransform.rotation * new Vector3(halfExtents.x, -halfExtents.y, halfExtents.z);
            cubeCorners[3] = targetTransform.position + targetTransform.rotation * new Vector3(-halfExtents.x, -halfExtents.y, halfExtents.z);
            cubeCorners[4] = targetTransform.position + targetTransform.rotation * new Vector3(-halfExtents.x, halfExtents.y, -halfExtents.z);
            cubeCorners[5] = targetTransform.position + targetTransform.rotation * new Vector3(halfExtents.x, halfExtents.y, -halfExtents.z);
            cubeCorners[6] = targetTransform.position + targetTransform.rotation * new Vector3(halfExtents.x, halfExtents.y, halfExtents.z);
            cubeCorners[7] = targetTransform.position + targetTransform.rotation * new Vector3(-halfExtents.x, halfExtents.y, halfExtents.z);

            // 判断抓取点是否在四个角范围内
            foreach (var corner in cubeCorners)
            {
                if (Vector3.Distance(grabPoint, corner) < 0.5f) // 可以设置合适的阈值来判断是否在范围内
                {
                    return true;
                }
            }

            return false;
        }

        #region Inject

        public void InjectOptionalConstraints(OneGrabTranslateTransformer.OneGrabTranslateConstraints constraints)
        {
            _translateTransformer.InjectOptionalConstraints(constraints);
        }

        #endregion
    }
}
