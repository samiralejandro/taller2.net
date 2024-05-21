using Taller2Net.Data;
using Taller2Net.Models;
using Microsoft.EntityFrameworkCore;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Taller2Net.Services
{
    public class ServicioInventario
    {
        private readonly ApplicationDbContext _contexto;
        private readonly HttpClient _httpClient;

        public ServicioInventario(ApplicationDbContext contexto, HttpClient httpClient)
        {
            _contexto = contexto;
            _httpClient = httpClient;
        }

        public async Task ReordenarProductosAsync()
        {
            var productos = await _contexto.Productos.ToListAsync();
            foreach (var producto in productos)
            {
                if (producto.Stock < 10)
                {
                    var reabastecimientoExitoso = await RealizarReabastecimientoAsync(producto);
                    if (reabastecimientoExitoso)
                    {
                        producto.Stock = 50; // Actualizar el stock a 50 unidades
                        _contexto.Productos.Update(producto);
                        await _contexto.SaveChangesAsync();
                    }
                }
            }
        }

        private async Task<bool> RealizarReabastecimientoAsync(Product producto)
        {
            var contenido = new StringContent(JsonSerializer.Serialize(new { ProductoId = producto.Id, Stock = 40 }), Encoding.UTF8, "application/json");
            var respuesta = await _httpClient.PostAsync("http://localhost:5046/replenishment", contenido);

            if (respuesta.IsSuccessStatusCode)
            {
                var mensaje = JsonSerializer.Deserialize<ReabastecimientoRespuesta>(await respuesta.Content.ReadAsStringAsync());
                if (mensaje != null && mensaje.message == "Listo mi broco se hizo el restablecimiento")
                {
                    
                    return true;
                }
            }

            return false;
        }
    }

    public class ReabastecimientoRespuesta
    {
        public string message { get; set; }
    }
}
