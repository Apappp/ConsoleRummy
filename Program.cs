using System;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using WatsonTcp;

namespace ConsoleRummy;

class Program
{
    static async Task Main(string[] args)
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("--- REMIK MULTIPLAYER ---");
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
        List<Player> lobbyPlayers = new List<Player>();
        
        server.Events.ClientConnected += async (sender, e) =>
        {
            
            if (isGameStarted)
            {
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

                if (adminId == Guid.Empty)
                {
                    adminId = e.Client.Guid;
                }
                
                lobbyPlayers.Add(new Player(playerId, playerName));

                foreach (var client in server.ListClients())
                {
                    byte[] info = Encoding.UTF8.GetBytes($"MESSAGE|{playerName} dołączył do gry!");
                    await server.SendAsync(client.Guid, info);
                }
            }
            else if (message.StartsWith("ACTION|"))
            {
                string jsonText = message.Split('|')[1];
                
                IPlayerAction? action = JsonSerializer.Deserialize<IPlayerAction>(jsonText);

                if (action != null)
                {
                    try{
                        action.ExecuteAction(table);
                        await BroadcastGameState(server, table);
                    }
                    catch(GameLogicException ex)
                    {
                        byte[] responseBytes = Encoding.UTF8.GetBytes($"MESSAGE|{ex.Message}");
                        await server.SendAsync(e.Client.Guid, responseBytes);  
                        await BroadcastGameState(server, table);
                    }
                    catch(Exception ex)
                    {
                        byte[] responseBytes = Encoding.UTF8.GetBytes($"MESSAGE|Krytyczny błąd serwera:{ex.Message}, {ex.StackTrace}");
                        await server.SendAsync(e.Client.Guid, responseBytes);  
                        await BroadcastGameState(server, table);
                    }
                }
            }
            else if (message.StartsWith("/"))
            {
                if((message == "/start") && (e.Client.Guid == adminId))
                {
                    if(lobbyPlayers.Count < 2)
                    {
                        byte[] info = Encoding.UTF8.GetBytes($"MESSAGE|Niewystarczająca ilość graczy.");
                        await server.SendAsync(e.Client.Guid, info);
                        return;
                    }

                    byte[] info2 = Encoding.UTF8.GetBytes($"MESSAGE|Rozpoczynanie gry...");
                    await server.SendAsync(e.Client.Guid, info2);
                    
                    table.Players = lobbyPlayers;
                    table.ChangeState(new DealingState());
                    await BroadcastGameState(server, table);
                }
                else if(message == "/draw")
                {
                    
                }
                else
                {
                    string textToSend = "MESSAGE|Nieznana komenda";
                    byte[] bytesToSend = Encoding.UTF8.GetBytes(textToSend);
                    await server.SendAsync(e.Client.Guid, bytesToSend);
                }
            }
            else
            {
                foreach (var client in server.ListClients())
                {
                    string textToSend = $"MESSAGE|{lobbyPlayers.Find(x => x.NetworkId == e.Client.Guid.ToString())?.Nickname}: {message}";
                    byte[] bytesToSend = Encoding.UTF8.GetBytes(textToSend);
                    await server.SendAsync(client.Guid, bytesToSend);
                }
            }
        };
        server.Start();

        await RunClientLoop("127.0.0.1", hostName);
    }

    static async Task StartClient()
    {
        Console.Clear();
        Console.Write("Podaj swój NICK: ");
        string playerName = Console.ReadLine() ?? "Gracz";

        Console.Write("Podaj adres IP Hosta (zostaw puste dla 127.0.0.1): ");
        string ip = Console.ReadLine() ?? "";
        if (string.IsNullOrWhiteSpace(ip)) ip = "127.0.0.1";

        await RunClientLoop(ip, playerName);
    }

    static async Task RunClientLoop(string ipAddress, string playerName)
    {
        WatsonTcpClient client = new WatsonTcpClient(ipAddress, 9000);
        ConsoleRenderer renderer = new ConsoleRenderer();
        LocalGameState localGame = new LocalGameState();
        List<string> messages = new List<string>();
        bool isGameStarted = false;

        client.Events.ServerConnected += (sender, e) => 
        {
            Console.WriteLine("Udało się połączyć z Serwerem!");
        };

        client.Events.MessageReceived += (sender, e) => 
        {
            string message = Encoding.UTF8.GetString(e.Data);
            if (message.StartsWith("MESSAGE|"))
            {
                string trimmedMessage = message.Split('|')[1];
                messages.Add(trimmedMessage);
                if(!isGameStarted){
                    renderer.DrawLobbyScreen(messages);
                }
                else
                {
                    renderer.DrawGameScreen(localGame, messages, playerName);
                }
            }
            else if(message.StartsWith("STATE_UPDATE|"))
            {
                string jsonText = message.Split('|')[1];

                try
                {
                    LocalGameState? gameState = JsonSerializer.Deserialize<LocalGameState>(jsonText);

                    if (gameState != null)
                    {
                        isGameStarted = true;
                        localGame = gameState;

                        renderer.DrawGameScreen(localGame, messages, playerName);
                    }
                }
                catch (JsonException ex)
                {
                    Console.WriteLine($"[Błąd Klienta] Nie udało się rozpakować stanu gry: {ex.Message}");
                }
                
            }
        };

        try
        {
            client.Connect();

            string joinCommand = $"JOIN|{playerName}";
            byte[] joinData = Encoding.UTF8.GetBytes(joinCommand);
            await client.SendAsync(joinData);

            Console.WriteLine($"Witaj {playerName}! Jesteś w grze! Wpisz coś i wciśnij Enter.");
            renderer.DrawLobbyScreen(messages);
            
            async Task SendActionAsync(IPlayerAction action)
            {
                string actionJson = JsonSerializer.Serialize<IPlayerAction>(action);
                byte[] dataToSend = Encoding.UTF8.GetBytes($"ACTION|{actionJson}");
                await client.SendAsync(dataToSend);
            }

            while (true)
            {
                string input = Console.ReadLine() ?? "";
                
                if (string.IsNullOrWhiteSpace(input)) continue;

                if (!isGameStarted)
                {
                    byte[] data = Encoding.UTF8.GetBytes(input);
                    await client.SendAsync(data);
                    continue;   
                }

                string[] parts = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                string command = parts[0].ToLower();

                switch (command)
                {
                    case "/discard":
                        if (parts.Length != 2)
                        {
                            messages.Add("Wybierz kartę do odrzucenia. Użyj: /discard [numer]");
                            break; 
                        }
                        if (!int.TryParse(parts[1], out int cardIndex))
                        {
                            messages.Add("Niepoprawny numer karty! Użyj: /discard [numer]");
                            break;
                        }

                        await SendActionAsync(new DiscardAction(localGame.Seat, cardIndex - 1));
                        break;

                    case "/draw":
                        await SendActionAsync(new DrawCardAction(localGame.Seat));
                        break;

                    case "/swap":
                        if (parts.Length != 3) 
                        {
                            messages.Add("Podaj dokładnie dwie karty! Użyj: /swap [numer1] [numer2]");
                            break;
                        }
                        if (!int.TryParse(parts[1], out int c1) || !int.TryParse(parts[2], out int c2))
                        {
                            messages.Add("Numery kart muszą być liczbami! Użyj: /swap [numer1] [numer2]");
                            break;
                        }

                        await SendActionAsync(new SwapCardsAction(localGame.Seat, c1 - 1, c2 - 1));
                        break;

                    default:
                        if (command.StartsWith("/"))
                        {
                            messages.Add($"Nieznana komenda");
                        }
                        else 
                        {
                            byte[] data = Encoding.UTF8.GetBytes(input);
                            await client.SendAsync(data);
                        }
                        break;
                }

                renderer.DrawGameScreen(localGame, messages, playerName);
            }
            
        }
        catch (Exception ex)
        {
            Console.WriteLine($"\n[BŁĄD] Nie można podłączyć się do serwera. ({ex.Message})");
        }
    }

    static async Task BroadcastGameState(WatsonTcpServer server, GameManager table)
    {
        foreach (var client in server.ListClients())
        {
            LocalGameState personalState = table.GetStateForPlayer(client.Guid);
            
            if (personalState != null)
            {
                string jsonState = JsonSerializer.Serialize(personalState);
                
                byte[] dataToSend = Encoding.UTF8.GetBytes($"STATE_UPDATE|{jsonState}");
                await server.SendAsync(client.Guid, dataToSend);
            }
        }
    }
}