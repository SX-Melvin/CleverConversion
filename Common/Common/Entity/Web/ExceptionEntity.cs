using System;

namespace CleverConversion.Common.Common.Entity.Web
{
    /// <summary>
    /// Exception entity
    /// </summary>
    public class ExceptionEntity
    {
        public string message { get; set; }
        public Exception exception { get; set; }
    }
}