using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text.RegularExpressions;

namespace ClipboardGrabber
{
	[Export]
	public class HttpServer
	{
		public int Port { get; private set; }
		public string Server { get; set; }

		private List<KeyValuePair<Regex, HttpHandler>> handlers;
		private Socket listener;

		public void Setup(IPAddress ipAddress, int port)
		{
			Port = port;
			handlers = new List<KeyValuePair<Regex, HttpHandler>>();
			listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
			listener.Bind(new IPEndPoint(ipAddress, port));
		}

		public void Start()
		{
			try
			{
				listener.Listen(5000);
			}
			catch
			{
				System.Windows.Forms.MessageBox.Show("Cannot listen on port");
				return;
			}

			listener.BeginAccept(ProcessClient, null);
		}

		public void Handle(string pattern, HttpHandler handler)
		{
			handlers.Add(new KeyValuePair<Regex, HttpHandler>(new Regex(pattern, RegexOptions.Compiled), handler));
		}

		public static void NotFoundHandler(HttpRequest request, HttpResponse response)
		{
			response.StatusCode = HttpStatusCode.NotFound;
		}

		public static void BadRequestHandler(HttpRequest request, HttpResponse response)
		{
			response.StatusCode = HttpStatusCode.BadRequest;
		}

		private static bool ParseRequest(HttpRequest request)
		{
			var reader = new StreamReader(request.Stream);
			var line = reader.ReadLine();
			if (String.IsNullOrEmpty(line))
				return false;

			var parts = line.Split(' ');
			if (parts.Length != 3)
				return false;

			request.Method = parts[0];
			request.Address = new Uri(parts[1], UriKind.RelativeOrAbsolute);
			switch (parts[2])
			{
				case "HTTP/1.0":
					request.ProtocolVersion = HttpVersion.Version10;
					break;
				case "HTTP/1.1":
					request.ProtocolVersion = HttpVersion.Version11;
					break;
				default:
					return false;
			}

			while (!String.IsNullOrEmpty(line = reader.ReadLine()))
			{
				var pos = line.IndexOf(':');
				if (pos < 0)
					return false;

				request.Headers[line.Substring(0, pos)] = line.Substring(pos + 2);
			}

			return true;
		}

		private void GetResponse(HttpRequest request, HttpResponse response)
		{
			if (!ParseRequest(request))
			{
				BadRequestHandler(request, response);
				return;
			}

			foreach (var pair in handlers)
			{
				var match = pair.Key.Match(request.Address.OriginalString);
				if (match.Success)
				{
					pair.Value(request, response, match.Groups);
					return;
				}
			}

			NotFoundHandler(request, response);
		}

		private void ProcessClient(IAsyncResult iar)
		{
			try
			{
				var client = listener.EndAccept(iar);
				listener.BeginAccept(ProcessClient, null);

				var stream = new NetworkStream(client, true);
				var response = new HttpResponse(stream);
				GetResponse(new HttpRequest(client.RemoteEndPoint, stream), response);
				response.Send();
			}
			catch (Exception)
			{
			}
		}
	}

	public class HttpRequest
	{
		public String Method { get; set; }
		public Version ProtocolVersion { get; set; }
		public Uri Address { get; set; }
		public String Host { get { return Headers.GetValueOrDefault("Host", null); } }
		public String Accept { get { return Headers.GetValueOrDefault("Accept", null); } }
		public String Connection { get { return Headers.GetValueOrDefault("Connection", null); } }
		public String UserAgent { get { return Headers.GetValueOrDefault("User-Agent", null); } }

		public Dictionary<string, string> Headers;
		public EndPoint RemoteEndPoint;
		public Stream Stream;

		public HttpRequest(EndPoint remoteEndPoint, Stream stream)
		{
			RemoteEndPoint = remoteEndPoint;
			Stream = stream;

			Headers = new Dictionary<string, string>();
		}

		public HttpResponse GetResponse()
		{
			return new HttpResponse(Stream);
		}
	}

	public class HttpResponse
	{
		public Version ProtocolVersion { get; set; }
		public HttpStatusCode StatusCode { get; set; }
		public Dictionary<string, string> Headers;
		public String Location { set { Headers["Location"] = value; } }
		public String ContentType { set { Headers["Content-Type"] = value; } }
		public String Server { set { Headers["Server"] = value; } }
		public long ContentLength { set { Headers["Content-Length"] = value.ToString(); } }
		public MemoryStream Stream;

		private Stream networkStream;
		private StreamWriter streamWriter;

		public HttpResponse(Stream networkStream)
		{
			this.networkStream = networkStream;
			streamWriter = new StreamWriter(networkStream);
			Stream = new MemoryStream();
			Headers = new Dictionary<string, string>();

			ProtocolVersion = HttpVersion.Version10;
			StatusCode = HttpStatusCode.OK;
		}

		public void Send()
		{
			streamWriter.WriteLine("HTTP/{0} {1} {2}", ProtocolVersion, (int) StatusCode, StatusCode);

			var buffer = Stream.ToArray();
			ContentLength = buffer.Length;
			foreach (var header in Headers)
				streamWriter.WriteLine("{0}: {1}", header.Key, header.Value);
			streamWriter.WriteLine();
			streamWriter.Flush();

			try
			{
				networkStream.BeginWrite(buffer, 0, buffer.Length, iar =>
				{
					networkStream.EndWrite(iar);

					streamWriter.Dispose();
					Stream.Dispose();
				}, null);
			}
			catch (Exception)
			{
			}
		}
	}

	public delegate void HttpHandler(HttpRequest request, HttpResponse response, GroupCollection groups);
}
