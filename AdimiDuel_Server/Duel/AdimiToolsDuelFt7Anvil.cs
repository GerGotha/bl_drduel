using AdimiToolsShared;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;
using static AdimiDuel_Server.Duel.AdimiToolsMissionMultiplayerDuel;

namespace AdimiDuel_Server.Duel;

internal class AdimiToolsDuelFt7Anvil : SynchedMissionObject
{
    protected override void OnInit()
    {
        base.OnInit();
    }

    protected override bool OnHit(Agent attackerAgent, int damage, Vec3 impactPosition, Vec3 impactDirection, in MissionWeapon weapon, ScriptComponentBehavior attackerScriptComponentBehavior, out bool reportDamage)
    {
        reportDamage = false;
        if (attackerAgent != null && attackerAgent.IsActive() && attackerAgent.IsHuman)
        {
            MissionPeer missionPeer = attackerAgent.MissionPeer;
            NetworkCommunicator? networkPeer = missionPeer.GetNetworkPeer();
            if (missionPeer == null || networkPeer == null)
            {
                return false;
            }

            if (missionPeer.Team.IsDefender)
            {
                AdimiToolsNotifier.ServerSendMessageToPlayer(networkPeer, $"You cannot change the duel mode mid duel.");
                return false;
            }

            if (PlayersDuelConfig.TryGetValue(missionPeer.Peer.Id, out DuelConfig? duelConfig))
            {
                if (!duelConfig.FirstToSeven)
                {
                    duelConfig.FirstToSeven = true;
                    duelConfig.FirstToLimit = 7;
                    AdimiToolsNotifier.ServerSendMessageToPlayer(networkPeer, $"You are now in a first to {duelConfig.FirstToLimit} mode. You can hit the anvil to stop the first to 7 mode or use the !ft 1-10 command.");
                }
                else
                {
                    duelConfig.FirstToSeven = false;
                    duelConfig.FirstToLimit = 1;
                    AdimiToolsNotifier.ServerSendMessageToPlayer(networkPeer, $"You are now in a first to 1 mode.");
                }
            }
            else
            {
                AdimiToolsNotifier.ServerSendMessageToPlayer(networkPeer, $"An error occured.");
            }
        }

        return false;
    }
}
