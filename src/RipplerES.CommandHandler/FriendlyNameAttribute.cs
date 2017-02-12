using System;

namespace RipplerES.CommandHandler
{
    [AttributeUsage(AttributeTargets.Class |  AttributeTargets.Struct)]
    public class FriendlyName : Attribute
    {
        public string Name { get; set; }

        public FriendlyName(string name)
        {
            this.Name = name;
        }
    }
}
