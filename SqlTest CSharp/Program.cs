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

            Console.WriteLine("Press enter to proceed to the next test. (1) addRecord");
            Console.ReadLine();
            String a = "HtmlContent.getText something something github ";
            String[] result = a.Split((char[])null, StringSplitOptions.RemoveEmptyEntries); //split by whitespace, slightly optimized
            foreach (String entry in result)
            {
                Mss.addRecord(entry, new Uri("https://github.com"));
            }
            String b = "HtmlContent.getText something something git";
            result = b.Split((char[])null, StringSplitOptions.RemoveEmptyEntries); 
            foreach (String entry in result)
            {
                Mss.addRecord(entry, new Uri("https://github.com"));
            }

            //getWordsFromUri
            Console.WriteLine("Press enter to proceed to the next test. (2) getWordsFromUri");
            Console.ReadLine();
            Console.WriteLine("Print list");
            var list = Mss.getWordsFromUri(new Uri("https://github.com"));
            foreach (var i in list)
            {
                Console.WriteLine(i);
            }

            //getUriFromWords
            Console.WriteLine("Press enter to proceed to the next test. (3) getUriFromWords");
            Console.ReadLine();
            list = Mss.getUriFromWords("git");
            Console.WriteLine("Print list");
            foreach (var i in list)
            {
                Console.WriteLine(i);
            }
            list = Mss.getUriFromWords("something");
            Console.WriteLine("Print list");
            foreach (var i in list)
            {
                Console.WriteLine(i);
            }

            //dropRecordByUri
            Console.WriteLine("Press enter to proceed to the next test. (4) dropRecordByUri");
            Console.ReadLine();
            Mss.dropRecordByUri(new Uri("https://github.com"));
            Console.WriteLine("Print list");
            list = Mss.getWordsFromUri(new Uri("https://github.com"));
            foreach (var i in list)
            {
                Console.WriteLine(i);
            }

            Console.WriteLine("End of program");
            Console.ReadLine();
        }
    }
}
