using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using Newtonsoft.Json;

using System.Net;
using System.IO;


using SharpConnect.WebServers;
namespace SharpConnect
{


    //4.  
    [AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
    class HttpMethodAttribute : Attribute
    {
        public string AlternativeName { get; set; }
    }

    class AppHost
    {

        public string CurrentServerMsg = "";

        //test cross origin policy 1
        static CrossOriginPolicy crossOriginPolicy = new CrossOriginPolicy(AllowCrossOrigin.All, "*");
        static AppHost()
        {
            //eg.
            //stBuilder.Append("Access-Control-Allow-Methods: GET, POST\r\n");
            //stBuilder.Append("Access-Control-Allow-Headers: Content-Type\r\n");
            crossOriginPolicy.AllowHeaders = "Content-Type";
            crossOriginPolicy.AllowMethods = "GET, POST";
        }

        const string html = @"<html>
                <head>
                <script> 
                         
                        var wsUri=get_wsurl(); 
                        var websocket= null;
                        (function init(){
	  
		                        //command queue 
		                        websocket = new WebSocket(wsUri);
		                        websocket.onopen = function(evt) { 
			                        console.log('open');
			                        websocket.send('client: Hello!');
		                        };
		                        websocket.onclose = function(evt) { 
			                        console.log('close');
		                        };
		                        websocket.onmessage = function(evt) {  
			                        console.log(evt.data);
		                        };
		                        websocket.onerror = function(evt) {  
		                        };		
                         })();
                        function send_data(data){
	                            websocket.send(data);
                        } 
                        function get_wsurl(){
                               
                                if(window.location.protocol==""https:""){
                                    return  ""wss://localhost:8000"";
                                }else{
                                    return  ""ws://localhost:8080"";
                                }
                        }
                </script>                
                </head>
                <body>
                        hello-websocket
	                    <input type=""button"" id=""mytxt"" onclick=""send_data('hello')""></input>	
                </body>    
        </html>";


        const string php = @"<?php
        $decodedjson=json_decode($post,true);
        var_dump($decodedjson);

        ?>";



        Dictionary<string, MethodInfo> httpMethods = new Dictionary<string, MethodInfo>();
        Dictionary<string, MetAdapter> httpCategories = new Dictionary<string, MetAdapter>();

        public void RegisterModule(object moduleInstance)
        {
            Type tt = moduleInstance.GetType();
            foreach (MethodInfo m in tt.GetMethods())
            {
                string typename = tt.Name;
                //m.Name;
                int count = 0;
                foreach (Attribute httpAttr in m.GetCustomAttributes(typeof(HttpMethodAttribute)))
                {

                    HttpMethodAttribute httpMethodAttr = (HttpMethodAttribute)httpAttr;
                    MetAdapter metAdapter = new MetAdapter(moduleInstance, m);


                    if (count == 0)
                    {
                        //httpMethods.Add(" / " + m.Name.ToLower(), m);
                        //httpMethods.Add("/" + m.Name.ToLower(),r.methodName);
                        httpCategories.Add("/" + typename.ToLower() + "/" + metAdapter.metInfo.Name.ToLower(), metAdapter);
                    }
                    count++;
                    if (httpMethodAttr.AlternativeName != null)
                    {
                        //httpMethods.Add("/" + httpMethodAttr.AlternativeName.ToLower(), m);
                        httpCategories.Add("/" + typename.ToLower() + "/" + httpMethodAttr.AlternativeName.ToLower(), metAdapter);
                    }
                }
            }
        }

        public AppHost()
        {

        }

        public string saveJson()
        {
            string result = "";
            using (FileStream f_p = new FileStream("SavePanel.json", FileMode.Open))
            {
                using (StreamReader s_p = new StreamReader(f_p))
                {
                    string r_j = s_p.ReadToEnd();
                    result += r_j;
                    f_p.Close();
                }

            }
            using (FileStream f_p = new FileStream("History.json", FileMode.Open))
            {
                using (StreamReader s_p = new StreamReader(f_p))
                {
                    string r_j = s_p.ReadToEnd();
                    result += "|"+ r_j;
                    f_p.Close();
                }
            }
            using (FileStream f_p = new FileStream("ListCountHistory.json", FileMode.Open))
            {
                using (StreamReader s_p = new StreamReader(f_p))
                {
                    string r_j = s_p.ReadToEnd();
                    result += "|" + r_j;
                    f_p.Close();
                }
            }
            //WebClient client = new WebClient();
            //client.UploadString("http://localhost:8080/", result);
            return result;
        }

        public void HandleRequest(HttpRequest req, HttpResponse resp)
        {

            string req_url = req.Url.ToLower();
            switch (req_url)
            {


                case "/check_msg":
                    {
                        resp.End(CurrentServerMsg);
                    }
                    break;
                case "/savejson.json":
                    {
                        resp.End(saveJson());
                    }
                    break;
                case "/test.php":
                    {
                        resp.End("hello from php server!");
                    }
                    break;
                case "/":
                    {
                        resp.TransferEncoding = ResponseTransferEncoding.Chunked;
                        //resp.End(html);
                        resp.End("hello!" + (count++));
                    }
                    break;
                case "/websocket":
                    {
                        resp.ContentType = WebResponseContentType.TextHtml;
                        resp.End(html);
                    }
                    break;
                case "/version":
                    {
                        resp.End("1.0");
                    }
                    break;
                case "/cross":
                    {
                        resp.AllowCrossOriginPolicy = crossOriginPolicy;
                        resp.End("ok");
                    }
                    break;
                default:
                    {
                        if (httpCategories.TryGetValue(req_url, out MetAdapter met))
                        {
                            met.Invoke(req, resp);


                        }
                        else
                        {
                            resp.End("?");
                        }

                    }
                    break;
            }
        }

        int count = 0;
        public void HandleWebSocket(WebSocketRequest req, WebSocketResponse resp)
        {
            resp.Write("server:" + (count++));
        }
    }
}
