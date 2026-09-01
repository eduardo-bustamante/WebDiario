# 📓 WebDiario

> Um aplicativo web minimalista, seguro e intuitivo para registro de pensamentos, memórias e acompanhamento de humor.

Desenvolvido em **C#** utilizando a plataforma **ASP.NET Core MVC** e **Entity Framework Core**, o projeto foi estruturado com foco em simplicidade de uso, privacidade dos dados e design limpo.

---

## ✨ Funcionalidades

- **Autenticação & Privacidade:**
  - Sistema de cadastro e login com senhas criptografadas via **ASP.NET Core Identity**.
  - Isolamento total de dados: cada usuário tem acesso exclusivo aos seus próprios registros.
- **Gestão de Registros (CRUD Completo):**
  - Criação, leitura, edição e exclusão de anotações.
  - Classificação por estado de espírito/humor em cada entrada.
- **Busca e Filtros Inteligentes:**
  - Pesquisa dinâmica por texto (busca no título e no corpo da anotação).
  - Filtro combinatório por sentimento/humor.
- **Interface Moderna & Responsiva:**
  - Layout limpo construído com **Bootstrap 5**.
  - Cards interativos, visualização imersiva para leitura e navegação otimizada.

---

## 🛠️ Tecnologias Utilizadas

- **Linguagem:** C# (.NET 10)
- **Framework Web:** ASP.NET Core MVC
- **ORM:** Entity Framework Core
- **Banco de Dados:** SQLite
- **Autenticação & Segurança:** ASP.NET Core Identity
- **Front-end:** Razor Views (`.cshtml`), HTML5, CSS3, Bootstrap 5

---

## 🚀 Como Executar o Projeto Localmente

### Pré-requisitos
- [.NET SDK](https://dotnet.microsoft.com/download) instalado (versão 9.0 ou 10.0+).
- [Visual Studio](https://visualstudio.microsoft.com/) ou [VS Code](https://code.visualstudio.com/).
- Git instalado.

### Passo a passo

1. **Clone este repositório:**
   ```bash
   git clone [https://github.com/SEU-USUARIO/WebDiario.git](https://github.com/SEU-USUARIO/WebDiario.git)
   cd WebDiario