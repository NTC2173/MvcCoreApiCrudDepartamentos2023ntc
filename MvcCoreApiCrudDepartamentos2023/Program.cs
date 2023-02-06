using MvcCoreApiCrudDepartamentos2023.Services;

namespace MvcCoreApiCrudDepartamentos2023
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            //ARQUITECTURA INYECCION DE DEPENDENCIAS

            string urlApiDepartamentos = builder.Configuration.GetValue<string>("ApiUrls:ApiCrudDepartamentos");
            //Obtiene una cadena que representa la URL de la API relacionada con los departamentos,
            //a partir del objeto de configuración de la aplicación (builder.Configuration). La cadena se
            //obtiene accediendo a un valor específico en la configuración que se encuentra
            //bajo la clave "ApiUrls:ApiCrudDepartamentos".

            builder.Services.AddTransient<ServiceDepartamentos>
                (x => new ServiceDepartamentos(urlApiDepartamentos));
            //Agrega una nueva instancia de la clase 'ServiceDepartamentos' a la lista de dependencias
            //de la aplicación. La instancia se crea al llamar al constructor
            //'ServiceDepartamentos(urlApiDepartamentos)', pasándole la URL de la API obtenida
            //en la línea anterior como argumento


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}