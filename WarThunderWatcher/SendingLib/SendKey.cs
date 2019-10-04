using System;
using System.Runtime.InteropServices;
using System.Threading;

namespace SendKeys
{
    static public class SendKey
    { 
        [DllImport("user32.dll", SetLastError = true)]
        static extern uint SendInput(uint nInputs, ref INPUT pInputs, int cbSize);

        [DllImport("user32.dll")]
        static extern bool SetCursorPos(int X, int Y);


        [StructLayout(LayoutKind.Sequential)]
        private struct KEYBDINPUT
        {
            public short wVk;
            public short wScan;
            public int dwFlags;
            public int time;
            public IntPtr dwExtraInfo;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct HARDWAREINPUT
        {
            public int uMsg;
            public short wParamL;
            public short wParamH;
        }

        [Flags]
        private enum InputType
        {
            INPUT_MOUSE = 0,
            INPUT_KEYBOARD = 1,
            INPUT_HARDWARE = 2
        }

        [Flags]
        private enum KEYEVENTF
        {
            KEYDOWN = 0,
            EXTENDEDKEY = 0x0001,
            KEYUP = 0x0002,
            UNICODE = 0x0004,
            SCANCODE = 0x0008
        }

        [StructLayout(LayoutKind.Sequential)]
        struct INPUT
        {
            public SendInputEventType type;
            public MouseKeybdhardwareInputUnion mkhi;
        }

        [StructLayout(LayoutKind.Explicit)]
        struct MouseKeybdhardwareInputUnion
        {
            [FieldOffset(0)]
            public MouseInputData mi;

            [FieldOffset(0)]
            public KEYBDINPUT ki;

            [FieldOffset(0)]
            public HARDWAREINPUT hi;
        }

        //[StructLayout(LayoutKind.Sequential)]
        //struct HARDWAREINPUT
        //{
        //    public int uMsg;
        //    public short wParamL;
        //    public short wParamH;
        //}

        struct MouseInputData
        {
            public int dx;
            public int dy;
            public uint mouseData;
            public MouseEventFlags dwFlags;
            public uint time;
            public IntPtr dwExtraInfo;
        }

        [Flags]
        enum MouseEventFlags : uint
        {
            MOUSEEVENTF_MOVE = 0x0001,
            MOUSEEVENTF_LEFTDOWN = 0x0002,
            MOUSEEVENTF_LEFTUP = 0x0004,
            MOUSEEVENTF_RIGHTDOWN = 0x0008,
            MOUSEEVENTF_RIGHTUP = 0x0010,
            MOUSEEVENTF_MIDDLEDOWN = 0x0020,
            MOUSEEVENTF_MIDDLEUP = 0x0040,
            MOUSEEVENTF_XDOWN = 0x0080,
            MOUSEEVENTF_XUP = 0x0100,
            MOUSEEVENTF_WHEEL = 0x0800,
            MOUSEEVENTF_VIRTUALDESK = 0x4000,
            MOUSEEVENTF_ABSOLUTE = 0x8000
        }

        enum SendInputEventType
        {
            InputMouse,
            InputKeyboard,
            InputHardware
        }


        [DllImport("user32.dll", SetLastError = true)]
        private static extern uint SendInput(uint nInputs, INPUT[] pInputs, int cbSize);

        [DllImport("user32.DLL")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);


        enum SystemMetric
        {
            SM_CXSCREEN = 0,
            SM_CYSCREEN = 1
        }

        [DllImport("user32.dll")]
        static extern int GetSystemMetrics(SystemMetric smIndex);

        static int CalculateAbsoluteCoordinateX(int x)
        {
            return (x * 65536) / GetSystemMetrics(SystemMetric.SM_CXSCREEN);
        }

        static int CalculateAbsoluteCoordinateY(int y)
        {
            return (y * 65536) / GetSystemMetrics(SystemMetric.SM_CYSCREEN);
        }

        public static void SendMouseClick(string ProcessName, int x, int y)
        {
            IntPtr hwnd = FindWindow(null, ProcessName);
            SetForegroundWindow(hwnd);
            Thread.Sleep(20);

            INPUT[] inputs = new INPUT[1];
            INPUT mouseInput = new INPUT();
            mouseInput.type = SendInputEventType.InputMouse;
            mouseInput.mkhi.mi.dx = CalculateAbsoluteCoordinateX(x);
            mouseInput.mkhi.mi.dy = CalculateAbsoluteCoordinateY(y);
            mouseInput.mkhi.mi.mouseData = 0;

            Thread.Sleep(100);
            mouseInput.mkhi.mi.dwFlags = MouseEventFlags.MOUSEEVENTF_MOVE | MouseEventFlags.MOUSEEVENTF_ABSOLUTE;
            SendInput(1, ref mouseInput, Marshal.SizeOf(new INPUT()));
            Thread.Sleep(100);
            mouseInput.mkhi.mi.dwFlags = MouseEventFlags.MOUSEEVENTF_LEFTDOWN;
            SendInput(1, ref mouseInput, Marshal.SizeOf(new INPUT()));
            Thread.Sleep(100);
            mouseInput.mkhi.mi.dwFlags = MouseEventFlags.MOUSEEVENTF_LEFTUP;
            SendInput(1, ref mouseInput, Marshal.SizeOf(new INPUT()));
        }

        public static void SendMouseRightClick(string ProcessName, int x, int y)
        {
            IntPtr hwnd = FindWindow(null, ProcessName);
            SetForegroundWindow(hwnd);
            Thread.Sleep(20);

            INPUT[] inputs = new INPUT[1];
            INPUT mouseInput = new INPUT();
            mouseInput.type = SendInputEventType.InputMouse;
            mouseInput.mkhi.mi.dx = CalculateAbsoluteCoordinateX(x);
            mouseInput.mkhi.mi.dy = CalculateAbsoluteCoordinateY(y);
            mouseInput.mkhi.mi.mouseData = 0;

            Thread.Sleep(100);
            mouseInput.mkhi.mi.dwFlags = MouseEventFlags.MOUSEEVENTF_MOVE | MouseEventFlags.MOUSEEVENTF_ABSOLUTE;
            SendInput(1, ref mouseInput, Marshal.SizeOf(new INPUT()));
            Thread.Sleep(100);
            mouseInput.mkhi.mi.dwFlags = MouseEventFlags.MOUSEEVENTF_RIGHTDOWN;
            SendInput(1, ref mouseInput, Marshal.SizeOf(new INPUT()));
            Thread.Sleep(100);
            mouseInput.mkhi.mi.dwFlags = MouseEventFlags.MOUSEEVENTF_RIGHTUP;
            SendInput(1, ref mouseInput, Marshal.SizeOf(new INPUT()));
        }

        public static void SendMouseClick(IntPtr hwnd, int x, int y)
        {
            SetForegroundWindow(hwnd);
            Thread.Sleep(20);

            INPUT[] inputs = new INPUT[1];
            INPUT mouseInput = new INPUT();
            mouseInput.type = SendInputEventType.InputMouse;
            mouseInput.mkhi.mi.dx = CalculateAbsoluteCoordinateX(x);
            mouseInput.mkhi.mi.dy = CalculateAbsoluteCoordinateY(y);
            mouseInput.mkhi.mi.mouseData = 0;

            Thread.Sleep(100);
            mouseInput.mkhi.mi.dwFlags = MouseEventFlags.MOUSEEVENTF_MOVE | MouseEventFlags.MOUSEEVENTF_ABSOLUTE;
            SendInput(1, ref mouseInput, Marshal.SizeOf(new INPUT()));
            Thread.Sleep(100);
            mouseInput.mkhi.mi.dwFlags = MouseEventFlags.MOUSEEVENTF_LEFTDOWN;
            SendInput(1, ref mouseInput, Marshal.SizeOf(new INPUT()));
            Thread.Sleep(100);
            mouseInput.mkhi.mi.dwFlags = MouseEventFlags.MOUSEEVENTF_LEFTUP;
            SendInput(1, ref mouseInput, Marshal.SizeOf(new INPUT()));
        }

        public static void SendMouseRightClick(IntPtr hwnd, int x, int y)
        {
            SetForegroundWindow(hwnd);
            Thread.Sleep(20);

            INPUT[] inputs = new INPUT[1];
            INPUT mouseInput = new INPUT();
            mouseInput.type = SendInputEventType.InputMouse;
            mouseInput.mkhi.mi.dx = CalculateAbsoluteCoordinateX(x);
            mouseInput.mkhi.mi.dy = CalculateAbsoluteCoordinateY(y);
            mouseInput.mkhi.mi.mouseData = 0;

            Thread.Sleep(100);
            mouseInput.mkhi.mi.dwFlags = MouseEventFlags.MOUSEEVENTF_MOVE | MouseEventFlags.MOUSEEVENTF_ABSOLUTE;
            SendInput(1, ref mouseInput, Marshal.SizeOf(new INPUT()));
            Thread.Sleep(100);
            mouseInput.mkhi.mi.dwFlags = MouseEventFlags.MOUSEEVENTF_RIGHTDOWN;
            SendInput(1, ref mouseInput, Marshal.SizeOf(new INPUT()));
            Thread.Sleep(100);
            mouseInput.mkhi.mi.dwFlags = MouseEventFlags.MOUSEEVENTF_RIGHTUP;
            SendInput(1, ref mouseInput, Marshal.SizeOf(new INPUT()));
        }

        public static void MoveMouse(string ProcessName, int x, int y)
        {
            IntPtr hwnd = FindWindow(null, ProcessName);
            SetForegroundWindow(hwnd);
            Thread.Sleep(20);

            INPUT[] inputs = new INPUT[1];
            INPUT mouseInput = new INPUT();
            mouseInput.type = SendInputEventType.InputMouse;
            mouseInput.mkhi.mi.dx = CalculateAbsoluteCoordinateX(x);
            mouseInput.mkhi.mi.dy = CalculateAbsoluteCoordinateY(y);
            mouseInput.mkhi.mi.mouseData = 0;

            Thread.Sleep(100);
            mouseInput.mkhi.mi.dwFlags = MouseEventFlags.MOUSEEVENTF_MOVE | MouseEventFlags.MOUSEEVENTF_ABSOLUTE;
            SendInput(1, ref mouseInput, Marshal.SizeOf(new INPUT()));
        }

        public static void SendMouseDoubleClick(string ProcessName, int x, int y)
        {
            IntPtr hwnd = FindWindow(null, ProcessName);
            SetForegroundWindow(hwnd);
            Thread.Sleep(200);

            INPUT[] inputs = new INPUT[1];
            INPUT mouseInput = new INPUT();
            mouseInput.type = SendInputEventType.InputMouse;
            mouseInput.mkhi.mi.dx = CalculateAbsoluteCoordinateX(x);
            mouseInput.mkhi.mi.dy = CalculateAbsoluteCoordinateY(y);
            mouseInput.mkhi.mi.mouseData = 0;


            mouseInput.mkhi.mi.dwFlags = MouseEventFlags.MOUSEEVENTF_MOVE | MouseEventFlags.MOUSEEVENTF_ABSOLUTE;
            SendInput(1, ref mouseInput, Marshal.SizeOf(new INPUT()));
            Thread.Sleep(200);
            mouseInput.mkhi.mi.dwFlags = MouseEventFlags.MOUSEEVENTF_LEFTDOWN;
            SendInput(1, ref mouseInput, Marshal.SizeOf(new INPUT()));
            Thread.Sleep(50);
            mouseInput.mkhi.mi.dwFlags = MouseEventFlags.MOUSEEVENTF_LEFTUP;
            SendInput(1, ref mouseInput, Marshal.SizeOf(new INPUT()));
            Thread.Sleep(50);
            mouseInput.mkhi.mi.dwFlags = MouseEventFlags.MOUSEEVENTF_LEFTDOWN;
            SendInput(1, ref mouseInput, Marshal.SizeOf(new INPUT()));
            Thread.Sleep(50);
            mouseInput.mkhi.mi.dwFlags = MouseEventFlags.MOUSEEVENTF_LEFTUP;
            SendInput(1, ref mouseInput, Marshal.SizeOf(new INPUT()));
        }

        public static void SendKeyAsInput(string ProcessName, short KeyScan, short KeyCode)
        {
            IntPtr hwnd = FindWindow(null, ProcessName);
            SetForegroundWindow(hwnd);
            Thread.Sleep(200);

            INPUT[] inputs = new INPUT[1];
            inputs[0].type = SendInputEventType.InputKeyboard;
            inputs[0].mkhi.ki.wScan = KeyScan;
            inputs[0].mkhi.ki.dwFlags = (int)KEYEVENTF.KEYDOWN;
            inputs[0].mkhi.ki.wVk = KeyCode;
            uint intReturn = SendInput(1, inputs, Marshal.SizeOf(inputs[0]));

            Thread.Sleep(100);

            inputs = new INPUT[1];
            inputs[0].type = SendInputEventType.InputKeyboard;
            inputs[0].mkhi.ki.wScan = KeyScan;
            inputs[0].mkhi.ki.dwFlags = (int)KEYEVENTF.KEYUP;
            inputs[0].mkhi.ki.wVk = KeyCode;
            intReturn = SendInput(1, inputs, Marshal.SizeOf(inputs[0]));
        }

        public static void SendKeyAsInput(IntPtr hwnd, short KeyScan, short KeyCode)
        {
            SetForegroundWindow(hwnd);
            Thread.Sleep(200);

            INPUT[] inputs = new INPUT[1];
            inputs[0].type = SendInputEventType.InputKeyboard;
            inputs[0].mkhi.ki.wScan = KeyScan;
            inputs[0].mkhi.ki.dwFlags = (int)KEYEVENTF.KEYDOWN;
            inputs[0].mkhi.ki.wVk = KeyCode;
            uint intReturn = SendInput(1, inputs, Marshal.SizeOf(inputs[0]));

            Thread.Sleep(100);

            inputs = new INPUT[1];
            inputs[0].type = SendInputEventType.InputKeyboard;
            inputs[0].mkhi.ki.wScan = KeyScan;
            inputs[0].mkhi.ki.dwFlags = (int)KEYEVENTF.KEYUP;
            inputs[0].mkhi.ki.wVk = KeyCode;
            intReturn = SendInput(1, inputs, Marshal.SizeOf(inputs[0]));
        }

		public static void SendKeyAsInputDelay(string ProcessName, short[] KeyScan, int msDelay)
		{
			IntPtr hwnd = FindWindow(null, ProcessName);
			SetForegroundWindow(hwnd);

			Thread.Sleep(200);
			INPUT[] inputs = new INPUT[1];
			uint intReturn = new uint();

			foreach (var key in KeyScan)
			{
				inputs[0].type = SendInputEventType.InputKeyboard;
				inputs[0].mkhi.ki.wScan = key;
				inputs[0].mkhi.ki.dwFlags = (int)KEYEVENTF.KEYDOWN;
				inputs[0].mkhi.ki.wVk = key;
				intReturn = SendInput(1, inputs, Marshal.SizeOf(inputs[0]));
				Thread.Sleep(100);
			}
			Thread.Sleep(msDelay);
			foreach (var key in KeyScan)
			{
				inputs = new INPUT[1];
				inputs[0].type = SendInputEventType.InputKeyboard;
				inputs[0].mkhi.ki.wScan = key;
				inputs[0].mkhi.ki.dwFlags = (int)KEYEVENTF.KEYUP;
				inputs[0].mkhi.ki.wVk = key;
				intReturn = SendInput(1, inputs, Marshal.SizeOf(inputs[0]));
				Thread.Sleep(100);
			}
		}


		public static void SendKeyAsInput(string ProcessName, short[] KeyScan)
        {
            IntPtr hwnd = FindWindow(null, ProcessName);
            SetForegroundWindow(hwnd);

            Thread.Sleep(200);
            INPUT[] inputs = new INPUT[1];
            uint intReturn = new uint();

            foreach (var key in KeyScan)
            {
                inputs[0].type = SendInputEventType.InputKeyboard;
                inputs[0].mkhi.ki.wScan = key;
                inputs[0].mkhi.ki.dwFlags = (int)KEYEVENTF.KEYDOWN;
                inputs[0].mkhi.ki.wVk = key;
                intReturn = SendInput(1, inputs, Marshal.SizeOf(inputs[0]));
                Thread.Sleep(100);
            }

            foreach (var key in KeyScan)
            {
                inputs = new INPUT[1];
                inputs[0].type = SendInputEventType.InputKeyboard;
                inputs[0].mkhi.ki.wScan = key;
                inputs[0].mkhi.ki.dwFlags = (int)KEYEVENTF.KEYUP;
                inputs[0].mkhi.ki.wVk = key;
                intReturn = SendInput(1, inputs, Marshal.SizeOf(inputs[0]));
                Thread.Sleep(100);
            }
        }

        public static void SendKeyAsVirtInput(string ProcessName, short[] KeyScan)
        {
            IntPtr hwnd = FindWindow(null, ProcessName);
            SetForegroundWindow(hwnd);

            Thread.Sleep(200);
            INPUT[] inputs = new INPUT[1];
            uint intReturn = new uint();

            foreach (var key in KeyScan)
            {
                inputs[0].type = SendInputEventType.InputKeyboard;
                inputs[0].mkhi.ki.wScan = 13;
                inputs[0].mkhi.ki.dwFlags = (int)KEYEVENTF.KEYDOWN;
                inputs[0].mkhi.ki.wVk = key;
                intReturn = SendInput(1, inputs, Marshal.SizeOf(inputs[0]));
                Thread.Sleep(100);
            }

            foreach (var key in KeyScan)
            {
                inputs = new INPUT[1];
                inputs[0].type = SendInputEventType.InputKeyboard;
                inputs[0].mkhi.ki.wScan = 13;
                inputs[0].mkhi.ki.dwFlags = (int)KEYEVENTF.KEYUP;
                inputs[0].mkhi.ki.wVk = key;
                intReturn = SendInput(1, inputs, Marshal.SizeOf(inputs[0]));
                Thread.Sleep(100);
            }
        }

        public static void SendKeyAsInput(string ProcessName)
        {
         
            IntPtr hwnd = FindWindow(null, ProcessName);
            SetForegroundWindow(hwnd);
            Thread.Sleep(200);
            INPUT[] inputs = new INPUT[1];
            inputs[0].type = SendInputEventType.InputKeyboard;
            inputs[0].mkhi.ki.wScan = 0x28;
            inputs[0].mkhi.ki.dwFlags = (int)KEYEVENTF.KEYDOWN;
            inputs[0].mkhi.ki.wVk = 13;
            uint intReturn = SendInput(1, inputs, Marshal.SizeOf(inputs[0]));
            Thread.Sleep(100);
            inputs = new INPUT[1];
            inputs[0].type = SendInputEventType.InputKeyboard;
            inputs[0].mkhi.ki.wScan = 0x28;
            inputs[0].mkhi.ki.dwFlags = (int)KEYEVENTF.KEYUP;
            inputs[0].mkhi.ki.wVk = 13;
            intReturn = SendInput(1, inputs, Marshal.SizeOf(inputs[0]));
        }

        public static void SendKeyAsVirtInput(string ProcessName)
        {
            IntPtr hwnd = FindWindow(null, ProcessName);
            SetForegroundWindow(hwnd);
            Thread.Sleep(200);

            INPUT[] inputs = new INPUT[1];
            inputs[0].type = SendInputEventType.InputKeyboard;
            inputs[0].mkhi.ki.wScan = 13;
            inputs[0].mkhi.ki.dwFlags = (int)KEYEVENTF.KEYDOWN;
            inputs[0].mkhi.ki.wVk = 0x28;
            uint intReturn = SendInput(1, inputs, Marshal.SizeOf(inputs[0]));
            Thread.Sleep(100);
            inputs = new INPUT[1];
            inputs[0].type = SendInputEventType.InputKeyboard;
            inputs[0].mkhi.ki.wScan = 13;
            inputs[0].mkhi.ki.dwFlags = (int)KEYEVENTF.KEYUP;
            inputs[0].mkhi.ki.wVk = 0x28;
            intReturn = SendInput(1, inputs, Marshal.SizeOf(inputs[0]));
        }

        public static void SendKeyAsInputActiv(string ProcessName)
        {
            IntPtr hwnd = FindWindow(null, ProcessName);
            SetForegroundWindow(hwnd);

            Thread.Sleep(200);
        }
    }


    public class XspliteControl
    {
        [DllImport("user32.dll")]
        public static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, UIntPtr dwExtraInfo);

        public static void StartStopRecord()
        {
            keybd_event(0x10, 0, 0, UIntPtr.Zero);
            Thread.Sleep(125);
            keybd_event(0x78, 0, 0, UIntPtr.Zero);
            Thread.Sleep(125);
            keybd_event(0x78, 0, 0x02, UIntPtr.Zero);
            Thread.Sleep(125);
            keybd_event(0x10, 0, 0x02, UIntPtr.Zero);
            Thread.Sleep(125);
        }


        public static void StartLogo()
        {
            keybd_event(0x10, 0, 0, UIntPtr.Zero);
            Thread.Sleep(125);
            keybd_event(0x71, 0, 0, UIntPtr.Zero);
            Thread.Sleep(125);
            keybd_event(0x71, 0, 0x02, UIntPtr.Zero);
            Thread.Sleep(125);
            keybd_event(0x10, 0, 0x02, UIntPtr.Zero);
            Thread.Sleep(125);
        }

        public static void StartGame()
        {
            keybd_event(0x10, 0, 0, UIntPtr.Zero);
            Thread.Sleep(125);
            keybd_event(0x70, 0, 0, UIntPtr.Zero);
            Thread.Sleep(125);
            keybd_event(0x70, 0, 0x02, UIntPtr.Zero);
            Thread.Sleep(125);
            keybd_event(0x10, 0, 0x02, UIntPtr.Zero);
            Thread.Sleep(125);
        }

        public static void StartLogoF8()
        {
            keybd_event(0x10, 0, 0, UIntPtr.Zero);
            Thread.Sleep(125);
            keybd_event(0x77, 0, 0, UIntPtr.Zero);
            Thread.Sleep(125);
            keybd_event(0x77, 0, 0x02, UIntPtr.Zero);
            Thread.Sleep(125);
            keybd_event(0x10, 0, 0x02, UIntPtr.Zero);
            Thread.Sleep(125);
        }

        public static void StartGameF7()
        {
            keybd_event(0x10, 0, 0, UIntPtr.Zero);
            Thread.Sleep(125);
            keybd_event(0x76, 0, 0, UIntPtr.Zero);
            Thread.Sleep(125);
            keybd_event(0x76, 0, 0x02, UIntPtr.Zero);
            Thread.Sleep(125);
            keybd_event(0x10, 0, 0x02, UIntPtr.Zero);
            Thread.Sleep(125);
        }

        public static void PressEnter()
        {
            keybd_event(0x20, 0, 0, UIntPtr.Zero);
            Thread.Sleep(125);
            keybd_event(0x20, 0, 0x02, UIntPtr.Zero);
            Thread.Sleep(125);
        }

        public static void PressUp()
        {
            keybd_event(0x26, 0, 0, UIntPtr.Zero);
            Thread.Sleep(125);
            keybd_event(0x26, 0, 0x02, UIntPtr.Zero);
            Thread.Sleep(125);
        }

        public static void PressDown()
        {
            keybd_event(0x28, 0, 0, UIntPtr.Zero);
            Thread.Sleep(125);
            keybd_event(0x28, 0, 0x02, UIntPtr.Zero);
            Thread.Sleep(125);
        }
    }

}
