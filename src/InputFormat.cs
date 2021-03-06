﻿//
// MIT License
// Copyright ©2010 Eric Maupin
// All rights reserved.

// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:

// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.

// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.

using System;
using System.Runtime.InteropServices;

namespace libavnet
{
	public unsafe class InputFormat
		: Format, IDisposable
	{
		internal InputFormat (IntPtr pInput)
		{
			if (pInput == IntPtr.Zero)
				throw new ArgumentException ("Null pointer", "pInput");

			this.pInput = pInput;
			this.format = (AVInputFormat*)pInput;

			Names = Marshal.PtrToStringAnsi (new IntPtr (this.format->name));
			Description = Marshal.PtrToStringAnsi (new IntPtr (this.format->long_name));
		}

		#region Cleanup
		public void Dispose()
		{
			Dispose (true);
			GC.SuppressFinalize (this);
		}

		protected void Dispose (bool disposing)
		{
			if (this.disposed)
				return;

			this.disposed = true;

			FFmpeg.ReadCloseCallback close =
				(FFmpeg.ReadCloseCallback)Marshal.GetDelegateForFunctionPointer (this.format->read_close, typeof (FFmpeg.ReadCloseCallback));

			close (this.pInput);
		}

		~InputFormat()
		{
			Dispose (false);
		}
		#endregion

		private bool disposed;
		private readonly IntPtr pInput;
		private readonly AVInputFormat* format;
	}
}