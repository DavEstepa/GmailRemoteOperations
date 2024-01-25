// See https://aka.ms/new-console-template for more information
using GmailReader.Services;
using GmailReader.Domain.Utilities;

var app = new FlowExecution();
ProtectedExecutionMethod.Execute(app.RunMMFInstructions);

public class FlowExecution
{
    public async Task Run()
    {
        Console.WriteLine("App Running!");
        var client = new GmailClient();
        await client.Authenticate();
        await client.CleanMailbox("empleos_co@computrabajo.com");
    }

    public void RunMMFInstructions()
    {
        while (true)
        {
            Console.WriteLine("Desea Salir?");
            string option = Console.ReadLine()??"";
            if (option.ToLower() == "si") break;


        }
    }
}
