using TaleWorlds.Engine;
using TaleWorlds.MountAndBlade;

namespace AdimiDuel_Server.Duel;

internal class AdimiToolsTeleportDoorStandingPoint : StandingPoint
{
    private GameEntity? _parentDoor;

    public override void OnUse(Agent userAgent)
    {
        userAgent.StopUsingGameObject(isSuccessful: true, Agent.StopUsingGameObjectFlags.None);
        _parentDoor?.GetFirstScriptOfType<AdimiToolsTeleportDoors>()?.OnUse(userAgent);
    }

    public override string GetDescriptionText(GameEntity gameEntity)
    {
        return "Teleport";
    }

    public void SetParentDoor(GameEntity parentDoor)
    {
        _parentDoor = parentDoor;
    }

    protected override void OnInit()
    {
        base.OnInit();
    }
}
