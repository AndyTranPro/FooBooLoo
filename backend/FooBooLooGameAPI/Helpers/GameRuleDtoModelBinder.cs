using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Text.Json;

namespace FooBooLooGameAPI.Helpers;

public class GameRuleDtoModelBinder : IModelBinder
{
    public Task BindModelAsync(ModelBindingContext bindingContext)
    {
        if (bindingContext == null)
        {
            throw new ArgumentNullException(nameof(bindingContext));
        }

        var valueProviderResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);

        if (valueProviderResult.Length == 0)
        {
            return Task.CompletedTask;
        }

        var options = new JsonSerializerOptions()
        {
            PropertyNameCaseInsensitive = true
        };

        var deserialized = JsonSerializer.Deserialize(valueProviderResult.FirstValue, bindingContext.ModelType, options);

        bindingContext.Result = ModelBindingResult.Success(deserialized);
        return Task.CompletedTask;
    }
}