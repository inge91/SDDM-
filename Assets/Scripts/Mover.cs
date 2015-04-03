using UnityEngine;
using System.Collections;

public class Mover : MonoBehaviour {

    public float speed;

    //void Start () {
    //    (Rigidbody)this.
    //    GetComponent<Rigidbody>().velocity = transform.forward * speed;
    //    //GameObject.rigidbody.velocity = transform.forward * speed ;
    //}

    void Update()
    {
        this.transform.Translate(0f, 0f, (speed * Time.deltaTime), Space.Self);
    }
}
