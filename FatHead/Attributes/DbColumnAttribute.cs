using FatHead.Attributes.Interfaces;
using System;

namespace FatHead.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class DbColumnAttribute : Attribute, IAttribute
    {
        private string _attributeValue;

        public DbColumnAttribute(string attributeValue)
        {
            _attributeValue = attributeValue;
        }

        public string AttributeValue
        {
            get { return _attributeValue; }
        }
    }
}
