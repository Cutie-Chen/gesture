using UnityEngine;

public class JointVelocityData : MonoBehaviour
{
    
    public Vector3 velocity;  
    public Vector3 direction; 
    public void UpdateVelocity(Vector3 newVelocity, Vector3 newDirection)
    {
        velocity = newVelocity;
        direction = newDirection;
        Debug.Log(velocity);
        Debug.Log(direction);
        

    }
}
