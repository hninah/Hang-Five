using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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
    //  (fill with sprites from sceneInfo.directions)
    [SerializeField] [HideInInspector] Image leftImage;
    [SerializeField] [HideInInspector] Image rightImage;
    //background image
    [SerializeField] [HideInInspector] Image backgroundImage;

    //track which dialogue direction we're on
    private int currIndex;
    //track whether we're done the cutscene
    private bool cutsceneEnded;

    //cache lengths of the lists for convenience
    private int dirCount;
    private int speakerCount;
    private int stillCount;

    //store greyed-out colour and default unfaded
    Color32 fadedColour = new Color32(65, 61, 65, 255); //grey
    Color32 unfadedColour = new Color32(255, 255, 255, 255); //white


    void Start(){
        //start at first direction
        currIndex = 0;
        //get lengths of lists
        dirCount = sceneInfo.directions.Count;
        speakerCount = sceneInfo.speakerSprites.Count;
        stillCount = sceneInfo.sceneStills.Count;

        //start dialogue text on the first element
        if (dirCount > 0){
            dialogueText.text = sceneInfo.directions[0].dialogue;

            //set up speaker names on their sides
            leftNameText.text = sceneInfo.directions[0].leftSpeaker;
            rightNameText.text = sceneInfo.directions[0].rightSpeaker;
        }

        //get indices for the speaker images from current directions
        int leftSpriteIndex = sceneInfo.directions[currIndex].leftSpeakerIdx;
        int rightSpriteIndex = sceneInfo.directions[currIndex].rightSpeakerIdx;
        
        //get left image sprite from speakerSprites list
        if (leftSpriteIndex < speakerCount){
            leftImage.sprite = sceneInfo.speakerSprites[ leftSpriteIndex ];
        }
        //disable left image if there's no speaker on that side
        //  (removes empty white rectangle where sprite should be)
        else{
            leftImage.enabled = false;
        }

        //get right image sprite
        if (rightSpriteIndex < speakerCount){
            rightImage.sprite = sceneInfo.speakerSprites[ rightSpriteIndex ];
        }
        //disable right image if there's no speaker on that side
        else{
            rightImage.enabled = false;
        }

        //fade the first non-speaker and set up background
        updateSpeakerDisplay();
        updateBackgroundDisplay();
    }


    // Update is called once per frame
    void Update(){
        
        //check for input
        if (Input.GetKeyDown(KeyCode.Space)){

            //move on from scene if cutscene ended
            if( cutsceneEnded ){
                ///NOTE: something here to go to next scene??///
            }
            //otherwise go to next line of dialogue
            else{
                nextLine();
            }
        }
    }


    void nextLine(){

        //increment directions index
        ++currIndex; 

        //if didn't finish directions, set up next line
        if (currIndex < dirCount){
            //set next line of dialogue
            dialogueText.text = sceneInfo.directions[currIndex].dialogue;

            //update speaker and background displays
            updateSpeakerDisplay();
            updateBackgroundDisplay();
            
        }
        //otherwise end dialogue
        else{
            endCutscene();
        }
    }


    void endCutscene(){
        cutsceneEnded = true;

        //hide the speakers
        rightImage.color = fadedColour;
        rightNameText.enabled = false;
        leftImage.color = fadedColour;
        leftNameText.enabled = false;
        //hide the instructions text
        instructionsText.enabled = false;

        //show instruction text
        dialogueText.text = "[Press Space to move to next scene]";
    }


    void updateSpeakerDisplay(){
        //update left speaker
        //unfaded sprite and name showing for speaker character
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
        //unfaded sprite and name showing for speaker character
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
        //change background scene still
        if ( stillCount > 0){
            //get scene still index from the current directions
            int backgroundIndex = sceneInfo.directions[currIndex].imgIdx;
            //update background sprite
            backgroundImage.sprite = sceneInfo.sceneStills[ backgroundIndex ];
        }
    }
}
