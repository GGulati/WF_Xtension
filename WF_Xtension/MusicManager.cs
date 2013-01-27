using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Media;
using WMPLib;
using System.Runtime.InteropServices;

namespace WF_Xtension
{
	/// <summary>
	/// Manages and plays music
	/// </summary>
	public sealed class MusicManager
	{
		#region StaticMembers
		/// <summary>
		/// Only instance of the MusicManager;
		/// Used to manage and play music for the application.
		/// </summary>
		public static MusicManager Instance = new MusicManager();
		#endregion

		#region Variables
		Dictionary<string, string> m_music;
		Dictionary<string, bool> m_isOneInstance, m_isLooping;
		Dictionary<string, WindowsMediaPlayer> m_playing;
		int m_volume = 100;
		#endregion

		public MusicManager()
		{
			m_music = new Dictionary<string, string>();
			m_isOneInstance = new Dictionary<string, bool>();
			m_isLooping = new Dictionary<string, bool>();

			m_playing = new Dictionary<string, WindowsMediaPlayer>();
		}

		#region PublicMethods
		/// <summary>
		/// Adds a sound asset to the MusicManager for later use
		/// </summary>
		/// <param name="name">Name of the asset</param>
		/// <param name="musicPath">Fiele path of the asset</param>
		/// <param name="oneInstance">Can the asset be played alongside itself?</param>
		/// <param name="loop">Should the asset loop after finishing?</param>
		public void AddMusic(string name, string musicPath, bool oneInstance = false, bool loop = false)
		{
			if (string.IsNullOrEmpty(name))
				throw new ArgumentNullException("name");
			if (string.IsNullOrEmpty(name))
				throw new ArgumentNullException("music");
			if (m_music.ContainsKey(name))
				throw new Exception("There is already a music resource named " + name + " in the MusicManager");

			m_music.Add(name, musicPath);
			m_isLooping.Add(name, loop);
			m_isOneInstance.Add(name, oneInstance);
		}

		/// <summary>
		/// Plays a sound asset from the MusicManager.
		/// </summary>
		/// <param name="name">Name of the asset (case sensitive)</param>
		/// <returns>WindowsMediaPlayer used to play the asset.
		/// Must be used to stop music if the music is not
		/// looping and there can be multiple instances of it.</returns>
		public WindowsMediaPlayer PlayMusic(string name)
		{
			if (string.IsNullOrEmpty(name))
				throw new ArgumentNullException("name");
			if (!m_music.ContainsKey(name))
				throw new Exception("There is not a music resource named " + name + " in the MusicManager");

			WindowsMediaPlayer player = new WindowsMediaPlayer();
			player.settings.volume = m_volume;
			player.URL = m_music[name];

			if (m_isOneInstance[name])
			{
				if (!m_playing.ContainsKey(name))
				{
					m_playing.Add(name, player);
					player.StatusChange += new _WMPOCXEvents_StatusChangeEventHandler(WMP_Loop);
				}
				else
					player = m_playing[name];
				player.controls.play();
			}
			else
			{
				int append = 0;
				while (m_playing.ContainsKey(name + append) &&
					(m_playing[name + append].status != "Finished" && m_playing[name + append].status != "Stopped"))
					append++;

				if (m_playing.ContainsKey(name + append))
					player = m_playing[name + append];
				else
					m_playing.Add(name + append, player);

				player.controls.play();
				if (m_isLooping[name])
					player.StatusChange += new _WMPOCXEvents_StatusChangeEventHandler(WMP_Loop);
			}

			return player;
		}

		/// <summary>
		/// Stops a single instance or looping piece of music while playing.
		/// If the music is not limited to a single instance, the first music found playing will be stopped.
		/// </summary>
		/// <param name="name">Name of the asset</param>
		public void StopMusic(string name)
		{
			if (string.IsNullOrEmpty(name))
				throw new ArgumentNullException("name");
			if (!m_music.ContainsKey(name))
				throw new Exception("There is not a music resource named " + name + " in the MusicManager");
			if (!m_playing.ContainsKey(name) && !m_playing.ContainsKey(name + "0") /* Instance based looping music */)
				throw new Exception("No music with that name is currently playing");

			if (m_playing.ContainsKey(name))
				m_playing[name].controls.stop();//it won't try to loop again because the status will be "stopped" not "finished"
			else
			{
				int append = 0;
				while (m_playing.ContainsKey(name + append) &&
					(m_playing[name + append].status != "Finished" && m_playing[name + append].status != "Stopped"))
					append++;

				if (m_playing.ContainsKey(name + append))
					m_playing[name + append].controls.stop();
			}
		}

		/// <summary>
		/// Sets the volume for each sound in this MusicManager
		/// </summary>
		/// <param name="volume">Volume, between 0 and 100, to set</param>
		public void SetVolume(int volume)
		{
			m_volume = (int)MathHelper.Clamp(volume, 0, 100);
			foreach (var track in m_playing)
			{
				string status = track.Value.status;
				track.Value.settings.volume = m_volume;
				track.Value.controls.play();
				if (status == "Stopped")
					track.Value.controls.stop();
			}
		}

		/// <summary>
		/// Searches the MusicManager for an asset
		/// </summary>
		/// <param name="name">Name of the asset</param>
		/// <returns>Whether or not the MusicManager contains the asset</returns>
		public bool HasMusic(string name)
		{
			return m_music.ContainsKey(name);
		}
		#endregion

		#region Accessors
		/// <summary>
		/// Gets the current volume of the MusicManager
		/// </summary>
		public int Volume { get { return m_volume; } }
		#endregion

		#region PrivateMethods
		private void WMP_Loop()
		{
			foreach (var player in m_playing)
			{
				if (player.Value.status == "Finished")
					player.Value.controls.playItem(player.Value.currentMedia);
			}
		}
		#endregion
	}
}
