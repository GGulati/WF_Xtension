using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Windows.Forms;
using System.Threading;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace WF_Xtension
{
    /// <summary>
    /// Creates FlickerProofForm oriented towards creating games with Windows Forms
    /// </summary>
    public class GameForm : FlickerProofForm
    {
        #region GameLoop
        delegate void GameUpdate(double frames);
        delegate void GameDraw();

        static void GameLoop(object arg)
        {
            GameSettings settings = arg as GameSettings;
            if (settings == null)
                return;

            long lastTick = -1, lastDrawTick = -1, currentTick,
                incr = (long)((double)Stopwatch.Frequency / settings.FPS), incrDraw = (long)((double)Stopwatch.Frequency / settings.RenderFPS);

            lastTick = lastDrawTick = Stopwatch.GetTimestamp();

            while (true)
            {
                currentTick = Stopwatch.GetTimestamp();
                double timeDelta = (double)(currentTick - lastTick);
                if (timeDelta >= incr)
                {
                    if (settings.OnGameUpdate != null)
                        settings.OnGameUpdate(timeDelta * 1000.0 / Stopwatch.Frequency);
                    lastTick = currentTick;
                }

                if (currentTick - lastDrawTick >= incrDraw)
                {
                    if (settings.OnGameDraw != null)
                        settings.OnGameDraw();
                    lastDrawTick = currentTick;
                }
            }
        }

        class GameSettings
        {
            public GameSettings(double fps, double renderFPS, GameUpdate update, GameDraw draw)
            {
                FPS = fps;
                RenderFPS = renderFPS;
                OnGameDraw = draw;
                OnGameUpdate = update;
            }

            public double FPS { get; protected set; }
            public double RenderFPS { get; protected set; }

            public GameUpdate OnGameUpdate;
            public GameDraw OnGameDraw;
        }
        #endregion

        public delegate void PanelRegistrationChange(FlickerProofPanel panel);

        #region Variables
        GameSettings m_settings;
        Thread m_threadGameLoop;
        List<FlickerProofPanel> m_drawnPanels;
        #endregion

        /// <summary>
        /// Creates a form that can be used to simulate a game with an update and draw loop at 60 Hz and 30 Hz, respectively
        /// </summary>
        public GameForm() : this(60.0, 30.0)
        {
        }
        
        /// <summary>
        /// Creates a form that can be used to simulate a game with an update and draw loop
        /// </summary>
        /// <param name="fps">Amount of update per second</param>
        /// <param name="renderFPS">Amount of times the game is drawn per second</param>
        public GameForm(double fps, double renderFPS) : base()
        {
            if (fps > 1000 || fps <= 0)
                throw new ArgumentException("fps must be between 0 and 1000, non-inclusive");
            if (renderFPS > 1000 || renderFPS <= 0)
                throw new ArgumentException("renderFPS must be between 0 and 1000, non-inclusive");

            m_drawnPanels = new List<FlickerProofPanel>();

            FormClosing += (object sender, FormClosingEventArgs e) =>
            {
                m_threadGameLoop.Abort();
            };

            Load += (object sender, EventArgs e) =>
            {
                m_settings = new GameSettings(fps, renderFPS, Update, Draw);
                m_threadGameLoop = new Thread(GameLoop);
                m_threadGameLoop.Start(m_settings);
            };
        }

        #region ProtectedMembers
        /// <summary>
        /// Registers a panel to be drawn during the draw call
        /// </summary>
        /// <param name="panel">Panel to have drawn</param>
        /// <returns>True if the panel is successfully added, and false otherwise</returns>
        protected bool RegisterDrawnPanel(FlickerProofPanel panel)
        {
            if (panel == null)
                throw new ArgumentNullException("panel");

            lock (m_drawnPanels)
            {
                if (m_drawnPanels.Contains(panel))
                    return true;
                m_drawnPanels.Add(panel);
            }
            return true;
        }

        /// <summary>
        /// Unregisters a panel from being drawn during the draw call
        /// </summary>
        /// <param name="panel">Panel to have drawing cease</param>
        protected void UnregisterDrawnPanel(FlickerProofPanel panel)
        {
            if (panel == null)
                throw new ArgumentNullException("panel");

            lock (m_drawnPanels)
            {
                if (m_drawnPanels.Contains(panel))
                    m_drawnPanels.Remove(panel);
            }
        }

        /// <summary>
        /// Unregisters a panel from being drawn during the draw call
        /// </summary>
        /// <param name="index">Panel to have drawing cease</param>
        protected void UnregisterDrawnPanel(int index)
        {
            if (index >= 0 && index < m_drawnPanels.Count)
            {
                FlickerProofPanel panel = m_drawnPanels[index];
                m_drawnPanels.RemoveAt(index);
            }
        }

        /// <summary>
        /// Updates the game
        /// </summary>
        /// <param name="updateTime">Time, in milliseconds, since last update</param>
        protected virtual void Update(double updateTime)
        {
        }

        /// <summary>
        /// Renders the game.
        /// Remember to call base.Draw if overidden
        /// </summary>
        protected virtual void Draw()
        {
            lock (m_drawnPanels)
            {
                foreach (FlickerProofPanel panel in m_drawnPanels)
                    panel.Invalidate();
            }
        }
        #endregion

        #region Accessors
        /// <summary>
        /// Frames per second of the GameForm.
        /// One update call occurs each frame.
        /// Draw calls occur at 30 FPS
        /// </summary>
        public double FPS { get { return m_settings.FPS; } }
        #endregion
    }
}