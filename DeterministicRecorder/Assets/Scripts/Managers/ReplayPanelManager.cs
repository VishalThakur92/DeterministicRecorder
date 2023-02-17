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

    [SerializeField]
    InputField recordingInputField;

    #endregion



    #region Unity Methods
    private void OnEnable()
    {
        //Subscribe to recording Callbacks
        ReplayManager.Instance.OnStoppedRecording += OnStoppedRecording;
        ReplayManager.Instance.OnStartedRecording += OnStartedRecording;
        ReplayManager.Instance.OnStartedReplaying += OnStartedReplaying;
        ReplayManager.Instance.OnStoppedReplaying += OnStoppedReplaying;
    }

    private void OnDisable()
    {
        //UnSubscribe from recording Callbacks
        ReplayManager.Instance.OnStartedRecording -= OnStartedRecording;
        ReplayManager.Instance.OnStoppedRecording -= OnStoppedRecording;
        ReplayManager.Instance.OnStartedReplaying -= OnStartedReplaying;
        ReplayManager.Instance.OnStoppedReplaying -= OnStoppedReplaying;
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
    #endregion
}
