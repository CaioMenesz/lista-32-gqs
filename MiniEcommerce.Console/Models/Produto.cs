namespace MiniEcommerce.Console.Models;

public class Produto
{
    public string Nome { get; private set; }
    public decimal Preco { get; private set; }
    public int QuantidadeEmEstoque { get; private set; }

    public Produto(string nome, decimal preco, int quantidadeEmEstoque)
    {
        if (string.IsNullOrWhiteSpace(nome))
            throw new ArgumentException("O nome do produto não pode ser vazio.", nameof(nome));

        if (preco < 0)
            throw new ArgumentException("O preço do produto não pode ser negativo.", nameof(preco));

        if (quantidadeEmEstoque < 0)
            throw new ArgumentException("A quantidade em estoque não pode ser negativa.", nameof(quantidadeEmEstoque));

        Nome = nome;
        Preco = preco;
        QuantidadeEmEstoque = quantidadeEmEstoque;
    }

    public void AdicionarEstoque(int quantidade)
    {
        if (quantidade < 0)
            throw new ArgumentException("A quantidade a adicionar não pode ser negativa.", nameof(quantidade));

        QuantidadeEmEstoque += quantidade;
    }

    public void RemoverEstoque(int quantidade)
    {
        if (quantidade < 0)
            throw new ArgumentException("A quantidade a remover não pode ser negativa.", nameof(quantidade));

        if (quantidade > QuantidadeEmEstoque)
            throw new InvalidOperationException("Estoque insuficiente para a operação solicitada.");

        QuantidadeEmEstoque -= quantidade;
    }
}
