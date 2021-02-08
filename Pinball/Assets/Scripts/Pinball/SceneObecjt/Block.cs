using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    public SpriteRenderer sprite;
    public BoxCollider col;
    // Start is called before the first frame update
    void Start()
    {
        col.size = new Vector3(sprite.size.x, sprite.size.y, 100);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
