using UnityEngine;

public class Camera : MonoBehaviour
{
    public float speed = 10.0f;

    void Update()
    {
        var moveHorizontal = Input.GetAxis("Horizontal");
        var moveVertical = Input.GetAxis("Vertical");

        var movement = new Vector3(moveHorizontal, 0.0f, moveVertical);

        transform.position += movement * speed * Time.deltaTime;    
    }
}