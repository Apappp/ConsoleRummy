using System;
using System.Text;
using System.Threading.Tasks; // <--- Dodajemy obsługę asynchroniczności (Taski)
using WatsonTcp;

namespace ConsoleRummy;

class Program
{
    static async Task Main(string[] args)
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("=== REMIK MULTIPLAYER ===");
            Console.WriteLine("1. Stwórz grę");
            Console.WriteLine("2. Dołącz do gry");
            Console.WriteLine("3. Wyjście z programu");
            Console.Write("\nWybierz opcję: ");

            string choice = Console.ReadLine();

            if (choice == "1")
            {
                await StartHost();
                break;
            }
            else if (choice == "2")
            {
                await StartClient();
                break;
            }
            else if (choice == "3")
            {
                Environment.Exit(0);
            }
        }
    }

    static async Task StartHost()
    {
        Console.Clear();
        Console.Write("Podaj swój NICK: ");
        string hostName = Console.ReadLine() ?? "Gracz";
        Guid adminId = Guid.Empty;

        Console.WriteLine("Serwer się uruchamia...");
        WatsonTcpServer server = new WatsonTcpServer("0.0.0.0", 9000);
        bool isGameStarted = false;

        GameManager table = new GameManager();
        
        server.Events.ClientConnected += async (sender, e) =>
        {
            
            if (isGameStarted)
            {
                Console.WriteLine($"[Serwer] Odrzucono gracza {e.Client.IpPort} - gra już trwa.");
                
                // Wysyłamy mu powód wyrzucenia (żeby jego klient wiedział, co się stało)
                byte[] rejectMsg = Encoding.UTF8.GetBytes("BŁĄD|Gra już się rozpoczęła. Nie możesz dołączyć.");
                await server.SendAsync(e.Client.Guid, rejectMsg);
                
                await server.DisconnectClientAsync(e.Client.Guid);
                return;
            }

        };

        server.Events.MessageReceived += async (sender, e) =>
        {
            string message = Encoding.UTF8.GetString(e.Data);
            
            
            if (message.StartsWith("JOIN|"))
            {
                string playerName = message.Split('|')[1];
                string playerId = e.Client.Guid.ToString(); 

                Console.WriteLine($"[Serwer] Gracz dołączył: {playerName} (ID: {playerId})");
                
                table.Players.Add(new Player(playerId, playerName));

                foreach (var client in server.ListClients())
                {
                    byte[] info = Encoding.UTF8.GetBytes($"{playerName} dołączył do gry");
                    await server.SendAsync(client.Guid, info);
                }
            }
            else
            {
                foreach (var client in server.ListClients())
                {
                    string textToSend = $"{table.GetPlayerNameById(e.Client.Guid.ToString())}): {message}";
                    byte[] bytesToSend = Encoding.UTF8.GetBytes(textToSend);
                    await server.SendAsync(client.Guid, bytesToSend);
                }
            }
        };
        /*
        server.Events.MessageReceived += async (sender, e) =>
        {
            string message = Encoding.UTF8.GetString(e.Data);
            
            foreach (var client in server.ListClients())
            {
                string textToSend = $"Gracz {e.Client.IpPort} wykonał ruch: {message}";
                byte[] bytesToSend = Encoding.UTF8.GetBytes(textToSend);
                
                // ZMIANA: Używamy SendAsync
                await server.SendAsync(client.Guid, bytesToSend);
            }
        };
        */
        server.Start();
        Console.WriteLine("[Serwer] Działa w tle! Podłączam Twojego lokalnego klienta...\n");

        await RunClientLoop("127.0.0.1", hostName, true);
    }

    static async Task StartClient()
    {
        Console.Clear();
        Console.Write("Podaj swój NICK: ");
        string playerName = Console.ReadLine() ?? "Gracz";

        Console.Write("Podaj adres IP Hosta (zostaw puste dla 127.0.0.1): ");
        string ip = Console.ReadLine() ?? "";
        if (string.IsNullOrWhiteSpace(ip)) ip = "127.0.0.1";

        await RunClientLoop(ip, playerName, false);
    }

    static async Task RunClientLoop(string ipAddress, string playerName, bool isHost)
    {
        WatsonTcpClient client = new WatsonTcpClient(ipAddress, 9000);

        client.Events.ServerConnected += (sender, e) => 
        {
            Console.WriteLine("[Klient] Udało się połączyć z Serwerem!");
        };

        client.Events.MessageReceived += (sender, e) => 
        {
            string message = Encoding.UTF8.GetString(e.Data);
            Console.WriteLine($"\n>> [STÓŁ]: {message}");
            Console.Write("Wpisz komendę: ");
        };

        try
        {
            client.Connect();

            string joinCommand = isHost ? $"JOIN_HOST|{playerName}" : $"JOIN|{playerName}";
            byte[] joinData = Encoding.UTF8.GetBytes(joinCommand);
            await client.SendAsync(joinData);

            Console.WriteLine($"Witaj {playerName}! Jesteś w grze! Wpisz coś i wciśnij Enter.");
            
            while (true)
            {
                Console.Write("Wpisz komendę: ");
                string input = Console.ReadLine() ?? "";

                if (input.ToLower() == "wyjscie")
                {
                    break;
                }

                if (!string.IsNullOrWhiteSpace(input))
                {
                    byte[] data = Encoding.UTF8.GetBytes(input);
                    // ZMIANA: Używamy SendAsync
                    await client.SendAsync(data);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"\n[BŁĄD] Nie można podłączyć się do serwera. ({ex.Message})");
        }
    }
}