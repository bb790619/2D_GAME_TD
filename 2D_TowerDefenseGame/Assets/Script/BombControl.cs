using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//放在"特效"上，讓特效0.5秒後就消除
public class BombControl : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Destroy(this.gameObject, 0.5f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
