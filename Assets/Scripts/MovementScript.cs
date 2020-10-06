using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementScript : MonoBehaviour
{
    // Character speed
    public float speed = 10.0f;
    // Rotation speeds
    public float rotationSpeed = 200.0f;
    public float autoRotationSpeed = 2.0f;
    // Object to seek
    public GameObject findObject;

    // AutoMove to seek object
    private bool autoMode = false;

    // Returns cross product of 2 vectors
    Vector3 Cross(Vector3 a, Vector3 b)
    {

        Vector3 crossProd = new Vector3(0, 0, 0);

        crossProd.x = (a.y * b.z) - (a.z * b.y);
        crossProd.y = (a.z * b.x) - (a.x * b.z);
        crossProd.z = (a.x * b.y) - (a.y * b.x);

        return crossProd;
    }

    // Calculate the vector to the seek object
    float CalculateAngle()
    {

        // Vector to the seek object
        Vector3 vectorToFindObject = findObject.transform.position - this.transform.position;

        // Dot product calculation

        float dot = (this.transform.forward.x * vectorToFindObject.x) + (this.transform.forward.y * vectorToFindObject.y) + (this.transform.forward.z * vectorToFindObject.z);

        // Calculate angle between objects

        float lengthA = Mathf.Sqrt((this.transform.forward.x * this.transform.forward.x) + (this.transform.forward.y * this.transform.forward.y) + (this.transform.forward.z * this.transform.forward.z));
        float lengthB = Mathf.Sqrt((vectorToFindObject.x * vectorToFindObject.x) + (vectorToFindObject.y * vectorToFindObject.y) + (vectorToFindObject.z * vectorToFindObject.z));

        float angle = Mathf.Acos(angle = dot / (lengthA * lengthB)) * 180 / Mathf.PI;

        // Gets cross product

        Vector3 crossProd = Cross(this.transform.forward, vectorToFindObject);

        // Checks if positive

        if (crossProd.y < 0)
        {
            angle *= -1;
        }

        // Forward facing ray
        Debug.DrawRay(this.transform.position, this.transform.forward * 10.0f, Color.green, 2.0f);
        // Ray to finder object
        Debug.DrawRay(this.transform.position, vectorToFindObject, Color.red, 2.0f);

        return angle;
    }

    // Calculate the distance from current position to finder object
    float CalculateDistance()
    {
        // Calculate the distance between the objects using pythagoras

        float distance = Mathf.Sqrt(((this.transform.position.x - findObject.transform.position.x) * (this.transform.position.x - findObject.transform.position.x)) + ((this.transform.position.z - findObject.transform.position.z) * (this.transform.position.z - findObject.transform.position.z)));

        return distance;
    }

    void AutonomousMode()
    {
        // Get distance and angle
        float distance = CalculateDistance();
        float angle_to_turn = CalculateAngle();

        // Rotate towards object

        this.transform.Rotate(0, angle_to_turn * Time.deltaTime * distance / 20 * autoRotationSpeed, 0);

        // Translate locally forward in z
        this.transform.Translate(new Vector3(0, 0, speed * Time.deltaTime), Space.Self);
    }

    void Update()
    {
        // Get the horizontal and vertical axis.
        // By default they are mapped to the arrow keys.
        // The value is in the range -1 to 1
        float translation = Input.GetAxis("Vertical") * speed;
        float rotation = Input.GetAxis("Horizontal") * rotationSpeed;

        translation *= Time.deltaTime;
        rotation *= Time.deltaTime;

        // Move translation along the object's z-axis
        transform.Translate(0, 0, translation);

        // Rotate around our y-axis
        transform.Rotate(0, rotation, 0);

        // Check if autonomous
        if (autoMode == true)
        {
            AutonomousMode();
        }

        // Check for the spacebar being pressed
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // Call CalculateAngle method
            CalculateAngle();
        }

        // Flip autoMode flag
        if (Input.GetKeyDown(KeyCode.T))
        {
            autoMode = !autoMode;
        }
    }
}
