using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostSystem : MonoBehaviour
{
    public GameObject recordTarget;                 // GameObject whose position will be recorded
    public GameObject replayGhostTarget;                  // GameObject which will act as a Ghost of a record target during replay

    private GhostShot[] shots;                       // Stores the snapshots of the latest recording

    public float maxRecordTime = 10;                // Maximum recording time in seconds
    public float snapshotFrequency = 0.1f;          // How often should position and rotation be recorded?

    public bool isRecording;
    public bool isReplaying;

    public event EventHandler RecordingStarted;
    public event EventHandler RecordingEnded;

    public event EventHandler ReplayStarted;
    public event EventHandler ReplayEnded;

    private int recordingIndex = 0;
    private int replayIndex = 0;

    public float replayTimescale = 1;              // change this value below 1 to make replays slower and above 1 to make them faster

    private float recordingTime = 0.0f;            // the total number of seconds for recording
    private float playbackTime = 0.0f;         // current time at which the replay is at, used for replaying the recording

    public void StartRecording()
    {
        if (!IsRecording() && !IsReplaying())
        {
            int arrSize = (int)(maxRecordTime / snapshotFrequency);
            shots = new GhostShot[arrSize];           //create an array of potential recordings
            recordingIndex = 0;
            recordingTime = Time.time;

            isRecording = true;
            OnRecordingStart();
            Debug.Log("Recording began with and can hold up to " + arrSize + " frames");

            InvokeRepeating("RecordSnapshot", 0, snapshotFrequency);                    // start recording snapshots at a specified frequency
        }
    }

    void RecordSnapshot()
    {
        if (IsRecording())
        {
            if (recordingIndex < shots.Length)
            {
                recordingTime += Time.deltaTime;                                // increase the elapsed time
                GhostShot newShot = new GhostShot();                       // create a new GhostShot


                newShot.timeMark = recordingTime;                              // mark the beginning time
                newShot.posMark= recordTarget.transform.position;
                newShot.rotMark = recordTarget.transform.rotation;

                shots[recordingIndex] = newShot;                                // allocate the new shot to a specified array item

                recordingIndex++;                                               // increase the index
            }
            else
            {
                StopRecording();
            }
        }
    }

    public void StopRecording()
    {
        if (IsRecording())
        {
            CancelInvoke();

            if (recordingIndex < shots.Length)              // if the recording has ended prematurely
            {
                Debug.Log("Recording ended prematurely at frame " + recordingIndex);
                shots[recordingIndex - 1].isFinal = true;
            }
            else
            {
                shots[shots.Length - 1].isFinal = true;     // otherwise, mark the last shot as Final
            }

            isRecording = false;

            OnRecordingEnd();
        }
    }

    public void StartReplay()
    {
        if (!IsReplaying())
        {
            replayIndex = 0;

            replayGhostTarget.transform.position = shots[0].posMark;
            replayGhostTarget.transform.rotation = shots[0].rotMark;

            replayGhostTarget.SetActive(true);

            isReplaying = true;

            OnReplayStart();
        }
    }

    private void Update()
    {
        if (IsReplaying())
        {
            if (replayIndex < shots.Length)                                             // if the current index is within the boundaries of the array...
            {
                GhostShot shot = shots[replayIndex];

                if (!shot.isFinal)                                        // the current shot is not final...
                {
                    if (playbackTime < shot.timeMark)
                    {
                        if (replayIndex == 0)       // if this is a the first shot mark...
                        {
                            playbackTime = shot.timeMark;
                        }
                        else
                        {
                            LerpGhost(replayGhostTarget, shots[replayIndex - 1], shot);
                            playbackTime += Time.deltaTime * replayTimescale;
                        }
                    }
                    else
                    {
                        replayIndex++;
                    }
                }
                else
                {
                    StopReplay();
                }
            }
            else
            {
                StopReplay();
            }
        }
    }

    private void LerpGhost(GameObject ghost, GhostShot a, GhostShot b)
    {
        ghost.transform.position = Vector3.Slerp(a.posMark, b.posMark, Mathf.Clamp(playbackTime, a.timeMark, b.timeMark));
        ghost.transform.rotation = Quaternion.Slerp(a.rotMark, b.rotMark, Mathf.Clamp(playbackTime, a.timeMark, b.timeMark));
    }

    public void StopReplay()
    {
        if (IsReplaying())
        {
            replayIndex = 0;
            playbackTime = 0.0f;

            replayGhostTarget.SetActive(false);
            isReplaying = false;

            OnReplayEnd();
        }
    }

    public bool IsRecording()
    {
        return isRecording;
    }

    public bool IsReplaying()
    {
        return isReplaying;
    }

    protected virtual void OnRecordingStart()
    {
        if (RecordingStarted != null)
        {
            RecordingStarted.Invoke(this, EventArgs.Empty);
        }
    }

    protected virtual void OnRecordingEnd()
    {
        if (RecordingEnded != null)
        {
            RecordingEnded.Invoke(this, EventArgs.Empty);
        }
    }

    protected virtual void OnReplayStart()
    {
        if (ReplayStarted != null)
        {
            ReplayStarted.Invoke(this, EventArgs.Empty);
        }
    }

    protected virtual void OnReplayEnd()
    {
        if (ReplayEnded != null)
        {
            ReplayEnded.Invoke(this, EventArgs.Empty);
        }
    }
}