using System.Net.Sockets;
using System.Net;
using System.IO;
using System.Text.Json;
using TCPServer.Model;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Microsoft.VisualBasic;
Console.WriteLine("TCP Server:");

// Start serveren på port 7000 (som er nyt)
TcpListener server = new TcpListener(IPAddress.Any, 7000);
//start server.
        server.Start();
        Console.WriteLine("Serveren venter på forbindelse...");

while (true)
{
            
//Acceptér forbindelse fra klienten
TcpClient client = server.AcceptTcpClient();
Console.WriteLine("Klient forbundet!");
    Console.WriteLine();

//Starter en ny asynkron opgave for at håndtere klienten i baggrunden
Task.Run(() => HandleClient(client));

}
    
    void HandleClient(TcpClient tcpClient)
    {
        TcpClient client = tcpClient;

      
        // Få en stream til at sende og modtage beskeder
        NetworkStream stream = client.GetStream();
        StreamReader reader = new StreamReader(stream);
        StreamWriter writer = new StreamWriter(stream) { AutoFlush = true };
        //Jeg har sat AutoFlush = true isted for at skrive writer.Flush efter værre gang vi laver en writer.WriteLine();
    
    //Metode til succcess bedsked da det indholder det sammen.
    void SuccusMessage(int result, int tal1, int tal2, string method)
    {
        //Derefter laver vi et obejkt som indholder om det lykkes, result, tallen og hvilken metode.
        var r = new
        {

            Method = method,
            Tal1 = tal1,
            Tal2 = tal2,
            Result = result,
            Message = "Success"

        };

        PrintClient(r);
    }

    //Da jeg skrive dette med at udskriver til flere sted, har jeg lavet en metode til det.
    void PrintClient(object message)
        {
            //Serialize objekt til json-format.
            var errorJson = JsonSerializer.Serialize(message);

            //udskriver til klient
            writer.WriteLine(errorJson);

            //udskriver i konsole.
            Console.WriteLine("Sent to client: " + errorJson);
            Console.WriteLine();
        }
    try
    {
        while (true)
        {
           
            //Laver et Instruction som er defult med automatisk tekst hvor jeg oprette en klasse til.
            Instruction I = new Instruction();

            //Laver om det json formaet
            var IJson = JsonSerializer.Serialize(I);

            //Udskriver til konsole og klient.
            Console.WriteLine($"Sent to client: {IJson}");
            writer.WriteLine(IJson);
            Console.WriteLine();

            //Læser tekste fra klient
            var messsagereceived = reader.ReadLine();

            //udskriver i konsole.
            Console.WriteLine(messsagereceived);
            Console.WriteLine(); 

            //Deserialize json-format til en Dictionary med key og value som string for at arbejde med alt er rigtig sat ind
            var jsonDictionaryDeserialize = JsonSerializer.Deserialize<Dictionary<string, string>>(messsagereceived);

            //der efter kan vi hente være ting fra Dictionary ved key (Method, tal1, tal2) for at får value.
            string method = jsonDictionaryDeserialize["Method"];
            string tal1 = jsonDictionaryDeserialize["Tal1"];
            string tal2 = jsonDictionaryDeserialize["Tal2"];

            //hvis den er tom vil der blive sendt en error bedsked eller metode ikke er add, subtract eller random./*method != "add" && method != "subtract" && method != "random"*/
            if (string.IsNullOrEmpty(method) || string.IsNullOrEmpty(tal1) || string.IsNullOrEmpty(tal2)|| method != "add" && method != "subtract" && method != "random")
            {

                //anonymt objekt for error
                var Error = new
                {
                    Error = "Invalid input. Please ensure the input is not empty and consists of exactly 3 parts: a command followed by two numbers.",
                    Code = 400,

                };

                PrintClient(Error);
            }
            //hvis tal1 og tal2 ikke er tal vil der kommen en anden error
            else if (!int.TryParse(tal1, out int _) || !int.TryParse(tal2, out int _))
            {
                //anonymt objekt for error
                var Error = new
                {
                    Error = "Invalid input. The two values after the command must be numbers.",
                    Code = 400,

                };

              PrintClient(Error);

            }

            //ellers vil format være ok så vil den går ind i else

            else
            {
                //omskriver string til en int for begge tal.
                int.TryParse(tal1, out int tal1Int);
                int.TryParse(tal2, out int tal2Int);

                //hvis metode er random vil den går inden
                if (method == "random")
                {

                    //hvis tal 1 er større end lige med tal 2 

                    if (tal1Int >= tal2Int)
                    { 
                        //får man error at den skal være højre.
                        var Error = new
                        {
                            Error = "Invalid input. Last number needs to be higher",
                            Code = 400,

                        };

                        PrintClient(Error);
                    }
                    else
                    {
                        //Bruger random obejkt til at kunne vælge et tal
                        Random random = new Random();

                        //får result tal i et int result derefter vælges mellem nummer1 og nummer2 +1 for at får begge tal med.
                        int result = random.Next(tal1Int, (tal2Int + 1));

                        //Lavet en metode til succusMessage
                        SuccusMessage(result,tal1Int,tal2Int,method);
                    }

                }
                //hvis metode er add
                else if (method == "add")
                {
                    //få result ved at plus de to tal sammen
                    int result = tal1Int + tal2Int;

                    //Lavet en metode til succusMessage
                    SuccusMessage(result, tal1Int, tal2Int, method);
                }
                else if (method == "subtract")
                { //hvis tal2 er større end tal1 vil den går inden i if-sætning

                    if (tal1Int < tal2Int)
                    {
                        //error bedsked man modtager
                        var Error = new
                        {

                            Error = "Invalid input. First number needs to be higher",
                            Code = 400,

                        };
                        
                        PrintClient(Error);

                    }
                    
                    //hvis den tal1 er større end tal2 vil den gå ind i else
                    else
                    {
                        //int result fra ral1 minus tal2 (derfor skal tal1 være højste eller vil man få et result som er minus)
                        int result = (tal1Int - tal2Int);

                        //Lavet en metode til succusMessage
                        SuccusMessage(result, tal1Int, tal2Int, method);
                    }
                }

            }
        }
       
           

    } 
    //håndeter fejl hvis der sker noget udvendt
        catch (Exception ex) 
            {
                Console.WriteLine("Fejl: " + ex.Message);
             
            }
    }
       
       