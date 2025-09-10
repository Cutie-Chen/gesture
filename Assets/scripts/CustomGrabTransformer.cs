using UnityEngine;

namespace Oculus.Interaction
{
    public class CustomGrabTransformer : MonoBehaviour, ITransformer
    {
        public OneGrabTranslateTransformer _translateTransformer;
        public OneGrabRotateTransformer _rotateTransformer;
        private IGrabbable _grabbable;
        private bool _isRotationMode = false; // ��ʼΪƽ��ģʽ

        private void Awake()
        {
            // ��ʼ��ƽ�ƺ���ת�任��
            _translateTransformer = gameObject.AddComponent<OneGrabTranslateTransformer>();
            _rotateTransformer = gameObject.AddComponent<OneGrabRotateTransformer>();
        }

        public void Initialize(IGrabbable grabbable)
        {
            // ��ʼ�������任��
            _translateTransformer.Initialize(grabbable);
            _rotateTransformer.Initialize(grabbable);
        }

        public void BeginTransform()
        {
            var grabPose = _grabbable.GrabPoints[0];
            var targetTransform = _grabbable.Transform;

            // ����ץȡ��λ���ж�ģʽ����������������Ľ���Ϊ�жϱ�׼��
            _isRotationMode = IsGrabPointInCubeBounds(grabPose.position, targetTransform);

            if (_isRotationMode)
            {
                _rotateTransformer.BeginTransform(); // ������תģʽ
            }
            else
            {
                _translateTransformer.BeginTransform(); // ����ƽ��ģʽ
            }
        }

        public void UpdateTransform()
        {
            var grabPose = _grabbable.GrabPoints[0];
            var targetTransform = _grabbable.Transform;

            // ����ץȡ��λ���ж�ģʽ����������������Ľ���Ϊ�жϱ�׼��
            _isRotationMode = IsGrabPointInCubeBounds(grabPose.position, targetTransform);

            if (_isRotationMode)
            {
                _rotateTransformer.BeginTransform(); // ������תģʽ
                _rotateTransformer.UpdateTransform(); // ������ת�任
            }
            else
            {
                _translateTransformer.BeginTransform(); // ����ƽ��ģʽ
                _translateTransformer.UpdateTransform(); // ����ƽ�Ʊ任
            }
           
        }

        public void EndTransform()
        {
            _translateTransformer.EndTransform();
            _rotateTransformer.EndTransform();
        }

        private bool IsGrabPointInCubeBounds(Vector3 grabPoint, Transform targetTransform)
        {
            // ��ȡ������ĸ��ǵ�����
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

            // �ж�ץȡ���Ƿ����ĸ��Ƿ�Χ��
            foreach (var corner in cubeCorners)
            {
                if (Vector3.Distance(grabPoint, corner) < 0.5f) // �������ú��ʵ���ֵ���ж��Ƿ��ڷ�Χ��
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
