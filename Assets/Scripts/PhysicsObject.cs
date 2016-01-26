using UnityEngine;
using System.Collections;

public class PhysicsObject : MonoBehaviour
{
	public float massInGrains = 180;
	public Vector2 muzzleVelocity = new Vector2(1000,0);// initial velocity in ft/s
	public float muzzleEnergy; // ft-lbs
	public Vector2 velocityOnImpact; // ft/s
	public float density;
	public float varyingBallisticDragCoefficient;
	public float radius = 0.005f; // m
	public float initialKineticEnergy;
	public float kineticEnergy;

	public float coefficientOfRestitution;

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

	public bool collided;

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
		initialKineticEnergy = muzzleEnergy;
		muzzleEnergy = muzzleEnergy / 1.356f;
		
		//varyingBallisticDragCoefficient = 0.170f;
	}
	
	// Update is called once per frame
	void FixedUpdate()
	{
		if (velocityFtPerSecond.x >= 1150.0f)
		{
			varyingBallisticDragCoefficient = 0.140f;
		}
		else if (velocityFtPerSecond.x >= 1000.0f && velocityFtPerSecond.x < 1150.0f)
		{
			varyingBallisticDragCoefficient = 0.160f;
		}
		else if (velocityFtPerSecond.x >= 850.0f && velocityFtPerSecond.x < 1000.0f)
		{
			varyingBallisticDragCoefficient = 0.170f;
		}
		else if (velocityFtPerSecond.x <= 850.0f)
		{
			varyingBallisticDragCoefficient = 0.120f;
		}

		if (s.simulationRunning && !collided)
		{
			velocity += acceleration * Time.fixedDeltaTime / s.runSpeed;
			velocityFtPerSecond = velocity / 0.3048f;

			velocitySquared.x = velocity.x * velocity.x;
			velocitySquared.y = velocity.y * velocity.y;

			netVelocity.x = Mathf.Sqrt(velocitySquared.x);
			netVelocity.y = Mathf.Sqrt(velocitySquared.y);

			drag = s.densityOfAir * varyingBallisticDragCoefficient * Mathf.PI * radius * radius;
			drag = drag / 2;

			acceleration.x = (-drag / massInKg) * netVelocity.x * velocity.x;
			acceleration.y = (-drag / massInKg) * netVelocity.y * velocity.y;

			force = acceleration * massInKg;

			kineticEnergy = 0.5f * massInKg * (velocity.x * velocity.x);

			rb.AddForce(force * (Time.fixedDeltaTime / s.runSpeed));
		}

		if(collided)
		{
			velocity.x = (((coefficientOfRestitution * s.target.GetComponent<PhysicsObject>().massInKg) - velocity.x) + (massInKg * velocity.x)) / massInKg + s.target.GetComponent<PhysicsObject>().massInKg;
		}
	}

	void OnCollisionEnter(Collision other)
	{
		if (other.gameObject.tag == "Target")
			collided = true;
	}
}
