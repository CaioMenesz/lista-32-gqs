# MiniEcommerce

Aplicação de console em C#/.NET que simula o fluxo básico de um e-commerce: cadastro de produtos, carrinho de compras e fechamento de pedido (checkout), com baixa automática de estoque.

Projeto desenvolvido para a disciplina de Gestão e Qualidade de Software, seguindo TDD básico e o padrão Conventional Commits.

## Arquitetura

O projeto é dividido em duas aplicações (`.csproj`), reunidas na solução [MiniEcommerce.slnx](MiniEcommerce.slnx):

```
MiniEcommerce/
├── MiniEcommerce.Console/      # Aplicação de console (domínio + UI)
│   ├── Models/
│   │   ├── Produto.cs          # Entidade: nome, preço e estoque, com validação
│   │   ├── CarrinhoDeCompras.cs# Adiciona/remove itens, calcula total, valida estoque
│   │   ├── ItemCarrinho.cs     # Produto + quantidade dentro do carrinho
│   │   ├── Pedido.cs           # Snapshot do pedido fechado (itens, valor total, data)
│   │   └── ItemPedido.cs       # Item do pedido com preço unitário congelado na compra
│   ├── Services/
│   │   ├── Catalogo.cs         # Cadastro e listagem de produtos
│   │   └── Checkout.cs         # Fecha o pedido: valida estoque, decrementa e limpa o carrinho
│   └── Program.cs              # Menu interativo de linha de comando
└── MiniEcommerce.Tests/        # Testes unitários (xUnit) para todo o domínio
    ├── ProdutoTests.cs
    ├── CatalogoTests.cs
    ├── CarrinhoDeComprasTests.cs
    └── CheckoutTests.cs
```

### Camadas e responsabilidades

- **`Models`** — entidades e regras de negócio. `Produto` nunca aceita preço ou estoque negativo. `CarrinhoDeCompras` nunca aceita uma quantidade maior do que o estoque disponível no produto (somando o que já está no carrinho).
- **`Services`** — orquestração de casos de uso. `Catalogo` cadastra produtos delegando a validação para `Produto`. `Checkout` valida o estoque de **todos** os itens do carrinho antes de decrementar qualquer um deles (evitando baixa parcial em caso de falha), gera o `Pedido` e esvazia o carrinho.
- **`Program.cs`** — camada de apresentação (menu de console); não contém regra de negócio, apenas chama `Catalogo`, `CarrinhoDeCompras` e `Checkout` e trata as exceções de domínio (`ArgumentException`, `InvalidOperationException`) como mensagens amigáveis.

### Fluxo de uma compra

1. **Cadastro** — `Catalogo.CadastrarProduto` cria um `Produto` validado.
2. **Carrinho** — `CarrinhoDeCompras.AdicionarItem` valida a quantidade contra o estoque atual do produto (sem decrementá-lo ainda).
3. **Checkout** — `Checkout.Finalizar` revalida o estoque, decrementa via `Produto.RemoverEstoque`, gera um `Pedido` com o resumo financeiro e esvazia o carrinho.

## Como executar

Requer o [.NET SDK 10](https://dotnet.microsoft.com/download).

```bash
# Executar a aplicação de console
dotnet run --project MiniEcommerce.Console

# Rodar a suíte de testes
dotnet test
```

## Testes

Todas as regras de negócio são cobertas por testes unitários (xUnit) em `MiniEcommerce.Tests`, incluindo:

- Rejeição de produto com preço ou estoque negativo.
- Cálculo de subtotal por item e total do carrinho.
- Rejeição de quantidade acima do estoque disponível (inclusive ao somar adições sucessivas do mesmo produto).
- Baixa de estoque e geração do resumo financeiro no checkout, com atomicidade (nenhum item tem o estoque decrementado se outro item do pedido não tiver estoque suficiente).

```bash
dotnet test
```

## Padrão de commits

O histórico segue [Conventional Commits](https://www.conventionalcommits.org/) (`feat`, `fix`, `test`, `refactor`, `docs`, `chore`), com os testes de cada funcionalidade comitados antes da respectiva implementação (TDD básico).
