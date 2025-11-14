# Documentación WarmPack - Clases de Utilidad

## Índice

1. [Clase Conexion](#clase-conexion)
2. [Clase Result](#clase-result)
3. [Clase ResultList](#clase-resultlist)
4. [Clase Castable](#clase-castable)

---

# Clase Conexion

## Descripción General

La clase `Conexion` es una capa de abstracción para el manejo de conexiones a bases de datos. Proporciona métodos para ejecutar consultas SQL, procedimientos almacenados, manejar transacciones y mapear resultados a objetos .NET.

**Namespace:** `WarmPack.Database`

---

## Constructores

### 1. Conexion(ConexionType, string, string, string, string)

Crea una conexión con parámetros individuales.

```csharp
// Ejemplo
var conexion = new Conexion(
    ConexionType.MSSQLServer,
    "localhost",          // servidor
    "MiBaseDatos",        // base de datos
    "usuario",            // usuario
    "contraseña"          // contraseña
);
```

### 2. Conexion(ConexionType, string)

Crea una conexión usando un string de conexión completo.

```csharp
// Ejemplo
var connectionString = "Data Source=localhost;Initial Catalog=MiBaseDatos;User ID=usuario;Password=contraseña";
var conexion = new Conexion(ConexionType.MSSQLServer, connectionString);
```

### 3. Conexion(ConexionType, ConnectionString)

Crea una conexión usando un objeto `ConnectionString`.

```csharp
// Ejemplo
var cs = new ConnectionString("localhost", "MiBaseDatos", "usuario", "contraseña");
var conexion = new Conexion(ConexionType.MSSQLServer, cs);
```

---

## Propiedades

### ConexionTimeOut
Establece el tiempo de espera para las consultas (en segundos). Por defecto es 30 segundos.

```csharp
conexion.ConexionTimeOut = 60; // 60 segundos
```

### ConnectionString
Obtiene el objeto ConnectionString de la conexión actual.

```csharp
var server = conexion.ConnectionString.Server;
var database = conexion.ConnectionString.Database;
```

### DbSchema
Proporciona acceso al esquema de la base de datos.

```csharp
var tablas = conexion.DbSchema.GetTables();
```

### IsMoreRecordsets
Indica si hay más conjuntos de resultados disponibles al usar métodos de Recordsets.

---

## Métodos de Transacciones

### TransactionBegin()

Inicia una transacción en la base de datos.

```csharp
// Ejemplo
conexion.TransactionBegin();
try 
{
    conexion.Execute("INSERT INTO Usuarios (Nombre) VALUES ('Juan')");
    conexion.Execute("INSERT INTO Usuarios (Nombre) VALUES ('María')");
    conexion.TransactionCommit();
}
catch 
{
    conexion.TransactionRollback();
}
```

### TransactionCommit([bool closeConexion])

Confirma la transacción actual. Por defecto cierra la conexión.

```csharp
// Confirmar y cerrar conexión
conexion.TransactionCommit();

// Confirmar pero mantener conexión abierta
conexion.TransactionCommit(false);
```

### TransactionRollback([bool closeConexion])

Revierte la transacción actual. Por defecto cierra la conexión.

```csharp
// Revertir y cerrar conexión
conexion.TransactionRollback();

// Revertir pero mantener conexión abierta
conexion.TransactionRollback(false);
```

---

## Métodos Execute

### Execute(string query)

Ejecuta una consulta SQL sin parámetros y retorna un objeto `Result`.

```csharp
// Ejemplo: Ejecutar INSERT
var result = conexion.Execute("INSERT INTO Productos (Nombre, Precio) VALUES ('Laptop', 1500)");

if (result.Value)
{
    Console.WriteLine("Inserción exitosa: " + result.Message);
}

// Ejemplo: Ejecutar UPDATE
var result2 = conexion.Execute("UPDATE Productos SET Precio = 1600 WHERE Nombre = 'Laptop'");

// Ejemplo: Ejecutar DELETE
var result3 = conexion.Execute("DELETE FROM Productos WHERE Nombre = 'Laptop'");
```

### Execute(string query, ConexionParameters parameters)

Ejecuta un procedimiento almacenado con parámetros.

```csharp
// Ejemplo: Ejecutar procedimiento almacenado
var parametros = new ConexionParameters();
parametros.Add("@Nombre", "Carlos", DbType.String);
parametros.Add("@Email", "carlos@email.com", DbType.String);
parametros.Add("@Result", ParameterDirection.Output, DbType.Boolean);
parametros.Add("@Message", ParameterDirection.Output, DbType.String, 500);

var result = conexion.Execute("sp_InsertarUsuario", parametros);

if (result.Value)
{
    Console.WriteLine($"Usuario insertado: {result.Message}");
}
else
{
    Console.WriteLine($"Error: {result.Message}");
}
```

---

## Métodos ExecuteWithResults

### ExecuteWithResults(string query, out DataTable dt)

Ejecuta una consulta y retorna los resultados en un `DataTable`.

```csharp
// Ejemplo: Obtener todos los usuarios
DataTable usuarios;
var result = conexion.ExecuteWithResults("SELECT * FROM Usuarios", out usuarios);

if (result.Value)
{
    foreach (DataRow row in usuarios.Rows)
    {
        Console.WriteLine($"Usuario: {row["Nombre"]}, Email: {row["Email"]}");
    }
}
```

### ExecuteWithResults(string query, ConexionParameters parameters, out DataTable dt)

Ejecuta un procedimiento almacenado y retorna los resultados en un `DataTable`.

```csharp
// Ejemplo: Buscar usuarios por departamento
var parametros = new ConexionParameters();
parametros.Add("@DepartamentoId", 5, DbType.Int32);

DataTable usuarios;
var result = conexion.ExecuteWithResults("sp_ObtenerUsuariosPorDepartamento", parametros, out usuarios);
```

### ExecuteWithResults(string query, out DataSet ds)

Ejecuta una consulta y retorna múltiples tablas en un `DataSet`.

```csharp
// Ejemplo: Obtener múltiples tablas
DataSet datos;
var result = conexion.ExecuteWithResults(@"
    SELECT * FROM Usuarios;
    SELECT * FROM Departamentos;
    SELECT * FROM Roles;
", out datos);

var tablaUsuarios = datos.Tables[0];
var tablaDepartamentos = datos.Tables[1];
var tablaRoles = datos.Tables[2];
```

### ExecuteWithResults(string query, ConexionParameters parameters, out DataSet ds)

Ejecuta un procedimiento almacenado que retorna múltiples conjuntos de resultados.

```csharp
// Ejemplo: Procedimiento que retorna múltiples tablas
var parametros = new ConexionParameters();
parametros.Add("@Año", 2025, DbType.Int32);

DataSet datos;
var result = conexion.ExecuteWithResults("sp_ObtenerReporteAnual", parametros, out datos);
```

---

## Métodos ExecuteWithResults con Genéricos

### ExecuteWithResults&lt;T&gt;(string query)

Ejecuta una consulta y mapea los resultados a una lista de objetos del tipo especificado.

```csharp
// Definir clase
public class Usuario
{
    public int Id { get; set; }
    public string Nombre { get; set; }
    public string Email { get; set; }
    public DateTime FechaCreacion { get; set; }
}

// Ejemplo de uso
var result = conexion.ExecuteWithResults<Usuario>("SELECT * FROM Usuarios");

if (result.Value)
{
    foreach (var usuario in result.Data)
    {
        Console.WriteLine($"ID: {usuario.Id}, Nombre: {usuario.Nombre}");
    }
}
```

### ExecuteWithResults&lt;T&gt;(string query, ConexionParameters parameters)

Ejecuta un procedimiento almacenado y mapea los resultados a objetos.

```csharp
// Ejemplo: Buscar usuarios activos
var parametros = new ConexionParameters();
parametros.Add("@Activo", true, DbType.Boolean);
parametros.Add("@Result", ParameterDirection.Output, DbType.Boolean);
parametros.Add("@Message", ParameterDirection.Output, DbType.String, 200);

var result = conexion.ExecuteWithResults<Usuario>("sp_ObtenerUsuariosActivos", parametros);

if (result.Value)
{
    Console.WriteLine($"Se encontraron {result.Data.Count} usuarios activos");
    Console.WriteLine($"Mensaje del servidor: {result.Message}");
}
```

### ExecuteWithResults&lt;T&gt;(string query, out List&lt;T&gt; results)

Variante que retorna la lista de objetos como parámetro de salida.

```csharp
// Ejemplo
List<Usuario> usuarios;
var result = conexion.ExecuteWithResults<Usuario>("SELECT * FROM Usuarios WHERE Activo = 1", out usuarios);

Console.WriteLine($"Total de usuarios: {usuarios.Count}");
```

### ExecuteWithResultsToObject&lt;T&gt;(string query)

Ejecuta una consulta que retorna un solo objeto.

```csharp
// Ejemplo: Obtener un usuario específico
var result = conexion.ExecuteWithResultsToObject<Usuario>(
    "SELECT TOP 1 * FROM Usuarios WHERE Email = 'juan@email.com'"
);

if (result.Value && result.Data != null)
{
    Console.WriteLine($"Usuario encontrado: {result.Data.Nombre}");
}
```

### ExecuteWithResultsToObject&lt;T&gt;(string query, ConexionParameters parameters)

Ejecuta un procedimiento almacenado que retorna un solo objeto.

```csharp
// Ejemplo: Obtener usuario por ID
var parametros = new ConexionParameters();
parametros.Add("@UsuarioId", 25, DbType.Int32);

var result = conexion.ExecuteWithResultsToObject<Usuario>("sp_ObtenerUsuarioPorId", parametros);

if (result.Data != null)
{
    Console.WriteLine($"Usuario: {result.Data.Nombre}");
}
```

---

## Métodos ExecuteWithResults con Action

### ExecuteWithResults(string query, Action&lt;ConexionCastableRow&gt; action)

Ejecuta una consulta y procesa cada fila con una acción, sin cargar todos los resultados en memoria.

```csharp
// Ejemplo: Procesar grandes volúmenes de datos
conexion.ExecuteWithResults("SELECT * FROM LogsHistorico", row =>
{
    var id = row["Id"].ToInt32();
    var fecha = row["Fecha"].ToDateTime();
    var mensaje = row["Mensaje"].ToString();
    
    // Procesar cada fila sin cargar todo en memoria
    Console.WriteLine($"Log {id}: {mensaje}");
});
```

### ExecuteWithResults(string query, ConexionParameters parameters, Action&lt;ConexionCastableRow&gt; action)

Ejecuta un procedimiento almacenado y procesa cada fila.

```csharp
// Ejemplo: Exportar datos a archivo
var parametros = new ConexionParameters();
parametros.Add("@Año", 2024, DbType.Int32);

using (var writer = new StreamWriter("ventas.csv"))
{
    conexion.ExecuteWithResults("sp_ObtenerVentas", parametros, row =>
    {
        var fecha = row["Fecha"].ToDateTime().ToString("yyyy-MM-dd");
        var monto = row["Monto"].ToDecimal();
        var cliente = row["Cliente"].ToString();
        
        writer.WriteLine($"{fecha},{monto},{cliente}");
    });
}
```

---

## Métodos ExecuteScalar

### ExecuteScalar(string query)

Ejecuta una consulta que retorna un único valor y lo envuelve en un objeto `Castable`.

```csharp
// Ejemplo: Contar registros
var total = conexion.ExecuteScalar("SELECT COUNT(*) FROM Usuarios").ToInt32();
Console.WriteLine($"Total de usuarios: {total}");

// Ejemplo: Obtener suma
var totalVentas = conexion.ExecuteScalar("SELECT SUM(Monto) FROM Ventas").ToDecimal();
Console.WriteLine($"Total ventas: ${totalVentas}");

// Ejemplo: Obtener valor máximo
var precioMaximo = conexion.ExecuteScalar("SELECT MAX(Precio) FROM Productos").ToDouble();
```

### ExecuteScalar(string storeProcedure, ConexionParameters parameters)

Ejecuta un procedimiento almacenado que retorna un valor escalar.

```csharp
// Ejemplo: Obtener siguiente número de factura
var parametros = new ConexionParameters();
parametros.Add("@Año", 2025, DbType.Int32);

var numeroFactura = conexion.ExecuteScalar("sp_ObtenerSiguienteNumeroFactura", parametros).ToInt32();
Console.WriteLine($"Siguiente factura: {numeroFactura}");
```

### ExecuteScalar&lt;T&gt;(string query)

Ejecuta una consulta escalar y retorna el resultado en un objeto `Result<T>`.

```csharp
// Ejemplo: Obtener total con manejo de resultado
var result = conexion.ExecuteScalar<int>("SELECT COUNT(*) FROM Usuarios WHERE Activo = 1");

if (result.Value)
{
    Console.WriteLine($"Usuarios activos: {result.Data}");
}
```

### ExecuteScalar&lt;T&gt;(string storeProcedure, ConexionParameters parameters)

Ejecuta un procedimiento almacenado escalar con tipo genérico.

```csharp
// Ejemplo: Obtener promedio de ventas
var parametros = new ConexionParameters();
parametros.Add("@MesInicio", 1, DbType.Int32);
parametros.Add("@MesFin", 6, DbType.Int32);
parametros.Add("@Result", ParameterDirection.Output, DbType.Boolean);

var result = conexion.ExecuteScalar<decimal>("sp_ObtenerPromedioVentas", parametros);

if (result.Value)
{
    Console.WriteLine($"Promedio de ventas: ${result.Data}");
}
```

---

## Métodos Recordsets

Los métodos Recordsets permiten procesar múltiples conjuntos de resultados de forma eficiente, sin cargar todo en memoria.

### RecordsetsExecute(string query)

Inicia la ejecución de una consulta con múltiples conjuntos de resultados.

```csharp
// Ejemplo: Consulta con múltiples SELECT
var hayDatos = conexion.RecordsetsExecute(@"
    SELECT * FROM Usuarios;
    SELECT * FROM Departamentos;
    SELECT * FROM Roles;
");

if (hayDatos)
{
    // Procesar primer conjunto de resultados (Usuarios)
    var usuarios = conexion.RecordsetsResults<Usuario>();
    
    if (conexion.IsMoreRecordsets)
    {
        // Procesar segundo conjunto (Departamentos)
        var departamentos = conexion.RecordsetsResults<Departamento>();
        
        if (conexion.IsMoreRecordsets)
        {
            // Procesar tercer conjunto (Roles)
            var roles = conexion.RecordsetsResults<Rol>();
        }
    }
}
```

### RecordsetsExecute(string storeProcedure, ConexionParameters parameters)

Ejecuta un procedimiento almacenado que retorna múltiples conjuntos de resultados.

```csharp
// Ejemplo: Procedimiento que retorna múltiples resultados
var parametros = new ConexionParameters();
parametros.Add("@ClienteId", 100, DbType.Int32);
parametros.Add("@Result", ParameterDirection.Output, DbType.Boolean);
parametros.Add("@Message", ParameterDirection.Output, DbType.String, 200);

var hayDatos = conexion.RecordsetsExecute("sp_ObtenerDatosCliente", parametros);

if (hayDatos)
{
    // Primera tabla: Datos del cliente
    var cliente = conexion.RecordsetsResultsToObject<Cliente>();
    
    if (conexion.IsMoreRecordsets)
    {
        // Segunda tabla: Pedidos del cliente
        var pedidos = conexion.RecordsetsResults<Pedido>();
        
        if (conexion.IsMoreRecordsets)
        {
            // Tercera tabla: Facturas del cliente
            var facturas = conexion.RecordsetsResults<Factura>();
        }
    }
}
```

### RecordsetsResults&lt;T&gt;()

Lee el siguiente conjunto de resultados y lo mapea a una lista de objetos.

```csharp
// Ver ejemplos en RecordsetsExecute
```

### RecordsetsResultsToObject&lt;T&gt;()

Lee el siguiente conjunto de resultados y retorna el primer objeto.

```csharp
// Ejemplo: Obtener encabezado y detalle
conexion.RecordsetsExecute("sp_ObtenerFactura", parametros);

// Primer resultado: Encabezado de la factura
var factura = conexion.RecordsetsResultsToObject<Factura>();

if (conexion.IsMoreRecordsets)
{
    // Segundo resultado: Líneas de detalle
    var detalles = conexion.RecordsetsResults<FacturaDetalle>();
    factura.Detalles = detalles;
}
```

### RecordsetsResults(Action&lt;ConexionCastableRow&gt; action)

Procesa el siguiente conjunto de resultados fila por fila.

```csharp
// Ejemplo: Procesar grandes conjuntos de datos
conexion.RecordsetsExecute("sp_ObtenerReporteCompleto", parametros);

// Procesar primer conjunto
conexion.RecordsetsResults(row =>
{
    var nombre = row["Nombre"].ToString();
    var total = row["Total"].ToDecimal();
    Console.WriteLine($"{nombre}: ${total}");
});

// Procesar segundo conjunto si existe
if (conexion.IsMoreRecordsets)
{
    conexion.RecordsetsResults(row =>
    {
        var fecha = row["Fecha"].ToDateTime();
        var cantidad = row["Cantidad"].ToInt32();
        Console.WriteLine($"{fecha:dd/MM/yyyy}: {cantidad}");
    });
}
```

---

## Métodos Auxiliares

### Test()

Verifica que la conexión a la base de datos funcione correctamente.

```csharp
// Ejemplo: Probar conexión antes de usarla
var conexion = new Conexion(ConexionType.MSSQLServer, connectionString);

if (conexion.Test())
{
    Console.WriteLine("Conexión exitosa");
    // Proceder con operaciones
}
else
{
    Console.WriteLine("Error de conexión");
}
```

### ChangeDatabase(string database)

Cambia la base de datos activa de la conexión.

```csharp
// Ejemplo: Cambiar entre bases de datos
var conexion = new Conexion(ConexionType.MSSQLServer, "localhost", "DB_Desarrollo", "user", "pass");

// Trabajar con DB_Desarrollo
var usuarios = conexion.ExecuteWithResults<Usuario>("SELECT * FROM Usuarios");

// Cambiar a producción
conexion.ChangeDatabase("DB_Produccion");

// Ahora trabaja con DB_Produccion
var usuariosProd = conexion.ExecuteWithResults<Usuario>("SELECT * FROM Usuarios");
```

### ExecuteScript(string script)

Ejecuta un script SQL completo que puede contener múltiples sentencias separadas por GO.

```csharp
// Ejemplo: Ejecutar script de migración
string script = @"
CREATE TABLE TempUsuarios (
    Id INT PRIMARY KEY,
    Nombre NVARCHAR(100)
);
GO

INSERT INTO TempUsuarios (Id, Nombre) VALUES (1, 'Usuario 1');
GO

INSERT INTO TempUsuarios (Id, Nombre) VALUES (2, 'Usuario 2');
GO

SELECT * FROM TempUsuarios;
GO

DROP TABLE TempUsuarios;
GO
";

var resultados = conexion.ExecuteScript(script);

foreach (var resultado in resultados)
{
    if (resultado.Value)
    {
        Console.WriteLine($"Comando ejecutado: {resultado.Message}");
    }
    else
    {
        Console.WriteLine($"Error: {resultado.Message}");
        Console.WriteLine($"Script: {resultado.Data}");
    }
}
```

### ConnectionStringBuilder(ConexionType, string, string, string, string)

Construye un string de conexión a partir de parámetros individuales.

```csharp
// Ejemplo: Construir string de conexión
var conexion = new Conexion(ConexionType.MSSQLServer, "localhost", "MiDB", "user", "pass");

var connectionString = conexion.ConnectionStringBuilder(
    ConexionType.MSSQLServer,
    "localhost",
    "OtraDB",
    "otroUser",
    "otraPass"
);

Console.WriteLine(connectionString);
// Output: Data Source=localhost;Initial Catalog=OtraDB;User ID=otroUser;Password=otraPass
```

---

## Uso del Atributo ConexionColumn

El atributo `ConexionColumnAttribute` permite mapear propiedades a columnas con nombres diferentes.

```csharp
// Ejemplo: Mapeo de columnas
public class Producto
{
    public int Id { get; set; }
    
    [ConexionColumn("NombreProducto")]
    public string Nombre { get; set; }
    
    [ConexionColumn("PrecioUnitario")]
    public decimal Precio { get; set; }
    
    [ConexionColumn("FechaCreacion")]
    public DateTime Fecha { get; set; }
}

// La consulta puede usar los nombres reales de la BD
var productos = conexion.ExecuteWithResults<Producto>(
    "SELECT Id, NombreProducto, PrecioUnitario, FechaCreacion FROM Productos"
);

// Los datos se mapean automáticamente a las propiedades
foreach (var producto in productos.Data)
{
    Console.WriteLine($"{producto.Nombre}: ${producto.Precio}");
}
```

---

## Manejo de Parámetros de Salida

### Parámetros Estándar de Result

Los procedimientos almacenados pueden retornar información de estado usando parámetros específicos:

- **@Result / @pResult / @Resultado / @pResultado**: Indica si la operación fue exitosa (Boolean)
- **@Msg / @pMsg / @Message / @pMessage / @Mensaje / @pMensaje**: Mensaje descriptivo (String)
- **@Codigo / @pCodigo / @Code / @pCode**: Código de error o estado (Int32)

```csharp
// Ejemplo: Procedimiento almacenado
/*
CREATE PROCEDURE sp_ActualizarProducto
    @ProductoId INT,
    @Precio DECIMAL(18,2),
    @Result BIT OUTPUT,
    @Message NVARCHAR(200) OUTPUT,
    @Code INT OUTPUT
AS
BEGIN
    IF EXISTS (SELECT 1 FROM Productos WHERE Id = @ProductoId)
    BEGIN
        UPDATE Productos SET Precio = @Precio WHERE Id = @ProductoId;
        SET @Result = 1;
        SET @Message = 'Producto actualizado correctamente';
        SET @Code = 0;
    END
    ELSE
    BEGIN
        SET @Result = 0;
        SET @Message = 'Producto no encontrado';
        SET @Code = 404;
    END
END
*/

// Código C#
var parametros = new ConexionParameters();
parametros.Add("@ProductoId", 10, DbType.Int32);
parametros.Add("@Precio", 1299.99m, DbType.Decimal);
parametros.Add("@Result", ParameterDirection.Output, DbType.Boolean);
parametros.Add("@Message", ParameterDirection.Output, DbType.String, 200);
parametros.Add("@Code", ParameterDirection.Output, DbType.Int32);

var resultado = conexion.Execute("sp_ActualizarProducto", parametros);

if (resultado.Value)
{
    Console.WriteLine($"Éxito: {resultado.Message}");
    Console.WriteLine($"Código: {resultado.Code}");
}
else
{
    Console.WriteLine($"Error: {resultado.Message}");
    Console.WriteLine($"Código de error: {resultado.Code}");
}
```

---

## Ejemplos Completos de Casos de Uso

### Caso 1: CRUD Completo

```csharp
public class Usuario
{
    public int Id { get; set; }
    public string Nombre { get; set; }
    public string Email { get; set; }
    public bool Activo { get; set; }
}

var conexion = new Conexion(ConexionType.MSSQLServer, "localhost", "MiApp", "user", "pass");

// CREATE
var parametrosInsert = new ConexionParameters();
parametrosInsert.Add("@Nombre", "Juan Pérez", DbType.String);
parametrosInsert.Add("@Email", "juan@email.com", DbType.String);
parametrosInsert.Add("@Result", ParameterDirection.Output, DbType.Boolean);
parametrosInsert.Add("@Message", ParameterDirection.Output, DbType.String, 200);

var resultInsert = conexion.Execute("sp_InsertarUsuario", parametrosInsert);

// READ
var usuarios = conexion.ExecuteWithResults<Usuario>("SELECT * FROM Usuarios WHERE Activo = 1");

// UPDATE
var parametrosUpdate = new ConexionParameters();
parametrosUpdate.Add("@UsuarioId", 1, DbType.Int32);
parametrosUpdate.Add("@Email", "nuevo_email@email.com", DbType.String);

var resultUpdate = conexion.Execute("sp_ActualizarEmailUsuario", parametrosUpdate);

// DELETE (lógico)
var parametrosDelete = new ConexionParameters();
parametrosDelete.Add("@UsuarioId", 1, DbType.Int32);

var resultDelete = conexion.Execute("sp_DesactivarUsuario", parametrosDelete);
```

### Caso 2: Transacción Compleja

```csharp
// Transferencia bancaria
conexion.TransactionBegin();

try
{
    // Retirar de cuenta origen
    var paramsRetiro = new ConexionParameters();
    paramsRetiro.Add("@CuentaId", 100, DbType.Int32);
    paramsRetiro.Add("@Monto", 500.00m, DbType.Decimal);
    paramsRetiro.Add("@Result", ParameterDirection.Output, DbType.Boolean);
    paramsRetiro.Add("@Message", ParameterDirection.Output, DbType.String, 200);
    
    var resultRetiro = conexion.Execute("sp_RetirarDeCuenta", paramsRetiro);
    
    if (!resultRetiro.Value)
    {
        throw new Exception($"Error al retirar: {resultRetiro.Message}");
    }
    
    // Depositar en cuenta destino
    var paramsDeposito = new ConexionParameters();
    paramsDeposito.Add("@CuentaId", 200, DbType.Int32);
    paramsDeposito.Add("@Monto", 500.00m, DbType.Decimal);
    paramsDeposito.Add("@Result", ParameterDirection.Output, DbType.Boolean);
    paramsDeposito.Add("@Message", ParameterDirection.Output, DbType.String, 200);
    
    var resultDeposito = conexion.Execute("sp_DepositarEnCuenta", paramsDeposito);
    
    if (!resultDeposito.Value)
    {
        throw new Exception($"Error al depositar: {resultDeposito.Message}");
    }
    
    // Registrar transferencia
    var paramsTransferencia = new ConexionParameters();
    paramsTransferencia.Add("@CuentaOrigen", 100, DbType.Int32);
    paramsTransferencia.Add("@CuentaDestino", 200, DbType.Int32);
    paramsTransferencia.Add("@Monto", 500.00m, DbType.Decimal);
    
    conexion.Execute("sp_RegistrarTransferencia", paramsTransferencia);
    
    // Todo OK, confirmar transacción
    conexion.TransactionCommit();
    Console.WriteLine("Transferencia completada exitosamente");
}
catch (Exception ex)
{
    conexion.TransactionRollback();
    Console.WriteLine($"Error en transferencia: {ex.Message}");
}
```

### Caso 3: Procesamiento de Grandes Volúmenes

```csharp
// Procesar millones de registros sin cargar todo en memoria
int contador = 0;
int lote = 0;

conexion.ExecuteWithResults("SELECT * FROM TransaccionesHistoricas WHERE Año = 2024", row =>
{
    var id = row["TransaccionId"].ToInt32();
    var monto = row["Monto"].ToDecimal();
    var fecha = row["Fecha"].ToDateTime();
    
    // Procesar cada registro
    // (por ejemplo, generar reporte, exportar, calcular estadísticas, etc.)
    
    contador++;
    
    if (contador % 10000 == 0)
    {
        lote++;
        Console.WriteLine($"Procesados {contador} registros (Lote {lote})");
    }
});

Console.WriteLine($"Total procesado: {contador} transacciones");
```

### Caso 4: Múltiples Conjuntos de Resultados

```csharp
// Obtener datos relacionados en una sola llamada
var parametros = new ConexionParameters();
parametros.Add("@PedidoId", 1001, DbType.Int32);

var hayDatos = conexion.RecordsetsExecute("sp_ObtenerPedidoCompleto", parametros);

if (hayDatos)
{
    // Primer recordset: Encabezado del pedido
    var pedido = conexion.RecordsetsResultsToObject<Pedido>();
    
    if (conexion.IsMoreRecordsets)
    {
        // Segundo recordset: Líneas de detalle
        pedido.Detalles = conexion.RecordsetsResults<PedidoDetalle>();
        
        if (conexion.IsMoreRecordsets)
        {
            // Tercer recordset: Cliente
            pedido.Cliente = conexion.RecordsetsResultsToObject<Cliente>();
            
            if (conexion.IsMoreRecordsets)
            {
                // Cuarto recordset: Dirección de envío
                pedido.DireccionEnvio = conexion.RecordsetsResultsToObject<Direccion>();
            }
        }
    }
    
    // Mostrar resumen
    Console.WriteLine($"Pedido #{pedido.NumeroPedido}");
    Console.WriteLine($"Cliente: {pedido.Cliente.Nombre}");
    Console.WriteLine($"Productos: {pedido.Detalles.Count}");
    Console.WriteLine($"Total: ${pedido.Total}");
}
```

### Caso 5: Manejo de Eventos InfoMessage

```csharp
// Capturar mensajes PRINT y RAISERROR del servidor
var conexion = new Conexion(ConexionType.MSSQLServer, connectionString);

conexion.InfoMessage += (args) =>
{
    foreach (SqlError error in args.Errors)
    {
        Console.WriteLine($"[SQL Server] {error.Message}");
    }
};

// Ejecutar procedimiento que genera mensajes
var result = conexion.Execute("sp_ProcesoLargo");

/*
Procedimiento almacenado ejemplo:
CREATE PROCEDURE sp_ProcesoLargo
AS
BEGIN
    PRINT 'Iniciando proceso...';
    -- Alguna operación
    PRINT 'Procesando datos...';
    -- Más operaciones
    PRINT 'Proceso completado.';
END
*/
```

---

## Mejores Prácticas

1. **Usar `using` para conexiones de corta duración:**
   ```csharp
   using (var conexion = new Conexion(ConexionType.MSSQLServer, connectionString))
   {
       var usuarios = conexion.ExecuteWithResults<Usuario>("SELECT * FROM Usuarios");
       // La conexión se cierra automáticamente
   }
   ```

2. **Configurar timeout apropiado:**
   ```csharp
   conexion.ConexionTimeOut = 120; // Para consultas largas
   ```

3. **Usar transacciones para operaciones relacionadas:**
   ```csharp
   conexion.TransactionBegin();
   try
   {
       // Múltiples operaciones
       conexion.TransactionCommit();
   }
   catch
   {
       conexion.TransactionRollback();
       throw;
   }
   ```

4. **Usar métodos con Action para grandes volúmenes:**
   ```csharp
   // Evitar cargar millones de registros en memoria
   conexion.ExecuteWithResults("SELECT * FROM BigTable", row =>
   {
       // Procesar fila por fila
   });
   ```

5. **Verificar conexión antes de operaciones críticas:**
   ```csharp
   if (!conexion.Test())
   {
       throw new Exception("No se pudo conectar a la base de datos");
   }
   ```

---

## Notas Importantes

- Los parámetros de salida `@Result`, `@Message` y `@Code` son opcionales pero recomendados para procedimientos almacenados.
- La clase usa reflexión para mapear objetos, asegúrate de que los nombres de propiedades coincidan con las columnas o usa `ConexionColumnAttribute`.
- Los métodos Recordsets mantienen la conexión abierta hasta que se leen todos los conjuntos de resultados.
- El método `ExecuteScript` separa comandos usando la palabra clave `GO`.

---

## Tipos Soportados

La clase puede mapear automáticamente los siguientes tipos primitivos:
- `string`
- `int` / `Int32`
- `decimal`
- `long` / `Int64`
- `bool` / `Boolean`
- `short` / `Int16`
- `float` / `Single`
- `double` / `Double`
- `char`

Para tipos complejos, crea clases POCO con propiedades públicas.

---
---

# Clase Result

## Descripción General

La clase `Result` es una clase de utilidad para encapsular el resultado de operaciones, proporcionando información sobre el éxito, mensaje, código de estado y datos opcionales. Existe en dos variantes: genérica (`Result<T>`) y no genérica (`Result`).

**Namespace:** `WarmPack.Classes`

---

## Variante: Result (No Genérica)

### Propiedades

```csharp
public bool Value { get; set; }      // Indica si la operación fue exitosa
public string Message { get; set; }  // Mensaje descriptivo del resultado
public object Data { get; set; }     // Datos asociados (tipo object)
public int Code { get; set; }        // Código de estado numérico
```

### Constructores

#### Result()
Constructor vacío que inicializa una instancia sin valores.

```csharp
var result = new Result();
result.Value = true;
result.Message = "Operación completada";
```

#### Result(bool value, string msg)
Crea un resultado con valor booleano y mensaje.

```csharp
var result = new Result(true, "Usuario creado exitosamente");
```

#### Result(bool value, string msg, object data)
Crea un resultado con valor, mensaje y datos.

```csharp
var usuarioId = 123;
var result = new Result(true, "Usuario creado", usuarioId);
```

#### Result(Exception ex)
Crea un resultado de error a partir de una excepción.

```csharp
try
{
    // Operación que puede fallar
    throw new Exception("Error de conexión");
}
catch (Exception ex)
{
    var result = new Result(ex);
    // result.Value = false
    // result.Message = "Error de conexión"
}
```

### Métodos Estáticos (Factory Methods)

#### Result.Create(bool value, string msg)
```csharp
var result = Result.Create(true, "Operación exitosa");
```

#### Result.Create(bool value, string msg, object data)
```csharp
var result = Result.Create(true, "Producto encontrado", productoObj);
```

#### Result.Create(Exception ex)
```csharp
var result = Result.Create(new Exception("Error al procesar"));
```

### Método InfoMessage

Permite almacenar y recuperar mensajes informativos adicionales.

```csharp
var result = new Result(true, "Proceso completado");

// Establecer mensaje informativo
result.InfoMessage("Procesados 150 registros en 2.5 segundos");

// Recuperar mensaje informativo
string info = result.InfoMessage();
Console.WriteLine(info); // "Procesados 150 registros en 2.5 segundos"
```

---

## Variante: Result&lt;T&gt; (Genérica)

### Propiedades

```csharp
public bool Value { get; set; }      // Indica si la operación fue exitosa
public string Message { get; set; }  // Mensaje descriptivo del resultado
public T Data { get; set; }          // Datos asociados (tipo genérico)
public int Code { get; set; }        // Código de estado numérico
```

### Constructores

#### Result&lt;T&gt;()
```csharp
var result = new Result<Usuario>();
```

#### Result&lt;T&gt;(bool value, string msg)
```csharp
var result = new Result<Usuario>(false, "Usuario no encontrado");
```

#### Result&lt;T&gt;(bool value, string msg, T data)
```csharp
var usuario = new Usuario { Id = 1, Nombre = "Juan" };
var result = new Result<Usuario>(true, "Usuario obtenido", usuario);
```

#### Result&lt;T&gt;(Exception ex)
```csharp
var result = new Result<Usuario>(new Exception("Error al consultar BD"));
```

---

## Casos de Uso de Result

### Caso 1: Validación de Formulario

```csharp
public Result ValidarUsuario(Usuario usuario)
{
    if (string.IsNullOrEmpty(usuario.Email))
    {
        return new Result(false, "El email es requerido");
    }
    
    if (!usuario.Email.Contains("@"))
    {
        return new Result(false, "El email no es válido");
    }
    
    if (usuario.Edad < 18)
    {
        return Result.Create(false, "El usuario debe ser mayor de 18 años");
    }
    
    return new Result(true, "Usuario válido");
}

// Uso
var usuario = new Usuario { Email = "juan@email.com", Edad = 25 };
var resultado = ValidarUsuario(usuario);

if (resultado.Value)
{
    Console.WriteLine("✓ " + resultado.Message);
}
else
{
    Console.WriteLine("✗ Error: " + resultado.Message);
}
```

### Caso 2: Operación de Base de Datos con Datos

```csharp
public Result<Usuario> ObtenerUsuarioPorId(int id)
{
    try
    {
        var usuario = _repository.Find(id);
        
        if (usuario == null)
        {
            return new Result<Usuario>(false, $"Usuario con ID {id} no encontrado");
        }
        
        return new Result<Usuario>(true, "Usuario encontrado", usuario);
    }
    catch (Exception ex)
    {
        return new Result<Usuario>(ex);
    }
}

// Uso
var resultado = ObtenerUsuarioPorId(100);

if (resultado.Value)
{
    Console.WriteLine($"Usuario: {resultado.Data.Nombre}");
    Console.WriteLine($"Email: {resultado.Data.Email}");
}
else
{
    Console.WriteLine($"Error: {resultado.Message}");
}
```

### Caso 3: Operación con Códigos de Estado

```csharp
public Result<string> ProcesarPago(decimal monto, string tarjeta)
{
    if (monto <= 0)
    {
        return new Result<string>(false, "Monto inválido")
        {
            Code = 400 // Bad Request
        };
    }
    
    if (!ValidarTarjeta(tarjeta))
    {
        return new Result<string>(false, "Tarjeta no válida")
        {
            Code = 422 // Unprocessable Entity
        };
    }
    
    try
    {
        var transaccionId = ProcesarEnPasarela(monto, tarjeta);
        
        return new Result<string>(true, "Pago procesado exitosamente", transaccionId)
        {
            Code = 200 // OK
        };
    }
    catch (Exception ex)
    {
        return new Result<string>(false, "Error en la pasarela de pago: " + ex.Message)
        {
            Code = 500 // Internal Server Error
        };
    }
}

// Uso
var resultado = ProcesarPago(150.50m, "4111111111111111");

switch (resultado.Code)
{
    case 200:
        Console.WriteLine($"Pago exitoso. ID: {resultado.Data}");
        break;
    case 400:
    case 422:
        Console.WriteLine($"Error de validación: {resultado.Message}");
        break;
    case 500:
        Console.WriteLine($"Error del servidor: {resultado.Message}");
        LogError(resultado.Message);
        break;
}
```

### Caso 4: Encadenamiento de Operaciones

```csharp
public Result<Usuario> CrearUsuario(string nombre, string email)
{
    // Validar
    var validacion = ValidarDatos(nombre, email);
    if (!validacion.Value)
    {
        return new Result<Usuario>(false, validacion.Message);
    }
    
    // Verificar si existe
    var existe = VerificarExistencia(email);
    if (existe.Value)
    {
        return new Result<Usuario>(false, "El email ya está registrado");
    }
    
    // Crear usuario
    try
    {
        var usuario = new Usuario { Nombre = nombre, Email = email };
        _repository.Insert(usuario);
        
        return new Result<Usuario>(true, "Usuario creado exitosamente", usuario);
    }
    catch (Exception ex)
    {
        return new Result<Usuario>(ex);
    }
}
```

### Caso 5: API REST Response

```csharp
[HttpPost]
public IActionResult CrearProducto([FromBody] ProductoDto dto)
{
    var resultado = _service.CrearProducto(dto);
    
    if (resultado.Value)
    {
        return Ok(new
        {
            success = resultado.Value,
            message = resultado.Message,
            data = resultado.Data
        });
    }
    else
    {
        return BadRequest(new
        {
            success = resultado.Value,
            message = resultado.Message,
            code = resultado.Code
        });
    }
}
```

### Caso 6: Manejo de Múltiples Errores con InfoMessage

```csharp
public Result ImportarArchivo(string rutaArchivo)
{
    var result = new Result(true, "Archivo procesado");
    var errores = new List<string>();
    var advertencias = new List<string>();
    
    foreach (var linea in File.ReadAllLines(rutaArchivo))
    {
        try
        {
            ProcesarLinea(linea);
        }
        catch (Exception ex)
        {
            errores.Add($"Línea {i}: {ex.Message}");
        }
    }
    
    if (errores.Count > 0)
    {
        result.Value = false;
        result.Message = $"Se encontraron {errores.Count} errores";
        result.InfoMessage(string.Join("\n", errores));
    }
    
    return result;
}

// Uso
var resultado = ImportarArchivo("datos.csv");
if (!resultado.Value)
{
    Console.WriteLine(resultado.Message);
    Console.WriteLine("Detalles:");
    Console.WriteLine(resultado.InfoMessage());
}
```

---
---

# Clase ResultList

## Descripción General

La clase `ResultList<T>` es una especialización de `Result` diseñada específicamente para encapsular resultados que contienen listas de datos. Hereda el patrón de Result pero optimiza el manejo de colecciones.

**Namespace:** `WarmPack.Classes`

---

## Propiedades

```csharp
public bool Value { get; set; }       // Indica si la operación fue exitosa
public string Message { get; set; }   // Mensaje descriptivo del resultado
public int Code { get; set; }         // Código de estado numérico
public List<T> Data { get; set; }     // Lista de datos del tipo T
```

---

## Constructores

### ResultList&lt;T&gt;()
Constructor vacío.

```csharp
var resultado = new ResultList<Usuario>();
```

### ResultList&lt;T&gt;(Result&lt;List&lt;T&gt;&gt; result)
Convierte un `Result<List<T>>` a `ResultList<T>`.

```csharp
var result = new Result<List<Usuario>>(true, "Usuarios obtenidos", listaUsuarios);
var resultList = new ResultList<Usuario>(result);
```

### ResultList&lt;T&gt;(bool value, string msg)
Crea un resultado con valor y mensaje.

```csharp
var resultado = new ResultList<Producto>(false, "No se encontraron productos");
```

### ResultList&lt;T&gt;(bool value, string msg, List&lt;T&gt; data)
Crea un resultado completo con lista de datos.

```csharp
var productos = new List<Producto> { producto1, producto2 };
var resultado = new ResultList<Producto>(true, "Productos encontrados", productos);
```

### ResultList&lt;T&gt;(Exception ex)
Crea un resultado de error desde una excepción.

```csharp
try
{
    // Operación
}
catch (Exception ex)
{
    var resultado = new ResultList<Cliente>(ex);
}
```

---

## Métodos Estáticos (Factory Methods)

### ResultList&lt;T&gt;.Create(bool value, string msg)
```csharp
var resultado = ResultList<Usuario>.Create(false, "No hay usuarios activos");
```

### ResultList&lt;T&gt;.Create(bool value, string msg, List&lt;T&gt; data)
```csharp
var usuarios = ObtenerUsuarios();
var resultado = ResultList<Usuario>.Create(true, "Usuarios encontrados", usuarios);
```

### ResultList&lt;T&gt;.Create(Exception ex)
```csharp
var resultado = ResultList<Producto>.Create(new Exception("Error al consultar"));
```

### ResultList&lt;T&gt;.Create(Result&lt;List&lt;T&gt;&gt; result)
```csharp
var result = new Result<List<Usuario>>(true, "OK", usuarios);
var resultList = ResultList<Usuario>.Create(result);
```

---

## Casos de Uso de ResultList

### Caso 1: Consulta de Múltiples Registros

```csharp
public ResultList<Producto> ObtenerProductosPorCategoria(int categoriaId)
{
    try
    {
        var productos = _repository
            .Where(p => p.CategoriaId == categoriaId)
            .ToList();
        
        if (productos.Count == 0)
        {
            return new ResultList<Producto>(
                false, 
                $"No se encontraron productos en la categoría {categoriaId}"
            );
        }
        
        return new ResultList<Producto>(
            true, 
            $"Se encontraron {productos.Count} productos", 
            productos
        );
    }
    catch (Exception ex)
    {
        return new ResultList<Producto>(ex);
    }
}

// Uso
var resultado = ObtenerProductosPorCategoria(5);

if (resultado.Value)
{
    Console.WriteLine(resultado.Message);
    
    foreach (var producto in resultado.Data)
    {
        Console.WriteLine($"- {producto.Nombre}: ${producto.Precio}");
    }
}
else
{
    Console.WriteLine($"Error: {resultado.Message}");
}
```

### Caso 2: Búsqueda con Filtros

```csharp
public ResultList<Usuario> BuscarUsuarios(string textoBusqueda, bool soloActivos)
{
    try
    {
        var query = _context.Usuarios.AsQueryable();
        
        if (!string.IsNullOrEmpty(textoBusqueda))
        {
            query = query.Where(u => 
                u.Nombre.Contains(textoBusqueda) || 
                u.Email.Contains(textoBusqueda)
            );
        }
        
        if (soloActivos)
        {
            query = query.Where(u => u.Activo);
        }
        
        var usuarios = query.ToList();
        
        return ResultList<Usuario>.Create(
            true, 
            $"Búsqueda completada. {usuarios.Count} resultados", 
            usuarios
        );
    }
    catch (Exception ex)
    {
        return ResultList<Usuario>.Create(ex);
    }
}

// Uso
var resultado = BuscarUsuarios("juan", true);

Console.WriteLine(resultado.Message);

if (resultado.Value && resultado.Data.Count > 0)
{
    foreach (var usuario in resultado.Data)
    {
        Console.WriteLine($"• {usuario.Nombre} - {usuario.Email}");
    }
}
else if (resultado.Value && resultado.Data.Count == 0)
{
    Console.WriteLine("No se encontraron resultados");
}
```

### Caso 3: Paginación de Resultados

```csharp
public class PaginacionResultList<T> : ResultList<T>
{
    public int PaginaActual { get; set; }
    public int TotalPaginas { get; set; }
    public int TotalRegistros { get; set; }
    public int RegistrosPorPagina { get; set; }
}

public PaginacionResultList<Producto> ObtenerProductosPaginados(int pagina, int tamaño)
{
    try
    {
        var totalRegistros = _repository.Count();
        var totalPaginas = (int)Math.Ceiling(totalRegistros / (double)tamaño);
        
        var productos = _repository
            .OrderBy(p => p.Nombre)
            .Skip((pagina - 1) * tamaño)
            .Take(tamaño)
            .ToList();
        
        return new PaginacionResultList<Producto>
        {
            Value = true,
            Message = $"Página {pagina} de {totalPaginas}",
            Data = productos,
            PaginaActual = pagina,
            TotalPaginas = totalPaginas,
            TotalRegistros = totalRegistros,
            RegistrosPorPagina = tamaño
        };
    }
    catch (Exception ex)
    {
        return new PaginacionResultList<Producto>
        {
            Value = false,
            Message = ex.Message
        };
    }
}

// Uso
var resultado = ObtenerProductosPaginados(1, 20);

if (resultado.Value)
{
    Console.WriteLine($"Mostrando {resultado.Data.Count} de {resultado.TotalRegistros} productos");
    Console.WriteLine($"Página {resultado.PaginaActual} de {resultado.TotalPaginas}");
    
    foreach (var producto in resultado.Data)
    {
        Console.WriteLine($"- {producto.Nombre}");
    }
}
```

### Caso 4: Operaciones en Lote

```csharp
public ResultList<ResultadoImportacion> ImportarUsuariosDesdeCSV(string archivo)
{
    var resultados = new List<ResultadoImportacion>();
    var lineas = File.ReadAllLines(archivo);
    
    foreach (var linea in lineas.Skip(1)) // Saltar encabezado
    {
        try
        {
            var datos = linea.Split(',');
            var usuario = new Usuario
            {
                Nombre = datos[0],
                Email = datos[1],
                Telefono = datos[2]
            };
            
            _repository.Insert(usuario);
            
            resultados.Add(new ResultadoImportacion
            {
                Exitoso = true,
                Linea = linea,
                Mensaje = "Usuario creado"
            });
        }
        catch (Exception ex)
        {
            resultados.Add(new ResultadoImportacion
            {
                Exitoso = false,
                Linea = linea,
                Mensaje = ex.Message
            });
        }
    }
    
    var exitosos = resultados.Count(r => r.Exitoso);
    var fallidos = resultados.Count(r => !r.Exitoso);
    
    return new ResultList<ResultadoImportacion>
    {
        Value = fallidos == 0,
        Message = $"Importación completada. Éxitos: {exitosos}, Fallos: {fallidos}",
        Data = resultados,
        Code = fallidos > 0 ? 206 : 200 // Partial Content : OK
    };
}

// Uso
var resultado = ImportarUsuariosDesdeCSV("usuarios.csv");

Console.WriteLine(resultado.Message);

if (!resultado.Value)
{
    Console.WriteLine("\nErrores encontrados:");
    foreach (var item in resultado.Data.Where(r => !r.Exitoso))
    {
        Console.WriteLine($"- {item.Mensaje}: {item.Linea}");
    }
}
```

### Caso 5: API REST con ResultList

```csharp
[HttpGet("productos")]
public IActionResult ObtenerProductos(
    [FromQuery] string categoria = null,
    [FromQuery] decimal? precioMin = null,
    [FromQuery] decimal? precioMax = null)
{
    var resultado = _service.BuscarProductos(categoria, precioMin, precioMax);
    
    if (resultado.Value)
    {
        return Ok(new
        {
            success = true,
            message = resultado.Message,
            count = resultado.Data.Count,
            data = resultado.Data
        });
    }
    else
    {
        return StatusCode(500, new
        {
            success = false,
            message = resultado.Message,
            code = resultado.Code
        });
    }
}
```

### Caso 6: Agregación y Estadísticas

```csharp
public ResultList<VentaDetalle> ObtenerVentasDelMes(int mes, int año)
{
    try
    {
        var ventas = _repository
            .Where(v => v.Fecha.Month == mes && v.Fecha.Year == año)
            .OrderByDescending(v => v.Fecha)
            .ToList();
        
        var total = ventas.Sum(v => v.Monto);
        var promedio = ventas.Average(v => v.Monto);
        
        var resultado = new ResultList<VentaDetalle>(true, "Ventas obtenidas", ventas)
        {
            Code = ventas.Count
        };
        
        resultado.InfoMessage($"Total: ${total:N2} | Promedio: ${promedio:N2}");
        
        return resultado;
    }
    catch (Exception ex)
    {
        return new ResultList<VentaDetalle>(ex);
    }
}

// Uso
var resultado = ObtenerVentasDelMes(11, 2025);

if (resultado.Value)
{
    Console.WriteLine($"Se encontraron {resultado.Code} ventas");
    Console.WriteLine(resultado.InfoMessage());
    
    foreach (var venta in resultado.Data.Take(10))
    {
        Console.WriteLine($"{venta.Fecha:dd/MM/yyyy} - ${venta.Monto}");
    }
}
```

### Caso 7: Conversión entre Result y ResultList

```csharp
// Convertir Result<List<T>> a ResultList<T>
public ResultList<Usuario> ObtenerUsuariosActivos()
{
    // Método que retorna Result<List<Usuario>>
    var resultOriginal = _conexion.ExecuteWithResults<Usuario>(
        "SELECT * FROM Usuarios WHERE Activo = 1"
    );
    
    // Convertir a ResultList
    var resultList = new ResultList<Usuario>(resultOriginal);
    
    return resultList;
}

// También usando el factory method
public ResultList<Cliente> ObtenerClientes()
{
    var result = _service.ConsultarClientes();
    
    return ResultList<Cliente>.Create(result);
}
```

---
---

# Clase Castable

## Descripción General

La clase `Castable` proporciona conversiones de tipo seguras con manejo de errores. Envuelve un valor de cualquier tipo y ofrece métodos para convertirlo a diferentes tipos primitivos de .NET, lanzando excepciones descriptivas cuando la conversión falla.

**Namespace:** `WarmPack.Classes`

---

## Constructor

```csharp
public Castable(object obj)
```

Crea una instancia de Castable envolviendo el objeto especificado.

```csharp
var castable = new Castable("123");
var numero = castable.ToInt32(); // 123
```

---

## Métodos de Conversión

Cada método de conversión tiene dos variantes:
- **To[Tipo]()**: Lanza excepción si la conversión falla
- **Try[Tipo](Func<Castable, [Tipo]> onError)**: Retorna valor por defecto si falla

---

### Conversión a Enteros

#### ToInt32() / TryInt32()
Convierte a `int` (32-bit).

```csharp
// Conversión exitosa
var c1 = new Castable("123");
int numero = c1.ToInt32(); // 123

// Conversión con error
var c2 = new Castable("abc");
try
{
    int numero = c2.ToInt32(); // Lanza excepción
}
catch (Exception ex)
{
    Console.WriteLine(ex.Message); 
    // "Unable to convert the value "abc" to the type "Int32""
}

// Conversión segura con valor por defecto
var c3 = new Castable("xyz");
int numero = c3.TryInt32(c => 0); // Retorna 0 si falla

// Con null
var c4 = new Castable(null);
int numero = c4.TryInt32(c => -1); // Retorna -1
```

#### ToUint32() / TryUint32()
Convierte a `uint` (unsigned 32-bit).

```csharp
var c = new Castable("4294967295");
uint numero = c.ToUint32();

// Seguro
var numero = new Castable("abc").TryUint32(c => 0);
```

#### ToInt64() / TryInt64()
Convierte a `long` (64-bit).

```csharp
var c = new Castable("9223372036854775807");
long numero = c.ToInt64();

// Seguro
var numero = new Castable(null).TryInt64(c => 0L);
```

#### ToUInt64() / TryUInt64()
Convierte a `ulong` (unsigned 64-bit).

```csharp
var c = new Castable("18446744073709551615");
ulong numero = c.ToUInt64();
```

---

### Conversión a Números Decimales

#### ToDecimal() / TryDecimal()
Convierte a `decimal` (alta precisión).

```csharp
var c1 = new Castable("123.456");
decimal precio = c1.ToDecimal(); // 123.456

var c2 = new Castable("$1,234.56"); // Puede fallar según cultura
try
{
    decimal precio = c2.ToDecimal();
}
catch (Exception ex)
{
    Console.WriteLine("Error al convertir: " + ex.Message);
}

// Conversión segura
var c3 = new Castable("inválido");
decimal precio = c3.TryDecimal(c => 0.00m);
```

#### ToSingle() / TrySingle()
Convierte a `float` (precisión simple).

```csharp
var c = new Castable("3.14159");
float numero = c.ToSingle(); // 3.14159

// Seguro
var numero = new Castable("x").TrySingle(c => 0.0f);
```

#### ToDouble() / TryDouble()
Convierte a `double` (precisión doble).

```csharp
var c = new Castable("3.141592653589793");
double numero = c.ToDouble();

// Seguro con null
var numero = new Castable(null).TryDouble(c => 0.0);
```

---

### Conversión a Booleano

#### ToBoolean() / TryBoolean()
Convierte a `bool`.

```csharp
var c1 = new Castable("true");
bool valor = c1.ToBoolean(); // true

var c2 = new Castable("1");
bool valor = c2.ToBoolean(); // true

var c3 = new Castable("false");
bool valor = c3.ToBoolean(); // false

var c4 = new Castable("0");
bool valor = c4.ToBoolean(); // false

// Seguro
var valor = new Castable("maybe").TryBoolean(c => false);
```

---

### Conversión a Byte

#### ToByte() / TryByte()
Convierte a `byte` (0-255).

```csharp
var c = new Castable("255");
byte valor = c.ToByte(); // 255

// Fuera de rango lanza excepción
var c2 = new Castable("256");
try
{
    byte valor = c2.ToByte(); // Excepción
}
catch
{
    // Manejar error
}

// Seguro
var valor = new Castable("300").TryByte(c => 0);
```

---

### Conversión a Array de Bytes

#### ToBytes() / TryBytes()
Convierte a `byte[]`.

```csharp
byte[] datos = new byte[] { 0x48, 0x65, 0x6C, 0x6C, 0x6F };
var c = new Castable(datos);
byte[] resultado = c.ToBytes(); // Retorna el array

// Seguro con null
var c2 = new Castable(null);
byte[] resultado = c2.TryBytes(c => new byte[0]);

// Con tipo incorrecto
var c3 = new Castable("texto");
byte[] resultado = c3.TryBytes(c => new byte[0]);
```

---

### Conversión a DateTime

#### ToDateTime() / TryDateTime()
Convierte a `DateTime`.

```csharp
var c1 = new Castable("2025-11-12");
DateTime fecha = c1.ToDateTime(); // DateTime(2025, 11, 12)

var c2 = new Castable("12/11/2025 15:30:00");
DateTime fecha = c2.ToDateTime();

// Con formato inválido
var c3 = new Castable("no es fecha");
try
{
    DateTime fecha = c3.ToDateTime(); // Excepción
}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
}

// Seguro con valor por defecto
var c4 = new Castable("inválido");
DateTime fecha = c4.TryDateTime(c => DateTime.Now);

// Con null
var c5 = new Castable(null);
DateTime fecha = c5.TryDateTime(c => DateTime.MinValue);
```

---

### Conversión a String

#### ToString() / TryString()
Convierte a `string`.

```csharp
var c1 = new Castable(123);
string texto = c1.ToString(); // "123"

var c2 = new Castable(true);
string texto = c2.ToString(); // "True"

var c3 = new Castable(null);
string texto = c3.ToString(); // "" (cadena vacía)

// Seguro con valor por defecto
var c4 = new Castable(null);
string texto = c4.TryString(c => "Sin valor");
```

---

## Casos de Uso de Castable

### Caso 1: Lectura Segura de DataReader

```csharp
public List<Usuario> ObtenerUsuarios(IDataReader reader)
{
    var usuarios = new List<Usuario>();
    
    while (reader.Read())
    {
        var usuario = new Usuario
        {
            Id = new Castable(reader["Id"]).ToInt32(),
            Nombre = new Castable(reader["Nombre"]).ToString(),
            Email = new Castable(reader["Email"]).ToString(),
            Edad = new Castable(reader["Edad"]).TryInt32(c => 0),
            Activo = new Castable(reader["Activo"]).ToBoolean(),
            FechaRegistro = new Castable(reader["FechaRegistro"]).ToDateTime(),
            Saldo = new Castable(reader["Saldo"]).TryDecimal(c => 0.00m)
        };
        
        usuarios.Add(usuario);
    }
    
    return usuarios;
}
```

### Caso 2: Procesamiento de Parámetros de Query String

```csharp
public IActionResult BuscarProductos()
{
    // Obtener parámetros del query string
    var categoriaId = new Castable(Request.Query["categoria"])
        .TryInt32(c => 0);
    
    var precioMin = new Castable(Request.Query["precioMin"])
        .TryDecimal(c => 0m);
    
    var precioMax = new Castable(Request.Query["precioMax"])
        .TryDecimal(c => decimal.MaxValue);
    
    var soloActivos = new Castable(Request.Query["activos"])
        .TryBoolean(c => true);
    
    var fechaDesde = new Castable(Request.Query["desde"])
        .TryDateTime(c => DateTime.Now.AddMonths(-1));
    
    // Usar los parámetros convertidos
    var productos = _service.BuscarProductos(
        categoriaId, 
        precioMin, 
        precioMax, 
        soloActivos, 
        fechaDesde
    );
    
    return Ok(productos);
}
```

### Caso 3: Lectura de Configuración

```csharp
public class ConfiguracionApp
{
    private Dictionary<string, object> _settings;
    
    public ConfiguracionApp(Dictionary<string, object> settings)
    {
        _settings = settings;
    }
    
    public int ObtenerPuertoServidor()
    {
        return new Castable(_settings["Puerto"])
            .TryInt32(c => 8080); // Puerto por defecto
    }
    
    public string ObtenerCadenaConexion()
    {
        return new Castable(_settings["ConnectionString"])
            .TryString(c => "Server=localhost;Database=Default");
    }
    
    public bool HabilitarCache()
    {
        return new Castable(_settings["EnableCache"])
            .TryBoolean(c => true);
    }
    
    public TimeSpan ObtenerTimeout()
    {
        var segundos = new Castable(_settings["TimeoutSegundos"])
            .TryInt32(c => 30);
        
        return TimeSpan.FromSeconds(segundos);
    }
}

// Uso
var config = new Dictionary<string, object>
{
    ["Puerto"] = "9000",
    ["EnableCache"] = "true",
    ["TimeoutSegundos"] = "60"
};

var app = new ConfiguracionApp(config);
int puerto = app.ObtenerPuertoServidor(); // 9000
bool cache = app.HabilitarCache(); // true
```

### Caso 4: Validación y Conversión de Formularios

```csharp
public Result RegistrarUsuario(Dictionary<string, string> formulario)
{
    try
    {
        var usuario = new Usuario();
        
        // Campos requeridos - usar To[Tipo]() para forzar validación
        usuario.Nombre = new Castable(formulario["nombre"]).ToString();
        usuario.Email = new Castable(formulario["email"]).ToString();
        
        // Campos opcionales - usar Try[Tipo]() con valores por defecto
        usuario.Edad = new Castable(formulario["edad"])
            .TryInt32(c => 18);
        
        usuario.Telefono = new Castable(formulario["telefono"])
            .TryString(c => "");
        
        usuario.RecibirNewsletter = new Castable(formulario["newsletter"])
            .TryBoolean(c => false);
        
        usuario.FechaNacimiento = new Castable(formulario["fechaNacimiento"])
            .TryDateTime(c => DateTime.Now.AddYears(-18));
        
        // Validar campos
        if (string.IsNullOrEmpty(usuario.Nombre))
        {
            return new Result(false, "El nombre es requerido");
        }
        
        if (usuario.Edad < 18)
        {
            return new Result(false, "Debe ser mayor de 18 años");
        }
        
        // Guardar usuario
        _repository.Insert(usuario);
        
        return new Result(true, "Usuario registrado exitosamente");
    }
    catch (Exception ex)
    {
        return new Result(false, "Error al registrar: " + ex.Message);
    }
}
```

### Caso 5: Importación de Datos desde CSV

```csharp
public ResultList<Producto> ImportarProductosCSV(string rutaArchivo)
{
    var productos = new List<Producto>();
    var errores = new List<string>();
    var lineas = File.ReadAllLines(rutaArchivo);
    
    for (int i = 1; i < lineas.Length; i++) // Saltar encabezado
    {
        try
        {
            var campos = lineas[i].Split(',');
            
            var producto = new Producto
            {
                Codigo = new Castable(campos[0]).ToString(),
                Nombre = new Castable(campos[1]).ToString(),
                Precio = new Castable(campos[2]).ToDecimal(),
                Stock = new Castable(campos[3]).TryInt32(c => 0),
                Activo = new Castable(campos[4]).TryBoolean(c => true),
                FechaVencimiento = new Castable(campos[5])
                    .TryDateTime(c => DateTime.MaxValue)
            };
            
            productos.Add(producto);
        }
        catch (Exception ex)
        {
            errores.Add($"Línea {i + 1}: {ex.Message}");
        }
    }
    
    if (errores.Count > 0)
    {
        return new ResultList<Producto>
        {
            Value = false,
            Message = $"Importación con errores. {errores.Count} líneas fallaron",
            Data = productos
        };
    }
    
    return new ResultList<Producto>(true, "Importación exitosa", productos);
}
```

### Caso 6: Manejo de Valores de Base de Datos (NULL-safe)

```csharp
public class ConexionCastableRow
{
    private IDataReader _reader;
    
    public ConexionCastableRow(IDataReader reader)
    {
        _reader = reader;
    }
    
    public Castable this[string columnName]
    {
        get
        {
            var valor = _reader[columnName];
            
            // Si es DBNull, convertir a null
            if (valor is DBNull)
            {
                valor = null;
            }
            
            return new Castable(valor);
        }
    }
    
    public Castable this[int columnIndex]
    {
        get
        {
            var valor = _reader[columnIndex];
            
            if (valor is DBNull)
            {
                valor = null;
            }
            
            return new Castable(valor);
        }
    }
}

// Uso
while (reader.Read())
{
    var row = new ConexionCastableRow(reader);
    
    var id = row["Id"].ToInt32();
    var nombre = row["Nombre"].TryString(c => "Sin nombre");
    var edad = row["Edad"].TryInt32(c => 0); // Si es NULL, retorna 0
    var saldo = row["Saldo"].TryDecimal(c => 0.00m);
    var activo = row["Activo"].TryBoolean(c => false);
    
    Console.WriteLine($"{id} - {nombre} ({edad} años) - ${saldo}");
}
```

### Caso 7: Conversión de Tipos Dinámicos

```csharp
public object ConvertirValor(object valor, Type tipoDestino)
{
    var castable = new Castable(valor);
    
    if (tipoDestino == typeof(int))
        return castable.TryInt32(c => 0);
    
    if (tipoDestino == typeof(decimal))
        return castable.TryDecimal(c => 0m);
    
    if (tipoDestino == typeof(bool))
        return castable.TryBoolean(c => false);
    
    if (tipoDestino == typeof(DateTime))
        return castable.TryDateTime(c => DateTime.MinValue);
    
    if (tipoDestino == typeof(double))
        return castable.TryDouble(c => 0.0);
    
    if (tipoDestino == typeof(long))
        return castable.TryInt64(c => 0L);
    
    return castable.TryString(c => "");
}

// Uso
var resultado = ConvertirValor("123", typeof(int)); // 123
var resultado2 = ConvertirValor("true", typeof(bool)); // true
var resultado3 = ConvertirValor("abc", typeof(int)); // 0 (valor por defecto)
```

### Caso 8: API con Conversiones de Parámetros Flexibles

```csharp
[HttpGet("reporte")]
public IActionResult GenerarReporte(
    [FromQuery] string fechaInicio,
    [FromQuery] string fechaFin,
    [FromQuery] string formatoExport,
    [FromQuery] string incluirDetalles)
{
    // Conversiones seguras de parámetros
    var inicio = new Castable(fechaInicio)
        .TryDateTime(c => DateTime.Now.AddMonths(-1));
    
    var fin = new Castable(fechaFin)
        .TryDateTime(c => DateTime.Now);
    
    var formato = new Castable(formatoExport)
        .TryString(c => "PDF").ToUpper();
    
    var detalles = new Castable(incluirDetalles)
        .TryBoolean(c => false);
    
    // Generar reporte con parámetros convertidos
    var reporte = _service.GenerarReporte(inicio, fin, formato, detalles);
    
    return File(reporte.Data, "application/octet-stream", $"reporte.{formato.ToLower()}");
}
```

---

## Comparación: Result vs ResultList vs Castable

| Característica | Result | ResultList | Castable |
|---------------|--------|------------|----------|
| **Propósito** | Encapsular resultado de operación | Encapsular lista de resultados | Convertir tipos de forma segura |
| **Tipo de Dato** | Genérico `T` o `object` | Siempre `List<T>` | Objeto `object` interno |
| **Uso Principal** | Retorno de métodos de negocio | Retorno de consultas múltiples | Conversión de tipos |
| **Manejo de Errores** | Por propiedades Value/Message | Por propiedades Value/Message | Por excepciones o valores default |
| **Información Adicional** | Code, InfoMessage | Code | Ninguna |

---

## Mejores Prácticas

### Para Result y ResultList

1. **Usar Result para operaciones únicas y ResultList para colecciones**
   ```csharp
   Result<Usuario> ObtenerUsuario(int id);
   ResultList<Usuario> ObtenerTodosLosUsuarios();
   ```

2. **Establecer códigos de estado consistentes**
   ```csharp
   result.Code = 200; // OK
   result.Code = 404; // No encontrado
   result.Code = 500; // Error del servidor
   ```

3. **Mensajes descriptivos**
   ```csharp
   new Result(false, "El usuario con ID 123 no existe");
   // Mejor que: new Result(false, "Error");
   ```

### Para Castable

1. **Usar To[Tipo]() para campos requeridos**
   ```csharp
   int id = new Castable(valor).ToInt32(); // Debe convertirse o falla
   ```

2. **Usar Try[Tipo]() para campos opcionales**
   ```csharp
   int edad = new Castable(valor).TryInt32(c => 0); // Safe con default
   ```

3. **Manejar null explícitamente**
   ```csharp
   var resultado = new Castable(valorNuleable)
       .TryString(c => "Valor por defecto");
   ```
