# Ejercicio 1: Aplicando KISS (Keep It Simple, Stupid)

## Enunciado
Un restaurante necesita calcular el total a pagar por los clientes. Se deben sumar los precios de los platos y agregar una propina opcional. Actualmente, el código es innecesariamente complicado y difícil de entender.

## Objetivo
Refactoriza el código para aplicar KISS, eliminando la complejidad innecesaria y haciéndolo más claro y fácil de entender.

## Código actual
```csharp
namespace KISS
{
    public class RestaurantBill
    {
        public decimal CalculateTotal(decimal[] prices, decimal? tipPercentage)
        {
            decimal total = 0;

            for (int i = 0; i < prices.Length; i++)
            {
                total += prices[i];
            }

            if (tipPercentage.HasValue)
            {
                total += total * (tipPercentage.Value / 100);
            }
            else
            {
                total += total * 0.10m;
            }
            return total;
        }
    }
}
```

## Requerimientos
- El usuario ingresará los precios de los platos (separados por comas).
- Se preguntará si desea agregar una propina personalizada o usar la predeterminada del 10%.
- El sistema debe calcular y mostrar el total a pagar.

## Ejemplo de ejecución
```
Ingrese los precios de los platos (separados por comas): 200,150,300
¿Desea agregar una propina personalizada? (s/n): n
Total a pagar (con propina del 10%): 715
```
