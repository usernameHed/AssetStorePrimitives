﻿using hedCommon.time;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace hedCommon.extension.runtime
{
    public static class ExtRotation
    {
        /// <summary>
        /// Rotate a Point around an axis
        /// usage:
        /// Vector3 position = ExtRotation.RotatePointAroundAxis(pivotPosition, pointToRotate, pivotUp, _rotateAxis * TimeEditor.deltaTime);
        /// 
        /// for a given A, R, U, rotation(x, y, z):
        /// 
        /// 
        ///   AR: vector director  
        ///   A: anchor            
        ///   U: up (AU)                                 [Pic 1]                        [Pic 2]
        ///   R: to rotate                              vector director
        ///   R': R projected                   U              ⭣                   U                   
        ///   R'': R' rotated                   |              ⭣                   |
        ///   Rfinal: R rotated                 |           ⭢⭢⭢⭢R                |              ⭢⭢⭢R
        ///                                     |       ⭧                          |              ⭡    
        ///                                     A⭢⭢⭢⭢⭢⭢⭢⭢⭢⭢⭢R'              A⭢⭢⭢⭢⭢⭢⭢⭢⭢⭢⭢R'
        ///                                    /           ⭡                      /  ⭨   Rf           ⭣
        ///                                   /        projectedAR               /     ⭨ |          ⭩      rotation  (0x, 45y, 0z)
        ///                                  /                                  /        R''     ⭠
        /// </summary>
        /// <param name="pivotPoint"></param>
        /// <param name="pointToRotate"></param>
        /// <param name="upNormalized"></param>
        /// <param name="rotationAxis"></param>
        /// <returns></returns>
        public static Vector3 RotatePointAroundAxis(Vector3 pivotPoint, Vector3 pointToRotate, Vector3 upNormalized, Vector3 rotationAxis)
        {
            Vector3 vectorDirector = pointToRotate - pivotPoint;                                       //get the vector director
            Vector3 finalPoint = RotateWithMatrix(pivotPoint, vectorDirector, upNormalized, rotationAxis);
            return (finalPoint);
        }

        /// <summary>
        /// Rotate a vectorDirector around an axis
        /// usage:
        /// Vector3 vectorDirector = ExtRotation.RotateVectorAroundAxis(pivotPoint, vectorDirector, pivotUp, _rotateAxis * TimeEditor.deltaTime);
        /// </summary>
        public static Vector3 RotateVectorAroundAxis(Vector3 pivotPoint, Vector3 vectorDirector, Vector3 upNormalized, Vector3 rotationAxis)
        {
            Vector3 finalPoint = RotateWithMatrix(pivotPoint, vectorDirector, upNormalized, rotationAxis);
            return (finalPoint - pivotPoint);
        }

        private static Vector3 RotateWithMatrix(Vector3 pivotPoint, Vector3 vectorDirector, Vector3 upNormalized, Vector3 rotationAxis)
        {
            Quaternion constrainRotation = TurretLookRotation(vectorDirector, upNormalized);            //constrain rotation from up !!
            //create a TRS matrix from point & rotation
            Matrix4x4 rotationMatrix = Matrix4x4.TRS(pivotPoint, constrainRotation, Vector3.one);

            Vector3 projectedForward = Vector3.Project(vectorDirector, rotationMatrix.ForwardFast());
            Vector3 projectedUp = Vector3.Project(vectorDirector, upNormalized);
            float distanceForward = projectedForward.magnitude;
            float distanceUp = projectedUp.magnitude;

            if (Vector3.Dot(upNormalized, vectorDirector) < 0)
            {
                distanceUp *= -1;
            }

            //display [Pic 1] axis
            //rotate matrix in x, y & z
            rotationMatrix = Matrix4x4.TRS(pivotPoint, constrainRotation * Quaternion.Euler(rotationAxis), Vector3.one);
            Vector3 finalPoint = rotationMatrix.MultiplyPoint3x4(new Vector3(0, distanceUp, distanceForward));
            return finalPoint;
        }

        public static Quaternion RotateVectorDirectorFromAxis(Vector3 vectorDirector, Vector3 upNormalized, Vector3 rotationAxis)
        {
            Quaternion constrainRotation = TurretLookRotation(vectorDirector, upNormalized);            //constrain rotation from up !!
            return (RotateQuaternonFromAxis(constrainRotation, rotationAxis));
        }

        public static Quaternion RotateQuaternonFromAxis(Quaternion rotation, Vector3 rotationAxis)
        {
            return (rotation * Quaternion.Euler(rotationAxis));
        }

        /// <summary>
        /// From a Line, Get the quaternion representing the Vector p2 - p1
        /// the up vector can be Vector3.up if you have no reference.
        /// </summary>
        /// <param name="p1">point 1</param>
        /// <param name="p2">point 2</param>
        /// <param name="upNormalized">default is Vector3.up</param>
        /// <returns>Quaternion representing the rotation of p2 - p1</returns>
        public static Quaternion QuaternionFromLine(Vector3 p1, Vector3 p2, Vector3 upNormalized)
        {
            Matrix4x4 rotationMatrix = ExtMatrix.LookAt(p1, p2, upNormalized);
            Quaternion rotation = rotationMatrix.ExtractRotation();
            return (rotation);
        }

        /// <summary>
        /// From a Vector director, Get the quaternion representing this vector
        /// the up vector can be Vector3.up if you have no reference.
        /// </summary>
        /// <param name="vectorDirector"></param>
        /// <param name="upNormalized">default is Vector3.up</param>
        /// <returns>Quaternion representing the rotation of p2 - p1</returns>
        public static Quaternion QuaternionFromVectorDirector(Vector3 vectorDirector, Vector3 upNormalized)
        {
            Matrix4x4 rotationMatrix = ExtMatrix.LookAt(vectorDirector, vectorDirector * 2, upNormalized);
            Quaternion rotation = rotationMatrix.ExtractRotation();
            return (rotation);
        }

        /// <summary>
        /// rotate a given quaternion in x, y and z
        /// </summary>
        /// <param name="currentQuaternion">current quaternion to rotate</param>
        /// <param name="axis">axis of rotation in degree</param>
        /// <returns>new rotated quaternion</returns>
        public static Quaternion RotateQuaternion(Quaternion currentQuaternion, Vector3 axis)
        {
            return (currentQuaternion * Quaternion.Euler(axis));
        }

        /// <summary>
        /// Turret lock rotation
        /// https://gamedev.stackexchange.com/questions/167389/unity-smooth-local-rotation-around-one-axis-oriented-toward-a-target/167395#167395
        /// 
        /// Vector3 relativeDirection = mainReferenceObjectDirection.right * dirInput.x + mainReferenceObjectDirection.forward * dirInput.y;
        /// Vector3 up = objectToRotate.up;
        /// Quaternion desiredOrientation = TurretLookRotation(relativeDirection, up);
        ///objectToRotate.rotation = Quaternion.RotateTowards(
        ///                         objectToRotate.rotation,
        ///                         desiredOrientation,
        ///                         turnRate* Time.deltaTime
        ///                        );
        /// </summary>
        public static Quaternion TurretLookRotation(Vector3 approximateForward, Vector3 exactUp)
        {
            Quaternion rotateZToUp = Quaternion.LookRotation(exactUp, -approximateForward);
            Quaternion rotateYToZ = Quaternion.Euler(90f, 0f, 0f);

            return rotateZToUp * rotateYToZ;
        }
        public static Quaternion TurretLookRotation(Quaternion approximateForward, Vector3 exactUp)
        {
            Vector3 forwardQuaternion = approximateForward * Vector3.forward;

            Quaternion rotateZToUp = Quaternion.LookRotation(exactUp, -forwardQuaternion);
            Quaternion rotateYToZ = Quaternion.Euler(90f, 0f, 0f);

            return rotateZToUp * rotateYToZ;
        }
        public static Vector3 TurretLookRotationVector(Vector3 approximateForward, Vector3 exactUp)
        {
            Quaternion rotateZToUp = Quaternion.LookRotation(exactUp, -approximateForward);
            Quaternion rotateYToZ = Quaternion.Euler(90f, 0f, 0f);

            return (rotateZToUp * rotateYToZ) * Vector3.forward;
        }

        public static Quaternion TurretLookRotation2D(Vector3 approximateForward, Vector3 exactUp)
        {
            Quaternion rotateZToUp = Quaternion.LookRotation(exactUp, approximateForward);
            Quaternion rotateYToZ = Quaternion.Euler(0, 0f, 0f);

            return rotateZToUp * rotateYToZ;
        }
        public static Vector3 TurretLookRotation2DVector(Vector3 approximateForward, Vector3 exactUp)
        {
            Quaternion rotateZToUp = Quaternion.LookRotation(exactUp, approximateForward);
            Quaternion rotateYToZ = Quaternion.Euler(0, 0f, 0f);

            return (rotateZToUp * rotateYToZ) * Vector3.forward;
        }

        public static Quaternion SmoothTurretLookRotation(Vector3 approximateForward, Vector3 exactUp,
            Quaternion objCurrentRotation, float maxDegreesPerSecond)
        {
            Quaternion desiredOrientation = TurretLookRotation(approximateForward, exactUp);
            Quaternion smoothOrientation = Quaternion.RotateTowards(
                                        objCurrentRotation,
                                        desiredOrientation,
                                        maxDegreesPerSecond * Time.deltaTime
                                     );
            return (smoothOrientation);
        }

        /// <summary>
        /// smooth look rotation with clamped angle
        /// </summary>
        /// <param name="referenceRotation">reference rotation up, forward, right</param>
        /// <param name="vectorDirectorToTarget">vectorDirector where we want to target</param>
        /// <param name="left">angle in degree: clamp left from Up Axis</param>
        /// <param name="right">angle in degree: clamp right from Up axis</param>
        /// <param name="up">angle in degree: clamp Up axis</param>
        /// <param name="down">angle in degree: clamp Down axis</param>
        /// <param name="currentRotation">Current rotation of the object we want to smoothly rotate</param>
        /// <param name="maxDegreesPerSecond">speed of rotation in degree per seconds</param>
        /// <returns>Smooth clamped rotation</returns>
        public static Quaternion SmoothTurretLookRotationWithClampedAxis(
            Quaternion referenceRotation,
            Vector3 vectorDirectorToTarget,
            float left,
            float right,
            float up,
            float down,
            Quaternion currentRotation,
            float maxDegreesPerSecond)
        {
            Quaternion finalHeadRotation = ExtRotation.TurrentLookRotationWithClampedAxis(referenceRotation, vectorDirectorToTarget, left, right, up, down);
            Quaternion smoothOrientation = Quaternion.RotateTowards(currentRotation,
                finalHeadRotation,
                maxDegreesPerSecond * TimeEditor.fixedDeltaTime);

            return (smoothOrientation);
        }

        /// <summary>
        /// turret look rotation with clamped angles
        /// </summary>
        /// <param name="referenceRotation">reference rotation up, forward, right</param>
        /// <param name="vectorDirectorToTarget">vectorDirector where we want to target</param>
        /// <param name="left">angle in degree: clamp left from Up Axis</param>
        /// <param name="right">angle in degree: clamp right from Up axis</param>
        /// <param name="up">angle in degree: clamp Up axis</param>
        /// <param name="down">angle in degree: clamp Down axis</param>
        /// <returns>Clamped rotation</returns>
        public static Quaternion TurrentLookRotationWithClampedAxis(
            Quaternion referenceRotation,
            Vector3 vectorDirectorToTarget,
            float left,
            float right,
            float up,
            float down)
        {
            Vector3 originalForward = referenceRotation * Vector3.forward;

            Vector3 yAxis = Vector3.up; // world y axis
            Vector3 dirXZ = Vector3.ProjectOnPlane(vectorDirectorToTarget, yAxis);
            Vector3 forwardXZ = Vector3.ProjectOnPlane(originalForward, yAxis);
            float yAngle = Vector3.Angle(dirXZ, forwardXZ) * Mathf.Sign(Vector3.Dot(yAxis, Vector3.Cross(forwardXZ, dirXZ)));
            float yClamped = Mathf.Clamp(yAngle, left, right);
            Quaternion yRotation = Quaternion.AngleAxis(yClamped, Vector3.up);

            originalForward = yRotation * referenceRotation * Vector3.forward;
            Vector3 xAxis = yRotation * referenceRotation * Vector3.right; // our local x axis
            Vector3 dirYZ = Vector3.ProjectOnPlane(vectorDirectorToTarget, xAxis);
            Vector3 forwardYZ = Vector3.ProjectOnPlane(originalForward, xAxis);
            float xAngle = Vector3.Angle(dirYZ, forwardYZ) * Mathf.Sign(Vector3.Dot(xAxis, Vector3.Cross(forwardYZ, dirYZ)));
            float xClamped = Mathf.Clamp(xAngle, -up, -down);
            Quaternion xRotation = Quaternion.AngleAxis(xClamped, Vector3.right);


            Quaternion newRotation = yRotation * referenceRotation * xRotation;
            return (newRotation);
        }
    }
}