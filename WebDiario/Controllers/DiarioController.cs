using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebDiario.Data;
using WebDiario.Models;

namespace WebDiario.Controllers;

[Authorize]
public class DiarioController : Controller
{
    private readonly AppDbContext _context;

    public DiarioController(AppDbContext context)
    {
        _context = context;
    }

    private string ObterUsuarioIdLogado()
    {
        return User.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? throw new InvalidOperationException("Usuário não autenticado.");
    }

    // GET: /Diario
    public async Task<IActionResult> Index(string? busca, int? humor)
    {
        var usuarioId = ObterUsuarioIdLogado();
        var query = _context.Diarios.Where(d => d.UsuarioId == usuarioId);

        if (!string.IsNullOrWhiteSpace(busca))
        {
            var termo = busca.Trim();
            query = query.Where(d => d.Titulo.Contains(termo) || d.Conteudo.Contains(termo));
        }

        if (humor.HasValue && humor.Value >= 1 && humor.Value <= 5)
        {
            query = query.Where(d => d.NivelHumor == humor.Value);
        }

        ViewData["BuscaAtual"] = busca;
        ViewData["HumorAtual"] = humor;

        var entradas = await query.OrderByDescending(d => d.DataRegistro)
                                  .ThenByDescending(d => d.DataCriacao)
                                  .ToListAsync();
        return View(entradas);
    }

    // GET: /Diario/Detalhes/5
    public async Task<IActionResult> Detalhes(int? id)
    {
        if (id == null) return NotFound();

        var usuarioId = ObterUsuarioIdLogado();
        var entrada = await _context.Diarios
            .FirstOrDefaultAsync(d => d.Id == id && d.UsuarioId == usuarioId);

        if (entrada == null) return NotFound();

        return View(entrada);
    }

    // GET: /Diario/Criar
    public IActionResult Criar()
    {
        return View(new Diario());
    }

    // POST: /Diario/Criar
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Criar(Diario diario)
    {
        if (ModelState.IsValid)
        {
            diario.UsuarioId = ObterUsuarioIdLogado();
            diario.DataCriacao = DateTime.Now;

            _context.Add(diario);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        return View(diario);
    }

    // GET: /Diario/Editar/5
    public async Task<IActionResult> Editar(int? id)
    {
        if (id == null) return NotFound();

        var usuarioId = ObterUsuarioIdLogado();
        var entrada = await _context.Diarios
            .FirstOrDefaultAsync(d => d.Id == id && d.UsuarioId == usuarioId);

        if (entrada == null) return NotFound();

        return View(entrada);
    }

    // POST: /Diario/Editar/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Editar(int id, Diario diario)
    {
        if (id != diario.Id) return NotFound();

        var usuarioId = ObterUsuarioIdLogado();
        var entradaOriginal = await _context.Diarios.AsNoTracking()
            .FirstOrDefaultAsync(d => d.Id == id && d.UsuarioId == usuarioId);

        if (entradaOriginal == null) return Unauthorized();

        if (ModelState.IsValid)
        {
            diario.UsuarioId = usuarioId;
            diario.DataCriacao = entradaOriginal.DataCriacao;

            _context.Update(diario);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        return View(diario);
    }

    // POST: /Diario/Excluir/5
    [HttpPost, ActionName("Excluir")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ConfirmarExclusao(int id)
    {
        var usuarioId = ObterUsuarioIdLogado();
        var entrada = await _context.Diarios
            .FirstOrDefaultAsync(d => d.Id == id && d.UsuarioId == usuarioId);

        if (entrada != null)
        {
            _context.Diarios.Remove(entrada);
            await _context.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Index));
    }
}