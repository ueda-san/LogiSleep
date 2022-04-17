using LedCSharp;
using Microsoft.Win32;
using System;
using System.Configuration;
using System.Drawing;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;


namespace LogiSleep
{
    public partial class Form1 : Form
    {
        private AppSettings appSettings = new AppSettings();
        private NativeMethods.INPUT input = new NativeMethods.INPUT();

        private Timer timerPreventScreenOff = new Timer(); // move mouse at intervals
        private Timer timerAfterResume = new Timer(); // wait after resume
        private Timer timerAfterClick = new Timer(); // wait after button click
        private Timer timerAfterInit = new Timer(); // wait after LogiLedInit() for SetLighting()

        private bool sdkInitialized = false;
        private IntPtr handlePowerNotify = IntPtr.Zero;
        private Guid GUID_CONSOLE_DISPLAY_STATE = new Guid(0x6fe69556, 0x704a, 0x47a0, 0x8f, 0x24, 0xc2, 0x8d, 0x93, 0x6f, 0xda, 0x47);

        private NotifyIcon icon;

        public Form1()
        {
            InitializeComponent();

            // i don't like satellite assembly
            string lang = System.Threading.Thread.CurrentThread.CurrentCulture.Name;
            if (lang.StartsWith("ja-")){ // ja-JP
                checkBoxLED.Text = Properties.Resources.StringLED_ja;
                checkBoxScreen.Text = Properties.Resources.StringScreen_ja;
                buttonTurnOff.Text = Properties.Resources.StringTurnOff_ja;
            }else{
                //checkBoxLED.Text = "Turns off keyboard LED when screen is off";
                //checkBoxScreen.Text = "Prevent the screen from turning off";
                //buttonTurnOff.Text = "Turn off now";
            }

            timerPreventScreenOff.Interval = 1000*50; // 50sec
            timerPreventScreenOff.Tick += (obj, args) =>{
                const int INPUT_MOUSE = 0;
                input.type = INPUT_MOUSE;
                input.U.mi.dx = 0;
                input.U.mi.dy = 0;
                input.U.mi.mouseData = 0;
                input.U.mi.dwFlags = NativeMethods.MOUSEEVENTF.MOVE;
                input.U.mi.time = 0;
                input.U.mi.dwExtraInfo = UIntPtr.Zero;
                NativeMethods.SendInput(1, ref input, NativeMethods.INPUT.Size);
            };

            timerAfterResume.Interval = 1000*10; // 10sec
            timerAfterResume.Tick += (obj, args) =>{
                timerAfterResume.Stop();
                setPreventScreenOff();
            };
            SystemEvents.PowerModeChanged += new PowerModeChangedEventHandler(OnPowerModeChanged);


            timerAfterClick.Interval = 1000/2; // 0.5sec
            timerAfterClick.Tick += (obj, args) =>{
                timerAfterClick.Stop();
                const int WM_SYSCOMMAND = 0x0112;
                const int SC_MONITORPOWER = 0xF170;
                NativeMethods.SendMessage(new HandleRef(this, this.Handle), WM_SYSCOMMAND, (IntPtr)SC_MONITORPOWER, (IntPtr)2);
            };

            timerAfterInit.Interval = 1000*2; // 2sec
            timerAfterInit.Tick += (obj, args) =>{
                timerAfterInit.Stop();
                tuenOffLED();
            };

            const int DEVICE_NOTIFY_WINDOW_HANDLE = 0x00000000;
            handlePowerNotify = NativeMethods.RegisterPowerSettingNotification(new HandleRef(this, this.Handle), ref GUID_CONSOLE_DISPLAY_STATE, DEVICE_NOTIFY_WINDOW_HANDLE);

            icon = new NotifyIcon {
                Icon = Icon.ExtractAssociatedIcon(Assembly.GetExecutingAssembly().Location),
                Visible = true,
                Text = "LogiSleep"
            };
            icon.DoubleClick += new System.EventHandler(NotifyIcon_Click);

            ToolStripMenuItem toolStripMenuItem1 = new ToolStripMenuItem("Open/Close");
            toolStripMenuItem1.Click += new System.EventHandler(NotifyIcon_Click);
            ToolStripMenuItem toolStripMenuItem2 = new ToolStripMenuItem("Quit");
            toolStripMenuItem2.Click += new System.EventHandler(NotifyIcon_Exit);
            ContextMenuStrip contextMenuStrip = new ContextMenuStrip();
            contextMenuStrip.Items.Add(toolStripMenuItem1);
            contextMenuStrip.Items.Add(toolStripMenuItem2);
            icon.ContextMenuStrip = contextMenuStrip;
        }


        //------------------------------------------------------------------------------

        private void OnPowerModeChanged(object sender, PowerModeChangedEventArgs e)
        {
            switch (e.Mode) {
              case PowerModes.Suspend:
                setPreventScreenOff(true);
                break;
              case PowerModes.Resume:
                timerAfterResume.Start();
                break;
              case PowerModes.StatusChange:
                setPreventScreenOff();
                break;
            }
        }

        private void OnScreenStateChanged(bool powerOn){
            if (checkBoxLED.Checked){
                if (powerOn){
                    if (sdkInitialized){
                        LogitechGSDK.LogiLedRestoreLighting();
                        LogitechGSDK.LogiLedShutdown();
                        sdkInitialized = false;
                    }
                }else{
                    if (!sdkInitialized){
                        if (LogitechGSDK.LogiLedInit()){
                            sdkInitialized = true;
                            timerAfterInit.Start(); // need wait for SetLighting()
                        }else{
                            sdkInitialized = false;
                        }
                    }
                }
            }
        }

        private void tuenOffLED() {
            if (sdkInitialized){
                LogitechGSDK.LogiLedSaveCurrentLighting();
                LogitechGSDK.LogiLedSetLighting(0,0,0);
            }
        }

        private void setPreventScreenOff(bool forceOff=false) {
            if (!forceOff && checkBoxScreen.Checked){
                NativeMethods.SetThreadExecutionState(NativeMethods.EXECUTION_STATE.ES_SYSTEM_REQUIRED | NativeMethods.EXECUTION_STATE.ES_CONTINUOUS);
                timerPreventScreenOff.Enabled = true;
            }else{
                NativeMethods.SetThreadExecutionState(NativeMethods.EXECUTION_STATE.ES_CONTINUOUS);
                timerPreventScreenOff.Enabled = false;
            }
        }

        private void NotifyIcon_Click(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized){
                Show();
                this.WindowState = FormWindowState.Normal;
            }else{
                this.WindowState = FormWindowState.Minimized;
                //Hide();
            }
        }

        private void NotifyIcon_Exit(object sender, EventArgs e)
        {
            this.Close();
        }

        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        private struct POWERBROADCAST_SETTING {
            public Guid PowerSetting;
            public uint DataLength;
            public byte Data;
        }

        protected override void WndProc(ref Message m)
        {
            const int WM_POWERBROADCAST = 0x0218;
            const int PBT_POWERSETTINGCHANGE = 0x8013;

            if (m.Msg == WM_POWERBROADCAST){
                if (m.WParam.ToInt32() == PBT_POWERSETTINGCHANGE){
                    var pbs = (POWERBROADCAST_SETTING)Marshal.PtrToStructure(m.LParam, typeof(POWERBROADCAST_SETTING));
                    if (pbs.PowerSetting == GUID_CONSOLE_DISPLAY_STATE) {
                        if (pbs.Data == 0){
                            OnScreenStateChanged(false);
                        }else if (pbs.Data == 1){
                            OnScreenStateChanged(true);
                        }else{
                            //DIMM?
                            //OnScreenStateChanged(false);
                        }
                    }
                }
            }

            base.WndProc(ref m);
        }


        //------------------------------------------------------------------------------

        private void checkBoxLED_CheckedChanged(object sender, EventArgs e)
        {
            appSettings.TurnOffLED = checkBoxLED.Checked;
            appSettings.Save();
        }

        private void checkBoxScreen_CheckedChanged(object sender, EventArgs e)
        {
            appSettings.PreventScreenOff = checkBoxScreen.Checked;
            appSettings.Save();
            setPreventScreenOff();
        }

        private void buttonTurnOff_Click(object sender, EventArgs e)
        {
            timerAfterClick.Start();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Icon = Icon.ExtractAssociatedIcon(Assembly.GetExecutingAssembly().Location);
            checkBoxLED.Checked = appSettings.TurnOffLED;
            checkBoxScreen.Checked = appSettings.PreventScreenOff;
            setPreventScreenOff();
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            NativeMethods.UnregisterPowerSettingNotification(handlePowerNotify);
            SystemEvents.PowerModeChanged -= new PowerModeChangedEventHandler(OnPowerModeChanged);
            OnScreenStateChanged(true);
            setPreventScreenOff(true);
            icon.Dispose();
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized){
                Hide();
            }
        }
    }


    //==============================================================================

    sealed class AppSettings : ApplicationSettingsBase
    {
        [UserScopedSettingAttribute()]
        [DefaultSettingValueAttribute("True")]
        public bool TurnOffLED
        {
            get { return (bool)this["TurnOffLED"]; }
            set { this["TurnOffLED"] = value; }
        }

        [UserScopedSettingAttribute()]
        [DefaultSettingValueAttribute("False")]
        public bool PreventScreenOff
        {
            get { return (bool)this["PreventScreenOff"]; }
            set { this["PreventScreenOff"] = value; }
        }
    }

    //------------------------------------------------------------------------------
    internal static class NativeMethods
    {
        [Flags]
        internal enum MOUSEEVENTF : uint
        {
            ABSOLUTE        = 0x8000,
            HWHEEL          = 0x1000,
            MOVE            = 0x0001,
            MOVE_NOCOALESCE = 0x2000,
            LEFTDOWN        = 0x0002,
            LEFTUP          = 0x0004,
            RIGHTDOWN       = 0x0008,
            RIGHTUP         = 0x0010,
            MIDDLEDOWN      = 0x0020,
            MIDDLEUP        = 0x0040,
            VIRTUALDESK     = 0x4000,
            WHEEL           = 0x0800,
            XDOWN           = 0x0080,
            XUP             = 0x0100,
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct MOUSEINPUT {
            internal int dx;
            internal int dy;
            internal int mouseData;
            internal MOUSEEVENTF dwFlags;
            internal uint time;
            internal UIntPtr dwExtraInfo;
        }

        [StructLayout(LayoutKind.Explicit)]
        internal struct InputUnion {
            [FieldOffset(0)]
            internal MOUSEINPUT mi;
            //[FieldOffset(0)]
            //internal KEYBOARDINPUT ki;
            //[FieldOffset(0)]
            //internal HARDWAREINPUT hi;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct INPUT {
            internal uint type;
            internal InputUnion U;
            internal static int Size {
                get { return Marshal.SizeOf(typeof(INPUT)); }
            }
        }

        [DllImport("user32.dll")]
        internal static extern uint SendInput(int cInputs, ref INPUT pInputs, int cbSize);


        [Flags]
        internal enum EXECUTION_STATE : uint {
            ES_SYSTEM_REQUIRED   = 0x00000001,
            ES_DISPLAY_REQUIRED  = 0x00000002,
            ES_USER_PRESENT      = 0x80000004,
            ES_AWAYMODE_REQUIRED = 0x00000040,
            ES_CONTINUOUS        = 0x80000000,
        }

        [DllImport("kernel32.dll")]
        internal static extern EXECUTION_STATE SetThreadExecutionState(EXECUTION_STATE esFlags);

        [DllImport("user32.dll")]
        internal static extern IntPtr SendMessage(HandleRef hWnd, uint Msg, IntPtr wParam, IntPtr lParam);
        [DllImport("user32.dll")]
        internal static extern IntPtr RegisterPowerSettingNotification(HandleRef hRecipient, ref Guid PowerSettingGuid, Int32 Flags);
        [DllImport("user32.dll")]
        internal static extern bool UnregisterPowerSettingNotification(IntPtr RegistrationHandle);

    }

}
