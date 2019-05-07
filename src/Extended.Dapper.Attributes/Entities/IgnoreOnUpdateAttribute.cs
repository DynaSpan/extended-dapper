using System;

namespace Extended.Dapper.Attributes.Entities
{
    /// <summary>
    /// Will not update this field when executing an update query
    /// </summary>
    public sealed class IgnoreOnUpdateAttribute : Attribute
    {

    }
}