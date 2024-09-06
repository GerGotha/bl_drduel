using NetworkMessages.FromServer;
using TaleWorlds.MountAndBlade;

namespace AdimiToolsShared;

public class AdimiToolsNotifier
{
    public static void SendMessageToAllPeers(string message)
    {
        GameNetwork.BeginBroadcastModuleEvent();
        GameNetwork.WriteMessage(new ServerMessage("[!]: " + message));
        GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.IncludeUnsynchronizedClients);
    }

    public static void ServerSendMessageToPlayer(NetworkCommunicator targetPlayer, string message)
    {
        if (targetPlayer.IsServerPeer == false)
        {
            if (!targetPlayer.IsInBList || !targetPlayer.IsInCList)
            {
                GameNetwork.BeginModuleEventAsServer(targetPlayer);
                GameNetwork.WriteMessage(new ServerMessage("[!]: " + message, false));
                GameNetwork.EndModuleEventAsServer();
            }
        }
    }

    public static void AdminAnnouncement(NetworkCommunicator? networkPeer, string message, bool isBroadcast)
    {
        if (networkPeer == null)
        {
            GameNetwork.BeginBroadcastModuleEvent();
            GameNetwork.WriteMessage(new ServerAdminMessage(message, isBroadcast));
            GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.None, null);
            return;
        }

        GameNetwork.BeginModuleEventAsServer(networkPeer);
        GameNetwork.WriteMessage(new ServerAdminMessage(message, isBroadcast));
        GameNetwork.EndModuleEventAsServer();
    }
}
