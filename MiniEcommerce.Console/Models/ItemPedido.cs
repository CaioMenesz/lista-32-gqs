namespace MiniEcommerce.Console.Models;

public class ItemPedido
{
    public Produto Produto { get; }
    public int Quantidade { get; }
    public decimal PrecoUnitario { get; }

    public decimal Subtotal => PrecoUnitario * Quantidade;

    public ItemPedido(Produto produto, int quantidade, decimal precoUnitario)
    {
        Produto = produto;
        Quantidade = quantidade;
        PrecoUnitario = precoUnitario;
    }
}
