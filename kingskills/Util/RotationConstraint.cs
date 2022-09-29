using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace kingskills
{

    //////////////////////////////////////////////////////////////
    // RotationConstraint
    //
    // RotationConstraint constrains the relative rotation of a
    // Transform. You select the constraint axis in the editor and
    // specify a min and max amount of rotation that is allowed
    // from the default rotation
    //
    // This code was borrowed from a standard unity asset script
    //////////////////////////////////////////////////////////////
    
    enum ConstraintAxis
    {
        X = 0,
        Y,
        Z
    }

    class KS_RotationConstraint : MonoBehaviour
    {
        public ConstraintAxis axis; // Rotation around this axis is constrained
        public float min;           // Relative value in degrees
        public float max;           // Relative value in degrees
        public bool active = true;
        private Transform thisTransform;
        private Vector3 rotateAround;
        private Quaternion minQuaternion;
        private Quaternion maxQuaternion;
        private float range;

        public void Start()
        {
            thisTransform = transform;

            // Set the axis that we will rotate around
            switch (axis)
            {
                case ConstraintAxis.X:
                    rotateAround = Vector3.right;
                    break;

                case ConstraintAxis.Y:
                    rotateAround = Vector3.up;
                    break;

                case ConstraintAxis.Z:
                    rotateAround = Vector3.forward;
                    break;
            }

            // Set the min and max rotations in quaternion space
            Quaternion axisRotation = Quaternion.AngleAxis(thisTransform.localRotation.eulerAngles[(int)axis], rotateAround);
            minQuaternion = axisRotation * Quaternion.AngleAxis(min, rotateAround);
            maxQuaternion = axisRotation * Quaternion.AngleAxis(max, rotateAround);
            range = max - min;
        }
        // We use LateUpdate to grab the rotation from the Transform after all Updates from
        // other scripts have occured
        public void LateUpdate()
        {
            if (!active) return;

            int axisI = (int)axis;
            // We use quaternions here, so we don't have to adjust for euler angle range [ 0, 360 ]
            Quaternion localRotation = thisTransform.localRotation;
            Quaternion axisRotation = Quaternion.AngleAxis(localRotation.eulerAngles[axisI], rotateAround);
            float angleFromMin = Quaternion.Angle(axisRotation, minQuaternion);
            float angleFromMax = Quaternion.Angle(axisRotation, maxQuaternion);

            if (angleFromMin <= range || angleFromMax <= range )
                return; // within range
    
            else
            {
                // Let's keep the current rotations around other axes and only
                // correct the axis that has fallen out of range.
                Vector3 euler = localRotation.eulerAngles;
                if (angleFromMin > angleFromMax)
                    euler[axisI] = maxQuaternion.eulerAngles[axisI];
                else
                    euler[axisI] = minQuaternion.eulerAngles[axisI];

                thisTransform.localEulerAngles = euler;
            }
        }
    }
}
