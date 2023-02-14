using UnityEngine;
using UnityEngine.UI;

public class ReplayPanelManager : MonoBehaviour
{
    #region Parameters
    //Button used to start and stop the recording
    public Button startStopRecordButton;

    //record button's text component
    public Text startStopRecordButtonText;

    //Button used to start and stop the replay of the selected recording
    public Button startStopReplayButton;

    //the replay button's text component
    public Text startStopReplayButtonText;
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
    }

    void OnStoppedRecording()
    {
        startStopRecordButtonText.text = "Start Recording";
        startStopReplayButton.interactable = true;
    }

    void OnStartedReplaying()
    {
        startStopReplayButtonText.text = "Stop Replay";
        startStopRecordButton.interactable = false;
    }

    void OnStoppedReplaying()
    {
        startStopReplayButtonText.text = "Start Replay";
        startStopRecordButton.interactable = true;
    }
    #endregion
}
