using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ExplodeParts : MonoBehaviour
{
    public void Explode()
    {
        var rbs = gameObject.GetComponentsInChildren<Rigidbody>().ToList();
        rbs.ForEach(x =>
        {
            x.AddForce(((Vector3.forward+Vector3.right*(Random.Range(0,10)>5?-0.3f:0.3f))*Random.Range(400,700)));
            x.angularVelocity = Random.insideUnitSphere * Random.Range(400f, 700f);
        });
    }
}
