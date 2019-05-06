using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderDetection : MonoBehaviour {

    [SerializeField] LayerMask collidingLayerMask;

    [Header("Forward detection")]
    //Raycast en grid n x n
    [SerializeField] int gridSize = 3;
    [SerializeField] Vector2 spread;
    [SerializeField] Vector3 offset;
    [SerializeField] float forwardRaycastLength;

    [Header("Ground detection")]
    [SerializeField] float groundRaycastLength;
    [SerializeField] float groundDetectionSize = 0.5f;
    [SerializeField] Transform lowestPointTransform;

    [Header("Point detection")]
    [SerializeField] float pointDetectionHeight;

    public CollisionData collisionData { get; private set; }


    private void Start()
    {
        collisionData = new CollisionData();
    }

    void FixedUpdate ()
    {
        collisionData.Reset();
        GridRayCast(collisionData, transform.forward);
        GroundRayCast();
        GroundPositionRayCast();
    }

    public CollisionData GridRayCast(Vector3 direction)
    {
        CollisionData newCollisionData = new CollisionData();
        GridRayCast(newCollisionData, direction);

        return newCollisionData;
    }

    void GridRayCast(CollisionData collisionData,Vector3 direction)
    {
        collisionData.hasHit = false;

        for (int x = 0; x < gridSize; x++)
        {
            for (int y = 0; y < gridSize; y++)
            {
                float xx = GameMath.CenterAlign(gridSize, spread.x, x);
                float yy = y * spread.y;
                Vector3 startPosition = transform.position + transform.right * xx + transform.up * yy + transform.TransformDirection(offset);
                Ray ray = new Ray(startPosition, direction);
                RaycastHit info;
                if (Physics.Raycast(ray, out info, forwardRaycastLength, collidingLayerMask))
                {
                    Debug.DrawRay(ray.origin, ray.direction * forwardRaycastLength, Color.red);
                    collisionData.collider = info.collider;
                    collisionData.hasHit = true;
                }
                else
                {
                    Debug.DrawRay(ray.origin, ray.direction * forwardRaycastLength, Color.green);
                }
            }
        }
    }

    void GroundRayCast()
    {
        Vector3[] directions = { Vector3.back, Vector3.forward, Vector3.right, Vector3.left };
        int count = 0;
        foreach (Vector3 dir in directions)
        {
            Vector3 startPosition = lowestPointTransform.position + dir * groundDetectionSize;
            Debug.DrawRay(startPosition, Vector3.down * groundRaycastLength);
            if (Physics.Raycast(startPosition, Vector3.down, groundRaycastLength, collidingLayerMask))
                count++;
        }

        if (count > 0)
            collisionData.onGround = true;
    }

    void GroundPositionRayCast()
    {
        float maxDistance = 10;
        Ray ray = new Ray(lowestPointTransform.position, Vector3.down);
        Debug.DrawRay(ray.origin, ray.direction * maxDistance, Color.magenta);
        RaycastHit info;
        if(Physics.Raycast(ray, out info, maxDistance, collidingLayerMask))
        {
            collisionData.groundPosition = info.point;
            collisionData.distanceFromGround = (transform.position - info.point).magnitude;
        }
    }

    public PointDetectionInfo GetAjustedDestination(Vector3 startPosition, Vector3 direction)
    {
        Vector3 realStartPosition = startPosition + Vector3.up * pointDetectionHeight;
        Vector3 overDestinationPosition = realStartPosition + direction;
        Vector3 realDestination = overDestinationPosition - Vector3.up * 1.5f * pointDetectionHeight;

        Debug.DrawLine(realStartPosition, overDestinationPosition, Color.blue);
        Debug.DrawLine(overDestinationPosition, realDestination, Color.blue);

        RaycastHit hit;
        if(Physics.Raycast(overDestinationPosition, Vector3.down, out hit, 2, collidingLayerMask))
        {
            return new PointDetectionInfo(hit.point, hit.collider.transform);
        }

        return new PointDetectionInfo(startPosition + direction, null);
    }

    public struct PointDetectionInfo
    {
        public Vector3 point;
        public Transform contactTransform;
        public bool hasHit;

        public PointDetectionInfo(Vector3 point, Transform contactTransform)
        {
            this.point = point;
            this.contactTransform = contactTransform;
            hasHit = contactTransform != null;
        }
    }

    [System.Serializable]
    public class CollisionData
    {
        public bool hasHit;
        public Collider collider;
        public bool onGround;
        public Vector3 groundPosition;
        public float distanceFromGround;

        public void Reset()
        {
            collider = null;
            hasHit = false;
            onGround = false;
        }
    }
}
