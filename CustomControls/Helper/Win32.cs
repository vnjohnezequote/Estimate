using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Interop;

namespace CustomControls.Helper
{
    public static class Win32
    {
        // Token: 0x0600025E RID: 606
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern bool GetCursorPos(out Win32.POINT pt);

		// Token: 0x0600025F RID: 607
		[DllImport("user32.dll")]
		public static extern bool SetCursorPos(Win32.POINT pt);

		// Token: 0x06000260 RID: 608
		[DllImport("user32.dll")]
		public static extern int ShowCursor(IntPtr bShow);

		// Token: 0x06000261 RID: 609
		[DllImport("user32.dll")]
		public static extern IntPtr GetCapture();

		// Token: 0x06000262 RID: 610
		[DllImport("user32.dll")]
		public static extern uint GetDoubleClickTime();

		// Token: 0x06000263 RID: 611 RVA: 0x000086EC File Offset: 0x000076EC
		public static System.Windows.Point GetCursorPosition()
		{
			Win32.POINT point;
			Win32.GetCursorPos(out point);
			return new System.Windows.Point((double)point.X, (double)point.Y);
		}

		// Token: 0x06000264 RID: 612 RVA: 0x00008714 File Offset: 0x00007714
		public static void SetCursorPosition(System.Windows.Point pt)
		{
			Win32.POINT cursorPos;
			cursorPos.X = (int)pt.X;
			cursorPos.Y = (int)pt.Y;
			Win32.SetCursorPos(cursorPos);
		}

		// Token: 0x06000265 RID: 613
		[DllImport("user32.dll")]
		public static extern bool IsWindow(IntPtr hWnd);

		// Token: 0x06000266 RID: 614
		[DllImport("user32.dll")]
		public static extern IntPtr GetParent(IntPtr hWnd);

		// Token: 0x06000267 RID: 615
		[DllImport("user32.dll")]
		public static extern IntPtr DestroyWindow(IntPtr hWnd);

		// Token: 0x06000268 RID: 616
		[DllImport("user32.dll")]
		public static extern bool GetWindowRect(IntPtr hWnd, out Win32.RECT lpRect);

		// Token: 0x06000269 RID: 617
		[DllImport("user32.dll")]
		public static extern bool EnumChildWindows(IntPtr parentHandle, Win32.EnumWindowProc callback, IntPtr lParam);

		// Token: 0x0600026A RID: 618
		[DllImport("user32.dll")]
		public static extern IntPtr EnableWindow(IntPtr hWnd, bool bEnable);

		// Token: 0x0600026B RID: 619
		[DllImport("user32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool EnumWindows(Win32.EnumWindowProc lpEnumFunc, IntPtr lParam);

		// Token: 0x0600026C RID: 620
		[DllImport("user32.dll")]
		public static extern bool PrintWindow(IntPtr hWnd, IntPtr hdcBlt, int nFlags);

		// Token: 0x0600026D RID: 621 RVA: 0x00008746 File Offset: 0x00007746
		public static IntPtr SetWindowLong(IntPtr hWnd, int index, IntPtr value)
		{
			if (IntPtr.Size == 4)
			{
				return Win32.SetWindowLong32(hWnd, index, value);
			}
			return Win32.SetWindowLongPtr64(hWnd, index, value);
		}

		// Token: 0x0600026E RID: 622
		[DllImport("user32.dll", CharSet = CharSet.Auto, EntryPoint = "SetWindowLong")]
		private static extern IntPtr SetWindowLong32(IntPtr hwnd, int index, IntPtr value);

		// Token: 0x0600026F RID: 623
		[DllImport("user32.dll", CharSet = CharSet.Auto, EntryPoint = "SetWindowLongPtr")]
		private static extern IntPtr SetWindowLongPtr64(IntPtr hwnd, int index, IntPtr value);

		// Token: 0x06000270 RID: 624 RVA: 0x00008761 File Offset: 0x00007761
		public static IntPtr GetWindowLong(IntPtr hWnd, int index)
		{
			if (IntPtr.Size == 4)
			{
				return Win32.GetWindowLong32(hWnd, index);
			}
			return Win32.GetWindowLongPtr64(hWnd, index);
		}

		// Token: 0x06000271 RID: 625
		[DllImport("user32.dll", CharSet = CharSet.Auto, EntryPoint = "GetWindowLong")]
		private static extern IntPtr GetWindowLong32(IntPtr hWnd, int index);

		// Token: 0x06000272 RID: 626
		[DllImport("user32.dll", CharSet = CharSet.Auto, EntryPoint = "GetWindowLongPtr")]
		private static extern IntPtr GetWindowLongPtr64(IntPtr hWnd, int index);

		// Token: 0x06000273 RID: 627
		[DllImport("user32.dll")]
		public static extern int CallWindowProc(IntPtr lpPrevWndFunc, IntPtr hWnd, int Msg, IntPtr wParam, IntPtr lParam);

		// Token: 0x06000274 RID: 628
		[DllImport("user32.dll")]
		public static extern bool SetWindowPos(IntPtr hwnd, IntPtr hwndInsertAfter, int x, int y, int width, int height, uint flags);

		// Token: 0x06000275 RID: 629
		[DllImport("User32.DLL")]
		public static extern IntPtr FindWindowEx(IntPtr hwnParent, IntPtr hwndChildAfter, string lpszClass, string lpszWindow);

		// Token: 0x06000276 RID: 630
		[DllImport("user32.dll")]
		public static extern IntPtr GetForegroundWindow();

		// Token: 0x06000277 RID: 631
		[DllImport("User32.dll")]
		public static extern IntPtr GetWindow(IntPtr hWnd, uint cmd);

		// Token: 0x06000278 RID: 632
		[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

		// Token: 0x06000279 RID: 633
		[DllImport("User32.dll")]
		public static extern bool IsWindowVisible(IntPtr hWnd);

		// Token: 0x0600027A RID: 634
		[DllImport("user32.dll")]
		public static extern IntPtr ShowWindow(IntPtr hWnd, Win32.ShowWindowConstants cmd);

		// Token: 0x0600027B RID: 635
		[DllImport("user32.dll")]
		public static extern IntPtr SetActiveWindow(IntPtr hWnd);

		// Token: 0x0600027C RID: 636
		[DllImport("user32.dll")]
		public static extern IntPtr SetFocus(IntPtr hWnd);

		// Token: 0x0600027D RID: 637
		[DllImport("user32.dll")]
		public static extern int GetWindowThreadProcessId(IntPtr handle, out int processId);

		// Token: 0x0600027E RID: 638
		[DllImport("user32.dll")]
		public static extern void SwitchToThisWindow(IntPtr hWnd, bool fAltTab);

		// Token: 0x0600027F RID: 639
		[DllImport("user32.dll")]
		public static extern bool SetForegroundWindow(IntPtr hWnd);

		// Token: 0x06000280 RID: 640 RVA: 0x0000877C File Offset: 0x0000777C
		public static void RemoveIcon(Window window)
		{
			IntPtr handle = new WindowInteropHelper(window).Handle;
			int num = Win32.GetWindowLong(handle, -20).ToInt32();
			Win32.SetWindowLong(handle, -20, new IntPtr(num | 1));
			Win32.SendMessage(handle, 128u, (IntPtr)0, IntPtr.Zero);
			Win32.SendMessage(handle, 128u, (IntPtr)1, IntPtr.Zero);
			Win32.SetWindowPos(handle, IntPtr.Zero, 0, 0, 0, 0, 39u);
		}

		// Token: 0x06000281 RID: 641 RVA: 0x000087F8 File Offset: 0x000077F8
		public static void HideMinimizeAndMaximizeButtons(Window window)
		{
			IntPtr handle = new WindowInteropHelper(window).Handle;
			int num = Win32.GetWindowLong(handle, -16).ToInt32();
			Win32.SetWindowLong(handle, -16, new IntPtr(num & -65537 & -131073));
		}

		// Token: 0x06000282 RID: 642 RVA: 0x00008840 File Offset: 0x00007840
		public static System.Windows.Size GetCurrentScreenSize(Window window)
		{
			IntPtr handle = new WindowInteropHelper(window).Handle;
			Screen screen = Screen.FromHandle(handle);
			return new System.Windows.Size((double)screen.WorkingArea.Width, (double)screen.WorkingArea.Height);
		}

		// Token: 0x06000283 RID: 643
		[DllImport("user32.dll")]
		public static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

		// Token: 0x06000284 RID: 644
		[DllImport("user32.dll")]
		public static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, string lParam);

		// Token: 0x06000285 RID: 645
		[DllImport("user32.dll")]
		public static extern IntPtr PostMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

		// Token: 0x06000286 RID: 646
		[DllImport("user32.dll")]
		public static extern bool DestroyIcon(IntPtr handle);

		// Token: 0x06000287 RID: 647
		[DllImport("user32.dll")]
		public static extern int LoadString(IntPtr hInstance, int uID, StringBuilder lpBuffer, int nBufferMax);

		// Token: 0x06000288 RID: 648
		[DllImport("gdi32.dll")]
		public static extern bool DeleteObject(IntPtr handle);

		// Token: 0x06000289 RID: 649
		[DllImport("User32.DLL")]
		public static extern bool EnumDisplayDevices(string lpDevice, uint iDevNum, ref Win32.DISPLAY_DEVICE lpDisplayDevice, uint dwFlags);

		// Token: 0x0600028A RID: 650
		[DllImport("gdi32.dll")]
		private static extern int GetDeviceCaps(IntPtr hdc, int nIndex);

		// Token: 0x0600028B RID: 651 RVA: 0x00008884 File Offset: 0x00007884
		public static float[] GetDisplayScalingFactors()
		{
			float[] result;
			using (Graphics graphics = Graphics.FromHwnd(IntPtr.Zero))
			{
				IntPtr hdc = graphics.GetHdc();
				int deviceCaps = Win32.GetDeviceCaps(hdc, 10);
				int deviceCaps2 = Win32.GetDeviceCaps(hdc, 117);
				int deviceCaps3 = Win32.GetDeviceCaps(hdc, 90);
				float num = (float)deviceCaps2 / (float)deviceCaps;
				float num2 = (float)deviceCaps3 / 96f;
				result = new float[]
				{
					num,
					num2
				};
			}
			return result;
		}

		// Token: 0x0600028C RID: 652
		[DllImport("user32.dll")]
		public static extern IntPtr GetSystemMenu(IntPtr hWnd, int bRevert);

		// Token: 0x0600028D RID: 653
		[DllImport("user32.dll")]
		public static extern bool DeleteMenu(IntPtr hMenu, int wPosition, int wFlags);

		// Token: 0x0600028E RID: 654
		[DllImport("User32.dll")]
		public static extern int DrawMenuBar(IntPtr hWnd);

		// Token: 0x0600028F RID: 655 RVA: 0x00008904 File Offset: 0x00007904
		public static void RemoveSizeContextMenu(Window window)
		{
			IntPtr handle = new WindowInteropHelper(window).Handle;
			IntPtr systemMenu = Win32.GetSystemMenu(handle, 0);
			bool flag = Win32.DeleteMenu(systemMenu, 5, 1024) & Win32.DeleteMenu(systemMenu, 4, 1024) & Win32.DeleteMenu(systemMenu, 3, 1024) & Win32.DeleteMenu(systemMenu, 2, 1024) & Win32.DeleteMenu(systemMenu, 0, 1024);
			Win32.DrawMenuBar(systemMenu);
		}

		// Token: 0x06000290 RID: 656
		[DllImport("Imm32.dll")]
		public static extern IntPtr ImmGetContext(IntPtr hWnd);

		// Token: 0x06000291 RID: 657
		[DllImport("Imm32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool ImmReleaseContext(IntPtr hWnd, IntPtr hIMC);

		// Token: 0x06000292 RID: 658
		[DllImport("Imm32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool ImmGetOpenStatus(IntPtr hIMC);

		// Token: 0x06000293 RID: 659
		[DllImport("kernel32.dll")]
		public static extern IntPtr LoadLibrary(string dllname);

		// Token: 0x06000294 RID: 660
		[DllImport("kernel32.dll")]
		public static extern bool FreeLibrary(IntPtr hModule);

		// Token: 0x06000295 RID: 661
		[DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern IntPtr GetModuleHandle(string lpModuleName);

		// Token: 0x06000296 RID: 662
		[DllImport("Shell32.dll")]
		public static extern IntPtr ExtractIcon(IntPtr hInst, string lpszExeFileName, int nIconIndex);

		// Token: 0x06000297 RID: 663
		[DllImport("Shell32.dll")]
		public static extern int ExtractIconEx(string lpszFile, int nIconIndex, IntPtr[] phIconLarge, IntPtr[] phIconSmall, int nIcons);

		// Token: 0x06000298 RID: 664
		[DllImport("shlwapi.dll", CharSet = CharSet.Unicode, EntryPoint = "PathFileExistsW", SetLastError = true)]
		public static extern bool PathFileExists([MarshalAs(UnmanagedType.LPTStr)] string pszPath);

		// Token: 0x06000299 RID: 665
		[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern IntPtr SetWindowsHookEx(int idHook, Win32.HookProc lpfn, IntPtr hMod, uint dwThreadId);

		// Token: 0x0600029A RID: 666
		[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool UnhookWindowsHookEx(IntPtr hhk);

		// Token: 0x0600029B RID: 667
		[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

		// Token: 0x0600029C RID: 668
		[DllImport("user32.dll")]
		public static extern int GetSystemMetrics(Win32.SystemMetricConstants smIndex);

		// Token: 0x0600029D RID: 669
		[DllImport("uxtheme.dll")]
		public static extern bool IsThemeActive();

		// Token: 0x0600029E RID: 670
		[DllImport("dwmapi.dll")]
		public static extern int DwmIsCompositionEnabled(ref int enabled);

		// Token: 0x0600029F RID: 671
		[DllImport("dwmapi.dll")]
		public static extern int DwmGetWindowAttribute(IntPtr hwnd, Win32.DWMWINDOWATTRIBUTE dwAttribute, out Win32.RECT pvAttribute, int cbAttribute);

		// Token: 0x040000D1 RID: 209
		public const int WM_USER = 1024;

		// Token: 0x040000D2 RID: 210
		public const int WM_ACAD_KEEPFOCUS = 28929;

		// Token: 0x040000D3 RID: 211
		public const int WM_MDIACTIVATE = 546;

		// Token: 0x040000D4 RID: 212
		public const int WM_USER_SIZE = 1025;

		// Token: 0x040000D5 RID: 213
		public const int GWL_WNDPROC = -4;

		// Token: 0x040000D6 RID: 214
		public const int GWL_STYLE = -16;

		// Token: 0x040000D7 RID: 215
		public const int GWLP_HWNDPARENT = -8;

		// Token: 0x040000D8 RID: 216
		public const int WM_KEYDOWN = 256;

		// Token: 0x040000D9 RID: 217
		public const int WM_KEYUP = 257;

		// Token: 0x040000DA RID: 218
		public const int WM_SYSKEYDOWN = 260;

		// Token: 0x040000DB RID: 219
		public const int WM_CHAR = 258;

		// Token: 0x040000DC RID: 220
		public const int WM_ENABLE = 10;

		// Token: 0x040000DD RID: 221
		public const int WS_POPUP = -2147483648;

		// Token: 0x040000DE RID: 222
		public const int WS_SYSMENU = 524288;

		// Token: 0x040000DF RID: 223
		public const int WS_VISIBLE = 268435456;

		// Token: 0x040000E0 RID: 224
		public const int WS_CHILD = 1073741824;

		// Token: 0x040000E1 RID: 225
		public const int WS_EX_TOOLWINDOW = 128;

		// Token: 0x040000E2 RID: 226
		public const int WS_EX_NOACTIVATE = 134217728;

		// Token: 0x040000E3 RID: 227
		public const int WM_ENTERSIZEMOVE = 561;

		// Token: 0x040000E4 RID: 228
		public const int WM_EXITSIZEMOVE = 562;

		// Token: 0x040000E5 RID: 229
		public const int WM_WINDOWPOSCHANGING = 70;

		// Token: 0x040000E6 RID: 230
		public const int WM_WINDOWPOSCHANGED = 71;

		// Token: 0x040000E7 RID: 231
		public const int WM_MOVING = 534;

		// Token: 0x040000E8 RID: 232
		public const int WM_QUERYCENTERWND = 875;

		// Token: 0x040000E9 RID: 233
		public const int VK_BACK = 8;

		// Token: 0x040000EA RID: 234
		public const int VK_TAB = 9;

		// Token: 0x040000EB RID: 235
		public const int VK_RETURN = 13;

		// Token: 0x040000EC RID: 236
		public const int VK_ESCAPE = 27;

		// Token: 0x040000ED RID: 237
		public const int VK_SPACE = 32;

		// Token: 0x040000EE RID: 238
		public const int VK_PRIOR = 33;

		// Token: 0x040000EF RID: 239
		public const int VK_NEXT = 34;

		// Token: 0x040000F0 RID: 240
		public const int VK_END = 35;

		// Token: 0x040000F1 RID: 241
		public const int VK_HOME = 36;

		// Token: 0x040000F2 RID: 242
		public const int VK_LEFT = 37;

		// Token: 0x040000F3 RID: 243
		public const int VK_UP = 38;

		// Token: 0x040000F4 RID: 244
		public const int VK_RIGHT = 39;

		// Token: 0x040000F5 RID: 245
		public const int VK_DOWN = 40;

		// Token: 0x040000F6 RID: 246
		public const int VK_INSERT = 45;

		// Token: 0x040000F7 RID: 247
		public const int VK_DELETE = 46;

		// Token: 0x040000F8 RID: 248
		public const int VK_C = 67;

		// Token: 0x040000F9 RID: 249
		public const int VK_V = 86;

		// Token: 0x040000FA RID: 250
		public const int VK_X = 88;

		// Token: 0x040000FB RID: 251
		public const int VK_F1 = 112;

		// Token: 0x040000FC RID: 252
		public const int VK_F2 = 113;

		// Token: 0x040000FD RID: 253
		public const int VK_F10 = 121;

		// Token: 0x040000FE RID: 254
		public const int GWL_EXSTYLE = -20;

		// Token: 0x040000FF RID: 255
		public const int WS_EX_DLGMODALFRAME = 1;

		// Token: 0x04000100 RID: 256
		public const int SWP_NOSIZE = 1;

		// Token: 0x04000101 RID: 257
		public const int SWP_NOMOVE = 2;

		// Token: 0x04000102 RID: 258
		public const int SWP_NOZORDER = 4;

		// Token: 0x04000103 RID: 259
		public const int SWP_NOACTIVATE = 16;

		// Token: 0x04000104 RID: 260
		public const int SWP_FRAMECHANGED = 32;

		// Token: 0x04000105 RID: 261
		public const uint WM_SETICON = 128u;

		// Token: 0x04000106 RID: 262
		public const int ICON_SMALL = 0;

		// Token: 0x04000107 RID: 263
		public const int ICON_BIG = 1;

		// Token: 0x04000108 RID: 264
		public const int HWND_NOTOPMOST = -2;

		// Token: 0x04000109 RID: 265
		public const int HWND_TOPMOST = -1;

		// Token: 0x0400010A RID: 266
		public const int GW_HWNDFIRST = 0;

		// Token: 0x0400010B RID: 267
		public const int GW_HWNDNEXT = 2;

		// Token: 0x0400010C RID: 268
		public const int GW_OWNER = 4;

		// Token: 0x0400010D RID: 269
		public const int MF_BYPOSITION = 1024;

		// Token: 0x0400010E RID: 270
		public const int WS_MAXIMIZEBOX = 65536;

		// Token: 0x0400010F RID: 271
		public const int WS_MINIMIZEBOX = 131072;

		// Token: 0x04000110 RID: 272
		public const uint WM_SETTEXT = 12u;

		// Token: 0x04000111 RID: 273
		public const uint WM_GETDLGCODE = 135u;

		// Token: 0x04000112 RID: 274
		public const uint DLGC_WANTMESSAGE = 4u;

		// Token: 0x020003FA RID: 1018
		public enum MouseMessages
		{
			// Token: 0x04000C14 RID: 3092
			WM_LEFTBUTTONDOWN = 513,
			// Token: 0x04000C15 RID: 3093
			WM_LEFTBUTTONUP,
			// Token: 0x04000C16 RID: 3094
			WM_MOUSEMOVE = 512,
			// Token: 0x04000C17 RID: 3095
			WM_MOUSEWHEEL = 522,
			// Token: 0x04000C18 RID: 3096
			WM_RIGHTBUTTONDOWN = 516,
			// Token: 0x04000C19 RID: 3097
			WM_RIGHTBUTTONUP,
			// Token: 0x04000C1A RID: 3098
			WM_MBUTTONDOWN = 519,
			// Token: 0x04000C1B RID: 3099
			WM_MBUTTONUP
		}

		// Token: 0x020003FB RID: 1019
		public enum WindowMessages
		{
			// Token: 0x04000C1D RID: 3101
			WM_SETFOCUS = 7
		}

		// Token: 0x020003FC RID: 1020
		public enum ShowWindowConstants
		{
			// Token: 0x04000C1F RID: 3103
			SW_HIDE,
			// Token: 0x04000C20 RID: 3104
			SW_MAXIMIZE = 3,
			// Token: 0x04000C21 RID: 3105
			SW_MINIMIZE = 6,
			// Token: 0x04000C22 RID: 3106
			SW_SHOWNA = 8,
			// Token: 0x04000C23 RID: 3107
			SW_RESTORE
		}

		// Token: 0x020003FD RID: 1021
		public enum SystemMetricConstants
		{
			// Token: 0x04000C25 RID: 3109
			SM_CXSIZE = 30,
			// Token: 0x04000C26 RID: 3110
			SM_CXFRAME = 32,
			// Token: 0x04000C27 RID: 3111
			SM_CYFRAME,
			// Token: 0x04000C28 RID: 3112
			SM_CXMENUSIZE = 54
		}

		// Token: 0x020003FE RID: 1022
		public enum DWMWINDOWATTRIBUTE
		{
			// Token: 0x04000C2A RID: 3114
			NCRenderingEnabled = 1,
			// Token: 0x04000C2B RID: 3115
			NCRenderingPolicy,
			// Token: 0x04000C2C RID: 3116
			TransitionsForceDisabled,
			// Token: 0x04000C2D RID: 3117
			AllowNCPaint,
			// Token: 0x04000C2E RID: 3118
			CaptionButtonBounds,
			// Token: 0x04000C2F RID: 3119
			NonClientRtlLayout,
			// Token: 0x04000C30 RID: 3120
			ForceIconicRepresentation,
			// Token: 0x04000C31 RID: 3121
			Flip3DPolicy,
			// Token: 0x04000C32 RID: 3122
			ExtendedFrameBounds,
			// Token: 0x04000C33 RID: 3123
			HasIconicBitmap,
			// Token: 0x04000C34 RID: 3124
			DisallowPeek,
			// Token: 0x04000C35 RID: 3125
			ExcludedFromPeek,
			// Token: 0x04000C36 RID: 3126
			Cloak,
			// Token: 0x04000C37 RID: 3127
			Cloaked,
			// Token: 0x04000C38 RID: 3128
			FreezeRepresentation
		}

		// Token: 0x020003FF RID: 1023
		public struct POINT
		{
			// Token: 0x06002167 RID: 8551 RVA: 0x000831EA File Offset: 0x000821EA
			public POINT(int x, int y)
			{
				this.X = x;
				this.Y = y;
			}

			// Token: 0x04000C39 RID: 3129
			public int X;

			// Token: 0x04000C3A RID: 3130
			public int Y;
		}

		// Token: 0x02000400 RID: 1024
		public struct RECT
		{
			// Token: 0x06002168 RID: 8552 RVA: 0x000831FA File Offset: 0x000821FA
			public RECT(int left, int top, int right, int bottom)
			{
				this.Left = left;
				this.Top = top;
				this.Right = right;
				this.Bottom = bottom;
			}

			// Token: 0x06002169 RID: 8553 RVA: 0x0008321C File Offset: 0x0008221C
			public override string ToString()
			{
				return string.Format("{0}, {1}, {2}, {3}", new object[]
				{
					this.Left,
					this.Top,
					this.Right,
					this.Bottom
				});
			}

			// Token: 0x04000C3B RID: 3131
			public int Left;

			// Token: 0x04000C3C RID: 3132
			public int Top;

			// Token: 0x04000C3D RID: 3133
			public int Right;

			// Token: 0x04000C3E RID: 3134
			public int Bottom;
		}

		// Token: 0x02000401 RID: 1025
		public struct WINDOWPOS
		{
			// Token: 0x04000C3F RID: 3135
			public IntPtr hwnd;

			// Token: 0x04000C40 RID: 3136
			public IntPtr hwndInsertAfter;

			// Token: 0x04000C41 RID: 3137
			public int x;

			// Token: 0x04000C42 RID: 3138
			public int y;

			// Token: 0x04000C43 RID: 3139
			public int cx;

			// Token: 0x04000C44 RID: 3140
			public int cy;

			// Token: 0x04000C45 RID: 3141
			public uint flags;
		}

		// Token: 0x02000402 RID: 1026
		public struct DISPLAY_DEVICE
		{
			// Token: 0x04000C46 RID: 3142
			[MarshalAs(UnmanagedType.U4)]
			public uint cb;

			// Token: 0x04000C47 RID: 3143
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
			public string DeviceName;

			// Token: 0x04000C48 RID: 3144
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
			public string DeviceString;

			// Token: 0x04000C49 RID: 3145
			[MarshalAs(UnmanagedType.U4)]
			public Win32.DisplayDeviceStateFlags StateFlags;

			// Token: 0x04000C4A RID: 3146
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
			public string DeviceID;

			// Token: 0x04000C4B RID: 3147
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
			public string DeviceKey;
		}

		// Token: 0x02000403 RID: 1027
		internal struct SHFILEINFO
		{
			// Token: 0x04000C4C RID: 3148
			public IntPtr hIcon;

			// Token: 0x04000C4D RID: 3149
			public IntPtr iIcon;

			// Token: 0x04000C4E RID: 3150
			public uint dwAttributes;

			// Token: 0x04000C4F RID: 3151
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
			public string szDisplayName;

			// Token: 0x04000C50 RID: 3152
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)]
			public string szTypeName;
		}

		// Token: 0x02000404 RID: 1028
		// (Invoke) Token: 0x0600216B RID: 8555
		public delegate bool EnumWindowProc(IntPtr hwnd, IntPtr lParam);

		// Token: 0x02000405 RID: 1029
		[Flags]
		public enum DisplayDeviceStateFlags : uint
		{
			// Token: 0x04000C52 RID: 3154
			AttachedToDesktop = 1u,
			// Token: 0x04000C53 RID: 3155
			MultiDriver = 2u,
			// Token: 0x04000C54 RID: 3156
			PrimaryDevice = 4u,
			// Token: 0x04000C55 RID: 3157
			MirroringDriver = 8u,
			// Token: 0x04000C56 RID: 3158
			VGACompatible = 22u,
			// Token: 0x04000C57 RID: 3159
			Removable = 32u,
			// Token: 0x04000C58 RID: 3160
			ModesPruned = 134217728u,
			// Token: 0x04000C59 RID: 3161
			Remote = 67108864u,
			// Token: 0x04000C5A RID: 3162
			Disconnect = 33554432u
		}

		// Token: 0x02000406 RID: 1030
		public enum DeviceCap
		{
			// Token: 0x04000C5C RID: 3164
			VERTRES = 10,
			// Token: 0x04000C5D RID: 3165
			LOGPIXELSY = 90,
			// Token: 0x04000C5E RID: 3166
			DESKTOPVERTRES = 117
		}

		// Token: 0x02000407 RID: 1031
		// (Invoke) Token: 0x0600216F RID: 8559
		public delegate IntPtr HookProc(int nCode, IntPtr wParam, IntPtr lParam);
    }
}
