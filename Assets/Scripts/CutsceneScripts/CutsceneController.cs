using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class CutsceneController : MonoBehaviour
{
    public CutsceneInfo sceneInfo;

    //UI panel elements for this cutscene
    //  (fill with text from sceneInfo.directions)
    [SerializeField] [HideInInspector] TextMeshProUGUI dialogueText;
    [SerializeField] [HideInInspector] TextMeshProUGUI leftNameText;
    [SerializeField] [HideInInspector] TextMeshProUGUI rightNameText;
    //this one isn't just text because it has to change based on UI input type
    // (see InstructionText.cs for details)
    [SerializeField] [HideInInspector] InstructionsText instructions; 

    //UI image elements for this cutscene
    //  (fill with sprites from sceneInfo.speakerSprites)
    [SerializeField] [HideInInspector] Image leftImage;
    [SerializeField] [HideInInspector] Image rightImage;

    //background image
    //  (fill with sprites from sceneInfo.sceneStills)
    [SerializeField] [HideInInspector] Image backgroundImage;

    //get references to the name panels to grey them out with their speakers
    private Image leftNamePanel;
    private Image rightNamePanel;

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

    // Progressive text updating
    private Queue<char> textQueue = new Queue<char>();
    private StringBuilder builder = new StringBuilder();
    [SerializeField] float charactersPerSecond = 30;
    float characterUpdateTime { get { return 1 / charactersPerSecond; } }
    private Coroutine currentTextProgress;

    //set up the input and first line
    void Awake(){
        //link to the name panels
        leftNamePanel = transform.GetChild(1).GetComponent<Image>();
        rightNamePanel = transform.GetChild(2).GetComponent<Image>();

       //start the cutscene input
        cutsceneInput = new PlayerInput();

        //get current cutscene info from manager
        if (CutsceneManager.Instance != null && !CutsceneManager.Instance.isFinished()){
            ///Debug.Log("getting a new cutscene");
            sceneInfo = CutsceneManager.Instance.getNextCutscene();
        }
        else{
            Debug.Log("reached the end of the cutscenes");
        }

        //get lengths of lists
        dirCount = sceneInfo.directions.Count;
        speakerCount = sceneInfo.speakerSprites.Count;
        stillCount = sceneInfo.sceneStills.Count;

        //start at first line of dialogue
        currIndex = 0;
        if (dirCount > 0){
            prepareDialogue();
        }

        //fade the first non-speaker and set up background and text colour
        dialogueText.color = getTextColour();
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


    void prepareDialogue()
    {
        // Clear anything related to the current typewriting
        if (currentTextProgress != null)
        {
            StopCoroutine(currentTextProgress);
            dialogueText.text = "";
            builder.Clear();
            textQueue.Clear();
        }

        // Enqueue the next dialogue segment character by character for a typewriting effect
        // NOTE: you could change this to word by word, but character by character was chosen for a little more freedom of speed control
        foreach (char textPiece in sceneInfo.directions[currIndex].dialogue)
        {
            textQueue.Enqueue(textPiece);
        }

        currentTextProgress = StartCoroutine(progressiveText());
    }

    IEnumerator progressiveText()
    {
        while (textQueue.Count > 0)
        {
            // Adds characters to the text at a speed of charactersPerSecond
            yield return new WaitForSeconds(characterUpdateTime);

            builder.Append(textQueue.Dequeue());

            dialogueText.text = builder.ToString();
        }
    }


    void nextLine(){
        ///Debug.Log("entering nextLine: currIndex = " + currIndex);
        //increment directions index
        ++currIndex; 

        //if didn't finish directions, set up next line
        if (currIndex < dirCount){
            dialogueText.color = getTextColour();

            //set next line of dialogue
            prepareDialogue();

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

            //InstructionsText handles the logic of which message to display
            instructions.lastLine();

            /// any other special things for the last line of 
            //  dialogue go here ///
        }
    }


    void endCutscene(){
        ///Debug.Log("Leaving the cutscene");

        //return to gameplay
        SceneManager.LoadScene("Gameplay");
    }


    Color getTextColour(){
        var colourIndex = sceneInfo.directions[currIndex].colourIdx;
        return sceneInfo.textColours[colourIndex];
    }


    void updateSpeakers(){
        //change the speaker sprites if needed
        //get speaker sprite indices from the current directions
        int leftIndex = sceneInfo.directions[currIndex].leftSpeakerIdx;
        int rightIndex = sceneInfo.directions[currIndex].rightSpeakerIdx;
        
        //update left speaker sprite
        if ( speakerCount > 0 && leftIndex < speakerCount && leftIndex >= 0){
            //re-enable the sprite if it had an invalid index earlier
            if (!leftImage.enabled){
                leftImage.enabled = true;
                leftNamePanel.enabled = true;
            } 

            //update left sprite if index is valid
            leftImage.sprite = sceneInfo.speakerSprites[ leftIndex ];
        }
        else{
            leftImage.enabled = false;
            leftNamePanel.enabled = false;
        }

        //update right speaker sprite
        if ( speakerCount > 0 && rightIndex < speakerCount && rightIndex >= 0){
            //re-enable the sprite if it had an invalid index earlier
            if (!rightImage.enabled){
                rightImage.enabled = true;
                rightNamePanel.enabled = true;
            }

            //update right sprite if index is valid
            rightImage.sprite = sceneInfo.speakerSprites[ rightIndex ];
        }
        else{
            rightImage.enabled = false;
            rightNamePanel.enabled = false;
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
            leftNamePanel.color = unfadedColour;
            leftNameText.enabled = true;
            //match name colour to the speaker's dialogue text colour
            leftNameText.color = getTextColour();
        }
        //otherwise greyed-out sprite if not speaking
        else{
            leftImage.color = fadedColour;
            leftNamePanel.color = fadedColour;
            leftNameText.enabled = false;
        }

        //update right speaker
        //have unfaded sprite and name showing for speaker character
        if ( sceneInfo.directions[currIndex].isRightSpeaking ){
            rightImage.color = unfadedColour;
            rightNamePanel.color = unfadedColour;
            rightNameText.enabled = true;
            //match name colour to the speaker's dialogue text colour
            rightNameText.color = getTextColour();  
        }
        //otherwise greyed-out sprite if not speaking
        else{
            rightImage.color = fadedColour;
            rightNamePanel.color = fadedColour;
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
