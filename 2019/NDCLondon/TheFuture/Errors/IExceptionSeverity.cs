
using System;
using Microsoft.AspNetCore.Mvc.Filters;

namespace TheFuture
{
    public interface IExceptionSeverity : IFilterMetadata
    {
        Severity Severity { get; }
    }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class ExceptionSeverityAttribute : Attribute, IExceptionSeverity
    {
        public ExceptionSeverityAttribute(Severity severity)
        {
            Severity = severity;
        }

        public Severity Severity { get; }
    }

    public enum Severity
    {
        NawRelaxItsOk,
        SomeoneShouldLookIntoThat,
        EverythingIsOnFire,
    }
}
