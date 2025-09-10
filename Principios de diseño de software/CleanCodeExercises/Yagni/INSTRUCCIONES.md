# Ejercicio 3: Aplicando YAGNI (You Aren't Gonna Need It)

## Enunciado
Un sistema de gestión de productos permite agregar y eliminar productos. Sin embargo, el código contiene un método para generar reportes que aún no es necesario.

## Objetivo
Refactoriza el código para eliminar la funcionalidad innecesaria y aplicar YAGNI.

## Requerimientos
- El usuario podrá agregar productos ingresando su nombre y precio.
- Podrá eliminar productos ingresando el ID.
- El método innecesario `GenerateProductReport()` debe ser eliminado.

## Ejemplo de ejecución
```
Seleccione una opción:
1. Agregar producto
2. Eliminar producto

Ingrese el nombre del producto: Laptop
Ingrese el precio: 750
Producto 'Laptop' agregado con éxito.
```
