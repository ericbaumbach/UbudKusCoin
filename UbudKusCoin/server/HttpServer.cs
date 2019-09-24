using System;
using System.IO;
using System.Net;
using System.Text;

class HTTPServer
{
    private HttpListener listener;
    public HTTPServer() { }
    public bool Start()
    {
        listener = new HttpListener();
        listener.Prefixes.Add("http://+:8090/");
        listener.Start();
        listener.BeginGetContext(new AsyncCallback(ListenerCallback), listener);
        return true;
    }
    private static void ListenerCallback(IAsyncResult result)
    {
        HttpListener listener = (HttpListener)result.AsyncState;
        listener.BeginGetContext(new AsyncCallback(ListenerCallback), listener);
        Console.WriteLine("New request.");

        HttpListenerContext context = listener.EndGetContext(result);
        HttpListenerRequest request = context.Request;
        HttpListenerResponse response = context.Response;

        byte[] page = Encoding.UTF8.GetBytes("Test");

        response.ContentLength64 = page.Length;
        Stream output = response.OutputStream;
        output.Write(page, 0, page.Length);
        output.Close();
    }
}