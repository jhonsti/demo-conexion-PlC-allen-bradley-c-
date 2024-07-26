using libplctag.DataTypes.Simple;
using libplctag;
using System;
using System.Collections.Generic;
using libplctag;
using LibplctagWrapper;
using System.Threading;
using System.Net;
using System.Diagnostics;


namespace ConexionPLC
{
  
    public class Program
    {
        private static string ipAddress;
        private static int leerPLCEntero(string direccionDeLectura)
        {
            int result;
            var tag = new LibplctagWrapper.Tag(ipAddress, CpuType.SLC, direccionDeLectura, DataType.Int16, 1);
            
            using (var client = new Libplctag())
            {
                client.AddTag(tag);
                while (client.GetStatus(tag) == Libplctag.PLCTAG_STATUS_PENDING)
                {
                    Thread.Sleep(10);
                }
                if (client.GetStatus(tag) != Libplctag.PLCTAG_STATUS_OK)
                {
                    Console.WriteLine($"error{client.DecodeError(client.GetStatus(tag))}");
                }
                else { Console.WriteLine("OK"); }


                result = client.ReadTag(tag, 20);

                // Check the read operation result
                if (result != Libplctag.PLCTAG_STATUS_OK)
                {
                    Console.WriteLine($"ERROR: Unable to read the data! Got error code {result}: {client.DecodeError(result)}\n");

                }
                var TestDintArray0 = client.GetInt16Value(tag, 0 * tag.ElementSize);
                client.RemoveTag(tag);
                return TestDintArray0;   
            }
            



        }
        private static  void EscribirPLCEntero(string direccionDeLectura,int valor)
        {
         
            var tag = new LibplctagWrapper.Tag(ipAddress, CpuType.SLC, direccionDeLectura, DataType.Int16, 1);
            using (var client = new Libplctag())
            {
                client.AddTag(tag);
                while (client.GetStatus(tag) == Libplctag.PLCTAG_STATUS_PENDING)
                {
                    Thread.Sleep(10);
                }
                if (client.GetStatus(tag) != Libplctag.PLCTAG_STATUS_OK)
                {
                    Console.WriteLine($"error{client.DecodeError(client.GetStatus(tag))}");
                }
                else { Console.WriteLine("OK"); }
  
             
                client.SetUint16Value(tag, 0 * tag.ElementSize, Convert.ToUInt16(valor));
                var escribir = client.WriteTag(tag, 10);


            }

        }
        private static void EscribirPLCBool(string direccionDeLectura,int posicion, bool valor)
        {
            Stopwatch stopwatch = new Stopwatch();
            var tag = new LibplctagWrapper.Tag(ipAddress, CpuType.SLC, direccionDeLectura, DataType.INT, 1);
            using (var client = new Libplctag())
            {
                client.AddTag(tag);
                while (client.GetStatus(tag) == Libplctag.PLCTAG_STATUS_PENDING)
                {
                    Thread.Sleep(50);
                }
                if (client.GetStatus(tag) != Libplctag.PLCTAG_STATUS_OK)
                {
                    Console.WriteLine($"error{client.DecodeError(client.GetStatus(tag))}");
                }
                else { Console.WriteLine("OK"); }
                stopwatch.Start();
                client.SetBitValue(tag, posicion, valor, 10);
                var escribir = client.WriteTag(tag,  10);
               

         
                // Detener el cronómetro
                stopwatch.Stop();

                // Obtener el tiempo transcurrido
                TimeSpan tiempoTranscurrido = stopwatch.Elapsed;

                // Mostrar el tiempo transcurrido en milisegundos
                Console.WriteLine($"Tiempo transcurrido: {tiempoTranscurrido.TotalMilliseconds} milisegundos");


            }

        }


         static void Main(string[] args)
        { 
 
      

            // Comenzar a medir el tiempo
            Stopwatch stopwatch = new Stopwatch();
            // Configuración de la conexión
            ipAddress = "10.52.0.170"; // Dirección IP del PLC MicroLogix 1400
            try
            {
                while (true)
                {
                  
                    stopwatch.Start();
                    var pos0 = leerPLCEntero("N7:0");

                    // Detener el cronómetro
                    stopwatch.Stop();
                   
                    // Obtener el tiempo transcurrido
                    TimeSpan tiempoTranscurrido = stopwatch.Elapsed;
                    stopwatch.Restart();
                    // Mostrar el tiempo transcurrido en milisegundos
                    Console.WriteLine($"Tiempo transcurrido: {tiempoTranscurrido.TotalMilliseconds} milisegundos");
                   
                    //EscribirPLCEntero("N7:9", 46);
                    //var pos1 = leerPLCEntero("N7:1");
                    //Console.WriteLine("Aqui Si escreibor");
                    //EscribirPLCEntero("N7:9", (int)(4.6));
                    //EscribirPLCEntero("B3:3", 66);

                    //EscribirPLCBool("B3:0",10,true);

                    Console.WriteLine($"lectura pos 0: {pos0} ");
                    //Console.WriteLine($"lectura pos 1: {pos1}");

                }

            }
            catch { }

            // Conectar al PLC
          
         }
    }
}
