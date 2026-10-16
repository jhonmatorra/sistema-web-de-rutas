using System.Globalization;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace sistema_web_de_rutas.ModelBinders
{
    public class CoordenadaModelBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext contexto)
        {
            var valor = contexto.ValueProvider
                .GetValue(contexto.ModelName);

            if (valor == ValueProviderResult.None)
                return Task.CompletedTask;

            contexto.ModelState.SetModelValue(
                contexto.ModelName, valor);

            var texto = valor.FirstValue;

            if (string.IsNullOrWhiteSpace(texto))
            {
                contexto.Result = ModelBindingResult.Success(null);
                return Task.CompletedTask;
            }

            if (double.TryParse(
                    texto.Trim().Replace(',', '.'),
                    NumberStyles.Float,
                    CultureInfo.InvariantCulture,
                    out var numero)
                && double.IsFinite(numero))
            {
                contexto.Result = ModelBindingResult.Success(numero);
            }
            else
            {
                contexto.ModelState.TryAddModelError(
                    contexto.ModelName,
                    "La coordenada debe ser un número decimal.");

                contexto.Result = ModelBindingResult.Failed();
            }

            return Task.CompletedTask;
        }
    }
}