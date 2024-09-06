using System;
using System.Reflection;

namespace AdimiToolsHelper;

/// <summary>
/// Provides helper methods for reflection to use only when is no other choice.
/// </summary>
internal static class AdimiHelpers
{
    public static object? GetField(object instance, string field) => GetFieldInfo(instance, field).GetValue(instance);
    public static void SetField(object instance, string field, object value) => GetFieldInfo(instance, field).SetValue(instance, value);
    public static void SetPublicField(object instance, string field, object value) => GetPublicFieldInfo(instance, field).SetValue(instance, value);
    public static object? GetProperty(object instance, string field) => GetPropertyInfo(instance, field)!.GetValue(instance);
    public static void SetProperty(object instance, string field, object value) => GetPropertyInfo(instance, field)!.SetValue(instance, value, null);

    public static object? InvokeMethod(object instance, string method, object[] parameters)
    {
        return instance
            .GetType()
            .GetMethod(method, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)!
            .Invoke(instance, parameters);
    }

    public static PropertyInfo? GetPropertyInfo(object instance, string prop)
    {
        Type? t = instance.GetType();
        while (t != null)
        {
            var f = t.GetProperty(prop, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            if (f != null)
            {
                return f.DeclaringType!.GetProperty(prop, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            }

            t = t.BaseType;
        }

        throw new ArgumentException($"Property {prop} not found in {instance.GetType()}");
    }

    public static void RaiseEvent(object instance, string evt, object[] parameters)
    {
        var deleg = (MulticastDelegate?)GetFieldInfo(instance, evt).GetValue(instance);
        if (deleg == null)
        {
            return;
        }

        foreach (var invocation in deleg.GetInvocationList())
        {
            invocation?.DynamicInvoke(parameters);
        }
    }

    private static FieldInfo GetFieldInfo(object instance, string field)
    {
        Type? t = instance.GetType();
        while (t != null)
        {
            var f = t.GetField(field, BindingFlags.Instance | BindingFlags.NonPublic);
            if (f != null)
            {
                return f;
            }

            t = t.BaseType;
        }

        throw new ArgumentException($"Field {field} not found in {instance.GetType()}");
    }

    private static FieldInfo GetPublicFieldInfo(object instance, string field)
    {
        Type? t = instance.GetType();
        while (t != null)
        {
            var f = t.GetField(field, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            if (f != null)
            {
                return f;
            }

            t = t.BaseType;
        }

        throw new ArgumentException($"Field {field} not found in {instance.GetType()}");
    }
}

internal class FactionColor
{
    public static List<FactionColor> AllColors { get; private set; } = new List<FactionColor>();
    public int PrimaryColor { get; set; } = 0;
    public int IconColor { get; set; } = 0;
    public List<FactionColor> Exclude { get; set; } = new();

    public FactionColor()
    {
        AllColors.Add(this);
    }
}
