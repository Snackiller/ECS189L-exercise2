using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Obscura
{
    public class PositionLockCameraController : AbstractCameraController
    {
        private Camera managedCamera;
        private LineRenderer cameraLineRenderer;
        // Start is called before the first frame update
        private void Awake()
        {
            managedCamera = gameObject.GetComponent<Camera>();
            cameraLineRenderer = gameObject.GetComponent<LineRenderer>();
        }
        void LateUpdate()
        {
            var targetPosition = this.Target.transform.position;
            var cameraPosition = managedCamera.transform.position;
            cameraPosition = new Vector3(targetPosition.x, targetPosition.y, cameraPosition.z);
            managedCamera.transform.position = cameraPosition;
            if (this.DrawLogic)
            {
                cameraLineRenderer.enabled = true;
                DrawCameraLogic();
            }
            else
            {
                cameraLineRenderer.enabled = false;
            }
        }

        // Update is called once per frame
        public override void DrawCameraLogic()
        {
            var z = this.Target.transform.position.z - this.managedCamera.transform.position.z;

            cameraLineRenderer.positionCount = 7;
            cameraLineRenderer.useWorldSpace = false;
            cameraLineRenderer.SetPosition(0, new Vector3(0, 0, z));
            cameraLineRenderer.SetPosition(1, new Vector3(cameraLineRenderer.positionCount, 0, z));
            cameraLineRenderer.SetPosition(2, new Vector3(-cameraLineRenderer.positionCount, 0, z));
            cameraLineRenderer.SetPosition(3, new Vector3(0, 0, z));
            cameraLineRenderer.SetPosition(4, new Vector3(0, cameraLineRenderer.positionCount, z));
            cameraLineRenderer.SetPosition(5, new Vector3(0, -cameraLineRenderer.positionCount, z));
            cameraLineRenderer.SetPosition(6, new Vector3(0, 0, z));
        }
    }
}
