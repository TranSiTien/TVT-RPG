using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordEnergy : MonoBehaviour
{
    enum attackDirection
    {
        LEFT,
        RIGHT,
        UP,
        DOWN
    }

    Rigidbody2D rb;
    attackDirection ad;
    // Start is called before the first frame update

   /* public static SwordEnergy Instantiate(attackDirection direction, Vector2 playerPosition)
    {

    }*/
    void Start()
    {

        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
