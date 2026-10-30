using MiniEcommerce.Console.Models;
using MiniEcommerce.Console.Services;

namespace MiniEcommerce.Tests;

public class CheckoutTests
{
    [Fact]
    public void Deve_Decrementar_Estoque_Apos_Finalizar_Pedido()
    {
        var produto = new Produto("Teclado", 100.00m, 10);
        var carrinho = new CarrinhoDeCompras();
        carrinho.AdicionarItem(produto, 3);
        var checkout = new Checkout();

        checkout.Finalizar(carrinho);

        Assert.Equal(7, produto.QuantidadeEmEstoque);
    }

    [Fact]
    public void Deve_Calcular_Valor_Total_Do_Pedido_Corretamente()
    {
        var teclado = new Produto("Teclado", 100.00m, 10);
        var mouse = new Produto("Mouse", 50.00m, 10);
        var carrinho = new CarrinhoDeCompras();
        carrinho.AdicionarItem(teclado, 2);
        carrinho.AdicionarItem(mouse, 1);
        var checkout = new Checkout();

        var pedido = checkout.Finalizar(carrinho);

        Assert.Equal(250.00m, pedido.ValorTotal);
    }

    [Fact]
    public void Deve_Gerar_Resumo_Com_Os_Itens_Comprados()
    {
        var teclado = new Produto("Teclado", 100.00m, 10);
        var mouse = new Produto("Mouse", 50.00m, 10);
        var carrinho = new CarrinhoDeCompras();
        carrinho.AdicionarItem(teclado, 2);
        carrinho.AdicionarItem(mouse, 1);
        var checkout = new Checkout();

        var pedido = checkout.Finalizar(carrinho);

        Assert.Equal(2, pedido.Itens.Count);
        var itemTeclado = Assert.Single(pedido.Itens, i => i.Produto == teclado);
        Assert.Equal(2, itemTeclado.Quantidade);
        Assert.Equal(100.00m, itemTeclado.PrecoUnitario);
        Assert.Equal(200.00m, itemTeclado.Subtotal);
    }

    [Fact]
    public void Deve_Esvaziar_Carrinho_Apos_Finalizar_Pedido()
    {
        var produto = new Produto("Teclado", 100.00m, 10);
        var carrinho = new CarrinhoDeCompras();
        carrinho.AdicionarItem(produto, 3);
        var checkout = new Checkout();

        checkout.Finalizar(carrinho);

        Assert.Empty(carrinho.Itens);
    }

    [Fact]
    public void Nao_Deve_Finalizar_Pedido_Com_Carrinho_Vazio()
    {
        var carrinho = new CarrinhoDeCompras();
        var checkout = new Checkout();

        Assert.Throws<InvalidOperationException>(() => checkout.Finalizar(carrinho));
    }

    [Fact]
    public void Nao_Deve_Finalizar_Pedido_Se_Estoque_Ficou_Insuficiente_Antes_Do_Checkout()
    {
        var produto = new Produto("Teclado", 100.00m, 5);
        var carrinhoA = new CarrinhoDeCompras();
        var carrinhoB = new CarrinhoDeCompras();
        carrinhoA.AdicionarItem(produto, 5);
        carrinhoB.AdicionarItem(produto, 5);
        var checkout = new Checkout();

        checkout.Finalizar(carrinhoA);

        Assert.Throws<InvalidOperationException>(() => checkout.Finalizar(carrinhoB));
        Assert.Single(carrinhoB.Itens);
        Assert.Equal(0, produto.QuantidadeEmEstoque);
    }

    [Fact]
    public void Nao_Deve_Decrementar_Estoque_De_Nenhum_Item_Se_Um_Deles_Tiver_Estoque_Insuficiente()
    {
        var teclado = new Produto("Teclado", 100.00m, 10);
        var mouse = new Produto("Mouse", 50.00m, 1);
        var carrinho = new CarrinhoDeCompras();
        carrinho.AdicionarItem(teclado, 5);
        carrinho.AdicionarItem(mouse, 1);
        mouse.RemoverEstoque(1);
        var checkout = new Checkout();

        Assert.Throws<InvalidOperationException>(() => checkout.Finalizar(carrinho));
        Assert.Equal(10, teclado.QuantidadeEmEstoque);
        Assert.Equal(2, carrinho.Itens.Count);
    }
}
