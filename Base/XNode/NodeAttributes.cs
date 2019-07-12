using System;
using XNode;

[AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
public class TriggerInputAttribute : Node.InputAttribute
{
    public TriggerInputAttribute() : base()
    {

    }
}