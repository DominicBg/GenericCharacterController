using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PhysicsBody : MonoBehaviour
{
    [SerializeField] float gravityModifier;

    float currentGravityModifier;
    Rigidbody rigidBody;

    Coroutine resetGravityCoroutine;

    void Start ()
    {
        rigidBody = GetComponent<Rigidbody>();
        rigidBody.useGravity = false;
        currentGravityModifier = gravityModifier;
    }
	
    public void ImpulseVelocity(Vector3 velocity)
    {
        rigidBody.AddForce(velocity, ForceMode.VelocityChange);
    }

    void FixedUpdate ()
    {
        rigidBody.AddForce(Physics.gravity * currentGravityModifier, ForceMode.Acceleration);
	}

    public void Jump(Vector3 velocity)
    {
        rigidBody.AddForce(velocity, ForceMode.Impulse);
    }

    public void ResetGravity(float min, float duration)
    {
        if (resetGravityCoroutine != null)
            StopCoroutine(resetGravityCoroutine);

        resetGravityCoroutine = StartCoroutine(ResetGravityCoroutine(min, gravityModifier, duration));
    }

    public void ResetGravity()
    {
        if (currentGravityModifier == gravityModifier)
            return;

        if (resetGravityCoroutine != null)
            StopCoroutine(resetGravityCoroutine);

        currentGravityModifier = gravityModifier;
    }

    //Mettre Dotween
    IEnumerator ResetGravityCoroutine(float min, float max, float duration)
    {
        float t = 0;
        float speed = 1 / duration;
        while(t < 1)
        {
            t += Time.deltaTime * speed;
            currentGravityModifier = Mathf.Lerp(min, max, t);
            yield return null;
        }
    }
}
