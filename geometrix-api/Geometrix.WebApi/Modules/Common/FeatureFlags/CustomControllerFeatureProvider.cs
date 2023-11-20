using System.Collections;
using System.Reflection;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.FeatureManagement;
using Microsoft.FeatureManagement.Mvc;

namespace Geometrix.WebApi.Modules.Common.FeatureFlags;

/// <summary>
///     Custom Controller Feature Provider.
/// </summary>
public sealed class CustomControllerFeatureProvider(IFeatureManager featureManager)
    : IApplicationFeatureProvider<ControllerFeature>
{
    /// <summary>
    ///     Populate Features.
    /// </summary>
    public void PopulateFeature(IEnumerable<ApplicationPart> parts, ControllerFeature feature)
    {
        for (var i = feature.Controllers.Count - 1; i >= 0; i--)
        {
            var controller = feature.Controllers[i].AsType();
            ProcessController(controller, feature, i);
        }
    }

    private void ProcessController(Type controller, ControllerFeature feature, int index)
    {
        if (controller.CustomAttributes.Any(ShouldRemoveController))
        {
            feature.Controllers.RemoveAt(index);
        }
    }

    private bool ShouldRemoveController(CustomAttributeData customAttribute)
    {
        if (customAttribute.AttributeType.FullName != typeof(FeatureGateAttribute).FullName)
        {
            return false;
        }

        var constructorArgument = customAttribute.ConstructorArguments.First();
        if (constructorArgument.Value is not IEnumerable arguments)
        {
            return false;
        }

        return arguments.Cast<CustomAttributeTypedArgument>()
            .Any(argument => !IsFeatureEnabled((CustomFeature)(int)argument.Value!));
    }

    private bool IsFeatureEnabled(CustomFeature feature)
        => featureManager
            .IsEnabledAsync(feature.ToString())
            .ConfigureAwait(false)
            .GetAwaiter()
            .GetResult();
}