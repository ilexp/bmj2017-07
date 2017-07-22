using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Game
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private Transform followTarget;
        [SerializeField] private float smoothness = 5.0f;
        [SerializeField] private Vector3 followOffset = new Vector3(0, 5, 0);

        private void Update()
        {
            Vector3 targetPos = followTarget.position + followOffset;
            Vector3 posDiff = targetPos - transform.position;
            transform.position = Vector3.MoveTowards(
                transform.position,
                targetPos,
                posDiff.magnitude * 60.0f * Mathf.Pow(2.0f, -this.smoothness) * Time.deltaTime);
        }
    }
}
