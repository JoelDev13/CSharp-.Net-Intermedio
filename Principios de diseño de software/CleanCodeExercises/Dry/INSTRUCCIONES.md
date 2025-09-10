# Ejercicio 2: Aplicando DRY (Don't Repeat Yourself)

## Enunciado
Una empresa necesita calcular el salario de sus empleados de manera eficiente. Actualmente, hay código duplicado en los cálculos de impuestos y bonificaciones para empleados de tiempo completo y medio tiempo.

## Objetivo
Refactoriza el código para aplicar DRY, eliminando la duplicación y reutilizando la lógica común en un método.

## Código actual
```csharp
namespace DRY
{
    public class Payroll
    {
        public decimal CalculateSalaryForFullTime(decimal baseSalary)
        {
            decimal tax = baseSalary * 0.18m;
            decimal bonus = baseSalary * 0.05m;
            return baseSalary - tax + bonus;
        }

        public decimal CalculateSalaryForPartTime(decimal hourlyRate, int hoursWorked)
        {
            decimal salary = hourlyRate * hoursWorked;
            decimal tax = salary * 0.18m;
            decimal bonus = salary * 0.05m;
            return salary - tax + bonus;
        }
    }
}
```

## Requerimientos
- El usuario ingresará el tipo de empleado ("1" para tiempo completo, "2" para medio tiempo).
- Para empleados de tiempo completo, se ingresará el salario base.
- Para empleados de medio tiempo, se ingresará el sueldo por hora y las horas trabajadas.
- Se calculará el salario neto aplicando un 18% de impuestos y un 5% de bono.

## Ejemplo de ejecución
```
Seleccione el tipo de empleado (1: Tiempo completo, 2: Medio tiempo): 1
Ingrese el salario base: 50000
Salario neto después de impuestos y bono: 43250
```
