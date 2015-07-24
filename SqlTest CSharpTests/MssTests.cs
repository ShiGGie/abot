using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlTest_CSharp;
using Microsoft.VisualStudio.TestTools.UnitTesting;

//Notes
//new Uri appends a "/" to the end of a url.

namespace SqlTest_CSharp.Tests
{
    [TestClass()]
    public class MssTests
    {
        [TestInitialize()]
        public void setupDatabaseTest()
        {
            Mss.setupDatabase();
            Mss.dropRecordByUri(new Uri("https://github.com"));
        }


        [TestMethod()]
        public void getWordsFromUriTest()
        {
            //addRecord
            String a = "HtmlContent.getText something something github ";
            String[] result = a.Split((char[])null, StringSplitOptions.RemoveEmptyEntries); //split by whitespace, slightly optimized
            foreach (String entry in result)
            {
               Assert.IsTrue( Mss.addRecord(entry, new Uri("https://github.com")));
            }

            String b = "HtmlContent.getText something something git";
            result = b.Split((char[])null, StringSplitOptions.RemoveEmptyEntries);
            foreach (String entry in result)
            {
               Assert.IsTrue(Mss.addRecord(entry, new Uri("https://github.com")));
            }

            //getWordsFromUri
            var list = Mss.getWordsFromUri(new Uri("https://github.com"));
            ArrayList list2 = new ArrayList {
                           "HtmlContent.getText", 
                           "something", 
                           "something",
                           "github",
                           "HtmlContent.getText", 
                           "something", 
                           "something",
                           "git"
                        };
            CollectionAssert.AreEqual(list, list2);

            
            //getUriFromWords
            list = Mss.getUriFromWords("git");
            list2 = new ArrayList(new string[] {"https://github.com/"});
            CollectionAssert.AreEqual(list, list2);
            list = Mss.getUriFromWords("something");
            list2 = new ArrayList(new string[] {"https://github.com/", "https://github.com/", "https://github.com/", "https://github.com/" });
            CollectionAssert.AreEqual(list, list2);


            //dropRecordByUri
            Assert.IsTrue(Mss.dropRecordByUri(new Uri("https://github.com")));
            list = Mss.getWordsFromUri(new Uri("https://github.com"));
            CollectionAssert.AreEqual(list, new ArrayList { });
        }
    }
}
