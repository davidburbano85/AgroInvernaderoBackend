using Dapper;
using invernaderoInteligenteBackend.Api.Hubs;
using invernaderoInteligenteBackend.Aplicacion.Interfaces.Context;
using invernaderoInteligenteBackend.Aplicacion.Interfaces.IAuth;
using invernaderoInteligenteBackend.Aplicacion.Interfaces.IRepositorios;
using invernaderoInteligenteBackend.Aplicacion.Interfaces.IServicios;
using invernaderoInteligenteBackend.Aplicacion.Interfaces.IServicios.ISignalR;
using invernaderoInteligenteBackend.Aplicacion.Interfaces.IunitOfWork;
using invernaderoInteligenteBackend.Aplicacion.Interfaces.IWebSockets;
using invernaderoInteligenteBackend.Aplicacion.Servicios.Auth;
using invernaderoInteligenteBackend.Aplicacion.Servicios.ServiciosDto;
using invernaderoInteligenteBackend.Aplicacion.Servicios.SignalR;
using invernaderoInteligenteBackend.Aplicacion.Servicios.WebSocketServicio;
using invernaderoInteligenteBackend.Dominio.Enums.EntidadEstadoInstrumento;
using invernaderoInteligenteBackend.Infraestructura.AccesoADatos;
using invernaderoInteligenteBackend.Infraestructura.Auth;
using invernaderoInteligenteBackend.Infraestructura.Conexion;
using invernaderoInteligenteBackend.Infraestructura.Context;
using invernaderoInteligenteBackend.Infraestructura.RepositoriosDapper;
using invernaderoInteligenteBackend.Infraestructura.UnitOfWork;
using invernaderoInteligenteBackend.Infraestructura.WebSockets;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Npgsql;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Text.Json.Serialization;


IdentityModelEventSource.ShowPII = true; // Habilita la visualización de información detallada de errores de autenticación para depuración


var builder = WebApplication.CreateBuilder(args);
Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;

// Crea un constructor de DataSource para la conexión a la base de datos PostgreSQL
// utilizando la cadena de conexión "DefaultConnection" del archivo de configuración

var dataSourceBuilder = new NpgsqlDataSourceBuilder(
    builder.Configuration.GetConnectionString("DefaultConnection"));


// Mapea la enumeración EstadoInstrumento a la columna "estado_instrumento" en la base de datos
// con el fin de que Dapper pueda convertir automáticamente entre la enumeración y el valor almacenado en la base de datos.
dataSourceBuilder.MapEnum<EstadoInstrumento>("estado_instrumento");

var dataSource = dataSourceBuilder.Build();

builder.Services.AddSingleton(dataSource);


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMemoryCache();
builder.Services.AddSignalR();


//=====================
//INYECCION DE DEPENDENCIAS
//=====================

// Inyección de dependencias para la conexión a la base de datos

builder.Services.AddScoped<IDBConnectionFactory, NpgsqlConnectionFactory>();

//===================== AUTH =====================
builder.Services.AddHttpClient<IAuthServicio, AuthServicio>();
//===================== JWT =====================
builder.Services.AddScoped< IJwtServicio,  JwtServicio>();

// ===================== CONTEXT     =====================
builder.Services.AddHttpContextAccessor();//registra el servicio IHttpContextAccessor para acceder al contexto HTTP en otras partes de la aplicación
builder.Services.AddScoped<IUsuarioContext, UsuarioContextServicio>();//registra el servicio IUsuarioContext para acceder a la información del usuario en otras partes de la aplicación

builder.Services.AddScoped<IControladorContext, ControladorContextServicio>();//registra el servicio IControladorContext para acceder a la información del controlador en otras partes de la aplicación

//===================== IunitFWork =====================
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
//===================== TOKE STORE =====================
builder.Services.AddSingleton<TokenStore>();
// ===================== USUARIO REPOSITORIO ==============
builder.Services.AddScoped<IUsuarioRepositorioDapper, UsuarioRepositorioDapper>();

// ===================== INVERNADERO ==============
builder.Services.AddScoped<IInvernaderoRepositorio, InvernaderoRepositorioDapper>();
builder.Services.AddScoped<IInvernaderoServicio, InvernaderoServicio>();
// ===================== CONTROLADOR IOT ==============
builder.Services.AddScoped<IControladorIotRepositorio, ControladorIotRepositorioDapper>();
builder.Services.AddScoped<IControladorIotServicio, ControladorIotServicio>();
// ===================== INSTRUMENTO ==============
builder.Services.AddScoped<IInstrumentoRepositorio, InstrumentoRepositorioDapper>();
builder.Services.AddScoped<IInstrumentoServicio, InstrumentoServicio>();
// ===================== TIPO INSTRUMENTO ==============
builder.Services.AddScoped<ITipoInstrumentoRepositorio, TipoInstrumentoRepositorioDapper>();
builder.Services.AddScoped<ITipoInstrumentoServicio, TipoInstrumentoServicio>();
// ===================== MEDICION ==============
builder.Services.AddScoped<IMedicionRepositorio, MedicionRepositorioDapper>();
builder.Services.AddScoped<IMedicionServicio, MedicionServicio>();

// ========================== USUARIOS =========================
builder.Services.AddScoped<IUsuarioRepositorioDapper, UsuarioRepositorioDapper>();
builder.Services.AddScoped<IUsuarioServicio, UsuarioServicio>();

// ========================== WEB SOCKETS =========================
builder.Services.AddSingleton<IWebSocketManager, WebSocketManagerRepositorio>();
builder.Services.AddScoped<IWebSocketServicio, WebSocketServicio>();

// =====================  SIGNALR =========================
builder.Services.AddScoped<ISignalRServicio, SignalRServicio>();




//===================== FIN DE INYECCION DE DEPENDENCIAS =====================



/*
=========================================
CONFIGURACIÓN AUTENTICACIÓN JWT SUPABASE
=========================================
*/
var supabaseIssuer = builder.Configuration.GetSection("Supabase")["Issuer"]; // Obtiene la URL del proyecto Supabase desde la configuración
var supabaseAudience = builder.Configuration.GetSection("Supabase")["Audience"]; // Obtiene la audiencia esperada para los tokens JWT desde la configuración


builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(options=>
{
    options.Authority = supabaseIssuer; // Establece la URL de Supabase como autoridad para la validación de tokens
    options.Audience = supabaseAudience; // Establece la audiencia esperada para los tokens JWT
    options.RequireHttpsMetadata = false; // Deshabilita la exigencia de HTTPS para el endpoint de metadatos (útil para desarrollo local)
    options.TokenValidationParameters = new TokenValidationParameters
    {

        ValidateIssuer = true, // Habilita la validación del emisor del token
        ValidIssuer = supabaseIssuer, // Establece el emisor válido (la URL de Supabase)
        ValidateAudience = true, // Habilita la validación de la audiencia del token
        ValidAudience = supabaseAudience, // Establece la audiencia válida (configurada en Supabase)
        ValidateLifetime = true, // Habilita la validación de la vida útil del token
        ValidateIssuerSigningKey = true, // Habilita la validación de la firma del token
        ClockSkew = TimeSpan.FromSeconds(30), // Permite un margen de tiempo para la expiración del token (útil para evitar problemas de sincronización de reloj)
        NameClaimType = "sub",
        RoleClaimType = "role"
    };
    options.Events = new JwtBearerEvents
    {
        // =================== EVENTOS JWT SUPABASE =================
     

        // ==========================================
        // 🔥 CUANDO LLEGA EL TOKEN
        // ==========================================

        OnMessageReceived = context =>
        {

            var header = context.Request.Headers["Authorization"].ToString();
            if (!string.IsNullOrEmpty(header))
            {
                var token = header.Replace("Bearer ", ""); // Extrae el token JWT del encabezado Authorization
                try
                {
                    var handler = new JwtSecurityTokenHandler();
                    var jwtToken = handler.ReadJwtToken(token); // Intenta leer el token JWT para verificar su formato

                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error al leer el token JWT: {ex.Message}"); // Log de error si el token no se puede leer
                }
            }
            Console.WriteLine($"TOKEN VALIDADO: {context.Principal?.Identity?.IsAuthenticated}");

            return Task.CompletedTask;
        },
        // ==========================================
        // 🔥 CUANDO .NET DESCARGA METADATA OPENID
        // ==========================================
        OnAuthenticationFailed = context =>
            {
                Console.WriteLine("\n===== JWT AUTHENTICATION FAILED =====");
                Console.WriteLine($"Error de autenticación: {context.Exception.Message}"); // Log del error de autenticación para depuración
                Console.WriteLine("===== FIN AUTHENTICATION FAILED =====\n");
                return Task.CompletedTask;
            },
        // ==========================================
        // 🔥 TOKEN VALIDADO
        // ==========================================
        OnTokenValidated = context =>
            {
                Console.WriteLine("\n===== JWT TOKEN VALIDATED =====");
                Console.WriteLine("Token JWT validado correctamente."); // Log de éxito al validar el token
                Console.WriteLine("===== FIN TOKEN VALIDATED =====\n");
                return Task.CompletedTask;
            },
        // ==========================================
        // 🔥 CUANDO FALLA AUTORIZACIÓN
        // ==========================================
        OnChallenge = context =>
            {
                Console.WriteLine("\n===== JWT CHALLENGE OCCURRED =====");
                Console.WriteLine($"Error de desafío: {context.Error}, Descripción: {context.ErrorDescription}"); // Log del error de desafío para depuración
                return Task.CompletedTask;
            },
        // ==========================================
        // 🔥 CUANDO USER NO TIENE PERMISOS
        // ==========================================
        OnForbidden = context =>
            {
                Console.WriteLine("\n===== JWT FORBIDDEN =====");
                Console.WriteLine("Acceso prohibido: el usuario no tiene permisos para acceder al recurso."); // Log de acceso prohibido para depuración
                return Task.CompletedTask;

            }

    };
    Console.WriteLine("===== FIN CONFIG JWT =====");
});
/*
 ==========================================
FIN CONFIGURACIÓN AUTENTICACIÓN JWT SUPABASE
 ===============
 */
//********  AUTORIZACION **********
builder.Services.AddAuthorization();

/*
 *****************************
 CONFIGURACION DE CORS
 *****************************
 */

builder.Services.AddCors(options =>
{
    options.AddPolicy("Policy", policy =>
    {
        policy.WithOrigins(
            "http://localhost:4200",
            "http://127.0.0.1:4200"

            )
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials(); ;
    });
});

/* *******************************
 CONFIGURACION DE SERIALIZACION JSON
*******************************
se agrega un convertidor para serializar y deserializar
enumeraciones como cadenas en lugar de números, 
lo que facilita la lectura y comprensión de los datos JSON.
 */
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(
            new JsonStringEnumConverter());
    });


builder.Services.AddEndpointsApiExplorer();


builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Escribe: Bearer {tu token}"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("Policy");
app.UseWebSockets();


app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();
app.MapHub<ControladorHub>("/controladorHub");


app.Run();