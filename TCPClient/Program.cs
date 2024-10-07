using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using TCPServer.Model;


Console.WriteLine("TCP Client:");

// Opret forbindelse til serveren på localhost (127.0.0.1) og port 7000 (som er nyt)
TcpClient client = new TcpClient("127.0.0.1", 7000);
Console.WriteLine("Forbundet til serveren!");

// Få en stream til at sende og modtage beskeder
NetworkStream stream = client.GetStream();
StreamReader reader = new StreamReader(stream);
StreamWriter writer = new StreamWriter(stream) { AutoFlush = true };
//Jeg har sat AutoFlush = true i sted for at skrive writer.Flush efter værre gang vi laver en writer.WriteLine();

// Klienten skal fortsætte med at sende kommandoer
try
{
    while (true)
    {

        //Første bedesked som klinet modtager fra server.
        var firstMessage = reader.ReadLine();
        Console.WriteLine();
        //Console.WriterLine så de kan læse den på konsolen
        Console.WriteLine("Received instruction message from server:" + firstMessage);
        Console.WriteLine();
        //Console.ReadLine() så de kan skrive et input fra den bedsked de har modtaget.
        var input = Console.ReadLine();

        //input bliver sat til at lowercase og spilter dem op så når man skriver 
        //Jeg laver det til et array af strenge som er navnet givet requstMessageParts 
        //Random 1 2 så bliver det i arrayet:
        //requstMessageParts[0] = "random" som er metoden
        //requstMessageParts[1] = "1" som er tal1
        //requstMessageParts[2] = "2" som er tal2

        string[] requstMessageParts = input.ToLower().Split(' ');
        //da vi skal havde 3 input så ser jeg om length er 3.
        if (requstMessageParts.Length == 3)
        {
            //derefter sætter vi alle del til være deres string er type og har være deres variabel.
            string method = requstMessageParts[0];
            string tal1 = requstMessageParts[1];
            string tal2 = requstMessageParts[2];
            //anonymt objekt hvor jeg efter opret JSON-format
            var request = new
            {
                Method = method,
                Tal1 = tal1,
                Tal2 = tal2
            };
            //opretter json-format
            var inputJson = JsonSerializer.Serialize(request);
            //udskriver det til server
            writer.WriteLine(inputJson);
            Console.WriteLine();
            //Læser svaret fra server
            var secMessage = reader.ReadLine();
            //udskriver det i konsole.
            Console.WriteLine("Received result from the server: " + secMessage);


        }
        //hvis ikke den er 3 elementer i arrayet eller er flere end 3 eller færre, vil jeg sætte dem til ingenting
        //for så vil server send en error bedsked tilbage.
        else
        {
            //anonymt objekt hvor jeg efter opret JSON-format med tom felt.
            var request = new
            {
                Method = "",
                Tal1 = "",
                Tal2 = ""
            };
            //laver det om til Json-format
            var inputJson = JsonSerializer.Serialize(request);
            //udskriver til server.
            writer.WriteLine(inputJson);
            //læser bedsked fra server
            var secMessage = reader.ReadLine();
            //udskriver bedsked i konsolen.
            Console.WriteLine(secMessage);


        }
    }


}
//håndeter fejl hvis der sker noget udvendt
catch (Exception ex)
{

    Console.WriteLine("Fejl: " + ex.Message);

}



           

        
  




