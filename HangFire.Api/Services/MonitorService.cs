using Hangfire;
using Hangfire.Console;
using Hangfire.Server;

namespace HangFire.Api.Services;

public class MonitorService : IHostedService
{
    public async Task StopAsync(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await AddJobHangFire();
    }

    private async Task AddJobHangFire()
    {
        //Agendando um serviço/Metodo
        BackgroundJob.Schedule(() => Print("Agendamento", null), TimeSpan.FromSeconds(5));

        //Agendamento em Fila
        //BackgroundJob.Enqueue("Test", () => Print($"Teste in queue <fila>", null));
        var jobId =BackgroundJob.Enqueue("test", () => Print($"Teste in queue <fila>", null));

        //coninue Job Roda o processo a partir de um Id pai (se o processo "jogId" rodar ao finalizar ele chama este)
        BackgroundJob.ContinueJobWith(jobId, () => Print($"Rodou apos terminar o job id {jobId}", null));


        //Recorrente (roda em um intevalo de tempo)
        RecurringJob.AddOrUpdate("RecurringJob",() => PrintRecurringJob($"Recurring",null), MinuteInterval(5));
    }
    
    //Redirecionando msg do console para o dashbord do hangFire
    public void Print(string message, PerformContext? context)
    {
        context.WriteLine(message);
    }

    //Criando rotinas com thread para monstrar tempo de execução no dashboard
    public void PrintRecurringJob(string message, PerformContext? context)
    {
        context.WriteLine("Inicioo do Processo");
        Thread.Sleep(TimeSpan.FromSeconds(5));
        context.WriteLine("Processo esta quse acabando");
        Thread.Sleep(TimeSpan.FromSeconds(5));
        context.WriteLine("Processo Finalizado");
        context.WriteLine(message);
    }

    public static string MinuteInterval(int interval)
    {
        return $"*/{interval} * * * * ";
    }
}