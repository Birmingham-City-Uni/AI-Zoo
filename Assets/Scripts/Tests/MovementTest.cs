using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class MovementTest
    {
        // Checks turn angle between 2 locations when angle is positive
        [UnityTest]
        public IEnumerator PositiveAngleCalculation()
        {
            GameObject objectOne = new GameObject();
            objectOne.AddComponent<MovementScript>();

            MovementScript moveScript = objectOne.GetComponent<MovementScript>();

            objectOne.transform.position = Vector3.zero;

            Vector3 targetPosition = new Vector3(1, 0, 1);

            float angle = moveScript.CalculateAngle(objectOne, targetPosition);

            Assert.AreEqual(angle, 45.0f);

            yield return null;
        }

        // Checks turn angle between 2 locations when angle is negative
        [UnityTest]
        public IEnumerator NegativeAngleCalculation()
        {
            GameObject objectOne = new GameObject();
            objectOne.AddComponent<MovementScript>();

            MovementScript moveScript = objectOne.GetComponent<MovementScript>();

            objectOne.transform.position = Vector3.zero;

            Vector3 targetPosition = new Vector3(-1, 0, 1);

            float angle = moveScript.CalculateAngle(objectOne, targetPosition);

            Assert.AreEqual(angle, -45.0f);

            yield return null;
        }

        // Checks turn distance between 2 locations when distance is positive
        [UnityTest]
        public IEnumerator PositiveDistanceTest()
        {
            GameObject objectOne = new GameObject();
            objectOne.AddComponent<MovementScript>();

            MovementScript moveScript = objectOne.GetComponent<MovementScript>();

            objectOne.transform.position = Vector3.zero;

            Vector3 targetPosition = new Vector3(10, 0, 0);

            float distance = moveScript.CalculateDistance(objectOne, targetPosition);

            Assert.AreEqual(distance, 10.0f);

            yield return null;
        }

        // Checks turn distance between 2 locations when distance is negative
        [UnityTest]
        public IEnumerator NegativeDistanceTest()
        {
            GameObject objectOne = new GameObject();
            objectOne.AddComponent<MovementScript>();

            MovementScript moveScript = objectOne.GetComponent<MovementScript>();

            objectOne.transform.position = Vector3.zero;

            Vector3 targetPosition = new Vector3(0, 0, -10);

            float distance = moveScript.CalculateDistance(objectOne, targetPosition);

            Assert.AreEqual(distance, 10.0f);

            yield return null;
        }
    }
}
