using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Coskunerov;
using Coskunerov.Actors;

public class CustomLevelActor : LevelActor
{
    public Transform playerPoint;
    public Transform botPoint;

    public override void SetupLevel()
    {
        base.SetupLevel();

        GameObject playerCreated = Instantiate(GameData.Instance.PlayerPrefab, playerPoint.position, Quaternion.identity);
        CameraActor.Instance.SetTarget(playerCreated.transform,true);
        playerCreated.transform.SetParent(transform);

        GameObject botCreated = Instantiate(GameData.Instance.BotPrefab, botPoint.position, Quaternion.identity);
        botCreated.transform.SetParent(transform);

    }
}
