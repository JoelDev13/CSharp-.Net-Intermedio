# Ejercicio 4: Aplicando SOLID

## Enunciado
Una empresa necesita un sistema de notificaciones que pueda enviar correos electrónicos y SMS, pero la clase actual viola el Principio de Responsabilidad Única (SRP) porque también maneja el registro de logs.

## Objetivo
Refactoriza el código dividiendo responsabilidades en diferentes clases para cumplir con SRP (Single Responsibility Principle) dentro del marco de SOLID.

## Ejemplo de ejecución
```
Seleccione el tipo de notificación:
1. Email
2. SMS

Ingrese el mensaje: Bienvenido a nuestro servicio.

Enviando Email: Bienvenido a nuestro servicio.
Notificación registrada en logs.
```
