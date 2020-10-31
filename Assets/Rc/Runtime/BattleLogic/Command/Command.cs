using System.Collections.Generic;

namespace BattleLogic
{
    public abstract class Command
    {
        string name = "";
        public string Name => name;

        Status[] senders = new Status[0];
        public IReadOnlyList<Status> Senders => senders;

        Status[] recievers = new Status[0];
        public IReadOnlyList<Status> Recievers => recievers;

        object result = null;
        public object Result => result;

        public Command(string _name)
        {
            name = _name;
        }

        public void SetSenders(params Status[] _senders)
        {
            senders = _senders;
        }

        public void SetRecievers(params Status[] _recievers)
        {
            recievers = _recievers;
        }

        protected void SetResult(object _result)
        {
            result = _result;
        }

        public T GetResult<T>() where T : class
        {
            return result as T;
        }

        abstract public void Execute();
    }
}