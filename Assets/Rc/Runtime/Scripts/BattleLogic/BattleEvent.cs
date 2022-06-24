using System.Collections.Generic;

namespace BattleLogic
{
    public delegate void OnCommandAddedEvent(Command addedCommand, IReadOnlyList<Command> commands);
    public delegate void OnCommandExecuteBeforeEvent(Command command);
    public delegate void OnCommandExecuteAfterEvent(Command command);

    public delegate void OnBattleStartEvent();
    public delegate void OnBattleEndEvent();
    public delegate void OnBattleTurnStartEvent();
    public delegate void OnBattleTurnEndEvent();
}