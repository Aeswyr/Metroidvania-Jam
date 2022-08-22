using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class LoadedState : MonoBehaviour
{
    public int dataID;

    void Awake()
    {
        if(DataHandler.Instance.managedObjects.ContainsKey(dataID))
        {
            DataHandler.ObjectState state = (DataHandler.ObjectState)DataHandler.Instance.managedObjects[dataID];
            if(state.destroyed)
            {
                GameObject.Destroy(gameObject);
            } else
            {
                gameObject.SetActive(state.active);
            }
        }
        else
        {
            DataHandler.ObjectState state;
            state.dataID = dataID;
            state.active = gameObject.activeSelf;
            state.destroyed = false;
            DataHandler.Instance.managedObjectIDs.Add(dataID);
            DataHandler.Instance.managedObjects.Add(dataID, state);
        }
    }

    void OnEnable()
    {
        DataHandler.ObjectState state;
        state.dataID = dataID;
        state.active = true;
        state.destroyed = false;
        DataHandler.Instance.managedObjects[dataID] = state;
    }

    void OnDisable()
    {
        DataHandler.ObjectState state;
        state.dataID = dataID;
        state.active = false;
        state.destroyed = false;
        DataHandler.Instance.managedObjects[dataID] = state;
    }

    void Destroy()
    {
        DataHandler.ObjectState state;
        state.dataID = dataID;
        state.active = gameObject.activeSelf;
        state.destroyed = true;
        DataHandler.Instance.managedObjects[dataID] = state;
        GameObject.Destroy(gameObject);
    }
}



// Assign a unique ID to this object
[CustomEditor (typeof(LoadedState))]
public class LoadedStateIntializer : Editor
{
    void OnEnable()
    {
        SerializedObject serializedObject = new SerializedObject(target);
        if(serializedObject.FindProperty("dataID").intValue == 0)
        {
            serializedObject.FindProperty("dataID").intValue = Mathf.FloorToInt(Random.value * int.MaxValue);
            serializedObject.ApplyModifiedProperties();
        }
    }
}
