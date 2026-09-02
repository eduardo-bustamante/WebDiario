# 🌿 Página Aberta

> Um espaço pessoal e seguro para acolher reflexões, memórias e leituras diárias.

O **Página Aberta** é uma aplicação web completa desenvolvida em **ASP.NET Core MVC** que combina o hábito do diário pessoal (com rastreador de estado emocional) à organização de uma biblioteca particular (com controle de progresso de páginas e capas personalizadas).

A plataforma foi construída com foco em minimalismo, privacidade individual e uma estética relaxante baseada em tons de verde sálvia e azul petróleo.

---

## ✨ Funcionalidades Principais

### 📓 Diário & Reflexões
- **Registro Pessoal:** Criação, edição, listagem e leitura detalhada de entradas do cotidiano.
- **Escala de Bem-Estar / Humor:** Classificação interativa em 5 níveis visuais (*Muito Difícil*, *Para Baixo*, *Neutro*, *Bem*, *Excelente*).
- **Isolamento de Dados:** Cada usuário autenticado acessa exclusivamente os seus próprios registros.
- **Busca e Filtros:** Pesquisa textual instantânea por palavras-chave em títulos e reflexões.

### 📚 Minha Biblioteca
- **Catalogação Completa:** Título, autor, gênero/categoria, status de leitura e resenhas/notas pessoais.
- **Acompanhamento de Leitura:**
  - Contador de páginas lidas em relação ao total da obra.
  - Barra de progresso percentual calculada automaticamente.
  - Atualização inteligente de status (*Quero Ler*, *Lendo*, *Lido*, *Abandonado*).
- **Gestão Flexível de Capas:**
  - Suporte a upload de arquivos locais (`.png`, `.jpg`, `.jpeg`, `.webp`).
  - Suporte a links diretos de imagens da internet (URL externa).
  - Pré-visualização instantânea da capa antes do salvamento.
- **Avaliação:** Sistema de classificação por estrelas (1 a 5).

### 🎨 Experiência do Usuário & Design
- **Paleta Relaxante:** Tons serenos de Verde Sálvia (`#2d6a4f`), Azul Petróleo e fundo off-white.
- **Navbar Glassmorphism:** Barra superior translúcida com efeito de vidro embaçado (`backdrop-filter`) e indicador de página ativa.
- **Home Dinâmica:** Página inicial adaptativa que funciona como vitrine acolhedora para visitantes e painel de boas-vindas com atalhos para usuários autenticados.
- **Design Totalmente Responsivo:** Interface fluida para dispositivos móveis e desktops construída em Bootstrap 5.

---

## 🛠️ Tecnologias Utilizadas

- **Back-end:** C# / .NET (ASP.NET Core MVC)
- **Acesso a Dados:** Entity Framework Core (Code-First)
- **Banco de Dados:** Microsoft SQL Server / LocalDB
- **Autenticação & Segurança:** ASP.NET Core Identity (cookies com expiração por inatividade e tokens antifalsificação)
- **Front-end:** Razor Views, HTML5, CSS3 moderno (variáveis CSS), JavaScript Vanilla e Bootstrap 5

---

## 📁 Estrutura do Projeto

```text
PaginaAberta/
├── Controllers/
│   ├── ContaController.cs       # Login, registro e encerramento de sessão
│   ├── DiarioController.cs      # CRUD de entradas do diário e escala de humor
│   ├── HomeController.cs        # Painel dinâmico e página de boas-vindas
│   └── LivrosController.cs      # Gestão do acervo, progresso e capas
├── Data/
│   └── AppDbContext.cs          # Contexto do EF Core e mapeamento de entidades
├── Models/
│   ├── Diario.cs                # Modelo de dados de anotações e humor
│   └── Livro.cs                 # Modelo de dados da biblioteca e progresso
├── Views/
│   ├── Conta/                   # Telas de login e cadastro
│   ├── Diario/                  # Telas de listagem, criação e edição de reflexões
│   ├── Home/                    # Página inicial
│   ├── Livros/                  # Estante, detalhes, cadastro e edição de livros
│   └── Shared/
│       ├── _Layout.cshtml       # Menu relaxante, estrutura base e rodapé
│       └── _ValidationScriptsPartial.cshtml
└── wwwroot/
    ├── capas/                   # Armazenamento local de uploads de capas
    ├── css/
    │   └── site.css             # Tema visual personalizado (Página Aberta)
    └── js/
