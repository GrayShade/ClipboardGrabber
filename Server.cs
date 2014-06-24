using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace ClipboardGrabber
{
	public interface IHandlerMetadata
	{
		string Pattern { get; }
	}

	[MetadataAttribute]
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
	public class ExportHandlerAttribute : ExportAttribute, IHandlerMetadata
	{
		public string Pattern { get; private set; }

		public ExportHandlerAttribute(string pattern)
			: base(typeof(HttpHandler))
		{
			Pattern = pattern;
		}
	}

	class RequestEventArgs : EventArgs
	{
		public HttpRequest Request;

		public RequestEventArgs(HttpRequest request)
		{
			Request = request;
		}
	}

	class ErrorEventArgs : EventArgs
	{
		public Exception Exception;

		public ErrorEventArgs(Exception exception)
		{
			Exception = exception;
		}
	}

	public interface IHistoryEntry
	{
		string Name { get; set; }

		void Send(HttpResponse response);
	}

	public class HtmlHistoryEntry : IHistoryEntry
	{
		public string FriendlyText { get; set; }
		public string Text { get; set; }
		public string Name { get; set; }

		public HtmlHistoryEntry(string friendlyText, string text)
		{
			FriendlyText = friendlyText;
			Text = text;
		}

		public void Send(HttpResponse response)
		{
			response.ContentType = "text/html";
			var bytes = Encoding.Default.GetBytes(Text);
			response.Stream.Write(bytes, 0, bytes.Length);
		}

		public override string ToString()
		{
			return Name;
		}
	}

	public class ImageHistoryEntry : IHistoryEntry
	{
		public Bitmap Bitmap { get; set; }
		public byte[] Data { get; set; }
		public string Name { get; set; }

		public ImageHistoryEntry(Bitmap bitmap)
		{
			Bitmap = bitmap;
			using (var ms = new MemoryStream())
			{
				Bitmap.Save(ms, ImageFormat.Png);
				Data = ms.ToArray();
			}
		}

		public void Send(HttpResponse response)
		{
			response.ContentType = "image/png";
			response.Stream.Write(Data, 0, Data.Length);
		}

		public override string ToString()
		{
			return Name;
		}
	}

	public interface IHistoryManager : IList<IHistoryEntry>
	{
	}

	[Export(typeof(IHistoryManager))]
	public class HistoryManager : IHistoryManager
	{
		private List<IHistoryEntry> entries;

		public HistoryManager()
		{
			entries = new List<IHistoryEntry>();
		}

		public int IndexOf(IHistoryEntry item)
		{
			return entries.IndexOf(item);
		}

		public void Insert(int index, IHistoryEntry item)
		{
			entries.Insert(index, item);
		}

		public void RemoveAt(int index)
		{
			entries.RemoveAt(index);
		}

		public IHistoryEntry this[int index]
		{
			get
			{
				return entries[index];
			}
			set
			{
				entries[index] = value;
			}
		}

		public void Add(IHistoryEntry item)
		{
			entries.Add(item);
		}

		public void Clear()
		{
			entries.Clear();
		}

		public bool Contains(IHistoryEntry item)
		{
			return entries.Contains(item);
		}

		public void CopyTo(IHistoryEntry[] array, int arrayIndex)
		{
			entries.CopyTo(array, arrayIndex);
		}

		public int Count
		{
			get
			{
				return entries.Count;
			}
		}

		public bool IsReadOnly
		{
			get
			{
				return ((ICollection<IHistoryEntry>) entries).IsReadOnly;
			}
		}

		public bool Remove(IHistoryEntry item)
		{
			return entries.Remove(item);
		}

		public IEnumerator<IHistoryEntry> GetEnumerator()
		{
			return entries.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable) entries).GetEnumerator();
		}
	}

	[Export]
	class Server
	{
		public event EventHandler<RequestEventArgs> Request;
		public event EventHandler<ErrorEventArgs> Error;

		[Import]
		private HttpServer httpServer { get; set; }
		
		[Import]
		private IHistoryManager historyManager { get; set; }
		
		[Import]
		private IConfigurationManager configurationManager { get; set; }

		[ImportMany(RequiredCreationPolicy = CreationPolicy.Shared)]
		private IEnumerable<Lazy<HttpHandler, IHandlerMetadata>> handlers { get; set; }

		private IHistoryEntry favIconEntry;

		public void Setup()
		{
			this.favIconEntry = new ImageHistoryEntry(configurationManager.FavoriteIcon.ToBitmap());

			httpServer.Setup(IPAddress.Any, configurationManager.Port);
			foreach (var handler in handlers)
				httpServer.Handle(handler.Metadata.Pattern, handler.Value);
		}

		public void Start()
		{
			httpServer.Start();
		}

		public void AddHistoryEntry(IHistoryEntry entry)
		{
			entry.Name = historyManager.Count.ToString();
			historyManager.Add(entry);
		}

		public string GetEntryPath(IHistoryEntry entry)
		{
			return "history/" + entry.Name;
		}

		protected virtual void OnRequest(RequestEventArgs e)
		{
			if (Request != null)
				Request(this, e);
		}

		protected virtual void OnError(ErrorEventArgs e)
		{
			if (Error != null)
				Error(this, e);
		}

		private void ServeHistoryEntry(HttpRequest request, HttpResponse response, IHistoryEntry entry)
		{
			OnRequest(new RequestEventArgs(request));

			entry.Send(response);
		}

		[ExportHandler("favicon.ico")]
		public void FavIconCallback(HttpRequest request, HttpResponse response, GroupCollection groups)
		{
			response.Server = "ClipboardGrabber";
			ServeHistoryEntry(request, response, favIconEntry);
		}

		[ExportHandler(@"/history/(?<index>\d+)")]
		private void RequestCallback(HttpRequest request, HttpResponse response, GroupCollection groups)
		{
			response.Server = "ClipboardGrabber";

			int index;
			if (Int32.TryParse(groups["index"].Value, out index) && historyManager.Count > index)
				ServeHistoryEntry(request, response, historyManager[index]);
			else
				HttpServer.NotFoundHandler(request, response);
		}

		[ExportHandler("")]
		private void NotFoundCallback(HttpRequest request, HttpResponse response, GroupCollection groups)
		{
			OnRequest(new RequestEventArgs(request));

			response.Server = "ClipboardGrabber";
			HttpServer.NotFoundHandler(request, response);
		}
	}
}
