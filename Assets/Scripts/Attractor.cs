using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.Serialization;
using Vector3 = UnityEngine.Vector3;

public class Attractor : MonoBehaviour
{

    public GameObject orbitingBody;
    
    public Rigidbody rb;
    public bool fixedPath = false;
    public Shader orbitShader;
    public GameObject orbitLine;
    public Vector3 velocity;
    public EllipseRenderer renderer;
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
    [Range(0,1)] public float mean_anomally;
    [Range(0, 1)] public float ecentricity;
    public float apoapsis;
    public float periapsis;
    public float semiMinor;
    public float semiMajor;
    
    public bool moving = false;
    private int futureOrbits = 0;
    public float theta;
    public float beta;
    public int steps = 10000;
    public int steps_per_frame = 100;
    public float timeStep = 0.001f;
    public float g = 1000;
    public bool ccw;

    Vector3[] futurePositions;
    Vector3[] futureVelocities;
    
    private int counter = 60;
    // Start is called before the first frame update
    void Start()
    {
        load();
    }

    private void load()
    {
        futureVelocities = new Vector3[steps];
        futurePositions = new Vector3[steps];
        OrbitParameters();
        rb.velocity += velocity;
        if (moving)
        {
            orbitLine.GetComponent<LineRenderer>().SetVertexCount(steps);
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
            if (futurePositions[futureOrbits] != null)
            {
                transform.position = futurePositions[futureOrbits];
            }
            OrbitParameters();
            futureOrbits += steps_per_frame;
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

    private void OrbitParameters()
    {
        if(!moving)
            return;
        
        Vector3 r = orbitingBody.transform.position - transform.position;
        radius = Vector3.Distance(orbitingBody.transform.position, transform.position);
        float mass1 = rb.mass;
        float mass2 = orbitingBody.GetComponent<Rigidbody>().mass;
        float micro = g * mass2;
        keneticEnergy = mass1 * velocity.magnitude * velocity.magnitude / 2;
        potenetialEnergy = g * mass2 * mass1 / r.magnitude;
        Energy =  mass1 * velocity.magnitude * velocity.magnitude/2 - g * mass2 * mass1 / r.magnitude;
        float l = Vector3.Cross(velocity, r).magnitude;
        init_energy = Energy;
        float a = -g*mass2*mass1/(2*Energy);
        float T = (float) Math.Sqrt(4 * Math.PI * Math.PI * a * a * a / (g * mass2));
        float e = (float) Math.Sqrt(1 + 2 * Energy * l * l / (mass1 * mass1 * mass1 * micro * micro));
        ecentricity = e;
        float per = (l * l/ (mass1*micro)) * (1 / (1 + e * (float)Math.Cos(0)));
        apoapsis = (l * l / (mass1 * micro)) * (1 / (1 + e * (float) Math.Cos(Math.PI)));
        periapsis = per;
        //Does work if sat mass is not 1
        orbital_period = T;
        semiMajor = (apoapsis + periapsis) / 2; 
        semiMinor = Mathf.Sqrt(semiMajor*semiMajor*(1-e*e)); 
        float radi = radius;
        theta = Mathf.Acos(l * l / (radi * mass1 * mass1 * micro * e) - 1 / e);
        beta = Mathf.Acos(Vector3.Dot(new Vector3(1, 0, 0), r)/r.magnitude);
        //theta = Mathf.Acos(l * l / (apoapsis * mass1 * mass1 * micro * e) - 1 / e);
        print(Mathf.Rad2Deg* theta);
        print(theta);
        Vector3 p2 = new Vector3(radius*Mathf.Cos(theta),0,radius*Mathf.Sin(Mathf.PI+theta));
        //true if ccw (negative ccw)
        renderer.ellipse.orbitSide = -1f;
        print(Vector3.Dot(Vector3.Cross(r, velocity), new Vector3(0,1,0)));
        float orbitSide = 1;
        float clockWise = 1;
        ccw = Vector3.Dot(Vector3.Cross(r, velocity), new Vector3(0, 1, 0)) > 0;
        if (ccw)
        {
            clockWise *= -1;
            //renderer.ellipse.orbitSide = 1f;
            //p2 = new Vector3(radius * Mathf.Cos(theta), 0, radius * Mathf.Sin(theta));
            //renderer.ellipse.orbitSide *= -1f;
            //true if descending
            if (Vector3.Dot(r, velocity) > 0)
            {
                orbitSide = -1;
            }
        }
        else
        {
            orbitSide = -1;
            if (Vector3.Dot(r, velocity) > 0)
            {
                orbitSide = 1;
            }
        }

        //theta *= clockWise;
        theta *= orbitSide;
        beta *= -Mathf.Sign(r.z);
        //renderer.ellipse.offsetAngle = -beta + theta - Mathf.PI;
        renderer.ellipse.offsetAngle = -beta + Mathf.PI - theta;
        //theta = Mathf.Acos((2 * radius * radius - Vector3.Distance(transform.position, p2)*Vector3.Distance(transform.position, p2)) / 
        //                   (2 * radius * radius));
        //theta = Mathf.Acos((2 * radius * radius - Vector3.Distance(transform.position, p2)*Vector3.Distance(transform.position, p2)) / (2 * radius * radius));
        print(Mathf.Rad2Deg* theta);
        print(theta);
        //print(Mathf.Rad2Deg* theta);
        renderer.ellipse.offsetPoint = orbitingBody.transform.position;
        renderer.ellipse.xAxis = semiMajor;
        renderer.ellipse.yAxis = semiMinor;
        renderer.ellipse.offset.x = orbitingBody.transform.position.x-semiMajor+periapsis;
        renderer.ellipse.offset.y = orbitingBody.transform.position.z;
        renderer.CalculateEllipse();
        theta = Mathf.Rad2Deg*theta;
        beta = Mathf.Rad2Deg*beta;
        //What position in the orbit am I in? How do I return a future x,y cord?
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
        for(int i = 0; i < steps; i++)
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

    private void OnValidate()
    {
        //if (orbitingBody)
        //{
            load();
            OrbitParameters();
        //}
    }
}
