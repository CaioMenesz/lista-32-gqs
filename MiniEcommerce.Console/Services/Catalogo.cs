using MiniEcommerce.Console.Models;

namespace MiniEcommerce.Console.Services;

public class Catalogo
{
    private readonly List<Produto> _produtos = new();

    public IReadOnlyList<Produto> Produtos => _produtos;

    public Produto CadastrarProduto(string nome, decimal preco, int quantidadeEmEstoque)
    {
        var produto = new Produto(nome, preco, quantidadeEmEstoque);
        _produtos.Add(produto);
        return produto;
    }
}
