namespace MiniEcommerce.Console.Models;

public class ItemCarrinho
{
    public Produto Produto { get; }
    public int Quantidade { get; internal set; }

    public decimal Subtotal => Produto.Preco * Quantidade;

    public ItemCarrinho(Produto produto, int quantidade)
    {
        Produto = produto;
        Quantidade = quantidade;
    }
}
