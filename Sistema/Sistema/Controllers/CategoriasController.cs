﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Sistema.Models;

namespace Sistema.Controllers
{
    public class CategoriasController : Controller
    {
        private readonly SistemaContext _context;

        public CategoriasController(SistemaContext context)
        {
            _context = context;
        }

        // GET: Categorias
        public async Task<IActionResult> Index(string ordenarFilas, string currentFilter, string cadenaBusqueda, int? page)
        {
            ViewData["NombreParametroOrdenamiento"] = String.IsNullOrEmpty(ordenarFilas) ? "nombreDescendiente" : "";
            ViewData["DescripcionParametroOrdenamiento"] = ordenarFilas == "descripcionAscendente" ? "descripcionDescendente" : "descripcionAscendente";
            if(cadenaBusqueda != null)
            {
                page = 1;
            }
            else
            {
                cadenaBusqueda = currentFilter;
            }
            ViewData["currentFilter"] = cadenaBusqueda;
            ViewData["CurrentSort"] = ordenarFilas;

            var categorias = from c in _context.Categoria
                             select c;

            if (!String.IsNullOrEmpty(cadenaBusqueda))
            {
                categorias = categorias.Where(c => c.Nombre.Contains(cadenaBusqueda) || c.Descripcion.Contains(cadenaBusqueda));
            }

            switch (ordenarFilas)
            {
                case ("nombreDescendiente"):
                    categorias = categorias.OrderByDescending(c => c.Nombre);
                    break;
                case ("descripcionDescendente"):
                    categorias = categorias.OrderByDescending(c => c.Descripcion);
                    break;
                case ("descripcionAscendente"):
                    categorias = categorias.OrderBy(c => c.Descripcion);
                    break;
                default:
                    categorias = categorias.OrderBy(c => c.Nombre);
                    break;
            }
            //return View(await categorias.AsNoTracking().ToListAsync());
            //return View(await _context.Categoria.ToListAsync());

            int pageSize = 3;
            return View(await Paginacion<Categoria>.CreateAsync(categorias.AsNoTracking(), page ?? 1, pageSize));
        }

        // GET: Categorias/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var categoria = await _context.Categoria
                .SingleOrDefaultAsync(m => m.CategoriaId == id);
            if (categoria == null)
            {
                return NotFound();
            }

            return View(categoria);
        }

        // GET: Categorias/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Categorias/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CategoriaId,Nombre,Descripcion,Estado")] Categoria categoria)
        {
            if (ModelState.IsValid)
            {
                _context.Add(categoria);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(categoria);
        }

        // GET: Categorias/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var categoria = await _context.Categoria.SingleOrDefaultAsync(m => m.CategoriaId == id);
            if (categoria == null)
            {
                return NotFound();
            }
            return View(categoria);
        }

        // POST: Categorias/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CategoriaId,Nombre,Descripcion,Estado")] Categoria categoria)
        {
            if (id != categoria.CategoriaId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(categoria);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoriaExists(categoria.CategoriaId))
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
            return View(categoria);
        }

        // GET: Categorias/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var categoria = await _context.Categoria
                .SingleOrDefaultAsync(m => m.CategoriaId == id);
            if (categoria == null)
            {
                return NotFound();
            }

            return View(categoria);
        }

        // POST: Categorias/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var categoria = await _context.Categoria.SingleOrDefaultAsync(m => m.CategoriaId == id);
            _context.Categoria.Remove(categoria);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CategoriaExists(int id)
        {
            return _context.Categoria.Any(e => e.CategoriaId == id);
        }
    }
}
