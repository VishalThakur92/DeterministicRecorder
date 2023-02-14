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
        memoryStream.Seek(0, SeekOrigin.Begin);
        binaryWriter.Seek(0, SeekOrigin.Begin);
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

    private void SaveData(Shape[] shapes)
    {
        for(int i=0;i<shapes.Length;i++)
        {
            SaveTransform(shapes[i].transform);
            SaveColor(shapes[i]);
        }
    }


    void SaveColor(Shape shape) {
        binaryWriter.Write(shape.currentColorIndex);
    }

    private void SaveTransform(Transform transform)
    {
        binaryWriter.Write(transform.localPosition.x);
        binaryWriter.Write(transform.localPosition.y);
        binaryWriter.Write(transform.localPosition.z);
        binaryWriter.Write(transform.localScale.x);
        binaryWriter.Write(transform.localScale.y);
        binaryWriter.Write(transform.localScale.z);
    }




    private void LoadData(Shape[] shapes)
    {
        for (int i = 0; i < shapes.Length; i++) {
            LoadTransform(shapes[i].transform);
            LoadColor(shapes[i]);
        }
    }



    void LoadColor(Shape shape)
    {
        int colorIndex = binaryReader.ReadInt32();
        shape.SetColor(colorIndex);
        Debug.LogError("Color = " +  colorIndex);
    }
    private void LoadTransform(Transform transform)
    {

        //Debug.LogError("pos x" + binaryReader.ReadDouble().ToString());
        //Debug.LogError("pos y" + binaryReader.ReadDouble().ToString());
        //Debug.LogError("pos z" + binaryReader.ReadDouble().ToString());

        //Debug.LogError("scale x" + binaryReader.ReadSingle().ToString());
        //Debug.LogError("scale y" + binaryReader.ReadSingle().ToString());
        //Debug.LogError("scale z" + binaryReader.ReadSingle().ToString());
        //return;
        float x = binaryReader.ReadSingle();
        float y = binaryReader.ReadSingle();
        float z = binaryReader.ReadSingle();
        transform.localPosition = new Vector3(x, y, z);
        x = binaryReader.ReadSingle();
        y = binaryReader.ReadSingle();
        z = binaryReader.ReadSingle();
        transform.localScale = new Vector3(x, y, z);
    }
    #endregion
}
