using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class GameHandler : Singleton<GameHandler>
{
    [Header("Scene data")]
    [SerializeField] private CinemachineVirtualCamera vCam;

    [Header("Level data")]
    [SerializeField] private LevelList levels;

    [Header("VFX")]
    [SerializeField] private GameObject screenWipe;
    
    private LevelHandler currentLevel;

    public void Start() {
        // TODO make this actually load from level data
        LoadLevel(default, 0);
    }
    public void GotoLevel(LevelType levelType, int spawnPointID) {
        StartCoroutine(DelayLoadLevel(levelType, spawnPointID));
    }

    private IEnumerator DelayLoadLevel(LevelType levelType, int spawnPointID) {
        PlayerHandler player = FindObjectOfType<PlayerHandler>();
        player.DisableInputs();

        GameObject wipe = Instantiate(screenWipe, Camera.main.transform);
        wipe.transform.position += new Vector3(0, 0, 10);
        Animator wipeanim = wipe.GetComponent<Animator>();
        wipeanim.SetTrigger("StartWipe");

        yield return new WaitForSeconds(0.5f);

        LoadLevel(levelType, spawnPointID);

        yield return new WaitForSeconds(0.3f);

        wipeanim.SetTrigger("EndWipe");
        yield return new WaitForSeconds(0.1f);
        player.EnableInputs();
        yield return new WaitForSeconds(0.4f);
        Destroy(wipe);
    }

    private void LoadLevel(LevelType levelType, int spawnPointID) {
        if (currentLevel != null)
            Destroy(currentLevel.gameObject);
        
        GameObject level = levels.GetLevel(levelType);
        level = Instantiate(level, Vector3.zero, Quaternion.identity);
        currentLevel = level.GetComponent<LevelHandler>();
        
        var camBounds = vCam.GetComponent<CinemachineConfiner2D>();
        camBounds.m_BoundingShape2D = currentLevel.GetCameraBounds();
        camBounds.InvalidateCache();

        Vector3 position = currentLevel.GetSpawn(spawnPointID).GetSpawnPos();
        position.z = 0;
        FindObjectOfType<PlayerHandler>().transform.position = position;
        position.z = -10;
        vCam.ForceCameraPosition(position, Quaternion.identity);
    }
}
