using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RippleManager : MonoBehaviour
{
    // ripples defined by (x coor, z coor, lifetime of ripple)
    private ArrayList<Vector3> ripples = new ArrayList<Vector2>();

    // set ripples to have an influence for some number of seconds
    private const int rippleLife = 5;

    // ripple inverse distance effect
    public const int rippleMultiplier = 0.5;

    void Start()
    {
        AddRipple(new Vector2(2, 2));
    }

    void Update()
    {
        // ensure old ripples disappear
        ripples = ripples.Where(x => x.z - _Time.y <= rippleLife).ToList();
    }

    public void AddRipple(Vector2 rippleOrigin) {
        // create the ripple at the given position using the creation time as the current time
        ripples.Add(new Vector3(rippleOrigin.x, rippleOrigin.z, _Time.y));
    }

    public Vector3 getRipples() {
        return ripples;
    }
}
