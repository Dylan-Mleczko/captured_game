using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RippleManager : MonoBehaviour
{
    // ripples defined by (x coor, z coor, lifetime of ripple)
    List<Vector3> ripples = new List<Vector3>();

    // set ripples to have an influence for some number of seconds
    private const int rippleLife = 5;

    // ripple inverse distance effect
    public const double rippleMultiplier = 0.5;

    void Start()
    {
        AddRipple(new Vector2(2, 2));
    }

    void Update()
    {
        // ensure old ripples disappear
        List<Vector3> remainingRipples = new List<Vector3>();
        foreach (Vector3 ripple in ripples)
        {
            if (ripple.z - _Time.y <= rippleLift) {
                remainingRipples.Add(ripple);
            }
        }
        ripples = remainingRipples;
    }

    public void AddRipple(Vector2 rippleOrigin) {
        // create the ripple at the given position using the creation time as the current time
        ripples.Add(new Vector3(rippleOrigin.x, rippleOrigin.z, _Time.y));
    }

    public List<Vector3> getRipples() {
        return ripples;
    }
}
