using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SharpConnect.WebServers;
using System.Reflection;
using System.Net.Http;


namespace WindowsFormsApp1
{
    class Loc_point
    {
        public Point locat = new Point();

        //..


    }
}

namespace SharpConnect
{
    enum ConvPlan
    {
        Unknown,
        ToString,
        ToDouble,
        ToInt32,
        ToFloat,
        ToShort
    }
    class ParameterAdapter
    {
        public string ParName;
        public Type ParType;
        public ConvPlan convPlan;

        public ParameterAdapter(string parName, Type parType)
        {
            ParName = parName;
            ParType = parType;
            //convert from string to target type
            switch (ParType.FullName)
            {
                default:
                    break;
                case "System.Int32":
                    convPlan = ConvPlan.ToInt32;
                    break;
                case "Systm.String":
                    convPlan = ConvPlan.ToString;
                    break;
                case "System.Double":
                    convPlan = ConvPlan.ToDouble;
                    break;
            }
        }
        public object GetActualValue(string strValue)
        {
            try
            {
                switch (convPlan)
                {
                    default:
                        return null;
                    case ConvPlan.ToDouble:
                        return double.Parse(strValue);
                    case ConvPlan.ToFloat:
                        return float.Parse(strValue);
                    case ConvPlan.ToInt32:
                        return int.Parse(strValue);
                }
            }
            catch { return null; }
        }
        public override string ToString()
        {
            return ParType + " " + ParName;
        }
    }
    class MetAdapter
    {
        public Object moduleInstance;
        public MethodInfo metInfo;
        bool _conservativeForm;

        ParameterAdapter[] parAdapters;
        public MetAdapter(Object moduleInstance, MethodInfo metInfo)
        {
            this.moduleInstance = moduleInstance;
            this.metInfo = metInfo;

            //conservative form
            ParameterInfo[] pars = metInfo.GetParameters();
            if (pars.Length == 2 &&
                pars[0].ParameterType == typeof(HttpRequest) &&
                pars[1].ParameterType == typeof(HttpResponse))
            {
                _conservativeForm = true;
            }
            else
            {
                parAdapters = new ParameterAdapter[pars.Length];

                int i = 0;
                foreach (ParameterInfo p in pars)
                {
                    ParameterAdapter parAdapter = new ParameterAdapter(p.Name, p.ParameterType);
                    parAdapters[i] = parAdapter;
                    i++;
                }
                _conservativeForm = false;
            }
        }
        object[] PrepareInputArgs(HttpRequest req)
        {

            int j = parAdapters.Length;
            object[] pars = new object[j];
            for (int i = 0; i < j; ++i)
            {
                ParameterAdapter a = parAdapters[i];
                string inputValue = req.GetReqParameterValue(a.ParName);
                pars[i] = a.GetActualValue(inputValue);
                if (pars[i] == null) { return null; }
            }
            return pars;
        }

        public void Invoke(HttpRequest req, HttpResponse resp)
        {
            if (_conservativeForm)
            {
                this.metInfo.Invoke(
                    moduleInstance, new object[] { req, resp }
                );
            }
            else
            {
                try
                {
                    //....prepare input 
                    object result = metInfo.Invoke(moduleInstance, PrepareInputArgs(req));

                    //prepare result
                    resp.End(result.ToString());
                }
                catch { resp.End("Error"); }
            }
        }
    }
    //3.
    class MyModule
    {
        //4.
        [HttpMethod]
        public void Go(HttpRequest req, HttpResponse resp)
        {
            resp.End("go!");
        }
        //4.
        [HttpMethod(AlternativeName = "mysay")]
        [HttpMethod(AlternativeName = "mysay1")]
        public void Say(HttpRequest req, HttpResponse resp)
        {
            resp.End("say!12345");
        }
    }
    class MyModule2
    {
        //4.
        [HttpMethod]
        public void Walk(HttpRequest req, HttpResponse resp)
        {
            resp.End("walk!");
        }
        //4.
        [HttpMethod(AlternativeName = "mysay")]
        [HttpMethod(AlternativeName = "mysay1")]
        public void Fly(HttpRequest req, HttpResponse resp)
        {
            resp.End("fly");
        }
    }

    class MyModule3
    {
        //4.
        [HttpMethod]
        public void Go1(HttpRequest req, HttpResponse resp)
        {
            resp.End("go1!");
        }
        //4.
        [HttpMethod(AlternativeName = "mysay")]
        [HttpMethod(AlternativeName = "mysay1")]
        public void Say1(HttpRequest req, HttpResponse resp)
        {
            resp.End("say1!7890");
        }
    }
    //5. 
    class MyAdvanceMathModule
    {
        [HttpMethod]
        public void CalculateSomething1(HttpRequest req, HttpResponse resp)
        {
            string s1_s = req.GetReqParameterValue("s1");
            string s2_s = req.GetReqParameterValue("s2");
            //..... 
            double result = CalculateSomething(double.Parse(s1_s), double.Parse(s2_s));


            //.....
            resp.End(result.ToString());
        }

        [HttpMethod]
        public double CalculateSomething(double s1, double s2)
        {
            return s1 + s2;
        }

        [HttpMethod]
        public double CalculateX(double s1, double s2)
        {
            return s1 * s2;
        }
    }
    class MMath1
    {

        [HttpMethod]
        public double CalculateSomething(double s1, double s2)
        {
            return s1 + s2;
        }

        [HttpMethod]
        public double CalculateX(double s1, double s2)
        {
            return s1 * s2;
        }
    }
}
