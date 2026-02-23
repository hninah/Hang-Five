using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SineWaveState : State
{
    float frequency = 0.08f;
    float amplitude = 4f;
    float arcDistance = 0.0f;
    float arcSpeed = 90f;

    //constructors
    public SineWaveState():base("SineWaveState") {}
    public SineWaveState(float freq, float amp):base("SineWaveState"){
        frequency = freq;
        amplitude = amp;
    }


    //update for this state
    public override State stateUpdate(Obstacle ob){

        ob.transform.position += Vector3.left * ob.scrollSpeed * Time.deltaTime;

        //add wave offset to position
        arcDistance = arcDistance + arcSpeed * Time.deltaTime;
        Debug.Log(arcDistance);
        Vector3 waveOffset = Vector3.up * Mathf.Sin(arcDistance * Mathf.Deg2Rad) * Time.deltaTime * amplitude;
        Debug.Log(waveOffset);
        ob.transform.position = ob.transform.position + waveOffset;

        if (ob.transform.position.x < ob.deathBoundX)
        {
            return new DeathState();
        }

        return this;
    }


    public override void onEnterState(){
        ///Debug.Log("entered " + this.Name);
    }
}
