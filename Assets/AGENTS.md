# Guía de Desarrollo para "Umbral: El Eco del Alma"

## 1. Convenciones de Código

### 1.1. Namespace

Todo el código C# creado para este proyecto **debe** estar encapsulado dentro del namespace `Umbral`.

**Ejemplo:**
```csharp
namespace Umbral
{
    public class MiClase
    {
        // ...
    }
}
```

Esta práctica es obligatoria para mantener el código organizado y evitar colisiones de nombres con assets de terceros.
