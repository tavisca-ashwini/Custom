using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Threading;
using System.IO;

namespace CustomAttribute
{
    [AttributeUsage(AttributeTargets.All)]

    public class TestLogAttribute : System.Attribute
    {
        private String developer;
        private String lastCheck;
        private int errorNumber;
        public String displayMessage;
        public String timeStamp;
        public String CurrentUser;
        

        public TestLogAttribute(int errNum, String dev, String lstChk)
        {
            this.errorNumber = errNum;
            this.developer = dev;
            this.lastCheck = lstChk;
            timeStamp = Convert.ToString(DateTime.Now);
            this.CurrentUser = (System.Security.Principal.WindowsIdentity.GetCurrent().Name);
        }

        public String User
        {
            get
            {
                return this.CurrentUser;
            }
        }

        public int ErrNo
        {
            get
            {
                return errorNumber;
            }
        }
        public string Developer
        {
            get
            {
                return developer;
            }
        }
        public string LastCheck
        {
            get
            {
                return lastCheck;
            }
        }
        public string Message
        {
            get
            {
                return displayMessage;
            }
            set
            {
                displayMessage = value;
            }
        }
    }
    [TestLogAttribute(1, "Developer_1", "05/07/2015", Message = "Circle is instantiated")]

    class Circle
    {
        protected double radius;
        protected double pi;
        
        public Circle(double r, double p)
        {
            radius = r;
            pi = p;
        }

        [TestLogAttribute(2, "Developer_2", "05/07/2015", Message = "GetCircumference is instantiated")]

        public double GetCircumference()
        {
            return 2 * pi * radius;
        }

        [TestLogAttribute(3, "Developer_3", "05/07/2015")]

        public void Display()
        {
            Console.WriteLine("Radius : {0}", radius);
            Console.WriteLine("Circumference :{0}", GetCircumference());
        }
    }

    class ExecuteCircle
    {
        static void Main(string[] args)
        {
            Circle circle = new Circle(2, 3.14);
            circle.Display();
            Type type = typeof(Circle);

            foreach (Object attributes in type.GetCustomAttributes(false))
            {
                TestLogAttribute logAttr = (TestLogAttribute)attributes;
                if (null != logAttr)
                {
                    Console.WriteLine(" " + logAttr.Message);
                    Console.WriteLine("Accessed Time {0}:", logAttr.timeStamp);
                    Console.WriteLine("Accessed By: " + logAttr.CurrentUser);
                    Console.WriteLine("");
                }
            }

            for (int i = 0; i < 5; i++)
            {
                circle.Display();
                Thread.Sleep(1000);
                foreach (MethodInfo method in type.GetMethods())
                {
                    foreach (Attribute attrib in method.GetCustomAttributes(true))
                    {
                        TestLogAttribute logAttr = (attrib as TestLogAttribute);
                        if (null != logAttr)
                        {
                            Console.WriteLine(" " + logAttr.Message);
                            Console.WriteLine("Accessed Time {0}:", logAttr.timeStamp);
                            Console.WriteLine("Accessed By: " + logAttr.CurrentUser);
                            Console.WriteLine("");

                            using (StreamWriter sw = File.AppendText("logFile.txt"))
                            {
                                sw.Write(" " + logAttr.Message);
                                sw.Write("Accessed Time {0}:", logAttr.timeStamp);
                                sw.Write("Accessed By: " + logAttr.CurrentUser);
                            }
                        }
                    }
                }
            }
            Console.ReadLine();
        }
    }
}


    
