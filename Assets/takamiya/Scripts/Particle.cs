//=========================================
//†•—‘D¶¬ˆ—
//AuthorF‚‹{—SãÄ
//Date2/28
//
//=========================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Timers;

public class Particle : MonoBehaviour
{
    [SerializeField] GameObject particle;

    [SerializeField] GameObject CrackerRight;

    [SerializeField] GameObject CrackerLeft;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("GenerateParticle",1.0f,1.0f);
        InvokeRepeating("GenerateParticle2", 1.0f, 1.0f);
    }
    // Update is called once per frame
    void Update()
    {
       
    }

    void GenerateParticle()
    {
        GameObject parent = particle;
        Instantiate(parent,CrackerRight.transform);

        Destroy(parent, 5f);
    }

    void GenerateParticle2()
    {
        GameObject parent = particle;
        Instantiate(parent, CrackerLeft.transform);

        Destroy(parent, 5f);
    }
}
