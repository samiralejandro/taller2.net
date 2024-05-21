using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Taller2Net.Data;
using Taller2Net.Models;
using Taller2Net.Services;
using System.Linq;
using System.Threading.Tasks;

namespace Taller2Net.Controllers;

public class ProductsController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly ServicioInventario _servicioInventario;


    public ProductsController(ApplicationDbContext context, ServicioInventario servicioInventario)
    {
        _context = context;
        _servicioInventario = servicioInventario;
    }

      // GET: Products
        public async Task<IActionResult> Index()
        {
            // Llamar al servicio de reabastecimiento
            await _servicioInventario.ReordenarProductosAsync();

            // Obtener la lista de productos después del reabastecimiento
            var productos = await _context.Productos.ToListAsync();
            return View(productos);
        }

    // GET: Products
 

    // GET: Products/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var product = await _context.Productos
            .FirstOrDefaultAsync(m => m.Id == id);
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
    public async Task<IActionResult> Create([Bind("Id,Name,Quantity,Price,ReorderLevel")] Product product)
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
    public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Quantity,Price,ReorderLevel")] Product product)
    {
        if (id != product.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(product);
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

        var product = await _context.Productos
            .FirstOrDefaultAsync(m => m.Id == id);
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

    private bool ProductExists(int id)
    {
        return _context.Productos.Any(e => e.Id == id);
    }
}
