using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Taller2Net.Data;
using Taller2Net.Models;
using Taller2Net.Services;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Taller2Net.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ServicioInventario _servicioInventario;
        private readonly ILogger<ProductsController> _logger;

        public ProductsController(ApplicationDbContext context, ServicioInventario servicioInventario, ILogger<ProductsController> logger)
        {
            _context = context;
            _servicioInventario = servicioInventario;
            _logger = logger;
        }

        // GET: Products
        public async Task<IActionResult> Index()
        {
            await _servicioInventario.ReordenarProductosAsync();
            var productos = await _context.Productos.ToListAsync();
            return View(productos);
        }

        [HttpPost]
    public async Task<IActionResult> MaximizeStock(int id)
    {
        var existingProduct = await _context.Productos.FindAsync(id);
        if (existingProduct == null)
        {
            return NotFound();
    }

       existingProduct.Stock = 100;
        _context.Update(existingProduct);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
}


        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Productos.FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Products/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Price,Description,Stock")] Product product)
        {
            if (ModelState.IsValid)
            {
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Productos.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        // POST: Products/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Price,Description,Stock")] Product product)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var existingProduct = await _context.Productos.FindAsync(id);
                    if (existingProduct == null)
                    {
                        return NotFound();
                    }

                    existingProduct.Name = product.Name;
                    existingProduct.Price = product.Price;
                    existingProduct.Description = product.Description;
                    existingProduct.Stock = product.Stock;

                    _context.Update(existingProduct);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Productos.FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Productos.FindAsync(id);
            _context.Productos.Remove(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // Método GET para mostrar el formulario de actualización del stock
        public async Task<IActionResult> UpdateStock(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Productos.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

[HttpPost]
[ValidateAntiForgeryToken]
public async Task<IActionResult> UpdateStock(int id, int stock)
{
    _logger.LogInformation("Entering UpdateStock method");

    var existingProduct = await _context.Productos.FindAsync(id);
    if (existingProduct == null)
    {
        _logger.LogWarning("Product not found: {Id}", id);
        return NotFound();
    }

    existingProduct.Stock = stock;

    if (!ModelState.IsValid)
    {
        _logger.LogWarning("Model state is invalid");
        foreach (var error in ModelState)
        {
            _logger.LogWarning("Key: {Key}, Error: {Error}", error.Key, string.Join(", ", error.Value.Errors.Select(e => e.ErrorMessage)));
        }
        return View(existingProduct);
    }

    try
    {
        _context.Update(existingProduct);
        _logger.LogInformation("Updating product ID: {Id}, New Stock: {Stock}", existingProduct.Id, existingProduct.Stock);

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
    catch (DbUpdateConcurrencyException ex)
    {
        _logger.LogError(ex, "Concurrency error updating product ID: {Id}", id);
        if (!ProductExists(existingProduct.Id))
        {
            return NotFound();
        }
        else
        {
            throw;
        }
    }
}



        private bool ProductExists(int id)
        {
            return _context.Productos.Any(e => e.Id == id);
        }
    }
}
