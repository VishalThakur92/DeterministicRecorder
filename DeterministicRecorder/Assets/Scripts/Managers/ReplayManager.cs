using System;
using UnityEngine;
using System.IO;

public class ReplayManager : MonoBehaviour
{

    #region Parameters
    //Singelton Instance
    public static ReplayManager Instance { get; private set; }

    [SerializeField, Multiline]
    string streamData;


    //List of Objects whose transform shall be recorded
    [SerializeField]
    Shape[] recordableObjects;

    private MemoryStream memoryStream = null;
    private BinaryWriter binaryWriter = null;
    private BinaryReader binaryReader = null;

    private bool recordingInitialized = false;
    private bool recording = false;
    private bool replaying = false;

    [SerializeField]
    private int currentRecordingFrames = 0;
    public int maxRecordingFrames = 360;

    public int replayFrameLength = 2;
    private int replayFrameTimer = 0;

    public Action OnStartedRecording;
    public Action OnStoppedRecording;
    public Action OnStartedReplaying;
    public Action OnStoppedReplaying;
    public Action OnSaveRecordingSuccess;
    #endregion


    #region Unity Methods
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
    #endregion

    #region Core
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

    private void InitializeRecording()
    {
        memoryStream = new MemoryStream();
        binaryWriter = new BinaryWriter(memoryStream);
        binaryReader = new BinaryReader(memoryStream);
        recordingInitialized = true;
    }

    private void StartRecording()
    {
        if (!recordingInitialized)
        {
            InitializeRecording();
        }
        else
        {
            memoryStream.SetLength(0);
        }
        ResetReplayFrame();

        StartReplayFrameTimer();
        recording = true;
        if (OnStartedRecording != null)
        {
            OnStartedRecording();
        }
    }

    private void UpdateRecording()
    {
        if (currentRecordingFrames > maxRecordingFrames)
        {
            StopRecording();
            currentRecordingFrames = 0;
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

    private void StopRecording()
    {
        recording = false;

        if (OnStoppedRecording != null)
        {
            OnStoppedRecording();
        }
    }

    private void ResetReplayFrame()
    {
        memoryStream?.Seek(0, SeekOrigin.Begin);
        binaryWriter?.Seek(0, SeekOrigin.Begin);
    }

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

    private void StartReplaying()
    {
        ResetReplayFrame();
        StartReplayFrameTimer();
        replaying = true;
        if (OnStartedReplaying != null)
        {
            OnStartedReplaying();
        }
    }

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

    private void StopReplaying()
    {
        replaying = false;
        if (OnStoppedReplaying != null)
        {
            OnStoppedReplaying();
        }
    }

    private void ResetReplayFrameTimer()
    {
        replayFrameTimer = replayFrameLength;
    }

    private void StartReplayFrameTimer()
    {
        replayFrameTimer = 0;
    }

    public void SaveRecording(string recordingName) {

        //Check if a file with the same name exists
        //Yes 
        //if (AppManager.Instance.fileIOManager.DoesFileExist(Application.persistentDataPath, recordingName))
        //{
        //    //Show Overwrite Popup
        //    OnSaveRecordingDuplicateException();
        //}
        //No - Save File
        //else {
            AppManager.Instance.fileIOManager.WriteMemorySteamToFile(memoryStream ,Path.Combine(Application.persistentDataPath ,recordingName + ".bin"));

            //TODO Call this only when we get success from the FileStream Writer , for now it assumes success only but there may be an exception , and it would be good to handle that exception
            OnSaveRecordingSuccess();
        //}
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


    void SaveColor(Shape shape) {
        binaryWriter.Write(shape.GetCurrentColorIndex());
    }

    private void SaveTransform(Transform transform)
    {
        //Save Position X and Y
        binaryWriter.Write(transform.localPosition.x);
        binaryWriter.Write(transform.localPosition.y);
    }




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



    void LoadColor(Shape shape)
    {
        //Load and Set Color Index
        shape.SetColor(binaryReader.ReadInt32());
    }
    private void LoadTransform(Transform transform)
    {
        //Load and Set Transform position
        transform.localPosition = new Vector3(binaryReader.ReadSingle(), binaryReader.ReadSingle(), transform.localPosition.z);
    }
    #endregion
}
