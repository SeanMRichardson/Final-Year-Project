using UnityEngine;
using System.Collections;

public class PhysicsObject : MonoBehaviour
{
    public bool useGravity = true;
    public float massInGrains = 180;
    public Vector2 muzzleVelocity = new Vector2(1000,0);// initial velocity in ft/s
    public float muzzleEnergy; // ft-lbs
    public Vector2 velocityOnImpact; // ft/s
    public float density;
    public float varyingBallisticDragCoefficient;
    public float radius = 0.005f; // m
    public float initialKineticEnergy;
    public float finalKineticEnergy;

    public float drag;

    Rigidbody rb;
    Simulation s;

    public float massInKg; // kg
    public Vector2 velocityFtPerSecond;
    public Vector2 velocity; // m/s
    public Vector2 netVelocity;
    public Vector2 velocitySquared;

    public Vector2 force;
    public Vector2 acceleration;

	// Use this for initialization
	void Start ()
    {
        rb = GetComponent<Rigidbody>();
        s = FindObjectOfType<Simulation>();

        massInKg = massInGrains * 0.00006479891f;
        rb.mass = massInKg;
        velocityFtPerSecond = muzzleVelocity;
        velocity = velocityFtPerSecond * 0.3048f;
        muzzleEnergy = 0.5f * massInKg * (velocity.x * velocity.x);
        muzzleEnergy = muzzleEnergy / 1.356f;
        varyingBallisticDragCoefficient = 0.170f;
	}
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        //if (velocity != muzzleVelocity)
        //{
        //velocities.x = transform.position.x / Time.fixedDeltaTime;
        //velocities.y = transform.position.y / Time.fixedDeltaTime;
        //velocitySquared = velocities.x * velocities.x + velocities.y * velocities.y;
        //velocity = Mathf.Sqrt(velocitySquared);
        //velocityFtPerSecond = velocity / 0.3048f;
        //}
        velocity += acceleration * Time.fixedDeltaTime;
        velocitySquared.x = velocity.x * velocity.x;
        velocitySquared.y = velocity.y * velocity.y;

        netVelocity.x = Mathf.Sqrt(velocitySquared.x);
        netVelocity.y = Mathf.Sqrt(velocitySquared.y);

        drag = s.densityOfAir * varyingBallisticDragCoefficient * Mathf.PI * radius * radius;
        drag = drag / 2;

        acceleration.x = (-drag / massInKg) * netVelocity.x * velocity.x;
        acceleration.y = (-drag / massInKg) * netVelocity.y * velocity.y;

        force = acceleration * massInKg;
        rb.AddForce(force);
    }
}
