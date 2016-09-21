using System;
using RDotNet;
using Quartz;
using Quartz.Impl;

namespace RCalculator
{
    class Program
    {
        static void Main(string[] args)
        {
            // construct a scheduler factory
            ISchedulerFactory schedFact = new StdSchedulerFactory();

            // get a scheduler
            IScheduler sched = schedFact.GetScheduler();
            

            // define the job and tie it to our HelloJob class
            IJobDetail job = JobBuilder.Create<Forecast>()
                .WithIdentity("myJob", "group1")
                .Build();

           

            // Trigger the job to run now, and then every 40 seconds
            ITrigger trigger = TriggerBuilder.Create()
              .WithIdentity("myTrigger", "group1")
              .StartNow()
              //.WithSimpleSchedule(x => x
              //    .WithIntervalInSeconds(5)
               //   .WithRepeatCount(5)
                //  )
              //.WithDailyTimeIntervalSchedule(x=>x.WithIntervalInHours(24).OnEveryDay().StartingDailyAt(TimeOfDay.HourAndMinuteOfDay(15,55)))                          
              .Build();

            //Start
            sched.Start();

            sched.ScheduleJob(job, trigger);

        }
    }

    class Forecast : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            REngine engine;

            //init the R engine            
            REngine.SetEnvironmentVariables();
            engine = REngine.GetInstance();
            engine.Initialize();

            //Simple calculation

            var demomsg = engine.Evaluate("3*4");
            Console.WriteLine(demomsg);

            //A matrix is simply a rectangular array of numbers and a vector is a row (or column) of a matrix.A vector can be considered as 1 by n matrix or n by 1 matrix .
            var values = engine.Evaluate("data<-c( 100,134,234,500,342,125,56,435,678,244,876,123,45,46,356)");
            
            engine.Evaluate("datats<-ts(data,start=c(1,1), frequency=7)"); //f=7 for daily. start=c(1,1) for 1st day of 1st week


            engine.Evaluate("dataHW<-HoltWinters(datats,gamma=FALSE)");
            // var package = engine.Evaluate("path<-find.package(\"forecast\")"); //In case library did not load find the path of package explicitly
            engine.Evaluate("library(\"forecast\")");
            engine.Evaluate("forecast.HoltWinters(dataHW,h=1)");

            Console.ReadLine();


            //clean up
            engine.Dispose();
        }
    }
}