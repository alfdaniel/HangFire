using Hangfire;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HangFireApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobController : ControllerBase
    {

        [HttpGet]
        public void ListaNumeros()
        {
            for (int i = 0; i < 100000; i++)
            {

                Console.WriteLine("Contador", i);

            }
        }

        [HttpPost]
        [Route("BackGroudLista")]
        public ActionResult BackGroudLista()
        {

            BackgroundJob.Enqueue(() => this.ListaNumeros());

            return Ok();
        }


        [HttpPost]
        [Route("scheduleLista")]
        public ActionResult ScheduleLista()
        {

            var scheduleDateTime = DateTime.UtcNow.AddSeconds(5);

            var dateTimeOffset = new DateTimeOffset(scheduleDateTime);

            BackgroundJob.Schedule(() => Console.WriteLine("Tarefa Agendada 5S"), dateTimeOffset);

            return Ok();
        }


        [HttpPost]
        [Route("continuationJob")]
        public ActionResult ContinuationJob()
        {

            var scheduleDateTime = DateTime.UtcNow.AddSeconds(5);

            var dateTimeOffset = new DateTimeOffset(scheduleDateTime);

            var job1 = BackgroundJob.Schedule(() => Console.WriteLine("Tarefa Agendada 5S"), dateTimeOffset);

            var job2 = BackgroundJob.ContinueJobWith(job1, () => Console.WriteLine("Job 2"));
            var job3 = BackgroundJob.ContinueJobWith(job2, () => Console.WriteLine("Job 3"));

            var job4 = BackgroundJob.ContinueJobWith(job1, () => Console.WriteLine("Job 4 após job 1"));

            return Ok();
        }

        [HttpPost]
        [Route("callRecurringJob")]
        public ActionResult CallRecurringJob()
        {

            RecurringJob.AddOrUpdate("easyjob", () => Console.Write("Easy!"), "* * * * *");

            return Ok();
        }

    }
}
