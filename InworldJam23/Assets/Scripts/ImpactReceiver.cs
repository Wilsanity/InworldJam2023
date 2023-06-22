using UnityEngine;

using System.Collections;

public class ImpactReceiver : MonoBehaviour
{
    public float mass = 3.0F; // defines the character mass
    Vector3 impact = Vector3.zero;
    private CharacterController character;
    public bool impacting;
    public bool gravity = false;
    float gravityStrength = 0;

    // Use this for initialization
    void Start()
    {
        character = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        // apply the impact force:
        Vector3 movement = gravity ? impact + (Vector3.down * 9.81f) : impact;

        if (impact.magnitude > 0.2F){ character.Move(movement * Time.deltaTime); impacting = true; }
        else impacting = false;

        // consumes the impact energy each cycle:
        impact = Vector3.Lerp(impact, Vector3.zero, 5 * Time.deltaTime);
        gravityStrength += 9.81f * Time.deltaTime;
    }
    // call this function to add an impact force:
    public void AddImpact(Vector3 dir, float force)
    {
        gravityStrength = 0;

        dir.Normalize();
        if (dir.y < 0) dir.y = -dir.y; // reflect down force on the ground
        impact += dir.normalized * force / mass;
    }
}