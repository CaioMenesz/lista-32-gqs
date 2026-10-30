namespace MiniEcommerce.Console.Models;

public class CarrinhoDeCompras
{
    private readonly List<ItemCarrinho> _itens = new();

    public IReadOnlyList<ItemCarrinho> Itens => _itens;

    public decimal Total => _itens.Sum(item => item.Subtotal);

    public void AdicionarItem(Produto produto, int quantidade)
    {
        ArgumentNullException.ThrowIfNull(produto);

        if (quantidade <= 0)
            throw new ArgumentException("A quantidade deve ser maior que zero.", nameof(quantidade));

        var itemExistente = _itens.FirstOrDefault(item => item.Produto == produto);
        var quantidadeJaNoCarrinho = itemExistente?.Quantidade ?? 0;

        if (quantidadeJaNoCarrinho + quantidade > produto.QuantidadeEmEstoque)
            throw new InvalidOperationException("Quantidade solicitada excede o estoque disponível.");

        if (itemExistente is not null)
            itemExistente.Quantidade += quantidade;
        else
            _itens.Add(new ItemCarrinho(produto, quantidade));
    }

    public void RemoverItem(Produto produto)
    {
        _itens.RemoveAll(item => item.Produto == produto);
    }
}
