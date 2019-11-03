using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaBounds : MonoBehaviour
{
    // Store data about the boundaries of the arena
    public float boundsRadius;
    public Vector3 PointA;
    public Vector3 PointB;

    public GameObject ObjA;
    public GameObject ObjB;

    public Vector3 center = Vector3.zero;
    public Vector3 quad0 = Vector3.zero;
    public Vector3 quad1 = Vector3.zero;
    public Vector3 quad2 = Vector3.zero;
    public Vector3 quad3 = Vector3.zero;

    public bool debug = false;

    /* Coordinate grid used for calculations
                     Z
                     ^
                     |
                1    |    0
                     |
           -X<-------C------->X  
                     |
                2    |    3
                     |
                     v
                    -Z
         
         
         */

    // Start is called before the first frame update
    void Start()
    {
        PointA = ObjA.transform.position;
        PointB = ObjB.transform.position;
        center = (PointB + PointA) / 2;

        Vector3 quadSize = (PointA - PointB) / 4;

        // Set quadrant centers
        for(int oppQuad = 0; oppQuad < 4; oppQuad++)
        {
            Vector3 quadCenter = center;
            if (oppQuad == 0) // 2
            {
                quadCenter.x -= quadSize.x;
                quadCenter.z -= quadSize.z;
                quad2 = quadCenter;
            }
            else if (oppQuad == 1) // 3
            {
                quadCenter.x += quadSize.x;
                quadCenter.z -= quadSize.z;
                quad3 = quadCenter;
            }
            else if (oppQuad == 2) // 0
            {
                quadCenter.x += quadSize.x;
                quadCenter.z += quadSize.z;
                quad0 = quadCenter;
            }
            else if (oppQuad == 3) // 1
            {
                quadCenter.x -= quadSize.x;
                quadCenter.z += quadSize.z;
                quad1 = quadCenter;
            }
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Checks if a given position is outside the spherical bounds of the arena (useful for deleting objects)
    /// </summary>
    /// <param name="pos">Location to check</param>
    /// <returns>A bool representing the collision</returns>
    public bool CheckBoundsSphere(Vector3 pos)
    {
        return (pos - center).magnitude > boundsRadius;
    }

    void OnDrawGizmos()
    {
        if (!debug) return;

        Vector3 size = PointA - PointB;
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(center, size);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(center, boundsRadius);

        Gizmos.color = Color.magenta;
        Gizmos.DrawSphere(quad0, 1);
        Gizmos.DrawSphere(quad1, 1);
        Gizmos.DrawSphere(quad2, 1);
        Gizmos.DrawSphere(quad3, 1);
    }
}
