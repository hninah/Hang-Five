using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;

public class CutsceneController : MonoBehaviour
{
    public CutsceneInfo sceneInfo;

    //UI panel elements for this cutscene
    //  (fill with text from sceneInfo.directions)
    [SerializeField] [HideInInspector] TextMeshProUGUI dialogueText;
    [SerializeField] [HideInInspector] TextMeshProUGUI leftNameText;
    [SerializeField] [HideInInspector] TextMeshProUGUI rightNameText;
    [SerializeField] [HideInInspector] TextMeshProUGUI instructionsText;

    //UI image elements for this cutscene
    //  (fill with sprites from sceneInfo.speakerSprites)
    [SerializeField] [HideInInspector] Image leftImage;
    [SerializeField] [HideInInspector] Image rightImage;
    //background image
    //  (fill with sprites from sceneInfo.sceneStills)
    [SerializeField] [HideInInspector] Image backgroundImage;

    //track which dialogue direction (from the CutsceneInfo) we're on
    private int currIndex;

    //cache lengths of the lists for convenience
    private int dirCount; //length of the cutscene directions list
    private int speakerCount; //length of the speaker sprites list
    private int stillCount; //length of the background image sprites list

    //store greyed-out colour and default unfaded colour
    Color32 fadedColour = new Color32(65, 61, 65, 255); //grey
    Color32 unfadedColour = new Color32(255, 255, 255, 255); //white

    //input handlers
    private PlayerInput cutsceneInput;
    private InputAction advance;


    //set up the input and first line
    void Awake(){
       //start the cutscene input
        cutsceneInput = new PlayerInput();

        //get lengths of lists
        dirCount = sceneInfo.directions.Count;
        speakerCount = sceneInfo.speakerSprites.Count;
        stillCount = sceneInfo.sceneStills.Count;

        //start at first line of dialogue
        currIndex = 0;
        if (dirCount > 0){
            dialogueText.text = sceneInfo.directions[currIndex].dialogue;
        }

        //fade the first non-speaker and set up background
        updateSpeakers();
        updateSpeakerDisplay();
        updateBackgroundDisplay();
    }


    void OnEnable(){
        advance = cutsceneInput.Cutscene.Advance;
        advance.Enable();

        //pressing space calls nextLine
        // nextLine handles the logic
        advance.performed += context => nextLine();
    }


    void OnDisable(){
        cutsceneInput.Disable();
    }


    void nextLine(){

        //increment directions index
        ++currIndex; 

        //if didn't finish directions, set up next line
        if (currIndex < dirCount){
            //set next line of dialogue
            dialogueText.text = sceneInfo.directions[currIndex].dialogue;

            //update speaker and background displays
            updateSpeakers();
            updateSpeakerDisplay();
            updateBackgroundDisplay();
        }
        else{
            endCutscene();
        }

        //special case: modify instructions if this is the last dialogue
        if (currIndex == (dirCount - 1)){

            instructionsText.text = "Press Space to return to gameplay";

            /// any other special things for the last line of 
            //  dialogue go here ///
        }
    }


    void endCutscene(){
        Debug.Log("Leaving the cutscene");

        /// NOTE: go to next scene here ///
    }


    void updateSpeakers(){
        //change the speaker sprites if needed
        //get speaker sprite indices from the current directions
        int leftIndex = sceneInfo.directions[currIndex].leftSpeakerIdx;
        int rightIndex = sceneInfo.directions[currIndex].rightSpeakerIdx;

        //update left speaker sprite
        if ( speakerCount > 0 && leftIndex < speakerCount && leftIndex >= 0){
            //update left sprite if index is valid
            leftImage.sprite = sceneInfo.speakerSprites[ leftIndex ];
        }
        else{
            leftImage.enabled = false;
        }

        //update right speaker sprite
        if ( speakerCount > 0 && rightIndex < speakerCount && rightIndex >= 0){
            //update right sprite if index is valid
            rightImage.sprite = sceneInfo.speakerSprites[ rightIndex ];
        }
        else{
            rightImage.enabled = false;
        }

        //update speaker names in case the speaker changed
        leftNameText.text = sceneInfo.directions[currIndex].leftSpeaker;
        rightNameText.text = sceneInfo.directions[currIndex].rightSpeaker;
    }


    void updateSpeakerDisplay(){

        //update left speaker
        //have unfaded sprite and name showing for speaker character
        if ( sceneInfo.directions[currIndex].isLeftSpeaking ){
            leftImage.color = unfadedColour;
            leftNameText.enabled = true;
        }
        //otherwise greyed-out sprite if not speaking
        else{
            leftImage.color = fadedColour;
            leftNameText.enabled = false;
        }

        //update right speaker
        //have unfaded sprite and name showing for speaker character
        if ( sceneInfo.directions[currIndex].isRightSpeaking ){
            rightImage.color = unfadedColour;
            rightNameText.enabled = true;
        }
        //otherwise greyed-out sprite if not speaking
        else{
            rightImage.color = fadedColour;
            rightNameText.enabled = false;
        }
    }   


    void updateBackgroundDisplay(){
        //get scene still index from the current directions
        int backgroundIndex = sceneInfo.directions[currIndex].imgIdx;

        //change background scene still
        if ( stillCount > 0 && backgroundIndex < stillCount){
            //update background sprite
            backgroundImage.sprite = sceneInfo.sceneStills[ backgroundIndex ];
        }
    }
}
