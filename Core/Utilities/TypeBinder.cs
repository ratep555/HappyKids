using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;

namespace Core.Utilities
{
    public class TypeBinder<T> : IModelBinder
    {
        /// <summary>
        /// Model binder
        /// <param name="value">If there is no value, there is nothing to bind, else we are deserialising value.</param>
        /// </summary>
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            var propertyName = bindingContext.ModelName;
            var value = bindingContext.ValueProvider.GetValue(propertyName);
            
            if (value == ValueProviderResult.None)
            {
                return Task.CompletedTask;
            }
            else
            {
                try
                {
                    var deserializedValue = JsonConvert.DeserializeObject<T>(value.FirstValue);
                    bindingContext.Result = ModelBindingResult.Success(deserializedValue);
                }
                catch
                {
                    bindingContext.ModelState.TryAddModelError(propertyName, "The given value is not of the correct type");
                }
                return Task.CompletedTask;
            }
        }
    }
}
