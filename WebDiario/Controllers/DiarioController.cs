using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebDiario.Data;
using WebDiario.Models;

namespace WebDiario.Controllers;

[Authorize] // Bloqueia qualquer acesso anônimo ao diário
public class DiarioController : Controller
{
    private readonly AppDbContext _context;

    public DiarioController(AppDbContext context)
    {
        _context = context;
    }

    // Helper privado para pegar o Id do usuário autenticado
    private string ObterUsuarioId() => User.FindFirstValue(ClaimTypes.NameIdentifier)!;

    // GET: Diario
    public async Task<IActionResult> Index(string? termoBusca, string? filtroHumor)
    {
        var usuarioId = ObterUsuarioId();

        // IMPORTANTE: Só busca as anotações pertencentes ao usuário conectado
        var query = _context.EntradasDiario
            .Where(e => e.UsuarioId == usuarioId);

        if (!string.IsNullOrWhiteSpace(termoBusca))
        {
            var termo = termoBusca.Trim();
            query = query.Where(e => e.Titulo.Contains(termo) || e.Conteudo.Contains(termo));
        }

        if (!string.IsNullOrWhiteSpace(filtroHumor))
        {
            query = query.Where(e => e.Humor == filtroHumor);
        }

        var entradas = await query.OrderByDescending(e => e.DataCriacao).ToListAsync();

        ViewData["TermoBuscaAtual"] = termoBusca;
        ViewData["HumorAtual"] = filtroHumor;

        return View(entradas);
    }

    // GET: Diario/Criar
    public IActionResult Criar() => View(new EntradaDiario());

    // POST: Diario/Criar
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Criar(EntradaDiario entrada)
    {
        // Vincula a entrada ao usuário logado antes de salvar
        entrada.UsuarioId = ObterUsuarioId();
        ModelState.Remove(nameof(entrada.UsuarioId));

        if (ModelState.IsValid)
        {
            _context.Add(entrada);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(entrada);
    }

    // GET: Diario/Detalhes/5
    public async Task<IActionResult> Detalhes(int? id)
    {
        if (id == null) return NotFound();

        var usuarioId = ObterUsuarioId();
        var entrada = await _context.EntradasDiario
            .AsNoTracking()
            .FirstOrDefaultAsync(m => m.Id == id && m.UsuarioId == usuarioId);

        if (entrada == null) return NotFound();

        return View(entrada);
    }

    // GET: Diario/Editar/5
    public async Task<IActionResult> Editar(int? id)
    {
        if (id == null) return NotFound();

        var usuarioId = ObterUsuarioId();
        var entrada = await _context.EntradasDiario
            .FirstOrDefaultAsync(e => e.Id == id && e.UsuarioId == usuarioId);

        if (entrada == null) return NotFound();

        return View(entrada);
    }

    // POST: Diario/Editar/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Editar(int id, EntradaDiario entrada)
    {
        if (id != entrada.Id) return NotFound();

        var usuarioId = ObterUsuarioId();

        // Impede que um usuário altere a titularidade do registro
        var registroExistente = await _context.EntradasDiario
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.Id == id && e.UsuarioId == usuarioId);

        if (registroExistente == null) return Unauthorized();

        entrada.UsuarioId = usuarioId;
        ModelState.Remove(nameof(entrada.UsuarioId));

        if (ModelState.IsValid)
        {
            _context.Update(entrada);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(entrada);
    }

    // GET: Diario/Excluir/5
    public async Task<IActionResult> Excluir(int? id)
    {
        if (id == null) return NotFound();

        var usuarioId = ObterUsuarioId();
        var entrada = await _context.EntradasDiario
            .AsNoTracking()
            .FirstOrDefaultAsync(m => m.Id == id && m.UsuarioId == usuarioId);

        if (entrada == null) return NotFound();

        return View(entrada);
    }

    // POST: Diario/Excluir/5
    [HttpPost, ActionName("Excluir")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ConfirmarExclusao(int id)
    {
        var usuarioId = ObterUsuarioId();
        var entrada = await _context.EntradasDiario
            .FirstOrDefaultAsync(e => e.Id == id && e.UsuarioId == usuarioId);

        if (entrada != null)
        {
            _context.EntradasDiario.Remove(entrada);
            await _context.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Index));
    }
}