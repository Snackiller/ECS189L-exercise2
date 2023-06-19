using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Obscura
{
    public class TargetFocusCameraController : AbstractCameraController
    {
        [SerializeField] public float idleDuration;
        [SerializeField] public float returnSpeed;
        [SerializeField] public float leadSpeedMultiplier;
        [SerializeField] public float leadMaxDistance;
        private Camera managedCamera;
        private LineRenderer cameraLineRenderer;
        private float time;
        private Vector3 targetVelocity;
        private Vector3 lasttargetPosition;
        // Start is called before the first frame update
        private void Awake()
        {
            managedCamera = gameObject.GetComponent<Camera>();
            cameraLineRenderer = gameObject.GetComponent<LineRenderer>();
        }
        private void Start()
        {
            managedCamera.transform.position = new Vector3(Target.transform.position.x, Target.transform.position.y, managedCamera.transform.position.z);
            lasttargetPosition = Target.transform.position;
        }
        void LateUpdate()
        {
            var targetPosition = this.Target.transform.position;
            var cameraPosition = managedCamera.transform.position;
            Vector3 direction = cameraPosition - targetPosition;
            targetVelocity = (targetPosition - lasttargetPosition) / Time.deltaTime;
            Vector3 cameraDirection = targetVelocity.normalized;
            float distanceToTarget = Vector2.Distance(targetPosition, cameraPosition);
            Vector3 newtargetPosition = new Vector3(targetPosition.x+cameraDirection.x, targetPosition.y+cameraDirection.y, cameraPosition.z);
            float cameraSpeed=0;
            if (targetVelocity.magnitude != 0)
            {
                time = 0;
                if (distanceToTarget > leadMaxDistance)
                {
                    cameraSpeed = targetVelocity.magnitude;
                    cameraPosition = Vector3.Lerp(cameraPosition, newtargetPosition, cameraSpeed * Time.deltaTime);
                }
                else
                {
                    cameraSpeed = leadSpeedMultiplier * targetVelocity.magnitude;
                    cameraPosition = Vector3.Lerp(cameraPosition, newtargetPosition, cameraSpeed * Time.deltaTime);
                }
            }
            else
            {
                time += Time.deltaTime;
                if (time > idleDuration)
                {
                    cameraSpeed = returnSpeed;
                    cameraPosition = Vector3.Lerp(cameraPosition, newtargetPosition, cameraSpeed * Time.deltaTime);
                }
            }
            managedCamera.transform.position = cameraPosition;
            lasttargetPosition = targetPosition;
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
