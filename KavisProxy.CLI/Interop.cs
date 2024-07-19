using System.Runtime.InteropServices;

namespace KavisProxy.CLI
{
	public unsafe class Interop
	{
		[DllImport("user32", SetLastError = true, CharSet = CharSet.Auto)]
		public static extern int MessageBox(IntPtr hWnd, string text, string caption, uint type);
		public static int MessageBox(string text) => MessageBox(0, text, "", 0);

		[DllImport("kernel32", CharSet = CharSet.Unicode, SetLastError = true)]
		public static extern IntPtr GetModuleHandle([MarshalAs(UnmanagedType.LPWStr)] string lpModuleName);

		[DllImport("kernel32", CharSet = CharSet.Ansi, ExactSpelling = true, SetLastError = true)]
		public static extern IntPtr GetProcAddress(IntPtr hModule, string procName);

		public static void* GetProcAddressPtr(IntPtr hModule, [MarshalAs(UnmanagedType.LPWStr)] string lpModuleName) => GetProcAddress(hModule, lpModuleName).ToPointer();

		[DllImport("kernel32", SetLastError = true, CharSet = CharSet.Ansi)]
		public static extern IntPtr LoadLibrary([MarshalAs(UnmanagedType.LPStr)] string lpFileName);

		[DllImport("kernel32")]
		public static extern IntPtr GetCurrentThread();


		[Flags]
		public enum LoadLibraryFlags : uint
		{
			None = 0,
			DONT_RESOLVE_DLL_REFERENCES = 0x00000001,
			LOAD_IGNORE_CODE_AUTHZ_LEVEL = 0x00000010,
			LOAD_LIBRARY_AS_DATAFILE = 0x00000002,
			LOAD_LIBRARY_AS_DATAFILE_EXCLUSIVE = 0x00000040,
			LOAD_LIBRARY_AS_IMAGE_RESOURCE = 0x00000020,
			LOAD_LIBRARY_SEARCH_APPLICATION_DIR = 0x00000200,
			LOAD_LIBRARY_SEARCH_DEFAULT_DIRS = 0x00001000,
			LOAD_LIBRARY_SEARCH_DLL_LOAD_DIR = 0x00000100,
			LOAD_LIBRARY_SEARCH_SYSTEM32 = 0x00000800,
			LOAD_LIBRARY_SEARCH_USER_DIRS = 0x00000400,
			LOAD_WITH_ALTERED_SEARCH_PATH = 0x00000008,
			LOAD_LIBRARY_REQUIRE_SIGNED_TARGET = 0x00000080,
			LOAD_LIBRARY_SAFE_CURRENT_DIRS = 0x00002000,
		}

		[DllImport("kernel32", SetLastError = true)]
		public static extern IntPtr LoadLibraryEx(string lpFileName, IntPtr hReservedNull, LoadLibraryFlags dwFlags);

		[DllImport("kernel32")]
		public static extern void Sleep(uint dwMilliseconds);

		private static int counter = 0;
		public static void Show(object message)
		{
			MessageBox(0, message.ToString(), counter.ToString(), 0);
			counter++;
		}

		/*
		[DllImport("kernel32", CharSet = CharSet.Auto, SetLastError = true)]
		private unsafe static extern uint CreateThread(uint* lpThreadAttributes, uint dwStackSize, ThreadStart lpStartAddress, uint* lpParameter, uint dwCreationFlags, out uint lpThreadId);

		[DllImport("kernel32", SetLastError = true)]
		public unsafe static extern bool CloseHandle(uint* hObject);
		 */

		[DllImport("kernel32")]
		public static unsafe extern IntPtr CreateThread(
			IntPtr lpThreadAttributes,
			uint dwStackSize,
			ThreadStart lpStartAddress,
			IntPtr lpParameter,
			uint dwCreationFlags,
			out uint lpThreadId);

		[DllImport("kernel32", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static unsafe extern bool CloseHandle(IntPtr hObject);

		public static nint StartThread(ThreadStart ThreadFunc)
		{
			uint lpThreadID = 0;
			nint dwHandle = CreateThread(IntPtr.Zero, 0, ThreadFunc, IntPtr.Zero, 0, out lpThreadID);
			return dwHandle;
		}
	}
}
