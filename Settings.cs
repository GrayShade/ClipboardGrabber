using System.ComponentModel.Composition;
using System.Drawing;
using Microsoft.Win32;

namespace ClipboardGrabber
{
	public interface ISettingRepository
	{
		T GetValue<T>(string name, T defaultValue);
		void SetValue<T>(string name, T value);
	}

	public interface IConfigurationManager
	{
		string Address { get; set; }
		int Port { get; set; }
		string Language { get; set; }

		Icon FavoriteIcon { get; set; }
	}

	[Export(typeof(ISettingRepository))]
	public class SettingRepository : ISettingRepository
	{
		private const string registryKey = @"Software\Pingabuse\ClipboardGrabber";

		public T GetValue<T>(string name, T defaultValue)
		{
			using (var key = Registry.CurrentUser.OpenSubKey(registryKey, true))
				if (key != null)
				{
					return (T) key.GetValue(name, defaultValue);
				}
				else
					return defaultValue;
		}

		public void SetValue<T>(string name, T value)
		{
			using (var key = Registry.CurrentUser.CreateSubKey(registryKey))
				if (key != null)
					key.SetValue(name, value);
		}
	}

	[Export(typeof(IConfigurationManager))]
	public class ConfigurationManager : IConfigurationManager
	{
		[Import]
		private ISettingRepository settingRepository { get; set; }

		public string Address
		{
			get
			{
				return settingRepository.GetValue<string>("Address", null);
			}
			set
			{
				settingRepository.SetValue("Address", value);
			}
		}

		public int Port
		{
			get
			{
				return settingRepository.GetValue("Port", defaultPort);
			}
			set
			{
				settingRepository.SetValue("Port", value);
			}
		}

		public string Language
		{
			get
			{
				return settingRepository.GetValue("Language", "Plain");
			}
			set
			{
				settingRepository.SetValue("Language", value);
			}
		}

		public Icon FavoriteIcon { get; set; }

		private const int defaultPort = 8888;
	}
}
