using AleedShooping.Data;
using AleedShooping.Data.Entities;
using AleedShooping.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Vereyon.Web;
using static AleedShooping.Helpers.ModalHelper;

namespace AleedShooping.Controllers
{
    [Authorize(Roles="Admin")]
    public class CategoriesController : Controller
    {
        private readonly DataContext _context;
        private readonly IFlashMessage _flashMessage;

        public CategoriesController(DataContext context, IFlashMessage flashMessage)
        {
            _context = context;
            _flashMessage = flashMessage;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Categories
                //.Include(c => c.ProductCategories)
                .ToListAsync());
        }

        [NoDirectAccess]
        public async Task<IActionResult> Delete(int? id)
        {
            Category category = await _context.Categories.FirstOrDefaultAsync(c => c.Id == id);
            try
            {
                _context.Categories.Remove(category);
                await _context.SaveChangesAsync();
                _flashMessage.Info("Registro borrado Exitosamente.");
            }
            catch
            {
                _flashMessage.Danger("No se puede borrar la categoría porque tiene registros relacionados.");
            }

            return RedirectToAction(nameof(Index));
        }

        [NoDirectAccess]
        public async Task<IActionResult> Create()
        {

            return View();

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Category category)
        {
            if (ModelState.IsValid)
            {

                try
                {
                    _context.Add(category);
                    await _context.SaveChangesAsync();
                    _flashMessage.Info("Registro creado Exitosamente.");


                }
                catch (DbUpdateException dbUpdateException)
                {
                    if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                    {
                        _flashMessage.Danger("Ya existe una categoria con el mismo nombre.");
                        
                    }
                    else
                    {
                        _flashMessage.Danger(string.Empty, dbUpdateException.InnerException.Message);
                        
                    }

                    return Json(new { isValid = false, html = ModalHelper.RenderRazorViewToString(this, "Create", category) });
                }
                catch (Exception exception)
                {
                    ModelState.AddModelError(string.Empty, exception.Message);
                    return Json(new { isValid = false, html = ModalHelper.RenderRazorViewToString(this, "Create", category) });

                }
                return Json(new { isValid = true, html = ModalHelper.RenderRazorViewToString(this, "_ViewAll",category) });
            }


            return Json(new { isValid = false, html = ModalHelper.RenderRazorViewToString(this, "Create", category) });
        }

        [NoDirectAccess]
        public async Task<IActionResult> Edit(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Category category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }




    

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Category category)
        {
            if (id != category.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {

                    _context.Update(category);
                    await _context.SaveChangesAsync();
                    _flashMessage.Info("Registro actualizado Exitosamente.");
                }
                catch (DbUpdateException dbUpdateException)
                {
                    if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                    {
                        _flashMessage.Danger( "Ya existe una categoria con el mismo nombre.");
                    }
                    else
                    {
                        _flashMessage.Danger(string.Empty, dbUpdateException.InnerException.Message);
                    }
                    return Json(new { isValid = false, html = ModalHelper.RenderRazorViewToString(this, "Edit", category) });


                }
                catch (Exception exception)
                {
                    ModelState.AddModelError(string.Empty, exception.Message);
                    return Json(new { isValid = false, html = ModalHelper.RenderRazorViewToString(this, "Edit", category) });


                }

                return Json(new { isValid = true, html = ModalHelper.RenderRazorViewToString(this, "_ViewAll", category) });

            }

            return Json(new { isValid = false, html = ModalHelper.RenderRazorViewToString(this, "Edit", category) });
        }
    }
}
