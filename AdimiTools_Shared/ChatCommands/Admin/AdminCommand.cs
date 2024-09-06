using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.MountAndBlade;

namespace AdimiToolsShared.ChatCommands;

internal abstract class AdminCommand : ChatCommand
{
    protected AdminCommand()
    {
    }

    protected override bool CheckRequirements(NetworkCommunicator fromPeer)
    {
        return fromPeer.IsAdmin;
    }
}
