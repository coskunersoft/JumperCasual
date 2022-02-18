using System.Collections.Generic;
using UnityEngine;
using Coskunerov.Actors;
using Cinemachine;

public class CameraActor : GameSingleActor<CameraActor>
{
    [Header("All CameraDatas")]
    public List<CameraProfil> cameraProfils;

    public void SwitchCamera(CameraType currentcamera)
    {
        CameraProfil findedcamera= cameraProfils.Find(x => x.cameraType == currentcamera);
        if (findedcamera == null) return;
        cameraProfils.ForEach(x => x.camera.gameObject.SetActive(false));
        findedcamera.camera.gameObject.SetActive(true);
    }

    public void SetTarget(Transform target,bool setNormal=false)
    {
        cameraProfils.ForEach(x =>
        {
            x.camera.Follow = target;
            x.camera.LookAt = target;
        });
        if (setNormal) SwitchCamera(CameraType.CharacterFollow);
    }

    [System.Serializable]
    public class CameraProfil
    {
        public CameraType cameraType;
        public CinemachineVirtualCamera camera;
    }
}
public enum CameraType
{
    CharacterFollow
}
