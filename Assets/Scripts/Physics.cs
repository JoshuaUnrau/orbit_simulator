using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[ExecuteInEditMode]
public class OrbitalPhysics : MonoBehaviour
{

    /*public Rigidbody rb;
    public bool fixedPath = false;
    public Shader orbitShader;
    private GameObject orbitLine;
    
    public Vector3 velocity;
    
    public bool moving = false;
    private int futureOrbits = 0;
    private static int orbitOffset = 3000;
    Vector3[] futurePositions = new Vector3[orbitOffset];
    Vector3[] futureVelocities = new Vector3[orbitOffset];
    public Vector3 gravityVector;

    private int counter = 60;
    // Start is called before the first frame update
    void Start()
    {
        rb.velocity += velocity;
        if (moving)
        {
            orbitLine = new GameObject();
            orbitLine.AddComponent<LineRenderer>();
            orbitLine.GetComponent<LineRenderer>().SetVertexCount(3001);
            //orbitLine.GetComponent<LineRenderer>().materials
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        Attractor[] attractors = FindObjectsOfType<Attractor>();
        gravityVector.Set(0,0,0);
        if(!fixedPath) //for now if you are small enough to be impacted by objects then you are too small to attract them
        { 
            velocity += Attract(rb.position)*Time.deltaTime;
            transform.position += velocity*Time.deltaTime;
            futureOrbits--;
            drawOrbit();
        }
        counter++;
    }

    Vector3 Attract(Vector3 position)
    {
        Attractor[] attractors = FindObjectsOfType<Attractor>();
        Vector3 sumForce = new Vector3();
        // Debug.Log(position);
        float g = 1;
        foreach (var attractor in attractors)
        {
            if (attractor != this)
            {
                Vector3 direction = position - attractor.rb.position;
                //Debug.Log(direction);
                float distance = direction.magnitude;
                float forceMagnitude = g*(rb.mass * attractor.rb.mass) / Mathf.Pow(distance, 2);
                //float forceMagnitude = g / Mathf.Pow(distance, 2);
                Vector3 force = direction.normalized * forceMagnitude;
                sumForce -= force;
            }
        }
        return sumForce;
    }

    void drawOrbit()
    {
        //Draw orbit
        //TODO: Redo with exact orbit position formula for exact values
        Vector3 startPos = futurePositions[futureOrbits];
        Vector3 startVel = futureVelocities[futureOrbits];
        
        float timeStep = 0.03f;
        Vector3 accel = Attract(startPos)*timeStep;
        startVel += accel;
        Vector3 endPos = startPos + startVel*timeStep + accel*timeStep*timeStep/2;
        for(int i = 0; i < orbitOffset; i++)
        {
            DrawLine(startPos, endPos, Color.red, orbitOffset-futureOrbits);
        }
    }
    
    void DrawLine(Vector3 start, Vector3 end, Color color, int poolingIndex, float duration = 0.2f)
    {
        LineRenderer lr = orbitLine.GetComponent<LineRenderer>();
        lr.material =  new Material(Shader.Find("Unlit/Texture"));
        lr.SetColors(color, color);
        lr.SetWidth(1f, 1f);
        lr.SetPosition(poolingIndex, start);
        lr.SetPosition(poolingIndex+1, end);
        Vector3[] allPoints = new Vector3[orbitOffset];
        lr.GetPositions(allPoints);
    }*/
}
