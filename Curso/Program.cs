using System;
using CursoEFCore.Domain;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using CursoEFCore.StatusPedidos;
using System.Collections.Generic;

namespace CursoEFCore
{
    class Program
    {
        static void Main(string[] args)
        {
            using var db = new Data.ApplicationContext();
            // db.Database.Migrate();

            // var existe = db.Database.GetPendingMigrations().Any();
            // if (existe)
            // {
            //     
            // }

            InserirDadosEmMassa();
        }
        private static void RemoverRegistro()
        {
            using var db = new Data.ApplicationContext();
            var cliente = db.Cliente.Find(2); //-> procura pela chave primaria da tabela
            //db.Cliente.Remove(cliente);
            //db.Remove(cliente);
            db.Entry(cliente).State = EntityState.Deleted;

            db.SaveChanges();
        }
        private static void AtualizarDados()
        {
            using var db = new Data.ApplicationContext();
            // var cliente = db.Cliente.FirstOrDefault(p => p.Id == 1);
            var cliente = db.Cliente.Find(1);
            cliente.Nome = "Cliente Alterado Passo 1";

            //db.Attach(cliente); -> Faz com que o objeto comece a ser rastreado 
            //db.Entry(cliente).State = EntityState.Modified; -> Informando ao rastreamento que houve alteração
            //db.Cliente.Update(cliente); -> Dessa maneira irá atualizar todos os dados novamente, mesmo que vc só tenha alterado 1
            db.SaveChanges();
        }

        private static void ConsultarPedidoCarregamentoAdiantado()
        {
            using var db = new Data.ApplicationContext();
            //var pedidos = db.Pedidos.ToList();
            //var pedidos = db.Pedidos.Include("Items").ToList();
            var pedidos = db.Pedidos
                    .Include(p => p.Items)
                    .ThenInclude(p => p.Produto)
                    .ToList();


            Console.WriteLine(pedidos.Count);

        }
        private static void CadastrarPedido()
        {
            using var db = new Data.ApplicationContext();

            var cliente = db.Cliente.FirstOrDefault();
            var produto = db.Produtos.FirstOrDefault();

            var pedido = new Pedido
            {
                ClienteId = cliente.Id,
                IniciadoEm = DateTime.Now,
                FinalizadoEm = DateTime.Now,
                Observacao = "Pedido Teste",
                Status = StatusPedido.Analise,
                TipoFrete = TipoFrete.SemFrete,
                Items = new List<PedidoItem>
                {
                    new PedidoItem
                    {
                        ProdutoId = produto.Id,
                        Desconto = 0,
                        Quantidade = 1,
                        Valor = 10
                    }
                }
            };

            db.Pedidos.Add(pedido);

            db.SaveChanges();
        }

        private static void ConsultarDados()
        {
            using var db = new Data.ApplicationContext();
            //var consultaPorSintaxe = (from c in db.Cliente where c.Id > 0 select c).ToList();
            //var consultaPorMetodo = db.Cliente.AsNoTracking.Where(p => p.Id > 0).ToList();
            var consultaPorMetodo = db.Cliente
                .Where(p => p.Id > 0)
                .OrderBy(p => p.Id)
                .ToList();

            foreach (var cliente in consultaPorMetodo)
            {
                //db.Cliente.Find(cliente.Id);
                db.Cliente.FirstOrDefault(p => p.Id == cliente.Id);
            }
        }
        private static void InserirDadosEmMassa()
        {
            var produto = new Produto
            {
                Descricao = "Produto Teste",
                CodigoBarras = "1234567891234",
                Valor = 10m,
                TipoProduto = TipoProduto.MercadoriaParaRevenda,
                Ativo = true
            };

            var cliente = new Cliente
            {
                Nome = "Victor Silva",
                CEP = "03977005",
                Cidade = "São Paulo",
                Estado = "SP",
                Telefone = "11930088261"
            };

            var listaClientes = new[]{
                new Cliente
                {
                    Nome = "Victor Silva",
                    CEP = "03977005",
                    Cidade = "São Paulo",
                    Estado = "SP",
                    Telefone = "11930088261"
                },
                new Cliente
                {
                    Nome = "Victor Silva",
                    CEP = "03977005",
                    Cidade = "São Paulo",
                    Estado = "SP",
                    Telefone = "11930088261"
                }
            };

            using var db = new Data.ApplicationContext();
            //db.AddRange(produto, cliente);
            //db.Set<Cliente>().Add(listaClientes);
            db.Cliente.AddRange(listaClientes);

            var registros = db.SaveChanges();
            System.Console.WriteLine($"Total de registro(s): {registros}");
        }
        private static void InserirDados()
        {
            var produto = new Produto
            {
                Descricao = "Produto Teste",
                CodigoBarras = "1234567891234",
                Valor = 10m,
                TipoProduto = TipoProduto.MercadoriaParaRevenda,
                Ativo = true
            };

            using var db = new Data.ApplicationContext();
            //db.Produtos.Add(produto);
            //db.Set<Produto>().Add(produto);
            //db.Entry(produto).State = EntityState.Added;
            db.Add(produto);

            var registro = db.SaveChanges();
            Console.WriteLine($"Total Registros(s): {registro}");
        }
    }
}
