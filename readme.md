# C# Chilean RUT

Librería para facilitar la validación y aplicación del número
de identificación utilizado en Chile (RUT).

## Índice

* [Instalación](#instalación)
    - [Versión](#versión)
    - [Nuget](#nuget)
* [Uso](#uso)
    - [Instanciar RUT](#instanciar-rut)
    - [Validar RUT](#validar-rut)
        - [Método IsValid()](#método-isvalid())
        - [Atributo ValidRut](#atributo-validrut)
        - [Atributo ValidRutDv](#atributo-validrutdv)
    - [Chequear RUT](#chequear-rut)
    - [Calcular DV](#calcular-dv)
    - [Formatear RUT](#formatear-rut)
    - [Destructuración](#destructuración)
    - [Comparación](#comparación)
    - [Generación Aleatoria](#generación-aleatoria)
    - [toString](#tostring)
* [Licencia](#licencia)

## Instalación

### Versión

La versión corresponde a: `'ChileanRut:1.2.0'`.

### Nuget

~~~bash
dotnet add package ChileanRut --version 1.2.0
~~~ 

## Uso

Los RUT se dividen en **parte numérica** y **dígito verificador (dv)**.
- La **parte numérica** debe contener entre **1 y 8 dígitos** (sin comenzar con 0)
- El **dv** debe ser un dígito (0-9), k o K

Internamente el dígito verificador estará en minúsculas (para el caso de la 'k').

### Instanciar RUT

Hay dos formas de instanciar el RUT; constructor y parse.

~~~csharp
using MrCoto.ChileanRut;

namespace MyProject
{
    class Program
    {
        static void Main(string[] args)
        {
            var rut1 = new Rut("12345678", "k");
            var rut2 = new Rut("12345678", "K");
            var rut3 = Rut.Parse(12345678); // Automáticamente cálcula el 'dv'
            var rut4 = Rut.Parse("12345678k");
            var rut5 = Rut.Parse("12345678K");
            var rut6 = Rut.Parse("12345678-k");
            var rut7 = Rut.Parse("12.345.678-k");
        }
    }
}
~~~

### Validar RUT

Se puede validar un Rut utilizando el método `IsValid()` o a través de atributos.

#### Método IsValid()

Se utiliza el método `IsValid()` a una instancia ya creada.

~~~csharp
using System;
using MrCoto.ChileanRut;

namespace MyProject
{
    class Program
    {
        static void Main(string[] args)
        {
            var rut1 = Rut.Parse("12345678k");
            var rut2 = Rut.Parse("123456785");
            Console.WriteLine(rut1.IsValid()); // false
            Console.WriteLine(rut2.IsValid()); // true
        }
    }
}
~~~

#### Atributo ValidRut

Se utiliza el atributo `MrCoto.ChileanRut.Validation.ValidRutAttribute`. Útil si estás haciendo una API.

~~~csharp
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MrCoto.ChileanRut.Validation;

namespace MrCoto.ChileanRut
{
    class MyClass
    {
        // MinLength: La parte numérica del Rut debe contener al menos dos dígitos
        // RutFormat: Formato del Rut (12345678-9 para este ejemplo). Si no se especifica, puede ser cualquiera
        [ValidRut(MinLength = 2, RutFormat = RutFormat.ONLY_DASH, ErrorMessage = "No es un Rut Válido")]
        public string Rut { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var obj1 = new MyClass() { Rut = "1-9" };
            printStatus(obj1); // No Válido, parte numérica tiene 1 solo dígito.
            var obj2 = new MyClass() { Rut = "123-6"};
            printStatus(obj2); // Válido
            var obj3 = new MyClass() { Rut = "1236"};
            printStatus(obj3); // No Válido, formato debe ser XXXXXXXXX-DV
            var obj4 = new MyClass() { Rut = "12.345.678-5"};
            printStatus(obj4); // No Válido, formato debe ser XXXXXXXXX-DV
            var obj5 = new MyClass() { Rut = "hola mundo"};
            printStatus(obj5); // No Válido, no es un Rut
        }

        static void printStatus(MyClass obj)
        {
            var validationContext = new ValidationContext(obj);
            var results = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(obj, validationContext, results, true);
            Console.WriteLine($"Válido: {isValid}");
        }

    }
}
~~~

#### Atributo ValidRutDv

Se utiliza el atributo `MrCoto.ChileanRut.Validation.ValidRutDvAttribute`. Útil si estás haciendo una API. A diferencia del atributo anterior, este sirve cuando la parte numérica y dígito verificador son dos atributos diferentes.

~~~csharp
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MrCoto.ChileanRut.Validation;

namespace MrCoto.ChileanRut
{
    class MyClass
    {
        public int RutNumber { get; set; } // Puede ser int o string

        // NumberName: Nombre del otro campo que posee la parte numérica del rut
        // MinLength: La parte numérica del Rut debe contener al menos dos dígitos
        [ValidRutDv(NumberName = "RutNumber", MinLength = 2, ErrorMessage="No es un Rut Válido")]
        public string RutDv { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var obj1 = new MyClass() { RutNumber = 1, RutDv = "9" };
            printStatus(obj1); // No Válido, parte numérica tiene 1 solo dígito.
            var obj2 = new MyClass() { RutNumber = 123, RutDv = "6"};
            printStatus(obj2); // Válido
            var obj3 = new MyClass() { RutNumber = 123, RutDv = "Mundo"};
            printStatus(obj3); // No Válido
        }

        static void printStatus(MyClass obj)
        {
            var validationContext = new ValidationContext(obj);
            var results = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(obj, validationContext, results, true);
            Console.WriteLine($"Válido: {isValid}");
        }

    }
}
~~~

### Chequear RUT

Se utiliza el método `Check(string message)` o `Check<T>(string message)` a una instancia ya creada (`T` debe heredar de `System.Exception`).
Este método, en vez de retornar un booleano, lanzará una excepción si el rut no es válido. 

El método por defecto lanza la excepción `MrCoto.ChileanRut.Exceptions.InvalidRutFormatException`.

~~~csharp
using System;

namespace MrCoto.ChileanRut
{
    class Program
    {
        static void Main(string[] args)
        {
            var rut = Rut.Parse("123-k");
            printError(() => rut.Check()); // InvalidRutFormatException: El Formato del Rut es inválido
            printError(() => rut.Check("Es inválido!")); // InvalidRutFormatException: Es inválido!
            printError(() => rut.Check<ArgumentException>()); // ArgumentException: El Formato del Rut es inválido
            printError(() => rut.Check<ArgumentException>("Argumento Inválido!")); // ArgumentException: Argumento Inválido!
            printError(() => rut.Check<Exception>()); // Exception: El Formato del Rut es inválido
        }

        static void printError(Action action)
        {
            try {
                action.Invoke();
            } catch(Exception ex) {
                Console.WriteLine($"{ex.GetType().Name}: {ex.Message}");
            }
        }

    }
}
~~~

### Calcular DV

Dada una parte numérica se puede calcular el dv.

~~~csharp
using System;
using MrCoto.ChileanRut;

namespace MyProject
{
    class Program
    {
        static void Main(string[] args)
        {
            var dv = Rut.CalcDv(12345678);
            Console.WriteLine(dv); // 5
        }
    }
}
~~~

### Formatear RUT

Se puede formatear de forma **completa**, **solo guión** o **sin puntos y guión**

~~~csharp
using System;
using MrCoto.ChileanRut;

namespace MyProject
{
    class Program
    {
        static void Main(string[] args)
        {
            var rut = Rut.Parse("12345678k");
            Console.WriteLine(rut.Format()); // 12.345.678-k
            Console.WriteLine(rut.Format(RutFormat.FULL)); // 12.345.678-k
            Console.WriteLine(rut.Format(RutFormat.ONLY_DASH)); // 12345678-k
            Console.WriteLine(rut.Format(RutFormat.ESCAPED)); // 12345678k
        }
    }
}
~~~

### Destructuración

~~~csharp
using System;
using MrCoto.ChileanRut;

namespace MyProject
{
    class Program
    {
        static void Main(string[] args)
        {
            var rut = Rut.Parse("12345678k");
            var (num, dv) = rut; // (12345678, k)
            var (number, _) = rut; // (12345678, _)
            var (_, dv2) = rut; // (_, k)
        }
    }
}
~~~

### Comparación

Se pueden comparar (menor, mayor) dos RUT distintos en base a su parte numérica.

~~~csharp
using System;
using MrCoto.ChileanRut;

namespace MyProject
{
    class Program
    {
        static void Main(string[] args)
        {
            var rut1 = Rut.Parse("12345678k");
            var rut2 = Rut.Parse("12345677k");
            Console.WriteLine(rut2 < rut1); // true
        }
    }
}
~~~

### Generación Aleatoria

Se puede generar un RUT aleatorio, una lista de RUT aleatorio, o 
una lista única de RUT aleatorio.

Como los RUT son comparables, se puede ordenar la 
lista resultante.

**Nota:** Por defecto, se genera un RUT entre 4.000.000 a 80.000.000,
se deben cambiar los parámetros min y max, para generar entre otro rango.

~~~csharp
using System;
using MrCoto.ChileanRut;

namespace MyProject
{
    class Program
    {
        static void Main(string[] args)
        {
            var rut = Rut.Random();
            var rut2 = Rut.Random(min: 100, max: 200, seed: 42);
            var rut3 = Rut.Random(min: 100, max: 200, seed: 42);
            Console.WriteLine(rut.Format());
            Console.WriteLine(rut2.Format());
            Console.WriteLine(rut3.Format());
            Console.WriteLine(rut3 == rut2); // true

            var ruts = Rut.Randoms(n: 5); // 5 ruts a generar
            var sortedRuts = Rut.Randoms(n: 5);
            sortedRuts.Sort();
            var uniqueRuts = Rut.Uniques(n: 5);

            ruts.ForEach(rut => Console.WriteLine(rut));
            Console.WriteLine("");
            sortedRuts.ForEach(rut => Console.WriteLine(rut));
            Console.WriteLine("");
            uniqueRuts.ForEach(rut => Console.WriteLine(rut));
            Console.WriteLine("");
        }
    }
}
~~~

### toString

Del ejemplo anterior, se puede observar la representación a String
de un RUT

~~~csharp
using System;
using MrCoto.ChileanRut;

namespace MyProject
{
    class Program
    {
        static void Main(string[] args)
        {
            var rut = Rut.Parse("12345678-k");
            Console.WriteLine(rut); // Rut(12345678, k)
        }
    }
}
~~~

## Licencia

Esta librería se distribuye bajo la licencia MIT.