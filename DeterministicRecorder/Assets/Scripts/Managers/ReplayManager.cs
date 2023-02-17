using System;
using UnityEngine;
using System.IO;

//This class manages the Recording and Replay Functionality
public class ReplayManager : MonoBehaviour
{

    #region Parameters
    //Singelton Instance
    public static ReplayManager Instance { get; private set; }

    //List of Objects whose transform shall be recorded
    [SerializeField]
    Shape[] recordableObjects;

    //Memory Stream in which data shall be recorded in a Deterministic manner
    private MemoryStream memoryStream = null;
    private BinaryWriter binaryWriter = null;
    private BinaryReader binaryReader = null;


    //Is Recording ?
    private bool recording = false;

    //Is replaying ?
    private bool replaying = false;


    //The Current Recording Frame
    [SerializeField]
    private int currentRecordingFrames = 0;

    //Max Frames to be recorded, Recording shall stop after this many frames has been recorded
    public int maxRecordingFrames = 360;

    public int replayFrameLength = 2;
    private int replayFrameTimer = 0;


    //Event used to Notify Record/Replay callbacks
    public Action OnStartedRecording;
    public Action OnStoppedRecording;
    public Action OnStartedReplaying;
    public Action OnStoppedReplaying;
    public Action OnSaveRecordingSuccess;
    #endregion


    #region Core
    void Awake()
    {
        //Singelton Instance Handling Algorithm
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        Instance = this;
    }

    //Handle Recording/Replaying
    public void FixedUpdate()
    {
        if (recording)
        {
            UpdateRecording();
        }
        else if (replaying)
        {
            UpdateReplaying();
        }
    }

    //Initialize the MemoryStream,BinaryWriter,BinaryReader so that they can be used by the Data Load/Save methods
    void InitializeRecording()
    {
        memoryStream = new MemoryStream();
        binaryWriter = new BinaryWriter(memoryStream);
        binaryReader = new BinaryReader(memoryStream);
    }

    //Start/Stop Recording behaviour
    public void StartStopRecording()
    {
        if (!recording)
        {
            StartRecording();
        }
        else
        {
            StopRecording();
        }
    }


    //Load the specified Recording from the FileSystem
    public void LoadRecording(string recordingName) {

        //File path
        string filePath = Path.Combine(Application.persistentDataPath, recordingName + ".bin");

        //check if file Exists
        //yes - Load into stream and play recording
        if (AppManager.Instance.fileIOManager.DoesFileExist(filePath))
        {
            //memoryStream.Dispose();
            //using (MemoryStream ms = new MemoryStream())
            using (FileStream file = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                byte[] bytes = new byte[file.Length];
                file.Read(bytes, 0, (int)file.Length);
                memoryStream = new MemoryStream();
                memoryStream.Write(bytes, 0, (int)file.Length);
            }

            //binaryReader.Dispose();
            binaryReader = new BinaryReader(memoryStream);

            //binaryReader = AppManager.Instance.fileIOManager.ReadSteamFromFile(filePath);
            StartStopReplaying();
        }
        //No - do nothing for now
        else
        {
            Debug.LogError("Recording Not found at " + filePath);
        }
    }


    //Starts the Recording
    void StartRecording()
    {
        InitializeRecording();

        //Resets the replay frame, for fresh replay start
        ResetReplayFrame();

        StartReplayFrameTimer();
        recording = true;
        if (OnStartedRecording != null)
        {
            //Fire Event for Notify
            OnStartedRecording();
        }
    }


    //Is Recording behaviour
    void UpdateRecording()
    {
        //cap the recording to avoid Endless recording
        if (currentRecordingFrames > maxRecordingFrames)
        {
            StopRecording();
            return;
        }

        if (replayFrameTimer == 0)
        {
            SaveData(recordableObjects);
            ResetReplayFrameTimer();
        }
        --replayFrameTimer;
        ++currentRecordingFrames;
    }


    //Stops the Recording
    void StopRecording()
    {
        recording = false;

        //Reset Current Recording Frames
        currentRecordingFrames = 0;
        if (OnStoppedRecording != null)
        {
            OnStoppedRecording();
        }
    }


    //Resets the Replay Seek Frame for fresh Loading
    private void ResetReplayFrame()
    {
        memoryStream?.Seek(0, SeekOrigin.Begin);
        binaryWriter?.Seek(0, SeekOrigin.Begin);
    }


    //Replay Start/Stop Behaviour, Acts in toggle manner
    public void StartStopReplaying()
    {
        if (!replaying)
        {
            StartReplaying();
        }
        else
        {
            StopReplaying();
        }
    }

    //Start Replay
    void StartReplaying()
    {
        ResetReplayFrame();
        StartReplayFrameTimer();
        replaying = true;
        if (OnStartedReplaying != null)
        {
            OnStartedReplaying();
        }
    }


    //replay Update Behaviour
    private void UpdateReplaying()
    {
        if (memoryStream.Position >= memoryStream.Length)
        {
            StopReplaying();
            return;
        }

        if (replayFrameTimer == 0)
        {
            LoadData(recordableObjects);
            ResetReplayFrameTimer();
        }
        --replayFrameTimer;
    }

    //Stop Replay
    private void StopReplaying()
    {
        replaying = false;
        if (OnStoppedReplaying != null)
        {
            //Fire Event to Notify
            OnStoppedReplaying();
        }
    }

    //Resets The Replay Frame timer to the specified value
    void ResetReplayFrameTimer()
    {
        replayFrameTimer = replayFrameLength;
    }

    //Sets The Replay Frame timer to 0 or the begining
    private void StartReplayFrameTimer()
    {
        replayFrameTimer = 0;
    }

    //Save the Specified Recording using the File system
    public void SaveRecording(string recordingName) {
        AppManager.Instance.fileIOManager.WriteMemorySteamToFile(memoryStream ,Path.Combine(Application.persistentDataPath ,recordingName + ".bin"));

        //TODO Call this only when we get success from the FileStream Writer , for now it assumes success only but there may be an exception , and it would be good to handle that exception
        OnSaveRecordingSuccess();
    }


    //Save data to the Memory Stream
    private void SaveData(Shape[] shapes)
    {

        //----Save Tooltip Data-------
        //Grab tooltip active state
        bool tooltipActiveState = AppManager.Instance.tooltipManager.GetIsActive();

        //Save Tooltip State active state
        binaryWriter.Write(tooltipActiveState);

        //If tooltip is in active state
        if (tooltipActiveState)
        {
            //Save Tooltip text 
            binaryWriter.Write(AppManager.Instance.tooltipManager.GetText());

            //Save Tooltip position X
            binaryWriter.Write(AppManager.Instance.tooltipManager.GetPosition().x);

            //Save Tooltip position Y
            binaryWriter.Write(AppManager.Instance.tooltipManager.GetPosition().y);
        }
        //----------------------------


        //----Save popup Data-------
        //Grab popup active state
        bool popupActiveState = AppManager.Instance.popupManager.GetIsActive();

        //Save popup State active state
        binaryWriter.Write(popupActiveState);

        //If popup is in active state
        if (popupActiveState)
        {
            //Save popup text 
            binaryWriter.Write(AppManager.Instance.popupManager.GetText());
        }
        //----------------------------



        //-----Save Transform and Color Data------
        for (int i=0;i<shapes.Length;i++)
        {
            //Save Transforms of each obj
            SaveTransform(shapes[i].transform);

            //Save Color of each obj
            SaveColor(shapes[i]);
        }
        //----------------------------------------
    }


    //Save the Color of the shape
    void SaveColor(Shape shape) {
        binaryWriter.Write(shape.GetCurrentColorIndex());
    }

    private void SaveTransform(Transform transform)
    {
        //Save Position X and Y
        binaryWriter.Write(transform.localPosition.x);
        binaryWriter.Write(transform.localPosition.y);
    }



    //Load data From Binary Reader used To replay a recording
    private void LoadData(Shape[] shapes)
    {
        //----Load Tooltip Data-------
        //Grab tooltip active state
        bool tooltipActiveState = binaryReader.ReadBoolean();//AppManager.Instance.tooltipManager.GetIsActive();

        //If tooltip is in active state
        if (tooltipActiveState)
        {
            //Show tooltip with the Text at a position
            AppManager.Instance.tooltipManager.Toggle(true , binaryReader.ReadString() , new Vector2(binaryReader.ReadSingle() , binaryReader.ReadSingle()));
        }
        else {
            //Disable Tooltip
            AppManager.Instance.tooltipManager.Toggle(false);
        }
        //----------------------------


        //----Load popup Data-------
        //Grab popup active state
        bool popupActiveState = binaryReader.ReadBoolean();

        //If popup is in active state
        if (popupActiveState)
        {
            //Show Popup with the Text
            AppManager.Instance.popupManager.Toggle(true, binaryReader.ReadString());
        }
        else{
            //Disable Popup
            AppManager.Instance.popupManager.Toggle(false);
        }
        //----------------------------


        //-----Load Transform and Color Data------
        for (int i = 0; i < shapes.Length; i++) {
            LoadTransform(shapes[i].transform);
            LoadColor(shapes[i]);
        }
        //----------------------------------------
    }


    //Loads color of the specified obj, used to replay the color related values
    void LoadColor(Shape shape)
    {
        //Load and Set Color Index
        shape.SetColor(binaryReader.ReadInt32());
    }

    //Loads Transform of the specified obj, used to replay the transform related values
    private void LoadTransform(Transform transform)
    {
        //Load and Set Transform position
        transform.localPosition = new Vector3(binaryReader.ReadSingle(), binaryReader.ReadSingle(), transform.localPosition.z);
    }
    #endregion
}
