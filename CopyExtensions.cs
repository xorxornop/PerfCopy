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
using System.Runtime.CompilerServices;

namespace PerfCopy
{
    /// <summary>
    ///     Extension methods for copying data.
    /// </summary>
    public static class CopyExtensions
    {
        #region Constants

        private const int BufferBlockCopyThreshold = 1024;
#if INCLUDE_UNSAFE
        private const int UnmanagedThreshold = 128;
#endif

        #endregion

        #region CopyBytes (aliases of byte[] DeepCopy methods)

        /// <summary>
        ///     Produce a deep copy (copied value by value) of <paramref name="data" /> array.
        /// </summary>
        /// <param name="data">Array to produce a copy of.</param>
        /// <returns>Copy of <paramref name="data" /> array.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte[] CopyBytes(this byte[] data)
        {
            return data.DeepCopy();
        }

        /// <summary>
        ///     Copy bytes from <paramref name="src" /> array into <paramref name="dst" /> array.
        /// </summary>
        /// <param name="src">Array to copy from.</param>
        /// <param name="dst">Array to copy into.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyBytes(this byte[] src, byte[] dst)
        {
            src.DeepCopy(dst);
        }

        /// <summary>
        ///     Copy <paramref name="length" /> bytes from <paramref name="src" />
        ///     array into <paramref name="dst" /> array.
        /// </summary>
        /// <param name="src">Array to copy from.</param>
        /// <param name="dst">Array to copy into.</param>
        /// <param name="length">Amount of bytes to copy.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyBytes(this byte[] src, byte[] dst, int length)
        {
            src.DeepCopy(0, dst, 0, length);
        }

        /// <summary>
        ///     Copy <paramref name="length" /> bytes from <paramref name="src" />
        ///     array into <paramref name="dst" /> array.
        /// </summary>
        /// <param name="src">Array to copy from.</param>
        /// <param name="dst">Array to copy into.</param>
        /// <param name="length">Amount of bytes to copy.</param>
        /// <param name="srcOff">The offset in <paramref name="src" /> to copy from.</param>
        /// <param name="dstOff">The offset in <paramref name="dst" /> to copy into.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyBytes(this byte[] src, int srcOff, byte[] dst, int dstOff, int length)
        {
            src.DeepCopy(srcOff, dst, dstOff, length);
        }

        /// <summary>
        ///     Copy <paramref name="length" /> bytes from <paramref name="src" />
        ///     array into <paramref name="dst" /> array.
        ///     Argument/parameter validation is not performed - caution!
        /// </summary>
        /// <param name="src">Array to copy from.</param>
        /// <param name="dst">Array to copy into.</param>
        /// <param name="length">Amount of bytes to copy.</param>
        /// <param name="srcOff">The offset in <paramref name="src" /> to copy from.</param>
        /// <param name="dstOff">The offset in <paramref name="dst" /> to copy into.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyBytes_NoChecks(this byte[] src, int srcOff, byte[] dst, int dstOff, int length)
        {
            src.DeepCopy_NoChecks(srcOff, dst, dstOff, length);
        }

        #endregion

        #region Byte

        /// <summary>
        ///     Produce a deep copy (copied value by value) of <paramref name="data" /> array.
        /// </summary>
        /// <param name="data">Array to produce a copy of.</param>
        /// <returns>Copy of <paramref name="data" /> array.</returns>
        public static byte[] DeepCopy(this byte[] data)
        {
            if (data == null) {
                return null;
            }
            var dst = new byte[data.Length];
            data.DeepCopy_NoChecks(0, dst, 0, data.Length);
            return dst;
        }

        /// <summary>
        ///     Copy values from <paramref name="src" /> array into <paramref name="dst" /> array.
        /// </summary>
        /// <param name="src">Array to copy from.</param>
        /// <param name="dst">Array to copy into.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void DeepCopy(this byte[] src, byte[] dst)
        {
            Shared.ThrowOnInvalidArgument(src, dst, src.Length);
            DeepCopy_NoChecks(src, 0, dst, 0, src.Length);
        }

        /// <summary>
        ///     Copy <paramref name="length" /> values from <paramref name="src" /> array into <paramref name="dst" /> array.
        /// </summary>
        /// <param name="src">Array to copy from.</param>
        /// <param name="srcOff">The offset in <paramref name="src" /> to copy from.</param>
        /// <param name="dst">Array to copy into.</param>
        /// <param name="dstOff">The offset in <paramref name="dst" /> to copy into.</param>
        /// <param name="length">Quantity of values to copy.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void DeepCopy(this byte[] src, int srcOff, byte[] dst, int dstOff, int length)
        {
            Shared.ThrowOnInvalidArgument(src, dst, length, srcOff, dstOff);
            DeepCopy_NoChecks(src, srcOff, dst, dstOff, length);
        }

        /// <summary>
        ///     Copy <paramref name="length" /> values from <paramref name="src" /> array into <paramref name="dst" /> array.
        ///     Argument/parameter validation is not performed - caution!
        /// </summary>
        /// <param name="src">Array to copy from.</param>
        /// <param name="srcOff">The offset in <paramref name="src" /> to copy from.</param>
        /// <param name="dst">Array to copy into.</param>
        /// <param name="dstOff">The offset in <paramref name="dst" /> to copy into.</param>
        /// <param name="length">Quantity of values to copy.</param>
        public static void DeepCopy_NoChecks(this byte[] src, int srcOff, byte[] dst, int dstOff, int length)
        {
#if INCLUDE_UNSAFE
            if (length >= UnmanagedThreshold) {
                unsafe {
                    fixed (byte* srcPtr = &src[srcOff]) {
                        fixed (byte* dstPtr = &dst[dstOff]) {
                            CopyMemory(srcPtr, dstPtr, length);
                        }
                    }
                }
            } else {
#endif
                if (length >= BufferBlockCopyThreshold) {
                    Buffer.BlockCopy(src, srcOff, dst, dstOff, length);
                } else {
                    Array.Copy(src, srcOff, dst, dstOff, length);
                }
#if INCLUDE_UNSAFE
            }
#endif
        }

        #endregion

        #region Char (16-bit UTF16 character, a 16-bit unsigned integer)

        /// <summary>
        ///     Produce a deep copy (copied value by value) of <paramref name="data" /> array.
        /// </summary>
        /// <param name="data">Array to produce a copy of.</param>
        /// <returns>Copy of <paramref name="data" /> array.</returns>
        public static char[] DeepCopy(this char[] data)
        {
            if (data == null) {
                return null;
            }
            var dst = new char[data.Length];
            data.DeepCopy_NoChecks(0, dst, 0, data.Length);
            return dst;
        }

        /// <summary>
        ///     Copy values from <paramref name="src" /> array into <paramref name="dst" /> array.
        /// </summary>
        /// <param name="src">Array to copy from.</param>
        /// <param name="dst">Array to copy into.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void DeepCopy(this char[] src, char[] dst)
        {
            Shared.ThrowOnInvalidArgument(src, dst, src.Length);
            DeepCopy_NoChecks(src, 0, dst, 0, src.Length);
        }

        /// <summary>
        ///     Copy values from <paramref name="src" /> array into <paramref name="dst" /> array.
        /// </summary>
        /// <param name="src">Array to copy from.</param>
        /// <param name="srcOff">The offset in <paramref name="src" /> to copy from.</param>
        /// <param name="dst">Array to copy into.</param>
        /// <param name="dstOff">The offset in <paramref name="dst" /> to copy into.</param>
        /// <param name="length">Quantity of values to copy.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void DeepCopy(this char[] src, int srcOff, char[] dst, int dstOff, int length)
        {
            Shared.ThrowOnInvalidArgument(src, dst, length, srcOff, dstOff);
            DeepCopy_NoChecks(src, srcOff, dst, dstOff, length);
        }

        /// <summary>
        ///     Copy values from <paramref name="src" /> array into <paramref name="dst" /> array.
        ///     Argument/parameter validation is not performed - caution!
        /// </summary>
        /// <param name="src">Array to copy from.</param>
        /// <param name="srcOff">The offset in <paramref name="src" /> to copy from.</param>
        /// <param name="dst">Array to copy into.</param>
        /// <param name="dstOff">The offset in <paramref name="dst" /> to copy into.</param>
        /// <param name="length">Quantity of values to copy.</param>
        public static void DeepCopy_NoChecks(this char[] src, int srcOff, char[] dst, int dstOff, int length)
        {
#if INCLUDE_UNSAFE
            const int umLimit = UnmanagedThreshold / sizeof(char);
            if (length >= umLimit) {
                unsafe {
                    fixed (char* srcPtr = &src[srcOff]) {
                        fixed (char* dstPtr = &dst[dstOff]) {
                            CopyMemory((byte*) srcPtr, (byte*) dstPtr, length * sizeof(char));
                        }
                    }
                }
            } else {
#endif
                const int bcLimit = BufferBlockCopyThreshold / sizeof(char);
                if (length >= bcLimit) {
                    Buffer.BlockCopy(src, srcOff, dst, dstOff, length * sizeof(char));
                } else {
                    Array.Copy(src, srcOff, dst, dstOff, length);
                }
#if INCLUDE_UNSAFE
            }
#endif
        }

        #endregion

        #region Int16/short (16-bit signed integers)

        /// <summary>
        ///     Produce a deep copy (copied value by value) of <paramref name="data" /> array.
        /// </summary>
        /// <param name="data">Array to produce a copy of.</param>
        /// <returns>Copy of <paramref name="data" /> array.</returns>
        public static Int16[] DeepCopy(this Int16[] data)
        {
            if (data == null) {
                return null;
            }
            var dst = new Int16[data.Length];
            data.DeepCopy_NoChecks(0, dst, 0, data.Length);
            return dst;
        }

        /// <summary>
        ///     Copy values from <paramref name="src" /> array into <paramref name="dst" /> array.
        /// </summary>
        /// <param name="src">Array to copy from.</param>
        /// <param name="dst">Array to copy into.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void DeepCopy(this Int16[] src, Int16[] dst)
        {
            Shared.ThrowOnInvalidArgument(src, dst, src.Length);
            DeepCopy_NoChecks(src, 0, dst, 0, src.Length);
        }

        /// <summary>
        ///     Copy values from <paramref name="src" /> array into <paramref name="dst" /> array.
        /// </summary>
        /// <param name="src">Array to copy from.</param>
        /// <param name="srcOff">The offset in <paramref name="src" /> to copy from.</param>
        /// <param name="dst">Array to copy into.</param>
        /// <param name="dstOff">The offset in <paramref name="dst" /> to copy into.</param>
        /// <param name="length">Quantity of values to copy.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void DeepCopy(this Int16[] src, int srcOff, Int16[] dst, int dstOff, int length)
        {
            Shared.ThrowOnInvalidArgument(src, dst, length, srcOff, dstOff);
            DeepCopy_NoChecks(src, srcOff, dst, dstOff, length);
        }

        /// <summary>
        ///     Copy values from <paramref name="src" /> array into <paramref name="dst" /> array.
        ///     Argument/parameter validation is not performed - caution!
        /// </summary>
        /// <param name="src">Array to copy from.</param>
        /// <param name="srcOff">The offset in <paramref name="src" /> to copy from.</param>
        /// <param name="dst">Array to copy into.</param>
        /// <param name="dstOff">The offset in <paramref name="dst" /> to copy into.</param>
        /// <param name="length">Quantity of values to copy.</param>
        public static void DeepCopy_NoChecks(this Int16[] src, int srcOff, Int16[] dst, int dstOff, int length)
        {
#if INCLUDE_UNSAFE
            const int umLimit = UnmanagedThreshold / sizeof(Int16);
            if (length >= umLimit) {
                unsafe {
                    fixed (Int16* srcPtr = &src[srcOff]) {
                        fixed (Int16* dstPtr = &dst[dstOff]) {
                            CopyMemory((byte*) srcPtr, (byte*) dstPtr, length * sizeof(Int16));
                        }
                    }
                }
            } else {
#endif
                const int bcLimit = BufferBlockCopyThreshold / sizeof(Int16);
                if (length >= bcLimit) {
                    Buffer.BlockCopy(src, srcOff, dst, dstOff, length * sizeof(Int16));
                } else {
                    Array.Copy(src, srcOff, dst, dstOff, length);
                }
#if INCLUDE_UNSAFE
            }
#endif
        }

        #endregion

        #region UInt16/ushort (16-bit unsigned integers)

        /// <summary>
        ///     Produce a deep copy (copied value by value) of <paramref name="data" /> array.
        /// </summary>
        /// <param name="data">Array to produce a copy of.</param>
        /// <returns>Copy of <paramref name="data" /> array.</returns>
        public static UInt16[] DeepCopy(this UInt16[] data)
        {
            if (data == null) {
                return null;
            }
            var dst = new UInt16[data.Length];
            data.DeepCopy_NoChecks(0, dst, 0, data.Length);
            return dst;
        }

        /// <summary>
        ///     Copy values from <paramref name="src" /> array into <paramref name="dst" /> array.
        /// </summary>
        /// <param name="src">Array to copy from.</param>
        /// <param name="dst">Array to copy into.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void DeepCopy(this UInt16[] src, UInt16[] dst)
        {
            Shared.ThrowOnInvalidArgument(src, dst, src.Length);
            DeepCopy_NoChecks(src, 0, dst, 0, src.Length);
        }

        /// <summary>
        ///     Copy values from <paramref name="src" /> array into <paramref name="dst" /> array.
        /// </summary>
        /// <param name="src">Array to copy from.</param>
        /// <param name="srcOff">The offset in <paramref name="src" /> to copy from.</param>
        /// <param name="dst">Array to copy into.</param>
        /// <param name="dstOff">The offset in <paramref name="dst" /> to copy into.</param>
        /// <param name="length">Quantity of values to copy.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void DeepCopy(this UInt16[] src, int srcOff, UInt16[] dst, int dstOff, int length)
        {
            Shared.ThrowOnInvalidArgument(src, dst, length, srcOff, dstOff);
            DeepCopy_NoChecks(src, srcOff, dst, dstOff, length);
        }

        /// <summary>
        ///     Copy values from <paramref name="src" /> array into <paramref name="dst" /> array.
        ///     Argument/parameter validation is not performed - caution!
        /// </summary>
        /// <param name="src">Array to copy from.</param>
        /// <param name="srcOff">The offset in <paramref name="src" /> to copy from.</param>
        /// <param name="dst">Array to copy into.</param>
        /// <param name="dstOff">The offset in <paramref name="dst" /> to copy into.</param>
        /// <param name="length">Quantity of values to copy.</param>
        public static void DeepCopy_NoChecks(this UInt16[] src, int srcOff, UInt16[] dst, int dstOff, int length)
        {
#if INCLUDE_UNSAFE
            const int umLimit = UnmanagedThreshold / sizeof(UInt32);
            if (length >= umLimit) {
                unsafe {
                    fixed (UInt16* srcPtr = &src[srcOff]) {
                        fixed (UInt16* dstPtr = &dst[dstOff]) {
                            CopyMemory((byte*) srcPtr, (byte*) dstPtr, length * sizeof(UInt16));
                        }
                    }
                }
            } else {
#endif
                const int bcLimit = BufferBlockCopyThreshold / sizeof(UInt16);
                if (length >= bcLimit) {
                    Buffer.BlockCopy(src, srcOff, dst, dstOff, length * sizeof(UInt16));
                } else {
                    Array.Copy(src, srcOff, dst, dstOff, length);
                }
#if INCLUDE_UNSAFE
            }
#endif
        }

        #endregion

        #region Int32/int (32-bit signed integers)

        /// <summary>
        ///     Produce a deep copy (copied value by value) of <paramref name="data" /> array.
        /// </summary>
        /// <param name="data">Array to produce a copy of.</param>
        /// <returns>Copy of <paramref name="data" /> array.</returns>
        public static Int32[] DeepCopy(this Int32[] data)
        {
            if (data == null) {
                return null;
            }
            var dst = new Int32[data.Length];
            data.DeepCopy_NoChecks(0, dst, 0, data.Length);
            return dst;
        }

        /// <summary>
        ///     Copy values from <paramref name="src" /> array into <paramref name="dst" /> array.
        /// </summary>
        /// <param name="src">Array to copy from.</param>
        /// <param name="dst">Array to copy into.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void DeepCopy(this Int32[] src, Int32[] dst)
        {
            Shared.ThrowOnInvalidArgument(src, dst, src.Length);
            DeepCopy_NoChecks(src, 0, dst, 0, src.Length);
        }

        /// <summary>
        ///     Copy values from <paramref name="src" /> array into <paramref name="dst" /> array.
        /// </summary>
        /// <param name="src">Array to copy from.</param>
        /// <param name="srcOff">The offset in <paramref name="src" /> to copy from.</param>
        /// <param name="dst">Array to copy into.</param>
        /// <param name="dstOff">The offset in <paramref name="dst" /> to copy into.</param>
        /// <param name="length">Quantity of values to copy.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void DeepCopy(this Int32[] src, int srcOff, Int32[] dst, int dstOff, int length)
        {
            Shared.ThrowOnInvalidArgument(src, dst, length, srcOff, dstOff);
            DeepCopy_NoChecks(src, srcOff, dst, dstOff, length);
        }

        /// <summary>
        ///     Copy values from <paramref name="src" /> array into <paramref name="dst" /> array.
        ///     Argument/parameter validation is not performed - caution!
        /// </summary>
        /// <param name="src">Array to copy from.</param>
        /// <param name="srcOff">The offset in <paramref name="src" /> to copy from.</param>
        /// <param name="dst">Array to copy into.</param>
        /// <param name="dstOff">The offset in <paramref name="dst" /> to copy into.</param>
        /// <param name="length">Quantity of values to copy.</param>
        public static void DeepCopy_NoChecks(this Int32[] src, int srcOff, Int32[] dst, int dstOff, int length)
        {
#if INCLUDE_UNSAFE
            const int umLimit = UnmanagedThreshold / sizeof(Int32);
            if (length >= umLimit) {
                unsafe {
                    fixed (Int32* srcPtr = &src[srcOff]) {
                        fixed (Int32* dstPtr = &dst[dstOff]) {
                            CopyMemory((byte*) srcPtr, (byte*) dstPtr, length * sizeof(Int32));
                        }
                    }
                }
            } else {
#endif
                const int bcLimit = BufferBlockCopyThreshold / sizeof(Int32);
                if (length >= bcLimit) {
                    Buffer.BlockCopy(src, srcOff, dst, dstOff, length * sizeof(Int32));
                } else {
                    Array.Copy(src, srcOff, dst, dstOff, length);
                }
#if INCLUDE_UNSAFE
            }
#endif
        }

        #endregion

        #region UInt32/uint (32-bit unsigned integers)

        /// <summary>
        ///     Produce a deep copy (copied value by value) of <paramref name="data" /> array.
        /// </summary>
        /// <param name="data">Array to produce a copy of.</param>
        /// <returns>Copy of <paramref name="data" /> array.</returns>
        public static UInt32[] DeepCopy(this UInt32[] data)
        {
            if (data == null) {
                return null;
            }
            var dst = new UInt32[data.Length];
            data.DeepCopy_NoChecks(0, dst, 0, data.Length);
            return dst;
        }

        /// <summary>
        ///     Copy values from <paramref name="src" /> array into <paramref name="dst" /> array.
        /// </summary>
        /// <param name="src">Array to copy from.</param>
        /// <param name="dst">Array to copy into.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void DeepCopy(this UInt32[] src, UInt32[] dst)
        {
            Shared.ThrowOnInvalidArgument(src, dst, src.Length);
            DeepCopy_NoChecks(src, 0, dst, 0, src.Length);
        }

        /// <summary>
        ///     Copy values from <paramref name="src" /> array into <paramref name="dst" /> array.
        /// </summary>
        /// <param name="src">Array to copy from.</param>
        /// <param name="srcOff">The offset in <paramref name="src" /> to copy from.</param>
        /// <param name="dst">Array to copy into.</param>
        /// <param name="dstOff">The offset in <paramref name="dst" /> to copy into.</param>
        /// <param name="length">Quantity of values to copy.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void DeepCopy(this UInt32[] src, int srcOff, UInt32[] dst, int dstOff, int length)
        {
            Shared.ThrowOnInvalidArgument(src, dst, length, srcOff, dstOff);
            DeepCopy_NoChecks(src, srcOff, dst, dstOff, length);
        }

        /// <summary>
        ///     Copy values from <paramref name="src" /> array into <paramref name="dst" /> array.
        ///     Argument/parameter validation is not performed - caution!
        /// </summary>
        /// <param name="src">Array to copy from.</param>
        /// <param name="srcOff">The offset in <paramref name="src" /> to copy from.</param>
        /// <param name="dst">Array to copy into.</param>
        /// <param name="dstOff">The offset in <paramref name="dst" /> to copy into.</param>
        /// <param name="length">Quantity of values to copy.</param>
        public static void DeepCopy_NoChecks(this UInt32[] src, int srcOff, UInt32[] dst, int dstOff, int length)
        {
#if INCLUDE_UNSAFE
            const int umLimit = UnmanagedThreshold / sizeof(UInt32);
            if (length >= umLimit) {
                unsafe {
                    fixed (UInt32* srcPtr = &src[srcOff]) {
                        fixed (UInt32* dstPtr = &dst[dstOff]) {
                            CopyMemory((byte*) srcPtr, (byte*) dstPtr, length * sizeof(UInt32));
                        }
                    }
                }
            } else {
#endif
                const int bcLimit = BufferBlockCopyThreshold / sizeof(UInt32);
                if (length >= bcLimit) {
                    Buffer.BlockCopy(src, srcOff, dst, dstOff, length * sizeof(UInt32));
                } else {
                    Array.Copy(src, srcOff, dst, dstOff, length);
                }
#if INCLUDE_UNSAFE
            }
#endif
        }

        #endregion

        #region Int64/long (64-bit signed integers)

        /// <summary>
        ///     Produce a deep copy (copied value by value) of <paramref name="data" /> array.
        /// </summary>
        /// <param name="data">Array to produce a copy of.</param>
        /// <returns>Copy of <paramref name="data" /> array.</returns>
        public static Int64[] DeepCopy(this Int64[] data)
        {
            if (data == null) {
                return null;
            }
            var dst = new Int64[data.Length];
            data.DeepCopy_NoChecks(0, dst, 0, data.Length);
            return dst;
        }

        /// <summary>
        ///     Copy values from <paramref name="src" /> array into <paramref name="dst" /> array.
        /// </summary>
        /// <param name="src">Array to copy from.</param>
        /// <param name="dst">Array to copy into.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void DeepCopy(this Int64[] src, Int64[] dst)
        {
            Shared.ThrowOnInvalidArgument(src, dst, src.Length);
            DeepCopy_NoChecks(src, 0, dst, 0, src.Length);
        }

        /// <summary>
        ///     Copy values from <paramref name="src" /> array into <paramref name="dst" /> array.
        /// </summary>
        /// <param name="src">Array to copy from.</param>
        /// <param name="srcOff">The offset in <paramref name="src" /> to copy from.</param>
        /// <param name="dst">Array to copy into.</param>
        /// <param name="dstOff">The offset in <paramref name="dst" /> to copy into.</param>
        /// <param name="length">Quantity of values to copy.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void DeepCopy(this Int64[] src, int srcOff, Int64[] dst, int dstOff, int length)
        {
            Shared.ThrowOnInvalidArgument(src, dst, length, srcOff, dstOff);
            DeepCopy_NoChecks(src, srcOff, dst, dstOff, length);
        }

        /// <summary>
        ///     Copy values from <paramref name="src" /> array into <paramref name="dst" /> array.
        ///     Argument/parameter validation is not performed - caution!
        /// </summary>
        /// <param name="src">Array to copy from.</param>
        /// <param name="srcOff">The offset in <paramref name="src" /> to copy from.</param>
        /// <param name="dst">Array to copy into.</param>
        /// <param name="dstOff">The offset in <paramref name="dst" /> to copy into.</param>
        /// <param name="length">Quantity of values to copy.</param>
        public static void DeepCopy_NoChecks(this Int64[] src, int srcOff, Int64[] dst, int dstOff, int length)
        {
#if INCLUDE_UNSAFE
            const int umLimit = UnmanagedThreshold / sizeof(Int64);
            if (length >= umLimit) {
                unsafe {
                    fixed (Int64* srcPtr = &src[srcOff]) {
                        fixed (Int64* dstPtr = &dst[dstOff]) {
                            CopyMemory((byte*) srcPtr, (byte*) dstPtr, length * sizeof(Int64));
                        }
                    }
                }
            } else {
#endif
                const int bcLimit = BufferBlockCopyThreshold / sizeof(Int64);
                if (length >= bcLimit) {
                    Buffer.BlockCopy(src, srcOff, dst, dstOff, length * sizeof(Int64));
                } else {
                    Array.Copy(src, srcOff, dst, dstOff, length);
                }
#if INCLUDE_UNSAFE
            }
#endif
        }

        #endregion

        #region UInt64/ulong (64-bit unsigned integers)

        /// <summary>
        ///     Produce a deep copy (copied value by value) of <paramref name="data" /> array.
        /// </summary>
        /// <param name="data">Array to produce a copy of.</param>
        /// <returns>Copy of <paramref name="data" /> array.</returns>
        public static UInt64[] DeepCopy(this UInt64[] data)
        {
            if (data == null) {
                return null;
            }
            var dst = new UInt64[data.Length];
            data.DeepCopy_NoChecks(0, dst, 0, data.Length);
            return dst;
        }

        /// <summary>
        ///     Copy values from <paramref name="src" /> array into <paramref name="dst" /> array.
        /// </summary>
        /// <param name="src">Array to copy from.</param>
        /// <param name="dst">Array to copy into.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void DeepCopy(this UInt64[] src, UInt64[] dst)
        {
            Shared.ThrowOnInvalidArgument(src, dst, src.Length);
            DeepCopy_NoChecks(src, 0, dst, 0, src.Length);
        }

        /// <summary>
        ///     Copy values from <paramref name="src" /> array into <paramref name="dst" /> array.
        /// </summary>
        /// <param name="src">Array to copy from.</param>
        /// <param name="srcOff">The offset in <paramref name="src" /> to copy from.</param>
        /// <param name="dst">Array to copy into.</param>
        /// <param name="dstOff">The offset in <paramref name="dst" /> to copy into.</param>
        /// <param name="length">Quantity of values to copy.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void DeepCopy(this UInt64[] src, int srcOff, UInt64[] dst, int dstOff, int length)
        {
            Shared.ThrowOnInvalidArgument(src, dst, length, srcOff, dstOff);
            DeepCopy_NoChecks(src, srcOff, dst, dstOff, length);
        }

        /// <summary>
        ///     Copy values from <paramref name="src" /> array into <paramref name="dst" /> array.
        ///     Argument/parameter validation is not performed - caution!
        /// </summary>
        /// <param name="src">Array to copy from.</param>
        /// <param name="srcOff">The offset in <paramref name="src" /> to copy from.</param>
        /// <param name="dst">Array to copy into.</param>
        /// <param name="dstOff">The offset in <paramref name="dst" /> to copy into.</param>
        /// <param name="length">Quantity of values to copy.</param>
        public static void DeepCopy_NoChecks(this UInt64[] src, int srcOff, UInt64[] dst, int dstOff, int length)
        {
#if INCLUDE_UNSAFE
            const int umLimit = UnmanagedThreshold / sizeof(UInt64);
            if (length >= umLimit) {
                unsafe {
                    fixed (UInt64* srcPtr = &src[srcOff]) {
                        fixed (UInt64* dstPtr = &dst[dstOff]) {
                            CopyMemory((byte*) srcPtr, (byte*) dstPtr, length * sizeof(UInt64));
                        }
                    }
                }
            } else {
#endif
                const int bcLimit = BufferBlockCopyThreshold / sizeof(UInt64);
                if (length >= bcLimit) {
                    Buffer.BlockCopy(src, srcOff, dst, dstOff, length * sizeof(UInt64));
                } else {
                    Array.Copy(src, srcOff, dst, dstOff, length);
                }
#if INCLUDE_UNSAFE
            }
#endif
        }

        #endregion

        #region Utility method(s)

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static int DivRem(int dividend, int divisor, out int remainder)
        {
            int quotient = dividend / divisor;
            remainder = dividend - (divisor * quotient);
            return quotient;
        }

#if INCLUDE_UNSAFE

        /// <summary>
        ///     Copy data from <paramref name="srcPtr" /> into <paramref name="dstPtr" />.
        /// </summary>
        /// <remarks>
        ///     If <paramref name="srcPtr" /> or <paramref name="dstPtr" /> are not originally for byte-oriented data,
        ///     length will need to be adjusted accordingly, e.g. UInt3232 pointer vs. byte pointer = 4x length.
        ///     Method auto-optimises for word size (32/64-bit) on the machine ISA.
        /// </remarks>
        /// <param name="srcPtr">Pointer to source of data.</param>
        /// <param name="dstPtr">Pointer to destination for data.</param>
        /// <param name="length">Length/quantity of data to copy, in bytes.</param>
        public static unsafe void CopyMemory(byte* srcPtr, byte* dstPtr, int length)
        {
            const int u32Size = sizeof(UInt32);
            if (Shared.PlatformWordSize == sizeof(UInt32)) {
                int remainingBytes;
                int words32 = DivRem(length, u32Size, out remainingBytes);
                var src32Ptr = (UInt32*)srcPtr;
                var dst32Ptr = (UInt32*)dstPtr;
                for (int i = 0; i < words32; i += 2) {
                    *(dst32Ptr + i) = *(src32Ptr + i);
                    *(dst32Ptr + i + 1) = *(src32Ptr + i + 1);
                }
                if (remainingBytes >= u32Size) {
                    *(dst32Ptr + words32) = *(src32Ptr + words32);
                    words32++;
                    remainingBytes -= u32Size;
                }
                srcPtr += words32 * u32Size;
                dstPtr += words32 * u32Size;
                length = remainingBytes;
            } else if (Shared.PlatformWordSize == sizeof(UInt64)) {
                const int u64Size = sizeof(UInt64);
                int remainingBytes;
                int words64 = DivRem(length, u64Size, out remainingBytes);
                var src64Ptr = (UInt64*)srcPtr;
                var dst64Ptr = (UInt64*)dstPtr;
                for (int i = 0; i < words64; i += 2) {
                    *(dst64Ptr + i) = *(src64Ptr + i);
                    *(dst64Ptr + i + 1) = *(src64Ptr + i + 1);
                }
                if (remainingBytes >= u64Size) {
                    *(dst64Ptr + words64) = *(src64Ptr + words64);
                    words64++;
                    remainingBytes -= u64Size;
                }
                if (remainingBytes >= u32Size) {
                    *(UInt32*)(dst64Ptr + words64) = *(UInt32*)(src64Ptr + words64);
                    dstPtr += u32Size;
                    srcPtr += u32Size;
                    remainingBytes -= u32Size;
                }
                srcPtr += words64 * u64Size;
                dstPtr += words64 * u64Size;
                length = remainingBytes;
            }

            if (length >= u32Size) {
                *(UInt32*)dstPtr = *(UInt32*)srcPtr;
                dstPtr += u32Size;
                srcPtr += u32Size;
                length -= u32Size;
            }

            if (length >= sizeof(UInt16)) {
                *(UInt16*)dstPtr = *(UInt16*)srcPtr;
                dstPtr += sizeof(UInt16);
                srcPtr += sizeof(UInt16);
                length -= sizeof(UInt16);
            }

            if (length > 0) {
                *dstPtr = *srcPtr;
            }
        }
#endif

        #endregion
    }
}
