using MvcCoreApiCrudDepartamentos2023.Models;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace MvcCoreApiCrudDepartamentos2023.Services
{
    public class ServiceDepartamentos
    {
        // La clase tiene dos propiedades privadas, una es "UrlApi" y otra es "header".
        // El constructor de la clase toma una URL como argumento y la asigna a la propiedad
        // "UrlApi". Además, se crea una instancia de "MediaTypeWithQualityHeaderValue"
        // y se asigna a la propiedad "header" con el valor "application/json"

        private string UrlApi;
        private MediaTypeWithQualityHeaderValue header;

        public ServiceDepartamentos(string url)
        {
            this.UrlApi = url;
            this.header = new MediaTypeWithQualityHeaderValue("application/json");
        }

        //METODO GENERICO PARA DEVOLVER CUALQUIER CLASE CON GET 

        private async Task<T> CallApiAsync<T>(string request)
        {
            //El método crea una instancia de "HttpClient", establece la dirección base en
            //la URL almacenada en "UrlApi", agrega un encabezado de aceptación con el tipo
            //"application/json" y luego realiza una solicitud GET a la URL proporcionada en
            //"request"
            using (HttpClient client = new HttpClient())
            {
                //La clase "HttpClient" es parte de la biblioteca .NET Standard y se utiliza
                //para realizar solicitudes HTTP en una aplicación.
                //Después de crear la instancia de "HttpClient", se puede establecer la dirección
                //base, agregar encabezados y realizar solicitudes HTTP mediante métodos como
                //"BaseAddress" o "DefaultRequestHeaders"

                //Este código establece la dirección base del cliente HTTP en la URL almacenada
                //en la propiedad "UrlApi". Luego, se limpian los encabezados de solicitud
                //predeterminados y se agrega un encabezado de aceptación con el
                //tipo "application/json"
                client.BaseAddress = new Uri(this.UrlApi);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.header);

                //Luego, se realiza una solicitud GET a la URL proporcionada en "request".
                //La respuesta se almacena en una variable "response" del
                //tipo "HttpResponseMessage".
                HttpResponseMessage response = await client.GetAsync(request);

                //se verifica si la respuesta es exitosa utilizando el método
                //"IsSuccessStatusCode" de "HttpResponseMessage"
                if (response.IsSuccessStatusCode)
                {
                    //Si la respuesta es exitosa, se lee el contenido de la respuesta y
                    //se almacena en una variable "data" genérica y se devuelve
                    T data = await response.Content.ReadAsAsync<T>();
                    return data;
                }
                else
                {
                    //Si la respuesta no es exitosa, se devuelve "default(T)" que es el
                    //valor por defecto para el tipo genérico "T".
                    return default(T);
                }
            }
        }
        

        //Metodo asíncrono llamado "GetDepartamentosAsync". Este método tiene como objetivo
        //recuperar una lista de objetos "Departamento" de una API
        public async Task<List<Departamento>> GetDepartamentosAsync()
        {
            //La variable "request" se establece en "api/departamentos", lo que significa
            //que la URL completa a la que se realizará la solicitud será
            //"https://api.example.com/api/departamentos" si se utiliza la dirección base
            //del cliente HTTP establecida previamente.
            string request = "api/departamentos";

            //A continuación, se llama al método genérico "CallApiAsync" con la variable
            //"request" y el tipo "List<Departamento>". La respuesta de la API se almacena
            //en la variable "departamentos" y finalmente se devuelve.
            List<Departamento> departamentos = await this.CallApiAsync<List<Departamento>>(request);
            return departamentos;

            //En resumen, este método hace una llamada a una API y devuelve una lista de objetos
            //"Departamento"


        }

        //"Task" es un tipo de valor en C# que representa una operación asíncrona que puede devolver
        //un resultado.
        //"Task<List<Departamento>>" significa que el método devolverá una tarea que, una vez
        //completada, proporcionará una lista de objetos "Departamento". Esto es útil para tareas
        //que pueden tardar mucho tiempo en completarse, como la recuperación de datos de una API,
        //ya que permite que la aplicación continúe ejecutándose mientras se espera la respuesta
        //de la tarea asíncrona.
        public async Task<Departamento> FindDepartamentoAsync(int iddepartamento)
        {
            string request = "/api/departamentos/" + iddepartamento;
            Departamento departamento = await this.CallApiAsync<Departamento>(request);
            return departamento; 
        }

        public async Task<List<Departamento>> GetDepartamentosLocalidadAsync(string localidad)
        {
            string request = "api/departamentos/finddepartamentoslocalidad/" + localidad;
            List<Departamento> departamentos = await this.CallApiAsync<List<Departamento>>(request);
            return departamentos; 
        }

        //LOS METODOS DE ACCION NO SUELEN TENER UN METODO GENERICO DEBIDO A QUE CADA UNO 
        //RECIBE UN VALOR DISTINTO

        public async Task DeleteDepartamentoAsync(int iddepartamento)
        {
            using(HttpClient client = new HttpClient())
            {
                string request = "/api/departamentos/" + iddepartamento;
                client.BaseAddress = new Uri(this.UrlApi);
                client.DefaultRequestHeaders.Clear();
                //COMO NO VAMOS A LEER NADA (OBJETO) SIMPLEMENTE SE REALIZA LA ACCION
                await client.DeleteAsync(request);
                
            }
        }

        //VAMOS A UTILIZAR INSERTAR OBJETO EN EL BODY
        public async Task InsertDepartamentoAsync(int id, string nombre, string localidad)
        {
            using(HttpClient client = new HttpClient())
            {
                string request = "/api/departamentos";
                client.BaseAddress = new Uri(this.UrlApi);
                client.DefaultRequestHeaders.Clear();
                //TENEMOS QUE ENVIAR UN OBJETO DEPARTAMENTO POR LO QUE CREAMOS UNA
                //CLASE DEL MODEL DEPARTAMENTO CON LOS DATOS QUE NOS HAN PROPORCIONADO
                Departamento departamento = new Departamento();
                departamento.IdDepartamento = id;
                departamento.Nombre = nombre;
                departamento.Localidad = localidad;
                //CONVERTIMOS EL OBJETO DEPARTAMENTO EN UN OBJETO JSON
                string departamentoJson = JsonConvert.SerializeObject(departamento);
                //PARA ENVIAR EL OBJETO JSON EN EL BODY SE REALIZA MEDIANTE UNA CLASE 
                //LLAMADA StringContent DONDE DEBEMOS INDICAR EL TIPO DE CONTENIDO QUE
                //ESTAMOS ENVIANDO (JSON)
                StringContent content = new StringContent(departamentoJson, Encoding.UTF8, "application/json");

                //Encoding.UTF8 es para indicar que incluye 'ñ' o cualquier palabra con acento

                //REALIZAMOS LA LLAMADA AL SERVICIO ENVIANDO EL OBJETO CONTENT
                await client.PostAsync(request, content);

            }
        }

        //METODO PUT CON OBJETO
        public async Task UpdateDepartamentoAsync(int id, string nombre, string localidad)
        {
            using(HttpClient client = new HttpClient())
            {
                string request = "/api/departamentos";
                client.BaseAddress = new Uri(this.UrlApi);
                client.DefaultRequestHeaders.Clear();
                Departamento departamento = new Departamento();
                departamento.IdDepartamento = id;
                departamento.Nombre = nombre;
                departamento.Localidad = localidad;

                string json = JsonConvert.SerializeObject(departamento);
                StringContent content = new StringContent(json,Encoding.UTF8, "application/json");
                await client.PutAsync(request, content);
            }
        }





    }
}
