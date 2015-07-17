using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/* This is a mini project to familiarize myself with Web crawlers and Search engine functions.
 * the original repository is from  https://github.com/sjdirect/abot
 * 
 * This project aims to store web content in a database which allows optimize querying of content, as the original 
 * repo stored Uris in memory.
 */

namespace SqlTest_CSharp
{
    /*Requires Manual setup of "csWebCrawler" Database on SQL_Server Express
     * This is a primitive testbench, will be integrated into Abot Webcrawler.cs->Crawlpage->ProcessPage
     * when time permits.
     */

    class Program
    {
        static void Main(string[] args)
        {
            //TODO: Unit tests
            Console.WriteLine("Setting up table..");
            Mss.setupDatabase();
            Console.WriteLine("Press enter to proceed to the next test. (1)");
            Console.ReadLine();
            Mss.findContentWithUri(new Uri("https://github.com"));
            
            //TODO: Regex search through content
            //Console.WriteLine("Press enter to proceed to the next test. (2)");
            //Console.ReadLine();
            //Mss.getUriWithCriteria("github");


            Console.WriteLine("End of program");
            Console.ReadLine();
        }
    }
}
