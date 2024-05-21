using Microsoft.AspNetCore.Mvc;
using Taller2Net.Services;
using System.Threading.Tasks;

namespace Taller2Net.Controllers;

[Route("api/[controller]")]
[ApiController]
public class InventarioController : ControllerBase
{
    private readonly ServicioInventario _servicioInventario;

    public InventarioController(ServicioInventario servicioInventario)
    {
        _servicioInventario = servicioInventario;
    }

    [HttpPost("reordenar")]
    public async Task<IActionResult> ReordenarProductos()
    {
        await _servicioInventario.ReordenarProductosAsync();
        return Ok(new { mensaje = "Proceso de reorden iniciado" });
    }
}
