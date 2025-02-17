﻿using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;

using BizHawk.Common;

using static BizHawk.Common.ShlobjImports;

namespace BizHawk.Client.EmuHawk
{
	/// <summary>
	/// Component wrapping access to the Browse For Folder common dialog box.
	/// Call the ShowDialog() method to bring the dialog box up.
	/// </summary>
	/// <remarks>
	/// I believe this code is from http://support.microsoft.com/kb/306285<br/>
	/// The license is assumed to be effectively public domain.<br/>
	/// I saw a version of it with at least one bug fixed at https://github.com/slavat/MailSystem.NET/blob/master/Queuing%20System/ActiveQLibrary/CustomControl/FolderBrowser.cs<br/>
	/// --zeromus
	/// </remarks>
	public sealed class FolderBrowserEx : Component
	{
		private const BROWSEINFO.FLAGS BrowseOptions = BROWSEINFO.FLAGS.RestrictToFilesystem | BROWSEINFO.FLAGS.RestrictToDomain |
			BROWSEINFO.FLAGS.NewDialogStyle | BROWSEINFO.FLAGS.ShowTextBox;

		public string Description = "Please select a folder below:";

		public string SelectedPath;

		/// <summary>Shows the folder browser dialog box with the specified owner window.</summary>
		public DialogResult ShowDialog(IWin32Window owner = null)
		{
			const int startLocation = 0; // = Desktop CSIDL
			int Callback(IntPtr hwnd, uint uMsg, IntPtr lParam, IntPtr lpData)
			{
				if (uMsg == BFFM_INITIALIZED)
				{
					var str = Marshal.StringToHGlobalUni(SelectedPath);
					try
					{
						WmImports.SendMessage(hwnd, BFFM_SETSELECTIONW, new(1), str);
					}
					finally
					{
						Marshal.FreeHGlobal(str);
					}
				}

				return 0;
			}

			var hWndOwner = owner?.Handle ?? WmImports.GetActiveWindow();
			_ = SHGetSpecialFolderLocation(hWndOwner, startLocation, out var pidlRoot);
			if (pidlRoot == IntPtr.Zero)
			{
				return DialogResult.Cancel;
			}

			var browseOptions = BrowseOptions;
			if (ApartmentState.MTA == Application.OleRequired())
			{
				browseOptions &= ~BROWSEINFO.FLAGS.NewDialogStyle;
			}

			var pidlRet = IntPtr.Zero;
			try
			{
				var buffer = Marshal.AllocHGlobal(Win32Imports.MAX_PATH);
				var bi = new BROWSEINFO
				{
					hwndOwner = hWndOwner,
					pidlRoot = pidlRoot,
					pszDisplayName = buffer,
					lpszTitle = Description,
					ulFlags = browseOptions,
					lpfn = Callback
				};

				pidlRet = SHBrowseForFolder(ref bi);
				Marshal.FreeHGlobal(buffer);
				if (pidlRet == IntPtr.Zero)
				{
					return DialogResult.Cancel; // user clicked Cancel
				}

				var path = new StringBuilder(Win32Imports.MAX_PATH);
				if (SHGetPathFromIDList(pidlRet, path) == 0)
				{
					return DialogResult.Cancel;
				}

				SelectedPath = path.ToString();
			}
			finally
			{
				_ = SHGetMalloc(out var malloc);
				malloc.Free(pidlRoot);

				if (pidlRet != IntPtr.Zero)
				{
					malloc.Free(pidlRet);
				}
			}

			return DialogResult.OK;
		}
	}
}
