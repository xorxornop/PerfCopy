#region License

//  	Copyright 2013-2014 Matthew Ducker
//  	
//  	Licensed under the Apache License, Version 2.0 (the "License");
//  	you may not use this file except in compliance with the License.
//  	
//  	You may obtain a copy of the License at
//  		
//  		http://www.apache.org/licenses/LICENSE-2.0
//  	
//  	Unless required by applicable law or agreed to in writing, software
//  	distributed under the License is distributed on an "AS IS" BASIS,
//  	WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//  	See the License for the specific language governing permissions and 
//  	limitations under the License.

#endregion

using System;

namespace FastCopyExtensions
{
    internal class Shared
    {
#if INCLUDE_UNSAFE
        internal static readonly int PlatformWordSize = IntPtr.Size;
        internal static readonly int PlatformWordSizeBits = PlatformWordSize * 8;
#endif

        internal static void ThrowOnInvalidArgument<T>(T[] src, T[] dst, int length, int srcOff = 0, int dstOff = 0) where T : struct
        {
            if (src == null) {
                throw new ArgumentNullException("src");
            } else if (src.Length < 0) {
                throw new ArgumentException("src.Length < 0", "src");
            }
            if (dst == null) {
                throw new ArgumentNullException("dst");
            } else if (dst.Length < 0) {
                throw new ArgumentException("dst.Length < 0", "dst");
            }
            if (srcOff < 0) {
                throw new ArgumentOutOfRangeException("srcOff", "srcOff < 0");
            }
            if (dstOff < 0) {
                throw new ArgumentOutOfRangeException("dstOff", "dstOff < 0");
            }
            int srcLength = src.Length;
            int dstLength = dst.Length;

            if (length < 0 || length > srcLength || length > dstLength) {
                throw new ArgumentOutOfRangeException("length", "length < 0, and/or length > src.Length and/or length > dst.Length");
            }
            if (srcOff > 0 && srcOff + length > srcLength) {
                if (srcOff >= srcLength) {
                    throw new ArgumentOutOfRangeException("srcOff", "srcOff >= srcLength");
                }
                // More common case
                throw new ArgumentException("srcOff + length > src.Length", "srcOff");
            }
            if (dstOff > 0 && dstOff + length > dstLength) {
                if (dstOff >= dstLength) {
                    throw new ArgumentOutOfRangeException("dstOff", "dstOff >= dstLength");
                }
                // More common case
                throw new ArgumentException("dstOff + length > dstLength", "dstOff");
            }
        }

//        internal static int SizeOf<T>() where T : struct
//        {
//            var typeOfT = typeof(T);
//            if (typeOfT == typeof(byte)) {
//                return 1;
//            } else if (typeOfT == typeof(char)) {
//                return sizeof(char);
//            } else if (typeOfT == typeof(short) || typeOfT == typeof(ushort)) {
//                return sizeof(short);
//            } else if (typeOfT == typeof(int) || typeOfT == typeof(uint)) {
//                return sizeof(int);
//            } else if (typeOfT == typeof(long) || typeOfT == typeof(ulong)) {
//                return sizeof(long);
//            } else if (typeOfT == typeof(float)) {
//                return sizeof(float);
//            } else if (typeOfT == typeof(double)) {
//                return sizeof(double);
//            }
//            // Other type
//            throw new NotSupportedException("T : " + typeof(T).Name + " - Not a supported type.");
//        }
    }
}
