using UnityEngine;
using UnityEngine.UI;

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
    GameObject saveRecordingPanel, saveOverwritePanel;

    #endregion



    #region Unity Methods
    private void OnEnable()
    {
        //Subscribe to recording Callbacks
        ReplayManager.Instance.OnStoppedRecording += OnStoppedRecording;
        ReplayManager.Instance.OnStartedRecording += OnStartedRecording;
        ReplayManager.Instance.OnStartedReplaying += OnStartedReplaying;
        ReplayManager.Instance.OnStoppedReplaying += OnStoppedReplaying;
        ReplayManager.Instance.OnSaveRecordingSuccess += OnSaveRecordingSuccess;
        ReplayManager.Instance.OnSaveRecordingDuplicateException+= OnSaveRecordingDuplicateException;
    }

    private void OnDisable()
    {
        //UnSubscribe from recording Callbacks
        ReplayManager.Instance.OnStartedRecording -= OnStartedRecording;
        ReplayManager.Instance.OnStoppedRecording -= OnStoppedRecording;
        ReplayManager.Instance.OnStartedReplaying -= OnStartedReplaying;
        ReplayManager.Instance.OnStoppedReplaying -= OnStoppedReplaying;
        ReplayManager.Instance.OnSaveRecordingSuccess -= OnSaveRecordingSuccess;
        ReplayManager.Instance.OnSaveRecordingDuplicateException -= OnSaveRecordingDuplicateException;
    }
    #endregion

    #region Callbacks
    void OnStartedRecording()
    {
        startStopRecordButtonText.text = "Stop Recording";

        //Disable Replay button
        startStopReplayButton.interactable = false;

        //Disable Input Field
        recordingInputField.interactable = false;
    }

    void OnStoppedRecording()
    {
        startStopRecordButtonText.text = "Start Recording";

        //Enable Replay button
        startStopReplayButton.interactable = true;


        //Enable Input Field
        recordingInputField.interactable = true;


        //Show save Recording screen
        saveRecordingPanel.SetActive(true);
    }

    void OnStartedReplaying()
    {
        startStopReplayButtonText.text = "Stop Replay";

        //Disable Recording button
        startStopRecordButton.interactable = false;

        //Disable Input Field
        recordingInputField.interactable = false;
    }

    void OnStoppedReplaying()
    {
        startStopReplayButtonText.text = "Start Replay";

        //Enable Recording button
        startStopRecordButton.interactable = true;

        //Enable Input Field
        recordingInputField.interactable = true;
    }

    public void OnSaveRecordingSuccess() {
        //Close Save Recording Menu
        saveRecordingPanel.SetActive(false);
    }


    void OnSaveRecordingDuplicateException() {
        //Show Overwrite Popup
        saveOverwritePanel.SetActive(true);
    }


    public void OnSaveRecordingButtonPressed() {
        if (!string.IsNullOrEmpty(recordingInputField.text))
            ReplayManager.Instance.SaveRecording(recordingInputField.text);
        else
            Debug.LogError("Enter a valid name please!!");
    }

    #endregion
}
