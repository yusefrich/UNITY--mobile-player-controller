using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent (typeof(BoxCollider2D))]
public class PlayerController : MonoBehaviour {

    //most of this code is a variation of the sebastian league code
    //you can find more in depth tutorials of how to do this, and the best way to implement a plataform 2d in his youtube channel
    //tutorial https://www.youtube.com/playlist?list=PLFt_AvWsXl0f0hqURlhyIoAabKPgRsqjz
    //channel https://www.youtube.com/channel/UCmtyQOKKmrMVaKuRXz02jbQ

    //the border width from the start of the ray
    const float borderWidth = .015f;
    //count of rays
    public int horizontalRayCount = 4;
    public int verticalRayCount = 4;

    //spacing between raycast
    float horizontalSpacing;
    float verticalSpacing;
    //collision mask
    public LayerMask collisionMask;
    //references
    BoxCollider2D boxCollider;
    StartOfRaycastings raycastStart;
    

	// Use this for initialization
	void Start () {
        boxCollider = GetComponent<BoxCollider2D>();
        RaySpacingCalculator();


    }


    public void Move(Vector3 velocity)
    {
        UpdateStartOfRaycasting();
        if (velocity.x != 0)
        {
            HorizontalCollisions(ref velocity);

        }
        if(velocity.y != 0)
        {
            VerticalCollisions(ref velocity);

        }

        transform.Translate(velocity);
    }

    void HorizontalCollisions(ref Vector3 velocity)
    {

        float directionX = Mathf.Sign(velocity.x);
        float rayLength = Mathf.Abs(velocity.x) + borderWidth;

        for (int i = 0; i < horizontalRayCount; i++)
        {
            Vector2 rayOrigin = (directionX == -1) ? raycastStart.botLeft : raycastStart.botRight;
            rayOrigin += Vector2.up * (horizontalSpacing * i);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, collisionMask);

            Debug.DrawRay(rayOrigin, Vector2.right * directionX * rayLength, Color.red);

            if (hit)
            {
                velocity.x = (hit.distance - borderWidth) * directionX;
                rayLength = hit.distance;
            }
        }

    }


    void VerticalCollisions(ref Vector3 velocity)
    {

        float directionY = Mathf.Sign(velocity.y);
        float rayLength = Mathf.Abs(velocity.y) + borderWidth; 

        for (int i = 0; i < verticalRayCount; i++)
        {
            Vector2 rayOrigin = (directionY == -1) ? raycastStart.botLeft : raycastStart.topLeft;
            rayOrigin += Vector2.right * (verticalSpacing * i + velocity.x);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, collisionMask);
             
            Debug.DrawRay(rayOrigin, Vector2.up * directionY * rayLength, Color.red); 

            if (hit)
            {
                velocity.y = (hit.distance - borderWidth) * directionY;
                rayLength = hit.distance;
            }
        }

    }

    void UpdateStartOfRaycasting()
    {
        //shrinking the border of the bounds, where the ray will start
        Bounds bounds = boxCollider.bounds;
        bounds.Expand(borderWidth * -2);

        //storing where the ray will start
        raycastStart.botLeft = new Vector2(bounds.min.x, bounds.min.y);
        raycastStart.botRight = new Vector2(bounds.max.x, bounds.min.y);
        raycastStart.topLeft = new Vector2(bounds.min.x, bounds.max.y);
        raycastStart.topRight = new Vector2(bounds.max.x, bounds.max.y);
    }

    void RaySpacingCalculator()
    {
        Bounds bounds = boxCollider.bounds;
        bounds.Expand(borderWidth * -2);

        horizontalRayCount = Mathf.Clamp(horizontalRayCount, 2, int.MaxValue);
        verticalRayCount = Mathf.Clamp(verticalRayCount, 2, int.MaxValue);

        horizontalSpacing = bounds.size.y / (horizontalRayCount - 1);
        verticalSpacing = bounds.size.x / (verticalRayCount - 1);
    }

    //struct to store the origin of raycasting
    struct StartOfRaycastings
    {
        public Vector2 topLeft;
        public Vector2 topRight;
        public Vector2 botLeft;
        public Vector2 botRight;

    }
}
