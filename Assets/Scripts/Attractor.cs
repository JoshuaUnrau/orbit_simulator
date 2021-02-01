using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.Serialization;
using Vector3 = UnityEngine.Vector3;

public class Attractor : MonoBehaviour
{

    public GameObject orbitingBody;
    
    public Rigidbody rb;
    public bool fixedPath = false;
    public Shader orbitShader;
    private GameObject orbitLine;
    public Vector3 velocity;
    
    public float radius;
    public float keneticEnergy;
    public float potenetialEnergy;
    public float Energy = 0;
    public float init_energy;
    public float delta_energy;
    public float time = 0;
    public float orbital_period;
    public float vel_mag = 0;
    public float ang_vel = 0;
    
    public bool moving = false;
    private int futureOrbits = 0;
    private static int orbitOffset = 10000;
    float timeStep = 0.1f;
    float g = 1000;

    Vector3[] futurePositions = new Vector3[orbitOffset];
    Vector3[] futureVelocities = new Vector3[orbitOffset];
    
    private int counter = 60;
    // Start is called before the first frame update
    void Start()
    {
        orbitParameters();
        rb.velocity += velocity;
        if (moving)
        {
            orbitLine = new GameObject();
            orbitLine.AddComponent<LineRenderer>();
            orbitLine.GetComponent<LineRenderer>().SetVertexCount(orbitOffset);
            drawOrbits();
            //orbitLine.GetComponent<LineRenderer>().materials
        }
    }

    // Update is called once per frame
    void Update()
    {
        Attractor[] attractors = FindObjectsOfType<Attractor>();
        //gravityVector.Set(0,0,0);
        if(!fixedPath) //for now if you are small enough to be impacted by objects then you are too small to attract them
        {
            //Deltas are too large
            velocity = futureVelocities[futureOrbits];
            transform.position = futurePositions[futureOrbits];
            futureOrbits++;
        }

        if (moving)
        {
            //if (radius < (orbitingBody.transform.position - transform.position).magnitude)
                //Debug.Break();
            
            radius = (orbitingBody.transform.position - transform.position).magnitude;
            keneticEnergy = rb.mass * velocity.magnitude * velocity.magnitude / 2;
            potenetialEnergy = (g * orbitingBody.GetComponent<Rigidbody>().mass * rb.mass)/ radius;
            Energy = keneticEnergy - potenetialEnergy;
            time += Time.deltaTime;
            vel_mag = velocity.magnitude;
            ang_vel = Vector3.Cross(velocity, orbitingBody.transform.position - transform.position).magnitude;
            delta_energy = init_energy - Energy;
        }

        counter++;
    }

    private void FixedUpdate()
    {
      
    }

    private void orbitParameters()
    {
        if(!moving)
            return;
        //should be cross product
        Vector3 r = orbitingBody.transform.position - transform.position;
        float mass1 = rb.mass;
        float mass2 = orbitingBody.GetComponent<Rigidbody>().mass;
        float micro = g * mass2;
        float E =  mass1 * velocity.magnitude * velocity.magnitude/2 - g * mass2 * mass1 / r.magnitude;
        float l = Vector3.Cross(velocity, r).magnitude;
        init_energy = E;
        float a = -g*mass2*mass1/(2*E);
        float T = (float) Math.Sqrt(4 * Math.PI * Math.PI * a * a * a / (g * mass2));
        //Below only works because r is also r_a
        float e = (float) Math.Sqrt(1 + 2 * E * l * l / (mass1 * mass1 * mass1 * micro * micro));

        float theta = (float) 0;
        float per = (l * l/ (mass1*micro)) * (1 / (1 + e * (float)Math.Cos(theta)));
        Debug.Log(E);
        Debug.Log(a);
        Debug.Log(e);
        Debug.Log(T);
        Debug.Log(l);
        Debug.Log(a*(1-e));
        Debug.Log(micro);
        Debug.Log(per);
        Debug.Log((l * l/ (mass1*micro)) * (1 / (1 + e * (float)Math.Cos(Math.PI))));
        Debug.Log((1 / (1 + e * (float)Math.Cos(theta))));
        orbital_period = T;
    }

    Vector3 Attract(Vector3 position)
    {
        Attractor[] attractors = FindObjectsOfType<Attractor>();
        Vector3 sumAccel = new Vector3();
        foreach (var attractor in attractors)
        {
            if (attractor != this)
            {
                Vector3 direction = position - attractor.rb.position;
                float distance = direction.magnitude;
                float forceMagnitude = g*(attractor.rb.mass) / Mathf.Pow(distance, 2);
                Vector3 accel = direction.normalized * forceMagnitude;
                sumAccel -= accel;
            }
        }
        return sumAccel;
    }

    void drawOrbits()
    {
        //Draw orbit
        //TODO: Redo with exact orbit position formula for exact values
        Vector3 startPos = rb.position;
        Vector3 startVel = velocity;
        for(int i = 0; i < orbitOffset; i++)
        {
            Vector3 accel = Attract(startPos) * timeStep;
            startVel += accel;
            Vector3 endPos = startPos + startVel*timeStep + accel*timeStep*timeStep/2;
            futurePositions[i] = endPos;
            futureVelocities[i] = startVel;
            startPos = endPos;
        }
        LineRenderer lr = orbitLine.GetComponent<LineRenderer>();
        lr.material =  new Material(Shader.Find("Unlit/Texture"));
        lr.SetColors(Color.red, Color.red);
        lr.SetWidth(5f, 5f);
        lr.SetPositions(futurePositions);
        
    }
    
    void DrawLine(Vector3 start, Vector3 end, Color color, int poolingIndex, float duration = 0.2f)
    {
        LineRenderer lr = orbitLine.GetComponent<LineRenderer>();
        lr.material =  new Material(Shader.Find("Unlit/Texture"));
        lr.SetColors(color, color);
        lr.SetWidth(1f, 1f);
        lr.SetPosition(poolingIndex, start);
        lr.SetPosition(poolingIndex+1, end);
    }
}
