using UnityEngine;
using UnityEngine.UI;

//This class handles the UI related to the Replay/Recording manager
public class ReplayPanelManager : MonoBehaviour
{
    #region Parameters
    //Button used to start and stop the recording
    [SerializeField] Button startStopRecordButton;

    //record button's text component
    [SerializeField] Text startStopRecordButtonText;

    //Button used to start and stop the replay of the selected recording
    [SerializeField] Button startStopReplayButton;

    //the replay button's text component
    [SerializeField]
    Text startStopReplayButtonText;

    //Replay input field - User can enter a name to replay saved recording
    //recording input field - The newly recorded recording will be saved by this name
    [SerializeField]
    InputField replayInputField, recordingInputField;


    //saveRecordingPanel - The save recording UI panel
    //saveOverwritePanel - The Save overwrite UI Panel
    [SerializeField]
    GameObject saveRecordingPanel;


    //This panel restricts the user from interacting during a replay is being played
    [SerializeField]
    GameObject inputBlocketPanel;
    #endregion



    #region Core
    //Subscribe to recording Callbacks
    void OnEnable()
    {
        AppManager.Instance.replayManager.OnStoppedRecording += OnStoppedRecording;
        AppManager.Instance.replayManager.OnStartedRecording += OnStartedRecording;
        AppManager.Instance.replayManager.OnStartedReplaying += OnStartedReplaying;
        AppManager.Instance.replayManager.OnStoppedReplaying += OnStoppedReplaying;
        AppManager.Instance.replayManager.OnSaveRecordingSuccess += OnSaveRecordingSuccess;
    }

    //UnSubscribe from recording Callbacks
    void OnDisable()
    {
        AppManager.Instance.replayManager.OnStartedRecording -= OnStartedRecording;
        AppManager.Instance.replayManager.OnStoppedRecording -= OnStoppedRecording;
        AppManager.Instance.replayManager.OnStartedReplaying -= OnStartedReplaying;
        AppManager.Instance.replayManager.OnStoppedReplaying -= OnStoppedReplaying;
        AppManager.Instance.replayManager.OnSaveRecordingSuccess -= OnSaveRecordingSuccess;
    }

    //Decide what happens once recording starts
    void OnStartedRecording()
    {
        startStopRecordButtonText.text = "Stop Recording";

        //Disable Replay button
        startStopReplayButton.interactable = false;

        //Disable Input Fields
        recordingInputField.interactable = false;
        replayInputField.interactable = false;
    }

    //Decide what happens once recording stops
    void OnStoppedRecording()
    {
        startStopRecordButtonText.text = "Start Recording";

        //Enable Replay button
        startStopReplayButton.interactable = true;


        //Enable Input Field
        recordingInputField.interactable = true;
        replayInputField.interactable = true;


        //Show save Recording screen
        saveRecordingPanel.SetActive(true);

        //Reset Rec name so that user can enter different name
        recordingInputField.text = null;
    }


    //Decide what happens once replay starts
    void OnStartedReplaying()
    {
        startStopReplayButtonText.text = "Stop Replay";

        //Disable Recording button
        startStopRecordButton.interactable = false;

        //Disable Input Field
        replayInputField.interactable = false;

        //Enable Input Blocker as we dont want the user to be interacting with objects while playback
        inputBlocketPanel.SetActive(true);
    }


    //Decide what happens once replay stops
    void OnStoppedReplaying()
    {
        startStopReplayButtonText.text = "Start Replay";

        //Enable Recording button
        startStopRecordButton.interactable = true;

        //Enable Input Field
        replayInputField.interactable = true;


        //Disbal Input Blocker 
        inputBlocketPanel.SetActive(false);
    }


    //decided what happens once a recording have been saved successfully
    public void OnSaveRecordingSuccess() {
        //Close Save Recording Menu
        saveRecordingPanel.SetActive(false);
    }



    //Decide what happens once save button is pressed after recording is done
    public void OnSaveRecordingButtonPressed()
    {
        if (!string.IsNullOrEmpty(recordingInputField.text))
            AppManager.Instance.replayManager.SaveRecording(recordingInputField.text);
        else
            Debug.LogError("Enter a valid name please!!");
    }


    //Decide what happens once the replay button is pressed
    public void OnReplayRecordingButtonPressed()
    {
        if (!string.IsNullOrEmpty(replayInputField.text))
            AppManager.Instance.replayManager.LoadRecording(replayInputField.text);
        else
            Debug.LogError("Enter a valid name please!!");
    }

    #endregion
}
