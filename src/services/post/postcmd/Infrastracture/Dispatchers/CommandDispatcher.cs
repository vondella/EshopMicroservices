﻿
using buildingBlock.CQRS.Commands;

namespace postcmd.Infrastracture.Dispatchers
{
    public class CommandDispatcher : ICommandDispatcher
    {
        private readonly Dictionary<Type, Func<BaseCommand, Task>> _handlers = new Dictionary<Type, Func<BaseCommand, Task>>();
        public void RegisterHandler<T>(Func<T, Task> handler) where T : BaseCommand
        {
            if(_handlers.ContainsKey(typeof(T)))
            {
                throw new IndexOutOfRangeException("you cannot register the same type of command twice");
            }

            _handlers.Add(typeof(T), x => handler((T)x));            
        }

        public async Task SendAsync(BaseCommand command)
        {
            if(_handlers.TryGetValue(command.GetType(),out Func<BaseCommand,Task> handler))
            {
                await handler(command);
            }
            else
            {
                throw new ArgumentNullException(nameof(handler),"no handler has been registered");
            }
        }
    }
}
