using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace Iot.Device.Model.Reflection;

public sealed class ReflectionModelElementReference : ReflectionModelElement
{
    internal ReflectionModelElementReference(ReflectionModelElement reference, MemberInfo memberInfo) : base(reference.Type, attribute: null)
    {
        Reference = reference;
        MemberInfo = memberInfo;
        // TODO: this is super incorrect !!! TBD
        //this.SetMemberName(memberInfo.Name);
    }

    public ReflectionModelElement Reference { get; set; }
    public MemberInfo MemberInfo { get; set; }

    public override Dictionary<string, string> Attributes => Reference.Attributes;

    public override Dictionary<string, ModelElement> Elements => Reference.Elements;

    public override Dictionary<string, TelemetryNode> Telemetries => Reference.Telemetries;

    public override Dictionary<string, PropertyNode> Properties => Reference.Properties;

    public override Dictionary<string, CommandNode> Commands => Reference.Commands;
}