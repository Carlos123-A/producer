using System;
using System.Text.Json;
using System.Threading.Tasks;
using Confluent.Kafka;

public class Program
{
    public static async Task Main(string[] args)
    {
        var config = new ProducerConfig { BootstrapServers = ":29092" };
        using var producer = new ProducerBuilder<Null, string>(config).Build();

        var books = new[]
        {
            new { Title = "1984", Author = "George Orwell", Year = 1949 },
            new { Title = "To Kill a Mockingbird", Author = "Harper Lee", Year = 1960 },
            new { Title = "The Great Gatsby", Author = "F. Scott Fitzgerald", Year = 1925 },
            new { Title = "One Hundred Years of Solitude", Author = "Gabriel García Márquez", Year = 1967 },
            new { Title = "Moby Dick", Author = "Herman Melville", Year = 1851 }
        };

        var random = new Random();

        while (true)
        {
            var book = books[random.Next(books.Length)];
            var message = new Message<Null, string> { Value = JsonSerializer.Serialize(book) };
            await producer.ProduceAsync("books", message);
            Console.WriteLine($"Produced: {book.Title} by {book.Author} ({book.Year})");

            await Task.Delay(2000); 
        }
    }
}
