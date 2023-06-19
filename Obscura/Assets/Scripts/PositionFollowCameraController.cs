using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Obscura
{
    public class PositionFollowCameraController : AbstractCameraController
    {
        [SerializeField] public float followSpeedFactor;
        [SerializeField] public float leashDistance;
        [SerializeField] public float catchUpSpeed;
        private Camera managedCamera;
        private LineRenderer cameraLineRenderer;
        Vector3 targetVelocity;
        Vector3 lastTargetPosition;
        // Start is called before the first frame update
        private void Awake()
        {
            managedCamera = gameObject.GetComponent<Camera>();
            cameraLineRenderer = gameObject.GetComponent<LineRenderer>();
        }
        private void Start()
        {
            managedCamera.transform.position = new Vector3(Target.transform.position.x, Target.transform.position.y, managedCamera.transform.position.z);
            lastTargetPosition = Target.transform.position;
        }
        void LateUpdate()
        {
            var targetPosition = this.Target.transform.position;
            var cameraPosition = managedCamera.transform.position;
            targetVelocity = (targetPosition - lastTargetPosition) / Time.deltaTime;
            float distanceToTarget = Vector2.Distance(targetPosition, cameraPosition);
            float cameraSpeed;
            Vector3 newtargetPosition = new Vector3(targetPosition.x, targetPosition.y, cameraPosition.z);

            if (targetVelocity.magnitude != 0)
            {
                if (distanceToTarget > leashDistance)
                {
                    cameraSpeed = targetVelocity.magnitude;
                    cameraPosition = Vector3.Lerp(cameraPosition, newtargetPosition, cameraSpeed * Time.deltaTime);
                }
                else
                {
                    cameraSpeed = targetVelocity.magnitude * followSpeedFactor;
                    cameraPosition = Vector3.Lerp(cameraPosition, newtargetPosition, cameraSpeed * Time.deltaTime);
                }
            }
            else
            {
                cameraSpeed = catchUpSpeed;
                cameraPosition = Vector3.Lerp(cameraPosition, newtargetPosition, cameraSpeed * Time.deltaTime);
            }
            
            managedCamera.transform.position = cameraPosition;
            lastTargetPosition = targetPosition;

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
