using UnityEngine;

namespace AI.FSM {
    public abstract class GrabPointChecker : FSMTrigger {
        private Vector3[] localVertices = {
            new Vector3(-0.5f, -0.5f, -0.5f),
            new Vector3(-0.5f, -0.5f,  0.5f),
            new Vector3(-0.5f,  0.5f, -0.5f),
            new Vector3(-0.5f,  0.5f,  0.5f),
            new Vector3( 0.5f, -0.5f, -0.5f),
            new Vector3( 0.5f, -0.5f,  0.5f),
            new Vector3( 0.5f,  0.5f, -0.5f),
            new Vector3( 0.5f,  0.5f,  0.5f)
        };
        private float threshold = 0.1f;

        protected bool isGrabPointNearCorner(FSMBase fsm) {
            //  从fsm中获取已被选中的grabbedItem，访问他的GrabPoints属性，对其中第一个Point进行检测
            //       检查第一个Point是否靠近grabbedItem.transform的角落
            var gestureFSM = fsm as GestureFSM;
            Vector3 point=gestureFSM.grabbedItem.GrabPoints[0].position;
            Vector3[] worldVertices = new Vector3[8];
            for (int i = 0; i < localVertices.Length; i++)
            {
                worldVertices[i] = gestureFSM.grabbedItem.transform.TransformPoint(localVertices[i]);
            }
           
            bool isNearVertex = false;

            foreach (var vertex in worldVertices)
            {
                if (Vector3.Distance(point, vertex) < threshold)
                {
                    isNearVertex = true;
                    break;
                }
            }

            if (isNearVertex)
            {
                gestureFSM.text.text = "在cube顶点附近";
            }
            else
            {
                gestureFSM.text.text = "不在cube顶点附近";
            }


            return isNearVertex;
        }
    }
}