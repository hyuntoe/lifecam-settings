using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

using Microsoft.Win32;

namespace CameraPrefsApp
{
	class Program
	{
		private static Configuration config;
		// 여기에 설정들과 값들 선언해버리고
		// args로 카메라 이름을 받고
		// device에서 카메라 이름이 없으면 종료
		// device에서 카메라 이름이 있으면 설정 실행

		static void Main(string[] args)
		{	// This is Main
			// read xml and write config var
			Console.WriteLine("\nReading CameraPrefs.xml...");

			String configPath = "CameraPrefs.xml";
			if (args.Length >= 1)
				configPath = args[0];

			config = loadAndParseConfig(configPath);

            // write VideoProcAmp and CameraControl with config var
            applyCameraSettings(config.Cameras);

			// I am not interested in this. only for MS Lifecam
			// delete1: so must delete
			//applyTrueColorSetting(config.TrueColorEnabled);
		}

		private static Configuration loadAndParseConfig(String pPath)
		{
			try
			{
				return Configuration.Deserialize(pPath);
			}
			catch (IOException pException)
			{
				TextWriter errorWriter = Console.Error;
				errorWriter.WriteLine("\n  ERROR: Config not found at \"" + pPath + "\"");
				return null;
			}
		}

		/* delete1: so must delete
		private static void applyTrueColorSetting(Boolean pValue)
		{
			Console.WriteLine("\nApplying TrueColor setting to registry...");
			RegistryKey hkcu = Registry.CurrentUser;
			hkcu = hkcu.OpenSubKey("Software\\Microsoft\\LifeCam", true);
			try
			{
				hkcu.SetValue("TrueColorOff", pValue ? 0 : 1);
			}
			catch (Exception pException)
			{
				Console.WriteLine("\n  ERROR: LifeCam entry not found. Is the LifeCam software installed?");
			}
		}
		*/

		private static void applyCameraSettings(CameraPrefs[] pCameraPrefsList)
		{
			foreach (CameraPrefs cameraPrefs in pCameraPrefsList)
			{
				try
				{
					LifeCamCamera camera = LifeCamCamera.CreateFromPrefs(cameraPrefs);
				}
				catch (Exception pException)
				{
					Console.WriteLine("\n  ERROR: " + pException.Message);
					continue;
				}

			}
		}

	}
}
