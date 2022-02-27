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
        if (playerCreated.TryGetComponent(out Player player))
        {
            player.UpdateCheckPoint(playerPoint);
        }


        GameObject botCreated = Instantiate(GameData.Instance.BotPrefab, botPoint.position, Quaternion.identity);
        botCreated.transform.SetParent(transform);
        if (botCreated.TryGetComponent(out BotPlayer botPlayer))
        {
            botPlayer.UpdateCheckPoint(botPoint);
        }
        Coskunerov.Managers.GameManager.Instance.StartLevel();

    }

[Coskunerov.EventBehaviour.Attributes.GE(Coskunerov.EventBehaviour.BaseGameEvents.FinishGame)]    
    private void WinGame()
    {
        Jumper.Tutorial = true;
    }
}
