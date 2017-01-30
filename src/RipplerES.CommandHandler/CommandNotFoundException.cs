using System;

namespace RipplerES.CommandHandler
{
    public class CommandNotFoundException : Exception
    {
        public Type Type { get; set; }
        public Type CommandType { get; set; }

        public CommandNotFoundException(Type type, Type commandType)
        {
            Type = type;
            CommandType = commandType;
        }
    }
}