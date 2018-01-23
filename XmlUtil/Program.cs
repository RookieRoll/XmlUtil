using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using XmlUtil.Models;

namespace XmlUtil
{
    class Program
    {
        static void Main(string[] args)
        {
            var path = new XmlHelper<MyClass>("E://1.xml");
            var list = new List<MyClass>()
            {
                new MyClass {name = "2r23"},
                new MyClass {name = "adf"},
                new MyClass {name = "fdsdf"},
                new MyClass {name = "fghgt23"},
                new MyClass {name = "oli67"},
            };
            //  path.Add(new MyClass(){name = "123"});
            // path.AddRange(list);
            //path.Finds();
            Func<MyClass, bool> tmep = m => m.name.Equals("2r23");
           // path.Remove(tmep);
            Console.WriteLine("Hello World!");
        }
    }

  

    class MyClass : IXmlEntity
    {
        public string name { get; set; }
        public int age { get; set; }
    }

  
}