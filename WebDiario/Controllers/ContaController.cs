using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace WebDiario.Controllers;

public class ContaController : Controller
{
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly UserManager<IdentityUser> _userManager;

    public ContaController(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager)
    {
        _signInManager = signInManager;
        _userManager = userManager;
    }

    // GET: /Conta/Login
    [HttpGet]
    [AllowAnonymous]
    public IActionResult Login(string? returnUrl = null)
    {
        ViewData["ReturnUrl"] = returnUrl;
        return View();
    }

    // POST: /Conta/Login
    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(string usuario, string password, bool rememberMe, string? returnUrl = null)
    {
        ViewData["ReturnUrl"] = returnUrl;

        if (string.IsNullOrWhiteSpace(usuario) || string.IsNullOrWhiteSpace(password))
        {
            ModelState.AddModelError(string.Empty, "Preencha o usuário e a senha.");
            return View();
        }

        var result = await _signInManager.PasswordSignInAsync(usuario.Trim(), password, rememberMe, lockoutOnFailure: false);
        if (result.Succeeded)
        {
            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        ModelState.AddModelError(string.Empty, "Usuário ou senha incorretos.");
        return View();
    }

    // GET: /Conta/Registrar
    [HttpGet]
    [AllowAnonymous]
    public IActionResult Registrar()
    {
        return View();
    }

    // POST: /Conta/Registrar
    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Registrar(string usuario, string password, string confirmPassword)
    {
        if (string.IsNullOrWhiteSpace(usuario))
        {
            ModelState.AddModelError(string.Empty, "Informe um nome de usuário.");
            return View();
        }

        if (password != confirmPassword)
        {
            ModelState.AddModelError(string.Empty, "As senhas informadas não conferem.");
            return View();
        }

        var user = new IdentityUser { UserName = usuario.Trim() };
        var result = await _userManager.CreateAsync(user, password);

        if (result.Succeeded)
        {
            await _signInManager.SignInAsync(user, isPersistent: false);
            return RedirectToAction("Index", "Home");
        }

        foreach (var error in result.Errors)
        {
            ModelState.AddModelError(string.Empty, error.Description);
        }

        return View();
    }

    // POST / GET: /Conta/Sair
    [HttpPost, HttpGet]
    [Authorize]
    public async Task<IActionResult> Sair()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Index", "Home");
    }
}