using System;
using WarmPack.Classes;

namespace WarmPack.Database
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class ConexionColumnAttribute : Attribute
    {
        public string Name { get; set; }

        public ConexionColumnAttribute(string name)
        {
            this.Name = name;
        }        
    }
}
