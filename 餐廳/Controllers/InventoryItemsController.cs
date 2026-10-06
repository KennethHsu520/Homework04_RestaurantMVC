using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using 餐廳.Data;
using 餐廳.Models;

namespace 餐廳.Controllers
{
    public class InventoryItemsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public InventoryItemsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: InventoryItems
        public async Task<IActionResult> Index(string selectedStore, string selectedCategory)
        {
            var stores = await _context.InventoryItems
                .Select(i => i.StoreName)
                .Distinct()
                .ToListAsync();

            var categories = await _context.InventoryItems
                .Select(i => i.Category)
                .Distinct()
                .ToListAsync();

            ViewBag.Stores = stores;
            ViewBag.Categories = categories;
            ViewBag.SelectedStore = selectedStore;
            ViewBag.SelectedCategory = selectedCategory;

            var itemsQuery = _context.InventoryItems.AsQueryable();

            if (!string.IsNullOrEmpty(selectedStore))
            {
                itemsQuery = itemsQuery.Where(i => i.StoreName == selectedStore);
            }

            if (!string.IsNullOrEmpty(selectedCategory) && selectedCategory != "全部")
            {
                itemsQuery = itemsQuery.Where(i => i.Category == selectedCategory);
            }

            var items = await itemsQuery.ToListAsync();
            var lowStockItems = items.Where(i => i.CurrentQuantity < i.MinRequiredQuantity).ToList();
            ViewBag.LowStockItems = lowStockItems;

            return View(items);
        }

        // POST: Quick Update Quantity
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateQuantity(int id, int newQuantity)
        {
            var item = await _context.InventoryItems.FindAsync(id);
            if (item != null)
            {
                item.CurrentQuantity = newQuantity;
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        // POST: Quick Restock
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Restock(int id, int orderQuantity)
        {
            var item = await _context.InventoryItems.FindAsync(id);
            if (item != null && orderQuantity > 0)
            {
                item.CurrentQuantity += orderQuantity;

                var record = new SupplyRecord
                {
                    SupplyDate = DateTime.Now,
                    StoreName = item.StoreName,
                    ItemName = item.ItemName,
                    Supplier = item.Supplier,
                    Quantity = orderQuantity,
                    TotalPrice = orderQuantity * item.UnitPrice
                };

                _context.SupplyRecords.Add(record);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: InventoryItems/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: InventoryItems/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(InventoryItem inventoryItem)
        {
            if (ModelState.IsValid)
            {
                _context.Add(inventoryItem);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(inventoryItem);
        }

        // GET: InventoryItems/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var item = await _context.InventoryItems.FindAsync(id);
            if (item == null) return NotFound();

            return View(item);
        }

        // POST: InventoryItems/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, InventoryItem inventoryItem)
        {
            if (id != inventoryItem.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(inventoryItem);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InventoryItemExists(inventoryItem.Id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(inventoryItem);
        }

        // GET: InventoryItems/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var item = await _context.InventoryItems
                .FirstOrDefaultAsync(m => m.Id == id);
            if (item == null) return NotFound();

            return View(item);
        }

        // POST: InventoryItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var item = await _context.InventoryItems.FindAsync(id);
            if (item != null)
            {
                _context.InventoryItems.Remove(item);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool InventoryItemExists(int id)
        {
            return _context.InventoryItems.Any(e => e.Id == id);
        }
    }
}