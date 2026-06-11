using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Cutscene Info")]
public class CutsceneInfo : ScriptableObject
{

    [System.Serializable]
    public struct SceneDirection
    {
        // Names of the characters on the left and right sides of the screen (really just needs to be a way to identify different character sprites to display)
        //these are displayed in text boxes by the character sprites
        public string leftSpeaker;
        public string rightSpeaker;

        //whether the character should be unfaded (reverse of original leftFaded and rightFaded)
        public bool isLeftSpeaking;
        public bool isRightSpeaking;
        
        //The speaker sprites to use for each thing
        public int leftSpeakerIdx;
        public int rightSpeakerIdx;

        // The sceneStill to use (see below)
        public int imgIdx;

        //The text colour for the current speaker (chosen from textColours list)
        public int colourIdx;

        // The actual dialogue to be displayed (TextArea is unfortunately the cleanest way to do it without using an external plugin)
        [TextArea] public string dialogue;
    }

    //sprites for each speaker
    public List<Sprite> speakerSprites;

    // Background Sprites that can be changed with the dialogue.
    public List<Sprite> sceneStills;

    //text colour for each speaker
    public List<Color> textColours;

    // A list of Scenedirections to load and play sequentially using a cue from another script (e.g. cutscene input system)
    public List<SceneDirection> directions = new List<SceneDirection>();
}