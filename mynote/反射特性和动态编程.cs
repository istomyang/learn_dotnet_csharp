using System;
using System.Diagnostics;
using System.Reflection;
using System.Collections.Generic;
using static System.Console;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.Xml.Linq;
using System.Dynamic;

namespace MyNote.Reflection
{
    class Class1
    {
        public static void Fn1()
        {
            DateTime datetime = new DateTime();

            Type type = datetime.GetType();
        }

        private static void PrintPropertyName<T>(T example)
        {
            Type type = example.GetType();
            foreach (System.Reflection.PropertyInfo property in type.GetProperties())
            {
                Console.WriteLine(property.Name);
            }
        }

    }
}

namespace MyNote.Attribute
{
    /// <summary>
    /// Customize your Attribute
    /// </summary>
    class MyAttribute : System.Attribute
    {
        public static string[] GetMemberFromMyAttribute(object obj)
        {
            List<string> members = new List<string>();

            /// <summary>
            /// Core ways: 
            ///     Object -> GetType().GetProprties() -> 
            ///     PropertyInfo -> GetCustomAttributes() ->
            ///     Now I can use all the Attributes.
            /// </summary>
            PropertyInfo[] props = obj.GetType().GetProperties();

            foreach (PropertyInfo prop in props)
            {
                System.Attribute[] attrs = (System.Attribute[])prop.GetCustomAttributes(typeof(MyAttribute), false);

                if ((attrs.Length > 0) && (prop.GetValue(obj) != null))
                {
                    members.Add(prop.Name);
                }
            }

            return members.ToArray();
        }
    }

    class Class1
    {
        [My]
        public bool Number1 { get; set; }

        [MyAttribute] /*Attribute is not required.*/
        public bool Number2 { get; set; }
    }

    public class R1
    {
        public static void F1()
        {
            WriteLine("Running!");

            Class1 c1 = new Class1();
            string[] members = Attribute.MyAttribute.GetMemberFromMyAttribute(c1);

            int count = 1;
            foreach (string member in members)
            {
                WriteLine($"Index: {count}，content: {member}");
                count++;
            }
        }
    }

    /// <summary>
    /// Use data in attribute.
    /// </summary>
    class ConstructorAttribute : System.Attribute
    {
        public ConstructorAttribute(string alias)
        {
            Alias = alias;
        }

        public string Alias { get; private set; }

        public static Dictionary<string, PropertyInfo> GetSwitches(object obj)
        {
            PropertyInfo[] props = null;
            Dictionary<string, PropertyInfo> dic = new Dictionary<string, PropertyInfo>();

            props = obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

            foreach (PropertyInfo prop in props)
            {
                dic.Add(prop.Name.ToLower(), prop);
                foreach (ConstructorAttribute attr in prop.GetCustomAttributes(typeof(ConstructorAttribute), false))
                {
                    dic.Add(attr.Alias.ToLower(), prop);
                }
            }

            return dic;
        }
    }

    class Class2
    {
        /// <summary>
        /// "?" must is const and typeof()
        /// </summary>
        [Constructor("?")]
        public string Help { get; set; }
    }

    public class R2
    {
        public static void F1()
        {
            PropertyInfo prop = typeof(Class2).GetType().GetProperty("Help");

            /// <summary>
            /// Core Way is GetCustomAttributes()
            /// </summary>
            ConstructorAttribute attr = (ConstructorAttribute)prop.GetCustomAttributes(typeof(ConstructorAttribute), false)[0];
            if (attr.Alias == "?")
            {
                WriteLine("Help(?)");
            }

            /// <summary>
            /// use dic.
            /// </summary>
            Dictionary<string, PropertyInfo> dic = ConstructorAttribute.GetSwitches(new Class2());

        }
    }

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)] /*Limit attribute only to apply for property and firld.*/
    public class FirstAttribute : System.Attribute { }

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class SecondAttriute : System.Attribute { }

    /// <summary>
    /// ⭐ Serialize the object.
    /// </summary>
    [Serializable]
    class Docuement
    {
        [NonSerialized]
        public string _noSave;

        public string Title { get; set; }
        public string Year { get; set; }

        public Docuement(string title, string year)
        {
            Title = title;
            Year = year;
        }
    }

    public class R3
    {
        public void F1()
        {
            Stream s;
            Docuement doc = new Docuement("Essential C#7.0", "2017");
            Docuement docOpen;

            /// <summary>
            /// Create seriable file.
            /// </summary>
            using (s = File.Open(doc.Title + ".bin", FileMode.Create))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(s, doc);
            }

            /// <summary>
            /// Open seriable file.
            /// </summary>
            using (s = File.Open(doc.Title + ".bin", FileMode.Open))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                docOpen = (Docuement)formatter.Deserialize(s);
            }

            /**
            * password and window point shouldn't be serialized.
            */
        }
    }
}

namespace MyNote.Dynamic
{
    class Class1
    {
        public void F1()
        {
            dynamic data = "Hello!adadadada.";
            WriteLine(data);
            data = (double)data.Length;
            data = data * 3.5 * 2;
            if (data == 112)
            {
                WriteLine($"{ data }");
            }
            else
            {
                // can call uncertian member but get error in runtime if hava nothing.
                data.Hei();
            }


            XElement person = XElement.Parse(@"
                <Person>
                    <FirstName>First</FirstName>
                    <SecondName>Second</SecondName>
                </Person>
            ");

            WriteLine("{0}{1}",
                person.Descendants("FirstName").ToString(),
                person.Descendants("SecondName").ToString()
            );

            dynamic person2 = XElement.Parse(@"
                <Person>
                    <FirstName>First</FirstName>
                    <SecondName>Second</SecondName>
                </Person>
            ");

            WriteLine("{0}",
                person2.FirstName
            );
        }
    }

    class DynamicXml : System.Dynamic.DynamicObject
    {
        private XElement Element { get; set; }
        public DynamicXml(System.Xml.Linq.XElement element)
        {
            Element = element;
        }
        public static DynamicXml  Parse(string text)
        {
            return new DynamicXml(XElement.Parse(text));
        }

    }
}


