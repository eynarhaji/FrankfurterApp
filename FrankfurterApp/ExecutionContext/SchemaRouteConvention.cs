using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace FrankfurterApp.ExecutionContext;

public class SchemaRouteConvention : IControllerModelConvention
{
    public void Apply(ControllerModel controller)
    {
        foreach (var selector in controller.Selectors)
        {
            if (selector.AttributeRouteModel != null)
            {
                selector.AttributeRouteModel.Template = "{schemaName}/" + selector.AttributeRouteModel.Template;
            }
        }
    }
}