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
using System.Runtime.CompilerServices;

namespace PerfCopy
{
    /// <summary>
    ///     Extension methods for copying data.
    /// </summary>
    public static class CopyExtensions
    {
        private const int BufferBlockCopyThreshold = 1024;
#if INCLUDE_UNSAFE
        private const int UnmanagedThreshold = 128;
#endif

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
            data.DeepCopy(0, dst, 0, data.Length);
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

        /// <summary>
        ///     Produce a deep copy (copied value by value) of <paramref name="data" /> array.
        /// </summary>
        /// <param name="data">Array to produce a copy of.</param>
        /// <returns>Copy of <paramref name="data" /> array.</returns>
        public static int[] DeepCopy(this int[] data)
        {
            if (data == null) {
                return null;
            }
            var dst = new int[data.Length];
            data.DeepCopy(0, dst, 0, data.Length);
            return dst;
        }

        /// <summary>
        ///     Copy values from <paramref name="src" /> array into <paramref name="dst" /> array.
        /// </summary>
        /// <param name="src">Array to copy from.</param>
        /// <param name="dst">Array to copy into.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void DeepCopy(this int[] src, int[] dst)
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
        public static void DeepCopy(this int[] src, int srcOff, int[] dst, int dstOff, int length)
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
        public static void DeepCopy_NoChecks(this int[] src, int srcOff, int[] dst, int dstOff, int length)
        {
#if INCLUDE_UNSAFE
            const int umLimit = UnmanagedThreshold / sizeof(int);
            if (length >= umLimit) {
                unsafe {
                    fixed (int* srcPtr = &src[srcOff]) {
                        fixed (int* dstPtr = &dst[dstOff]) {
                            CopyMemory((byte*)srcPtr, (byte*)dstPtr, length * sizeof(int));
                        }
                    }
                }
            } else {
#endif
                const int bcLimit = BufferBlockCopyThreshold / sizeof(int);
                if (length >= bcLimit) {
                    Buffer.BlockCopy(src, srcOff, dst, dstOff, length * sizeof(int));
                } else {
                    Array.Copy(src, srcOff, dst, dstOff, length);
                }
#if INCLUDE_UNSAFE
            }
#endif
        }

        /// <summary>
        ///     Produce a deep copy (copied value by value) of <paramref name="data" /> array.
        /// </summary>
        /// <param name="data">Array to produce a copy of.</param>
        /// <returns>Copy of <paramref name="data" /> array.</returns>
        public static uint[] DeepCopy(this uint[] data)
        {
            if (data == null) {
                return null;
            }
            var dst = new uint[data.Length];
            data.DeepCopy(0, dst, 0, data.Length);
            return dst;
        }

        /// <summary>
        ///     Copy values from <paramref name="src" /> array into <paramref name="dst" /> array.
        /// </summary>
        /// <param name="src">Array to copy from.</param>
        /// <param name="dst">Array to copy into.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void DeepCopy(this uint[] src, uint[] dst)
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
        public static void DeepCopy(this uint[] src, int srcOff, uint[] dst, int dstOff, int length)
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
        public static void DeepCopy_NoChecks(this uint[] src, int srcOff, uint[] dst, int dstOff, int length)
        {
#if INCLUDE_UNSAFE
            const int umLimit = UnmanagedThreshold / sizeof(uint);
            if (length >= umLimit) {
                unsafe {
                    fixed (uint* srcPtr = &src[srcOff]) {
                        fixed (uint* dstPtr = &dst[dstOff]) {
                            CopyMemory((byte*)srcPtr, (byte*)dstPtr, length * sizeof(uint));
                        }
                    }
                }
            } else {
#endif
                const int bcLimit = BufferBlockCopyThreshold / sizeof(uint);
                if (length >= bcLimit) {
                    Buffer.BlockCopy(src, srcOff, dst, dstOff, length * sizeof(uint));
                } else {
                    Array.Copy(src, srcOff, dst, dstOff, length);
                }
#if INCLUDE_UNSAFE
            }
#endif
        }

        /// <summary>
        ///     Produce a deep copy (copied value by value) of <paramref name="data" /> array.
        /// </summary>
        /// <param name="data">Array to produce a copy of.</param>
        /// <returns>Copy of <paramref name="data" /> array.</returns>
        public static long[] DeepCopy(this long[] data)
        {
            if (data == null) {
                return null;
            }
            var dst = new long[data.Length];
            data.DeepCopy(0, dst, 0, data.Length);
            return dst;
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
        public static void DeepCopy(this long[] src, int srcOff, long[] dst, int dstOff, int length)
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
        public static void DeepCopy_NoChecks(this long[] src, int srcOff, long[] dst, int dstOff, int length)
        {
#if INCLUDE_UNSAFE
            const int umLimit = UnmanagedThreshold / sizeof(long);
            if (length >= umLimit) {
                unsafe {
                    fixed (long* srcPtr = &src[srcOff]) {
                        fixed (long* dstPtr = &dst[dstOff]) {
                            CopyMemory((byte*)srcPtr, (byte*)dstPtr, length * sizeof(long));
                        }
                    }
                }
            } else {
#endif
                const int bcLimit = BufferBlockCopyThreshold / sizeof(long);
                if (length >= bcLimit) {
                    Buffer.BlockCopy(src, srcOff, dst, dstOff, length * sizeof(long));
                } else {
                    Array.Copy(src, srcOff, dst, dstOff, length);
                }
#if INCLUDE_UNSAFE
            }
#endif
        }

        /// <summary>
        ///     Produce a deep copy (copied value by value) of <paramref name="data" /> array.
        /// </summary>
        /// <param name="data">Array to produce a copy of.</param>
        /// <returns>Copy of <paramref name="data" /> array.</returns>
        public static ulong[] DeepCopy(this ulong[] data)
        {
            if (data == null) {
                return null;
            }
            var dst = new ulong[data.Length];
            data.DeepCopy(0, dst, 0, data.Length);
            return dst;
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
        public static void DeepCopy(this ulong[] src, int srcOff, ulong[] dst, int dstOff, int length)
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
        public static void DeepCopy_NoChecks(this ulong[] src, int srcOff, ulong[] dst, int dstOff, int length)
        {
#if INCLUDE_UNSAFE
            const int umLimit = UnmanagedThreshold / sizeof(ulong);
            if (length >= umLimit) {
                unsafe {
                    fixed (ulong* srcPtr = &src[srcOff]) {
                        fixed (ulong* dstPtr = &dst[dstOff]) {
                            CopyMemory((byte*)srcPtr, (byte*)dstPtr, length * sizeof(ulong));
                        }
                    }
                }
            } else {
#endif
                const int bcLimit = BufferBlockCopyThreshold / sizeof(ulong);
                if (length >= bcLimit) {
                    Buffer.BlockCopy(src, srcOff, dst, dstOff, length * sizeof(ulong));
                } else {
                    Array.Copy(src, srcOff, dst, dstOff, length);
                }
#if INCLUDE_UNSAFE
            }
#endif
        }

#if INCLUDE_UNSAFE
        /// <summary>
        ///     Copy data from <paramref name="srcPtr" /> into <paramref name="dstPtr" />.
        /// </summary>
        /// <remarks>
        ///     If <paramref name="srcPtr" /> or <paramref name="dstPtr" /> are not originally for byte-oriented data,
        ///     length will need to be adjusted accordingly, e.g. UInt32 pointer vs. byte pointer = 4x length.
        /// </remarks>
        /// <param name="srcPtr">Pointer to source of data.</param>
        /// <param name="dstPtr">Pointer to destination for data.</param>
        /// <param name="length">Length/quantity of data to copy, in bytes.</param>
        internal static unsafe void CopyMemory(byte* srcPtr, byte* dstPtr, int length)
        {
            if (Shared.PlatformWordSize == sizeof(UInt32)) {
                while (length >= sizeof(UInt64)) {
                    *(UInt32*)dstPtr = *(UInt32*)srcPtr;
                    dstPtr += sizeof(UInt32);
                    srcPtr += sizeof(UInt32);
                    *(UInt32*)dstPtr = *(UInt32*)srcPtr;
                    dstPtr += sizeof(UInt32);
                    srcPtr += sizeof(UInt32);
                    length -= sizeof(UInt64);
                }
            } else if (Shared.PlatformWordSize == sizeof(UInt64)) {
                while (length >= sizeof(UInt64) * 2) {
                    *(UInt64*) dstPtr = *(UInt64*) srcPtr;
                    dstPtr += sizeof(UInt64);
                    srcPtr += sizeof(UInt64);
                    *(UInt64*) dstPtr = *(UInt64*) srcPtr;
                    dstPtr += sizeof(UInt64);
                    srcPtr += sizeof(UInt64);
                    length -= sizeof(UInt64) * 2;
                }

                if (length >= sizeof(UInt64)) {
                    *(UInt64*) dstPtr = *(UInt64*) srcPtr;
                    dstPtr += sizeof(UInt64);
                    srcPtr += sizeof(UInt64);
                    length -= sizeof(UInt64);
                }
            }

            if (length >= sizeof(UInt32)) {
                *(UInt32*) dstPtr = *(UInt32*) srcPtr;
                dstPtr += sizeof(UInt32);
                srcPtr += sizeof(UInt32);
                length -= sizeof(UInt32);
            }

            if (length >= sizeof(UInt16)) {
                *(UInt16*) dstPtr = *(UInt16*) srcPtr;
                dstPtr += sizeof(UInt16);
                srcPtr += sizeof(UInt16);
                length -= sizeof(UInt16);
            }

            if (length > 0) {
                *dstPtr = *srcPtr;
            }
        }
#endif
    }
}
