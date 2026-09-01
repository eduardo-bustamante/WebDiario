using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebDiario.Models;

namespace WebDiario.Controllers;

public class ContaController : Controller
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signInManager;

    public ContaController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    // ==========================================
    // REGISTRO
    // ==========================================
    [HttpGet]
    [AllowAnonymous]
    public IActionResult Registrar()
    {
        if (User.Identity?.IsAuthenticated == true)
            return RedirectToAction("Index", "Diario");

        return View();
    }

    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Registrar(RegistroViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var usuario = new IdentityUser
        {
            UserName = model.Usuario.Trim()
            // Email = null (ou nem precisa preencher se não for usar)
        };
        var resultado = await _userManager.CreateAsync(usuario, model.Senha);

        if (resultado.Succeeded)
        {
            // Efetua o login logo após o cadastro
            await _signInManager.SignInAsync(usuario, isPersistent: false);
            return RedirectToAction("Index", "Diario");
        }

        foreach (var erro in resultado.Errors)
        {
            ModelState.AddModelError(string.Empty, erro.Description);
        }

        return View(model);
    }

    // ==========================================
    // LOGIN
    // ==========================================
    [HttpGet]
    [AllowAnonymous]
    public IActionResult Login(string? returnUrl = null)
    {
        if (User.Identity?.IsAuthenticated == true)
            return RedirectToAction("Index", "Diario");

        return View(new LoginViewModel { ReturnUrl = returnUrl });
    }

    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var resultado = await _signInManager.PasswordSignInAsync(
            model.Usuario,
            model.Senha,
            model.LembrarMe,
            lockoutOnFailure: false);

        if (resultado.Succeeded)
        {
            if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
                return Redirect(model.ReturnUrl);

            return RedirectToAction("Index", "Diario");
        }

        ModelState.AddModelError(string.Empty, "E-mail ou senha inválidos.");
        return View(model);
    }

    // ==========================================
    // LOGOUT
    // ==========================================
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Sair()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Index", "Home");
    }
}