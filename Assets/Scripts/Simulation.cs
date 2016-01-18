using UnityEngine;
using System.Collections;

public class Simulation : MonoBehaviour
{
    public float densityOfAir = 1.2922f;
    public int runSpeed;

    public Vector2 projectileVelocity;
    public float projectileAcceleration;
    public float force;

    public bool simulationRunning;

    public Collider projectile;
    public Collider target;

    public Vector3 projectileStartPosition;
    public Vector3 projectilePosition;
    public Vector3 targetPosition;

    Rigidbody rb;
    float mass;
    PhysicsObject po;
    // simulation needs
    // fire a projectile
    // instantiate the projectile and the target at a given distance to each other
    // calculate the path of the projectile with a given mass and speed
    // calculate the point of impact and how the material will deform
    // play the simulation

    // Use this for initialization
    void Start()
    {
        rb = projectile.GetComponent<Rigidbody>();
        po = projectile.GetComponent<PhysicsObject>();
        projectileStartPosition = projectile.transform.position;
        targetPosition = target.transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Physics.gravity = Physics.gravity / runSpeed;
        projectilePosition = projectile.transform.position;
        projectileVelocity = po.velocity;
        if (Input.GetKey("space"))
        {
            simulationRunning = true;
        }
        if(simulationRunning)
        {
            rb.AddForce(Vector3.right * force * (Time.fixedDeltaTime / runSpeed), ForceMode.Impulse);
            //projectilePosition.x += projectileVelocity * (Time.deltaTime/100);
            //projectile.transform.position = projectilePosition;
        } 
    }

    void OnCollisionEnter(Collision col)
    {
        //Debug.Log("collided");
        if (col.gameObject.name == "Cube 1")
            Debug.Log("collidied");
    }
}
