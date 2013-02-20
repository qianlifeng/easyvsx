using System;
using System.Collections.Generic;
using System.Text;

namespace easyvsx
{
    [AttributeUsage(AttributeTargets.Class)]
    public class CommandIDAttribute:Attribute
    {
        private readonly Guid guid;
        private readonly uint command;

        public CommandIDAttribute(string guid, uint command)
        {
            this.guid = new Guid(guid);
            this.command = command;
        }

        public Guid Guid
        {
            get { return guid; }
        }

        public uint Command
        {
            get { return command; }
        }
    }
}
