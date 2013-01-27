using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace WF_Xtension
{
    public static class AudioHelper
    {
        [DllImport("winmm.dll")]
        private static extern long mciSendString(string command, StringBuilder returnValue, int returnLength, IntPtr winHandle);

        const string LOAD_COMMAND_PREFIX = "Open \"", LOAD_COMMAND_POSTFIX = "\" alias ";
        /// <summary>
        /// Loads a media file from disk
        /// </summary>
        /// <param name="filename">Filepath of the media</param>
        /// <param name="media">Alias used to play the media</param>
        public static void LoadMedia(string filename, string media)
        {
            string command = string.Concat(LOAD_COMMAND_PREFIX, filename, LOAD_COMMAND_POSTFIX, media);
            mciSendString(command, null, 0, IntPtr.Zero);
        }

        const string CLOSE_COMMAND = "Close ";
        /// <summary>
        /// Releases media from memory
        /// </summary>
        /// <param name="media">Alias used to play the media</param>
        public static void UnloadMedia(string media)
        {
            string command = string.Concat(CLOSE_COMMAND, media);
            mciSendString(command, null, 0, IntPtr.Zero);
        }

        const string PLAY_COMMAND = "Play ";
        /// <summary>
        /// Plays an asset with the given alias
        /// </summary>
        /// <param name="media">Media to play. Any given alias cannot be played twice simultaneously</param>
        public static void PlayMedia(string media)
        {
            string command = string.Concat(PLAY_COMMAND, media);
            mciSendString(command, null, 0, IntPtr.Zero);
        }

        const string LOOP_COMMAND = " REPEAT";
        /// <summary>
        /// Plays an asset with the given alias
        /// </summary>
        /// <param name="media">Media to play. Any given alias cannot be played twice simultaneously</param>
        public static void PlayMediaLoop(string media)
        {
            string command = string.Concat(PLAY_COMMAND, media, LOOP_COMMAND);
            mciSendString(command, null, 0, IntPtr.Zero);
        }

        const string STOP_COMMAND = "Stop ";
        /// <summary>
        /// Stops playing an asset with the given alias
        /// </summary>
        /// <param name="media">Media to stop playing</param>
        public static void StopPlaying(string media)
        {
            string command = string.Concat(STOP_COMMAND, media);
            mciSendString(command, null, 0, IntPtr.Zero);
        }
    }
}
