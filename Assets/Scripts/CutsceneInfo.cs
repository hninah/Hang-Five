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
        public string leftSpeaker;
        public string rightSpeaker;

        // Whether the character should be "grayed out" (cue to darken their sprite a little bit using other scripts)
        ///public bool leftFaded;
        ///public bool rightFaded;
        //whether the character should be unfaded (reverse of leftFaded and rightFaded)
        public bool isLeftSpeaking;
        public bool isRightSpeaking;
        // The name of the person speaking (to display in a text box)
        /// public string speakerName;
        
        //The speaker sprites to use for each thing
        public int leftSpeakerIdx;
        public int rightSpeakerIdx;
        // The sceneStill to use (see below)
        public int imgIdx;

        // The actual dialogue to be displayed (TextArea is unfortunately the cleanest way to do it without using an external plugin)
        [TextArea] public string dialogue;
    }

    public List<Sprite> speakerSprites;
    // Background Sprites that can be changed with the dialogue.
    public List<Sprite> sceneStills;
    // A list of Scenedirections to load and play sequentially using a cue from another script (e.g. cutscene input system)
    public List<SceneDirection> directions = new List<SceneDirection>();
    
    // Whether the textbox should be in the lower half or upper half of the screen (useful for if an artist needs the textbox to be in a different position).
    ///public bool textAtTop;
}