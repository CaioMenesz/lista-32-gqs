using MiniEcommerce.Console.Services;

namespace MiniEcommerce.Tests;

public class CatalogoTests
{
    [Fact]
    public void Deve_Cadastrar_Produto_Valido_No_Catalogo()
    {
        var catalogo = new Catalogo();

        var produto = catalogo.CadastrarProduto("Teclado", 150.00m, 20);

        Assert.Single(catalogo.Produtos);
        Assert.Contains(produto, catalogo.Produtos);
    }

    [Fact]
    public void Nao_Deve_Cadastrar_Produto_Com_Preco_Negativo()
    {
        var catalogo = new Catalogo();

        Assert.Throws<ArgumentException>(() => catalogo.CadastrarProduto("Mouse", -10.00m, 5));
        Assert.Empty(catalogo.Produtos);
    }

    [Fact]
    public void Nao_Deve_Cadastrar_Produto_Com_Estoque_Negativo()
    {
        var catalogo = new Catalogo();

        Assert.Throws<ArgumentException>(() => catalogo.CadastrarProduto("Monitor", 500.00m, -1));
        Assert.Empty(catalogo.Produtos);
    }
}
