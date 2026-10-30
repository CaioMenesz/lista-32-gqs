using MiniEcommerce.Console.Models;

namespace MiniEcommerce.Console.Services;

public class Checkout
{
    public Pedido Finalizar(CarrinhoDeCompras carrinho)
    {
        ArgumentNullException.ThrowIfNull(carrinho);

        if (carrinho.Itens.Count == 0)
            throw new InvalidOperationException("Não é possível finalizar um pedido com o carrinho vazio.");

        foreach (var item in carrinho.Itens)
        {
            if (item.Quantidade > item.Produto.QuantidadeEmEstoque)
                throw new InvalidOperationException($"Estoque insuficiente para o produto '{item.Produto.Nome}'.");
        }

        foreach (var item in carrinho.Itens)
            item.Produto.RemoverEstoque(item.Quantidade);

        var pedido = new Pedido(carrinho.Itens);
        carrinho.Limpar();
        return pedido;
    }
}
