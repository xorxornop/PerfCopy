#region License

//  	Copyright 2013-2015 Matthew Ducker
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

namespace PerfCopy
{
    internal class Shared
    {
#if INCLUDE_UNSAFE
        internal static readonly int PlatformWordSize = IntPtr.Size;
        internal static readonly int PlatformWordSizeBits = PlatformWordSize * 8;
#endif

        /// <summary>
        ///     Used to verify arguments for a method of the form "copy <paramref name="length" /> items, 
        ///     possibly with modification, from <paramref name="src" />[<paramref name="srcOff" />] to 
        ///     <paramref name="dst" />[<paramref name="dstOff" />].".
        /// </summary>
        /// <typeparam name="T">Type of the source and destination arrays.</typeparam>
        /// <param name="src">Source data array.</param>
        /// <param name="dst">Destination array for data.</param>
        /// <param name="length">Number of items to copy from <paramref name="src"/> into <paramref name="dst"/>.</param>
        /// <param name="srcOff">Offset in <paramref name="src"/> to read from.</param>
        /// <param name="dstOff">Offset in <paramref name="dst"/> to write to.</param>
        /// <param name="srcName">
        ///     Name of the argument for <paramref name="src"/>. 
        ///     Set to null (default) if existing name matches.
        /// </param>
        /// <param name="dstName">
        ///     Name of the argument for <paramref name="dst"/>. 
        ///     Set to null (default) if existing name matches.
        /// </param>
        /// <param name="lengthName">
        ///     Name of the argument for <paramref name="length"/>. 
        ///     Set to null (default) if existing name matches.
        /// </param>
        /// <param name="srcOffName">
        ///     Name of the argument for <paramref name="srcOff"/>. 
        ///     Set to null (default) if existing name matches.
        /// </param>
        /// <param name="dstOffName">
        ///     Name of the argument for <paramref name="dstOff"/>. 
        ///     Set to null (default) if existing name matches.
        /// </param>
        internal static void ThrowOnInvalidArgument<T>(
            T[] src, T[] dst, int length, int srcOff = 0, int dstOff = 0,
            string srcName = null, string dstName = null, string lengthName = null, string srcOffName = null, string dstOffName = null) where T : struct
        {
            if (src == null) {
                throw new ArgumentNullException(srcName ?? "src");
            }
            int srcLength = src.Length;
            if (src.Length < 0) {
                throw new ArgumentException(String.Format("{0}.Length < 0 : {1} < 0", srcName ?? "src", srcLength), srcName ?? "src");
            }

            if (dst == null) {
                throw new ArgumentNullException(dstName ?? "dst");
            }
            int dstLength = dst.Length;
            if (dst.Length < 0) {
                throw new ArgumentException(String.Format("{0}.Length < 0 : {1} < 0", dstName ?? "dst", dstLength), dstName ?? "dst");
            }

            if (srcOff != 0 || dstOff != 0 || length != srcLength) {
                if (length < 0) {
                    throw new ArgumentOutOfRangeException(lengthName ?? "length",
                        String.Format("{0} < 0 : {1} < 0", lengthName ?? "length", length));
                }
                // Check source values
                if (srcOff + length > srcLength) {
                    if (srcOff >= srcLength) {
                        throw new ArgumentException(
                            String.Format("{0} >= {1}.Length : {2} >= {3}",
                                srcOffName ?? "srcOff", srcName ?? "src", srcOff, srcLength));
                    } else if (length > srcLength) {
                        throw new ArgumentOutOfRangeException(lengthName ?? "length",
                            String.Format("{0} > {1}.Length : {2} > {3}",
                                lengthName ?? "length", srcName ?? "src", length, srcLength));
                    } else {
                        // Either the array is smaller than expected/desired, 
                        // or the chosen offset and/or length are for a different size array...
                        throw new ArgumentException(
                            String.Format("{0} + {1} > {2}.Length : {3} + {4} > {5}",
                                srcOffName ?? "srcOff", lengthName ?? "length", srcName ?? "src",
                                srcOff, length, srcLength));
                    }
                } else if (srcOff < 0) {
                    throw new ArgumentOutOfRangeException(srcOffName ?? "srcOff",
                        String.Format("{0} < 0 : {1} < 0",
                            srcOffName ?? "srcOff", srcOff));
                }
                // Check destination values
                if (dstOff + length > dstLength) {
                    if (dstOff >= dstLength) {
                        throw new ArgumentException(
                            String.Format("{0} >= {1} : {2} >= {3}",
                                dstOffName ?? "dstOff", dstName ?? "dst", dstOff, dstLength));
                    } else if (length > dstLength) {
                        throw new ArgumentOutOfRangeException(lengthName ?? "length",
                            String.Format("{0} > {1}.Length : {2} > {3}",
                                lengthName ?? "length", dstName ?? "dst", length, dstLength));
                    } else {
                        // Either the array is smaller than expected/desired, 
                        // or the chosen offset and/or length are for a different size array...
                        throw new ArgumentException(
                            String.Format("{0} + {1} > {2}.Length : {3} + {4} > {5}",
                                dstOffName ?? "dstOff", lengthName ?? "length", dstName ?? "dst",
                                dstOff, length, dstLength));
                    }
                } else if (dstOff < 0) {
                    throw new ArgumentOutOfRangeException(dstOffName ?? "dstOff",
                        String.Format("{0} < 0 : {1} < 0",
                            dstOffName ?? "dstOff", dstOff));
                }
            }
        }
    }
}
