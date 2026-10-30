using MiniEcommerce.Console.Models;
using MiniEcommerce.Console.Services;

var catalogo = new Catalogo();
var carrinho = new CarrinhoDeCompras();
var checkout = new Checkout();

ExibirMenuPrincipal();

void ExibirMenuPrincipal()
{
    var continuar = true;
    while (continuar)
    {
        Console.WriteLine();
        Console.WriteLine("=== MiniEcommerce ===");
        Console.WriteLine("1. Cadastrar produto");
        Console.WriteLine("2. Listar catálogo");
        Console.WriteLine("3. Adicionar produto ao carrinho");
        Console.WriteLine("4. Remover produto do carrinho");
        Console.WriteLine("5. Ver carrinho");
        Console.WriteLine("6. Finalizar pedido");
        Console.WriteLine("0. Sair");
        Console.Write("Escolha uma opção: ");

        switch (Console.ReadLine())
        {
            case "1":
                CadastrarProduto();
                break;
            case "2":
                ListarCatalogo();
                break;
            case "3":
                AdicionarAoCarrinho();
                break;
            case "4":
                RemoverDoCarrinho();
                break;
            case "5":
                VerCarrinho();
                break;
            case "6":
                FinalizarPedido();
                break;
            case "0":
                continuar = false;
                break;
            default:
                Console.WriteLine("Opção inválida.");
                break;
        }
    }
}

void CadastrarProduto()
{
    Console.Write("Nome do produto: ");
    var nome = Console.ReadLine() ?? string.Empty;

    Console.Write("Preço: ");
    if (!decimal.TryParse(Console.ReadLine(), out var preco))
    {
        Console.WriteLine("Preço inválido.");
        return;
    }

    Console.Write("Quantidade em estoque: ");
    if (!int.TryParse(Console.ReadLine(), out var quantidade))
    {
        Console.WriteLine("Quantidade inválida.");
        return;
    }

    try
    {
        catalogo.CadastrarProduto(nome, preco, quantidade);
        Console.WriteLine($"Produto '{nome}' cadastrado com sucesso.");
    }
    catch (ArgumentException ex)
    {
        Console.WriteLine($"Erro ao cadastrar produto: {ex.Message}");
    }
}

void ListarCatalogo()
{
    if (catalogo.Produtos.Count == 0)
    {
        Console.WriteLine("Nenhum produto cadastrado.");
        return;
    }

    Console.WriteLine();
    Console.WriteLine("--- Catálogo de produtos ---");
    for (var i = 0; i < catalogo.Produtos.Count; i++)
    {
        var produto = catalogo.Produtos[i];
        Console.WriteLine($"{i + 1}. {produto.Nome} - R$ {produto.Preco:F2} - Estoque: {produto.QuantidadeEmEstoque}");
    }
}

Produto? SelecionarProduto()
{
    if (catalogo.Produtos.Count == 0)
    {
        Console.WriteLine("Nenhum produto cadastrado.");
        return null;
    }

    ListarCatalogo();
    Console.Write("Número do produto: ");
    if (!int.TryParse(Console.ReadLine(), out var indice) || indice < 1 || indice > catalogo.Produtos.Count)
    {
        Console.WriteLine("Produto inválido.");
        return null;
    }

    return catalogo.Produtos[indice - 1];
}

void AdicionarAoCarrinho()
{
    var produto = SelecionarProduto();
    if (produto is null)
        return;

    Console.Write("Quantidade: ");
    if (!int.TryParse(Console.ReadLine(), out var quantidade))
    {
        Console.WriteLine("Quantidade inválida.");
        return;
    }

    try
    {
        carrinho.AdicionarItem(produto, quantidade);
        Console.WriteLine($"{quantidade}x '{produto.Nome}' adicionado(s) ao carrinho.");
    }
    catch (Exception ex) when (ex is ArgumentException or InvalidOperationException)
    {
        Console.WriteLine($"Erro ao adicionar ao carrinho: {ex.Message}");
    }
}

void RemoverDoCarrinho()
{
    if (carrinho.Itens.Count == 0)
    {
        Console.WriteLine("O carrinho está vazio.");
        return;
    }

    VerCarrinho();
    Console.Write("Número do item a remover: ");
    if (!int.TryParse(Console.ReadLine(), out var indice) || indice < 1 || indice > carrinho.Itens.Count)
    {
        Console.WriteLine("Item inválido.");
        return;
    }

    var produto = carrinho.Itens[indice - 1].Produto;
    carrinho.RemoverItem(produto);
    Console.WriteLine($"'{produto.Nome}' removido do carrinho.");
}

void VerCarrinho()
{
    if (carrinho.Itens.Count == 0)
    {
        Console.WriteLine("O carrinho está vazio.");
        return;
    }

    Console.WriteLine();
    Console.WriteLine("--- Carrinho de compras ---");
    for (var i = 0; i < carrinho.Itens.Count; i++)
    {
        var item = carrinho.Itens[i];
        Console.WriteLine($"{i + 1}. {item.Produto.Nome} - {item.Quantidade}x R$ {item.Produto.Preco:F2} = R$ {item.Subtotal:F2}");
    }
    Console.WriteLine($"Total: R$ {carrinho.Total:F2}");
}

void FinalizarPedido()
{
    try
    {
        var pedido = checkout.Finalizar(carrinho);
        Console.WriteLine();
        Console.WriteLine("--- Pedido finalizado ---");
        foreach (var item in pedido.Itens)
            Console.WriteLine($"{item.Quantidade}x {item.Produto.Nome} - R$ {item.PrecoUnitario:F2} = R$ {item.Subtotal:F2}");
        Console.WriteLine($"Valor total: R$ {pedido.ValorTotal:F2}");
    }
    catch (InvalidOperationException ex)
    {
        Console.WriteLine($"Erro ao finalizar pedido: {ex.Message}");
    }
}
