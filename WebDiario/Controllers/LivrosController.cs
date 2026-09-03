using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebDiario.Data;
using WebDiario.Models;

namespace WebDiario.Controllers;

[Authorize]
public class LivrosController : Controller
{
    private readonly AppDbContext _context;
    private readonly IWebHostEnvironment _env;

    public LivrosController(AppDbContext context, IWebHostEnvironment env)
    {
        _context = context;
        _env = env;
    }

    private string ObterUsuarioIdLogado()
    {
        return User.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? throw new InvalidOperationException("Usuário não autenticado.");
    }

    // GET: /Livros
    public async Task<IActionResult> Index(string? busca, string? status)
    {
        var usuarioId = ObterUsuarioIdLogado();
        var query = _context.Livros.Where(l => l.UsuarioId == usuarioId);

        if (!string.IsNullOrWhiteSpace(busca))
        {
            var termo = busca.Trim();
            query = query.Where(l => l.Titulo.Contains(termo) || (l.Autor != null && l.Autor.Contains(termo)));
        }

        if (!string.IsNullOrWhiteSpace(status))
        {
            query = query.Where(l => l.Status == status);
        }

        ViewData["BuscaAtual"] = busca;
        ViewData["StatusAtual"] = status;

        var livros = await query.OrderByDescending(l => l.DataCadastro).ToListAsync();
        return View(livros);
    }

    // GET: /Livros/Detalhes/5
    public async Task<IActionResult> Detalhes(int? id)
    {
        if (id == null) return NotFound();

        var usuarioId = ObterUsuarioIdLogado();
        var livro = await _context.Livros
            .FirstOrDefaultAsync(l => l.Id == id && l.UsuarioId == usuarioId);

        if (livro == null) return NotFound();

        return View(livro);
    }

    // GET: /Livros/Criar
    public IActionResult Criar()
    {
        return View(new Livro());
    }

    // POST: /Livros/Criar
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Criar(Livro livro, IFormFile? arquivoCapa, string? urlCapa)
    {
        livro.UsuarioId = ObterUsuarioIdLogado();

        // Remove a validação do UsuarioId para evitar falso-positivo de ModelState inválido
        ModelState.Remove(nameof(Livro.UsuarioId));

        if (ModelState.IsValid)
        {
            if (arquivoCapa != null && arquivoCapa.Length > 0)
            {
                livro.FotoCapa = await SalvarArquivoCapa(arquivoCapa);
            }
            else if (!string.IsNullOrWhiteSpace(urlCapa))
            {
                livro.FotoCapa = urlCapa.Trim();
            }

            AjustarStatusPorPaginas(livro);

            _context.Add(livro);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        return View(livro);
    }

    // GET: /Livros/Editar/5
    public async Task<IActionResult> Editar(int? id)
    {
        if (id == null) return NotFound();

        var usuarioId = ObterUsuarioIdLogado();
        var livro = await _context.Livros
            .FirstOrDefaultAsync(l => l.Id == id && l.UsuarioId == usuarioId);

        if (livro == null) return NotFound();

        return View(livro);
    }

    // POST: /Livros/Editar/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Editar(int id, Livro livro, IFormFile? arquivoCapa, string? urlCapa, bool removerCapa = false)
    {
        if (id != livro.Id) return NotFound();

        var usuarioId = ObterUsuarioIdLogado();
        var livroOriginal = await _context.Livros.AsNoTracking()
            .FirstOrDefaultAsync(l => l.Id == id && l.UsuarioId == usuarioId);

        if (livroOriginal == null) return Unauthorized();

        livro.UsuarioId = usuarioId;
        ModelState.Remove(nameof(Livro.UsuarioId));

        if (ModelState.IsValid)
        {
            if (removerCapa)
            {
                RemoverArquivoFisico(livroOriginal.FotoCapa);
                livro.FotoCapa = null;
            }
            else if (arquivoCapa != null && arquivoCapa.Length > 0)
            {
                RemoverArquivoFisico(livroOriginal.FotoCapa);
                livro.FotoCapa = await SalvarArquivoCapa(arquivoCapa);
            }
            else if (!string.IsNullOrWhiteSpace(urlCapa))
            {
                livro.FotoCapa = urlCapa.Trim();
            }
            else
            {
                // Mantém a imagem anterior caso não tenha enviado nova
                livro.FotoCapa = livroOriginal.FotoCapa;
            }

            AjustarStatusPorPaginas(livro);

            _context.Update(livro);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        return View(livro);
    }

    // POST: /Livros/Excluir/5
    [HttpPost, ActionName("Excluir")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ConfirmarExclusao(int id)
    {
        var usuarioId = ObterUsuarioIdLogado();
        var livro = await _context.Livros
            .FirstOrDefaultAsync(l => l.Id == id && l.UsuarioId == usuarioId);

        if (livro != null)
        {
            RemoverArquivoFisico(livro.FotoCapa);
            _context.Livros.Remove(livro);
            await _context.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Index));
    }

    // ==========================================
    // MÉTODOS AUXILIARES DE IMAGEM E STATUS
    // ==========================================

    private async Task<string> SalvarArquivoCapa(IFormFile arquivo)
    {
        // Garante o caminho físico correto mesmo publicado via .exe
        var webRoot = !string.IsNullOrEmpty(_env.WebRootPath)
            ? _env.WebRootPath
            : Path.Combine(AppContext.BaseDirectory, "wwwroot");

        var pastaCapas = Path.Combine(webRoot, "capas");

        if (!Directory.Exists(pastaCapas))
        {
            Directory.CreateDirectory(pastaCapas);
        }

        var extensao = Path.GetExtension(arquivo.FileName).ToLowerInvariant();
        var extensoesPermitidas = new[] { ".png", ".jpg", ".jpeg", ".webp" };

        if (!extensoesPermitidas.Contains(extensao))
        {
            throw new InvalidOperationException("Formato de imagem inválido. Use PNG, JPG ou WEBP.");
        }

        var nomeArquivo = $"{Guid.NewGuid()}{extensao}";
        var caminhoFisico = Path.Combine(pastaCapas, nomeArquivo);

        using (var stream = new FileStream(caminhoFisico, FileMode.Create))
        {
            await arquivo.CopyToAsync(stream);
        }

        // Retorna a URL relativa web padrão para renderizar na tag <img>
        return $"/capas/{nomeArquivo}";
    }

    private void RemoverArquivoFisico(string? caminhoRelativo)
    {
        if (string.IsNullOrEmpty(caminhoRelativo) || !caminhoRelativo.StartsWith("/capas/")) return;

        var webRoot = !string.IsNullOrEmpty(_env.WebRootPath)
            ? _env.WebRootPath
            : Path.Combine(AppContext.BaseDirectory, "wwwroot");

        var nomeArquivo = Path.GetFileName(caminhoRelativo);
        var caminhoFisico = Path.Combine(webRoot, "capas", nomeArquivo);

        if (System.IO.File.Exists(caminhoFisico))
        {
            try
            {
                System.IO.File.Delete(caminhoFisico);
            }
            catch
            {
                // Silencia caso o arquivo esteja temporariamente bloqueado por outro processo
            }
        }
    }

    private static void AjustarStatusPorPaginas(Livro livro)
    {
        if (livro.TotalPaginas > 0 && livro.PaginasLidas >= livro.TotalPaginas)
        {
            livro.Status = "Lido";
        }
        else if (livro.PaginasLidas > 0 && (livro.Status == "Quero Ler" || livro.PaginasLidas < livro.TotalPaginas))
        {
            livro.Status = "Lendo";
        }
    }
}