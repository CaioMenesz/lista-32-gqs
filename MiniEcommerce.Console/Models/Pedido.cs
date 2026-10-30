namespace MiniEcommerce.Console.Models;

public class Pedido
{
    public IReadOnlyList<ItemPedido> Itens { get; }
    public decimal ValorTotal { get; }
    public DateTime DataHora { get; }

    public Pedido(IEnumerable<ItemCarrinho> itensCarrinho)
    {
        Itens = itensCarrinho
            .Select(item => new ItemPedido(item.Produto, item.Quantidade, item.Produto.Preco))
            .ToList();
        ValorTotal = Itens.Sum(item => item.Subtotal);
        DataHora = DateTime.Now;
    }
}
