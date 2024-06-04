using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;

class GetBook
{
    static async Task Main(string[] args)
    {
        while (true)
        {
            Console.WriteLine("Digite o título do livro para pesquisar (ou 'sair' para encerrar):");
            string bookTitle = Console.ReadLine();

            if (bookTitle.Equals("sair", StringComparison.OrdinalIgnoreCase))
            {
                break;
            }

            await SearchBookAsync(bookTitle);
        }
    }

    static async Task SearchBookAsync(string title)
    {
        using (var httpClient = new HttpClient())
        {
            var response = await httpClient.GetAsync($"http://openlibrary.org/search.json?title={title}");

            if (response.IsSuccessStatusCode)
            {
                string jsonResponse = await response.Content.ReadAsStringAsync();
                JObject data = JObject.Parse(jsonResponse);

                JArray docs = (JArray)data["docs"];

                Console.WriteLine($"Resultados para '{title}':");
                foreach (var doc in docs)
                {
                    string bookTitle = doc["title"]?.ToString() ?? "Título não disponível";
                    string authors = doc["author_name"] != null ? string.Join(", ", doc["author_name"]) : "Autor não disponível";
                    Console.WriteLine($"Título: {bookTitle} - Autor: {authors}");
                }
                Console.WriteLine();
            }
            else
            {
                Console.WriteLine($"Erro ao obter dados: {response.StatusCode}");
            }
        }
    }
}
