using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Windows.Forms;
using System.Threading;
using System.Drawing;

namespace WF_Xtension
{
    public class InputManager
    {
        [Flags]
        enum KeyState : byte
        {
            IsPressed = 1 << 0,
            WasPressed = 1 << 1
        }

        const int KEYS_TRACKED = 256;

        const int SPECIAL_SHIFT_SHIFT = 0, SPECIAL_SHIFT_CTRL = 2, SPECIAL_SHIFT_ALT = 4;
        const int SPECIAL_WASPRESSED_MASK = 0x2A;
        const int SPECIAL_CURRENTLYPRESSED_MASK = 0x15;

        KeyState[] m_states;
        int m_specialKeyStates;//first 2 bits are shift, second 2 bits are ctrl, third 2 bits are alt

        int m_mouseX, m_mouseY, m_mouseWheel, m_mouseWheelDelta;
        MouseButtons m_isPressed, m_wasPressed;

        public InputManager(Form form)
        {
            if (form == null)
                throw new ArgumentNullException("form");

            form.KeyDown += new KeyEventHandler(OnKeyDown);
            form.KeyUp += new KeyEventHandler(OnKeyUp);
            form.MouseMove += new MouseEventHandler(OnMouseEvent);
            form.MouseClick += new MouseEventHandler(OnMouseEvent);
            form.MouseWheel += new MouseEventHandler(OnMouseEvent);

            m_states = new KeyState[KEYS_TRACKED];
        }

        void OnMouseEvent(object sender, MouseEventArgs e)
        {
            m_mouseX = e.X;
            m_mouseY = e.Y;

            m_isPressed |= e.Button;

            m_mouseWheel += e.Delta;
            m_mouseWheelDelta = e.Delta;
        }

        public void Update(float time)
        {
            m_specialKeyStates = UpdateKey(m_specialKeyStates, SPECIAL_WASPRESSED_MASK, SPECIAL_CURRENTLYPRESSED_MASK);

            for (int i = 0; i < m_states.Length; i++)
                m_states[i] = (KeyState)UpdateKey((int)m_states[i], (int)KeyState.WasPressed, (int)KeyState.IsPressed);

            m_wasPressed = m_isPressed;
            m_isPressed = MouseButtons.None;
            m_mouseWheelDelta = 0;
        }
        private int UpdateKey(int prevState, int wasPressedMask, int currentlyPressedMask)
        {
            int state = (prevState << 1) & wasPressedMask;
            state |= prevState & currentlyPressedMask;
            return state;
        }

        public bool IsKeyDown(Keys key)
        {
            int keyCode = (int)(key & Keys.KeyCode);
            if (keyCode < 0 || keyCode > KEYS_TRACKED)
                return false;

            bool satisfied = keyCode > 0 ? (m_states[keyCode - 1] & KeyState.IsPressed) == KeyState.IsPressed : true;
            if ((key & Keys.Shift) == Keys.Shift)
                satisfied = satisfied && ((m_specialKeyStates >> SPECIAL_SHIFT_SHIFT) | 0x1) == 0x1;
            if ((key & Keys.Control) == Keys.Control)
                satisfied = satisfied && ((m_specialKeyStates >> SPECIAL_SHIFT_CTRL) | 0x1) == 0x1;
            if ((key & Keys.Alt) == Keys.Alt)
                satisfied = satisfied && ((m_specialKeyStates >> SPECIAL_SHIFT_ALT) | 0x1) == 0x1;

            return satisfied;
        }

        public bool WasKeyDown(Keys key)
        {
            int keyCode = (int)(key & Keys.KeyCode);
            if (keyCode < 0 || keyCode > KEYS_TRACKED)
                return false;

            bool satisfied = keyCode > 0 ? (m_states[keyCode - 1] & KeyState.WasPressed) == KeyState.WasPressed : true;
            if ((key & Keys.Shift) == Keys.Shift)
                satisfied = satisfied && ((m_specialKeyStates >> SPECIAL_SHIFT_SHIFT) | 0x2) == 0x2;
            if ((key & Keys.Control) == Keys.Control)
                satisfied = satisfied && ((m_specialKeyStates >> SPECIAL_SHIFT_CTRL) | 0x2) == 0x2;
            if ((key & Keys.Alt) == Keys.Alt)
                satisfied = satisfied && ((m_specialKeyStates >> SPECIAL_SHIFT_ALT) | 0x2) == 0x2;

            return satisfied;
        }

        public bool IsKeyTriggered(Keys key)
        {
            int keyCode = (int)(key & Keys.KeyCode);
            if (keyCode < 0 || keyCode > KEYS_TRACKED)
                return false;

            bool satisfied = keyCode > 0 ? m_states[keyCode - 1] == KeyState.IsPressed : true;
            if ((key & Keys.Shift) == Keys.Shift)
                satisfied = satisfied && (m_specialKeyStates >> SPECIAL_SHIFT_SHIFT) == 0x1;
            if ((key & Keys.Control) == Keys.Control)
                satisfied = satisfied && (m_specialKeyStates >> SPECIAL_SHIFT_CTRL) == 0x1;
            if ((key & Keys.Alt) == Keys.Alt)
                satisfied = satisfied && (m_specialKeyStates >> SPECIAL_SHIFT_ALT) == 0x1;

            return satisfied;
        }

        public bool IsButtonPressed(MouseButtons button)
        {
            return (m_isPressed & button) == button;
        }

        public bool WasButtonPressed(MouseButtons button)
        {
            return (m_wasPressed & button) == button;
        }

        public bool IsButtonTriggered(MouseButtons button)
        {
            return IsButtonPressed(button) && !WasButtonPressed(button);
        }

        public int MouseX { get { return m_mouseX; } }

        public int MouseY { get { return m_mouseY; } }

        public int MouseWheel { get { return m_mouseWheel; } }

        public int MouseWheelDelta { get { return m_mouseWheelDelta; } }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            if ((e.KeyCode | Keys.Shift) == Keys.Shift)
                m_specialKeyStates |= (int)KeyState.IsPressed << SPECIAL_SHIFT_SHIFT;
            if ((e.KeyCode | Keys.Control) == Keys.Control)
                m_specialKeyStates |= (int)KeyState.IsPressed << SPECIAL_SHIFT_CTRL;
            if ((e.KeyCode | Keys.Alt) == Keys.Alt)
                m_specialKeyStates |= (int)KeyState.IsPressed << SPECIAL_SHIFT_ALT;

            int key = (int)e.KeyCode & (int)Keys.KeyCode;
            if (key > 0 && key <= KEYS_TRACKED)//not KeyCode.None
                m_states[key - 1] |= KeyState.IsPressed;
        }

        private void OnKeyUp(object sender, KeyEventArgs e)
        {
            if ((e.KeyCode | Keys.Shift) == Keys.Shift)
                m_specialKeyStates &= ~((int)KeyState.IsPressed << SPECIAL_SHIFT_SHIFT);
            if ((e.KeyCode | Keys.Control) == Keys.Control)
                m_specialKeyStates &= ~((int)KeyState.IsPressed << SPECIAL_SHIFT_CTRL);
            if ((e.KeyCode | Keys.Alt) == Keys.Alt)
                m_specialKeyStates &= ~((int)KeyState.IsPressed << SPECIAL_SHIFT_ALT);

            int key = (int)e.KeyCode & (int)Keys.KeyCode;
            if (key > 0 && key <= KEYS_TRACKED)//not KeyCode.None
                m_states[key - 1] &= ~KeyState.IsPressed;
        }
    }
}
