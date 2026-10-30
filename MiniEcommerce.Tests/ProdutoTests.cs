using MiniEcommerce.Console.Models;

namespace MiniEcommerce.Tests;

public class ProdutoTests
{
    [Fact]
    public void Deve_Criar_Produto_Com_Dados_Validos()
    {
        var produto = new Produto("Teclado Mecânico", 250.00m, 10);

        Assert.Equal("Teclado Mecânico", produto.Nome);
        Assert.Equal(250.00m, produto.Preco);
        Assert.Equal(10, produto.QuantidadeEmEstoque);
    }

    [Theory]
    [InlineData(-0.01)]
    [InlineData(-100)]
    public void Nao_Deve_Criar_Produto_Com_Preco_Negativo(decimal preco)
    {
        var excecao = Assert.Throws<ArgumentException>(() => new Produto("Mouse", preco, 5));
        Assert.Equal("preco", excecao.ParamName);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(-50)]
    public void Nao_Deve_Criar_Produto_Com_Estoque_Negativo(int quantidade)
    {
        var excecao = Assert.Throws<ArgumentException>(() => new Produto("Monitor", 800.00m, quantidade));
        Assert.Equal("quantidadeEmEstoque", excecao.ParamName);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Nao_Deve_Criar_Produto_Com_Nome_Invalido(string? nome)
    {
        Assert.Throws<ArgumentException>(() => new Produto(nome!, 10.00m, 1));
    }

    [Fact]
    public void Deve_Permitir_Preco_E_Estoque_Zero()
    {
        var produto = new Produto("Brinde", 0m, 0);

        Assert.Equal(0m, produto.Preco);
        Assert.Equal(0, produto.QuantidadeEmEstoque);
    }

    [Fact]
    public void Deve_Adicionar_Estoque_Corretamente()
    {
        var produto = new Produto("Cabo HDMI", 30.00m, 5);

        produto.AdicionarEstoque(3);

        Assert.Equal(8, produto.QuantidadeEmEstoque);
    }

    [Fact]
    public void Deve_Remover_Estoque_Corretamente()
    {
        var produto = new Produto("Cabo HDMI", 30.00m, 5);

        produto.RemoverEstoque(2);

        Assert.Equal(3, produto.QuantidadeEmEstoque);
    }

    [Fact]
    public void Nao_Deve_Remover_Estoque_Maior_Que_O_Disponivel()
    {
        var produto = new Produto("Cabo HDMI", 30.00m, 5);

        Assert.Throws<InvalidOperationException>(() => produto.RemoverEstoque(10));
    }
}
