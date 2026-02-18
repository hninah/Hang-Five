using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SineWaveState : State
{
    float frequency = 3f;
    float amplitude = 0.08f;

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
        Vector3 waveOffset = Vector3.up * Mathf.Sin(Time.time * frequency) * amplitude;
        ob.transform.position = ob.transform.position + waveOffset;

        if (ob.transform.position.x < 0.0f)
        {
            onExitState();
            return new StationaryState();
        }

        return this;
    }


    public override void onEnterState(){
        ///Debug.Log("entered " + this.Name);
    }
}
