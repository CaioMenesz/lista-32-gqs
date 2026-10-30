using MiniEcommerce.Console.Models;

namespace MiniEcommerce.Tests;

public class CarrinhoDeComprasTests
{
    [Fact]
    public void Deve_Adicionar_Item_Com_Sucesso()
    {
        var produto = new Produto("Teclado", 100.00m, 10);
        var carrinho = new CarrinhoDeCompras();

        carrinho.AdicionarItem(produto, 3);

        var item = Assert.Single(carrinho.Itens);
        Assert.Equal(produto, item.Produto);
        Assert.Equal(3, item.Quantidade);
    }

    [Fact]
    public void Deve_Calcular_Subtotal_Do_Item_Corretamente()
    {
        var produto = new Produto("Teclado", 100.00m, 10);
        var carrinho = new CarrinhoDeCompras();

        carrinho.AdicionarItem(produto, 3);

        Assert.Equal(300.00m, carrinho.Itens[0].Subtotal);
    }

    [Fact]
    public void Deve_Calcular_Total_Do_Carrinho_Com_Multiplos_Itens()
    {
        var teclado = new Produto("Teclado", 100.00m, 10);
        var mouse = new Produto("Mouse", 50.00m, 10);
        var carrinho = new CarrinhoDeCompras();

        carrinho.AdicionarItem(teclado, 2);
        carrinho.AdicionarItem(mouse, 1);

        Assert.Equal(250.00m, carrinho.Total);
    }

    [Fact]
    public void Deve_Somar_Quantidade_Ao_Adicionar_Mesmo_Produto_Duas_Vezes()
    {
        var produto = new Produto("Teclado", 100.00m, 10);
        var carrinho = new CarrinhoDeCompras();

        carrinho.AdicionarItem(produto, 2);
        carrinho.AdicionarItem(produto, 3);

        var item = Assert.Single(carrinho.Itens);
        Assert.Equal(5, item.Quantidade);
    }

    [Fact]
    public void Nao_Deve_Adicionar_Quantidade_Superior_Ao_Estoque_Disponivel()
    {
        var produto = new Produto("Teclado", 100.00m, 5);
        var carrinho = new CarrinhoDeCompras();

        Assert.Throws<InvalidOperationException>(() => carrinho.AdicionarItem(produto, 6));
        Assert.Empty(carrinho.Itens);
    }

    [Fact]
    public void Nao_Deve_Adicionar_Mesmo_Produto_Excedendo_Estoque_Apos_Adicoes_Anteriores()
    {
        var produto = new Produto("Teclado", 100.00m, 5);
        var carrinho = new CarrinhoDeCompras();

        carrinho.AdicionarItem(produto, 3);

        Assert.Throws<InvalidOperationException>(() => carrinho.AdicionarItem(produto, 3));
        Assert.Equal(3, carrinho.Itens[0].Quantidade);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Nao_Deve_Adicionar_Quantidade_Zero_Ou_Negativa(int quantidade)
    {
        var produto = new Produto("Teclado", 100.00m, 5);
        var carrinho = new CarrinhoDeCompras();

        Assert.Throws<ArgumentException>(() => carrinho.AdicionarItem(produto, quantidade));
    }

    [Fact]
    public void Deve_Remover_Item_Do_Carrinho()
    {
        var produto = new Produto("Teclado", 100.00m, 5);
        var carrinho = new CarrinhoDeCompras();
        carrinho.AdicionarItem(produto, 2);

        carrinho.RemoverItem(produto);

        Assert.Empty(carrinho.Itens);
    }
}
