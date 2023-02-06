using Microsoft.AspNetCore.Mvc;
using MvcCoreApiCrudDepartamentos2023.Models;
using MvcCoreApiCrudDepartamentos2023.Services;

namespace MvcCoreApiCrudDepartamentos2023.Controllers
{
    public class DepartamentosController : Controller
    {
        private ServiceDepartamentos service;

        public DepartamentosController(ServiceDepartamentos service)
        {
            this.service = service;
        }

        public async Task<IActionResult> Index()
        {
            List<Departamento> departamentos =
                await this.service.GetDepartamentosAsync();
            return View(departamentos);
        }

        public async Task<IActionResult> Details(int iddepartamento)
        {
            Departamento departamento =
                await this.service.FindDepartamentoAsync(iddepartamento);
            return View(departamento);
        }

        public IActionResult BuscarLocalidades()
        {
            return View();
        }

        //La anotación [HttpPost] indica que este método solo se activará cuando se realice una
        //solicitud HTTP POST al servidor
        [HttpPost]
        public async Task<IActionResult> BuscarLocalidades(string localidad)
        {
            List<Departamento> departamentos =
                await this.service.GetDepartamentosLocalidadAsync(localidad);
            return View(departamentos);
        }

        public IActionResult CreateDepartamento()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateDepartamento(Departamento departamento)
        {
            await this.service.InsertDepartamentoAsync(departamento.IdDepartamento,
                departamento.Nombre, departamento.Localidad);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> UpdateDepartamento(int iddepartamento)
        {
            Departamento departamento = await this.service.FindDepartamentoAsync(iddepartamento);
            return View(departamento);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateDepartamento(Departamento departamento)
        {
            await this.service.UpdateDepartamentoAsync(departamento.IdDepartamento,
                departamento.Nombre, departamento.Localidad);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> DeleteDepartamento(int iddepartamento)
        {
            await this.service.DeleteDepartamentoAsync(iddepartamento);
            return RedirectToAction("Index");
        }

    }
}
