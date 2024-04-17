using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace ConsoleCRUD
{
    //Classe Pessoa/Cliente
    public class Pessoa
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Nome { get; set; }
    }

    //Banco de dados.
    public class Contexto : DbContext
    {
        public DbSet<Pessoa> Pessoas { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite("Data Source=projetocrud.db");
    }

    //Começa a rodar
    class Program
    {
        static void Main(string[] args)
        {
            using (var db = new Contexto())
            {
                db.Database.EnsureCreated();

                while (true)
                {
                    Console.WriteLine("\nEscolha uma opção:");
                    Console.WriteLine("1. Cad. Clientes");
                    Console.WriteLine("2. Pesquisar clientes");
                    Console.WriteLine("3. Atualizar cliente");
                    Console.WriteLine("4. Deletar cliente");
                    Console.WriteLine("5. Sair\n");

                    //Lê o input para colocar no Switch Case
                    string option = Console.ReadLine();

                    switch (option)
                    {
                        case "1":
                            AddCli(db);
                            break;
                        case "2":
                            PesquisaCli(db);
                            break;
                        case "3":
                            AtualizaCli(db);
                            break;
                        case "4":
                            DeletaCli(db);
                            break;
                        case "5":
                            Environment.Exit(0);
                            break;
                        default:
                            Console.WriteLine("Escolha uma das opções acima");
                            break;
                    }
                }
            }
        }

        //Create
        //Função para Adicionar os clientes à base
        static void AddCli(Contexto db)
        {
            Console.WriteLine("\nDigite o nome do Cliente:");
            string name = Console.ReadLine();

            var pessoa = new Pessoa { Nome = name };
            db.Pessoas.Add(pessoa);
            db.SaveChanges();

            Console.WriteLine("\nCliente adicionado com sucesso!");
        }

        //Read
        //Função para pesquisar os clientes já existentes na db
        static void PesquisaCli(Contexto db)
        {
            Console.WriteLine("\nLista de clientes:");

            var pessoas = db.Pessoas.ToList();
            foreach (var p in pessoas)
            {
                Console.WriteLine($"\nNum. de Controle: {p.Id} \nNome: {p.Nome}\n");
            }
        }

        //Update
        //Função para alterar o nome (valores) do cliente
        static void AtualizaCli(Contexto db)
        {
            Console.WriteLine("\nDigite o Controle do cliente que deseja atualizar:");
            int id = int.Parse(Console.ReadLine());

            Console.WriteLine("\nDigite o novo nome do cliente:");
            string name = Console.ReadLine();

            var pessoa = db.Pessoas.FirstOrDefault(p => p.Id == id);
            if (pessoa != null)
            {
                pessoa.Nome = name;
                db.SaveChanges();
                Console.WriteLine("\nCliente atualizado com sucesso!");
            }
            else
            {
                Console.WriteLine("\nCliente já excluído ou inexxistente.");
                Console.WriteLine("Utilize a opção 2 para pesquisar novamente.");
            }
        }

        //Delete
        //Função para excluir os clientes
        static void DeletaCli(Contexto db)
        {
            Console.WriteLine("\nDigite o controle do cliente que deseja deletar:");
            int id = int.Parse(Console.ReadLine());

            var pessoa = db.Pessoas.FirstOrDefault(p => p.Id == id);
            if (pessoa != null)
            {
                db.Pessoas.Remove(pessoa);
                db.SaveChanges();
                Console.WriteLine("\nCliente deletado com sucesso!");
            }
            else
            {
                Console.WriteLine("\nCliente já excluído ou inexxistente.");
                Console.WriteLine("Utilize a opção 2 para pesquisar novamente.");
            }
        }
    }
}
