using System;
using RDotNet;

namespace RCalculator
{
    class Program
    {
        static void Main(string[] args)
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
            NumericMatrix values = engine.Evaluate("data<-c( 4,234,12324234,13557788,2345,243,0,2,567886 )").AsNumericMatrix();
            engine.Evaluate("datats.ts<-ts(data,frequency=3)");
            engine.Evaluate("datadecompose<-decompose(datats.ts)");
            
            engine.Evaluate("HoltWinters(datats.ts)");

            
            //Console.WriteLine(assigntoTS.AsCharacter());

           
            Console.ReadLine();
            
            
            //clean up
            engine.Dispose();

        }
    }
}