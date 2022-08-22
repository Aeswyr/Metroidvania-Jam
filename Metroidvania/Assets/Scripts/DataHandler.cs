using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DataHandler : Singleton<DataHandler>
{
    [System.Serializable]
    public struct GameData
    {
        public List<ObjectState> objectStates;
        public int levelIndex;
        public int spawnIndex;
        public List<string> playerProperties;
    }

    public struct ObjectState
    {
        public int dataID;
        public bool active;
        public bool destroyed;
    }

    [System.NonSerialized]
    public List<int> managedObjectIDs = new List<int>();
    [System.NonSerialized]
    public Hashtable managedObjects = new Hashtable();

    public GameData gameData;

    public string Save()
    {
        gameData.objectStates = new List<ObjectState>();
        foreach(int id in managedObjectIDs)
        {
            gameData.objectStates.Add((ObjectState)managedObjects[id]);
        }
        string json = JsonUtility.ToJson(gameData);
        return json;
    }

    public void Load(string json)
    {
        gameData = JsonUtility.FromJson<GameData>(json);
        managedObjectIDs = new List<int>();
        managedObjects = new Hashtable();
        foreach(ObjectState state in gameData.objectStates)
        {
            managedObjectIDs.Add(state.dataID);
            managedObjects.Add(state.dataID, state);
        }
        GameHandler.Instance.DelayLoadLevel((LevelType)levelIndex, spawnIndex);
    }

    public bool hasProperty(string property)
    {
        return gameData.playerProperties.Contains(property);
    }

    public void AddProperty(string property)
    {
        gameData.playerProperties.Add(property);
    }

    public void RemoveProperty(string property)
    {
        gameData.playerProperties.Remove(property);
    }
}
