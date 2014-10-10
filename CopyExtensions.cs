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
            data.DeepCopy(dst, data.Length);
            return dst;
        }

        /// <summary>
        ///     Copy values from <paramref name="src" /> array into <paramref name="dst" /> array.
        /// </summary>
        /// <param name="src">Array to copy from.</param>
        /// <param name="dst">Array to copy into.</param>
        public static void DeepCopy(this byte[] src, byte[] dst)
        {
            Shared.ThrowOnInvalidArgument(src, dst, src.Length);
            DeepCopyInternal(src, 0, dst, 0, src.Length);
        }

        /// <summary>
        ///     Copy values from <paramref name="src" /> array into <paramref name="dst" /> array.
        /// </summary>
        /// <param name="src">Array to copy from.</param>
        /// <param name="dst">Array to copy into.</param>
        /// <param name="length">Amount of values to copy.</param>
        /// <param name="srcOff">The offset in <paramref name="src" /> at which to copy from.</param>
        /// <param name="dstOff">The offset in <paramref name="dst" /> at which to copy into.</param>
        public static void DeepCopy(this byte[] src, byte[] dst, int length, int srcOff = 0, int dstOff = 0)
        {
            Shared.ThrowOnInvalidArgument(src, dst, length, srcOff, dstOff);
            DeepCopyInternal(src, srcOff, dst, dstOff, length);
        }

        /// <summary>
        ///     Copy values from <paramref name="src" /> array into <paramref name="dst" /> array.
        /// </summary>
        /// <param name="src">Array to copy from.</param>
        /// <param name="srcOff">The offset in <paramref name="src" /> at which to copy from.</param>
        /// <param name="dst">Array to copy into.</param>
        /// <param name="dstOff">The offset in <paramref name="dst" /> at which to copy into.</param>
        /// <param name="length">Amount of values to copy.</param>
        internal static void DeepCopyInternal(byte[] src, int srcOff, byte[] dst, int dstOff, int length)
        {
#if INCLUDE_UNSAFE
            if (length >= UnmanagedThreshold) {
                unsafe {
                    fixed (byte* srcPtr = src) {
                        fixed (byte* dstPtr = dst) {
                            CopyMemory(srcPtr + srcOff, dstPtr + dstOff, length);
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
            data.DeepCopy(dst, data.Length);
            return dst;
        }

        /// <summary>
        ///     Copy values from <paramref name="src" /> array into <paramref name="dst" /> array.
        /// </summary>
        /// <param name="src">Array to copy from.</param>
        /// <param name="dst">Array to copy into.</param>
        public static void DeepCopy(this int[] src, int[] dst)
        {
            Shared.ThrowOnInvalidArgument(src, dst, src.Length);
            DeepCopyInternal(src, 0, dst, 0, src.Length);
        }

        /// <summary>
        ///     Copy values from <paramref name="src" /> array into <paramref name="dst" /> array.
        /// </summary>
        /// <param name="src">Array to copy from.</param>
        /// <param name="dst">Array to copy into.</param>
        /// <param name="length">Amount of values to copy.</param>
        /// <param name="srcOff">The offset in <paramref name="src" /> at which to copy from.</param>
        /// <param name="dstOff">The offset in <paramref name="dst" /> at which to copy into.</param>
        public static void DeepCopy(this int[] src, int[] dst, int length, int srcOff = 0, int dstOff = 0)
        {
            Shared.ThrowOnInvalidArgument(src, dst, length, srcOff, dstOff);
            DeepCopyInternal(src, srcOff, dst, dstOff, length);
        }

        /// <summary>
        ///     Copy values from <paramref name="src" /> array into <paramref name="dst" /> array.
        /// </summary>
        /// <param name="src">Array to copy from.</param>
        /// <param name="srcOff">The offset in <paramref name="src" /> at which to copy from.</param>
        /// <param name="dst">Array to copy into.</param>
        /// <param name="dstOff">The offset in <paramref name="dst" /> at which to copy into.</param>
        /// <param name="length">Amount of values to copy.</param>
        internal static void DeepCopyInternal(int[] src, int srcOff, int[] dst, int dstOff, int length)
        {
#if INCLUDE_UNSAFE
            const int umLimit = UnmanagedThreshold / sizeof(int);
            if (length >= umLimit) {
                unsafe {
                    fixed (int* srcPtr = src) {
                        fixed (int* dstPtr = dst) {
                            var srcBP = (byte*) (srcPtr + srcOff);
                            var dstBP = (byte*) (dstPtr + dstOff);
                            CopyMemory(srcBP, dstBP, length);
                        }
                    }
                }
            } else {
#endif
                const int bcLimit = BufferBlockCopyThreshold / sizeof(int);
                if (length >= bcLimit) {
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
        public static uint[] DeepCopy(this uint[] data)
        {
            if (data == null) {
                return null;
            }
            var dst = new uint[data.Length];
            data.DeepCopy(dst, data.Length);
            return dst;
        }

        /// <summary>
        ///     Copy values from <paramref name="src" /> array into <paramref name="dst" /> array.
        /// </summary>
        /// <param name="src">Array to copy from.</param>
        /// <param name="dst">Array to copy into.</param>
        public static void DeepCopy(this uint[] src, uint[] dst)
        {
            Shared.ThrowOnInvalidArgument(src, dst, src.Length);
            DeepCopyInternal(src, 0, dst, 0, src.Length);
        }

        /// <summary>
        ///     Copy values from <paramref name="src" /> array into <paramref name="dst" /> array.
        /// </summary>
        /// <param name="src">Array to copy from.</param>
        /// <param name="dst">Array to copy into.</param>
        /// <param name="length">Amount of values to copy.</param>
        /// <param name="srcOff">The offset in <paramref name="src" /> at which to copy from.</param>
        /// <param name="dstOff">The offset in <paramref name="dst" /> at which to copy into.</param>
        public static void DeepCopy(this uint[] src, uint[] dst, int length, int srcOff = 0, int dstOff = 0)
        {
            Shared.ThrowOnInvalidArgument(src, dst, length, srcOff, dstOff);
            DeepCopyInternal(src, srcOff, dst, dstOff, length);
        }

        /// <summary>
        ///     Copy values from <paramref name="src" /> array into <paramref name="dst" /> array.
        /// </summary>
        /// <param name="src">Array to copy from.</param>
        /// <param name="srcOff">The offset in <paramref name="src" /> at which to copy from.</param>
        /// <param name="dst">Array to copy into.</param>
        /// <param name="dstOff">The offset in <paramref name="dst" /> at which to copy into.</param>
        /// <param name="length">Amount of values to copy.</param>
        internal static void DeepCopyInternal(uint[] src, int srcOff, uint[] dst, int dstOff, int length)
        {
#if INCLUDE_UNSAFE
            const int umLimit = UnmanagedThreshold / sizeof(uint);
            if (length >= umLimit) {
                unsafe {
                    fixed (uint* srcPtr = src) {
                        fixed (uint* dstPtr = dst) {
                            var srcBP = (byte*) (srcPtr + srcOff);
                            var dstBP = (byte*) (dstPtr + dstOff);
                            CopyMemory(srcBP, dstBP, length);
                        }
                    }
                }
            } else {
#endif
                const int bcLimit = BufferBlockCopyThreshold / sizeof(uint);
                if (length >= bcLimit) {
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
        public static long[] DeepCopy(this long[] data)
        {
            if (data == null) {
                return null;
            }
            var dst = new long[data.Length];
            data.DeepCopy(dst, data.Length);
            return dst;
        }

        /// <summary>
        ///     Copy values from <paramref name="src" /> array into <paramref name="dst" /> array.
        /// </summary>
        /// <param name="src">Array to copy from.</param>
        /// <param name="dst">Array to copy into.</param>
        /// <param name="length">Amount of values to copy.</param>
        /// <param name="srcOff">The offset in <paramref name="src" /> at which to copy from.</param>
        /// <param name="dstOff">The offset in <paramref name="dst" /> at which to copy into.</param>
        public static void DeepCopy(this long[] src, long[] dst, int length, int srcOff = 0, int dstOff = 0)
        {
            Shared.ThrowOnInvalidArgument(src, dst, length, srcOff, dstOff);
            DeepCopyInternal(src, srcOff, dst, dstOff, length);
        }

        /// <summary>
        ///     Copy values from <paramref name="src" /> array into <paramref name="dst" /> array.
        /// </summary>
        /// <param name="src">Array to copy from.</param>
        /// <param name="srcOff">The offset in <paramref name="src" /> at which to copy from.</param>
        /// <param name="dst">Array to copy into.</param>
        /// <param name="dstOff">The offset in <paramref name="dst" /> at which to copy into.</param>
        /// <param name="length">Amount of values to copy.</param>
        internal static void DeepCopyInternal(long[] src, int srcOff, long[] dst, int dstOff, int length)
        {
#if INCLUDE_UNSAFE
            const int umLimit = UnmanagedThreshold / sizeof(long);
            if (length >= umLimit) {
                unsafe {
                    fixed (long* srcPtr = src) {
                        fixed (long* dstPtr = dst) {
                            var srcBP = (byte*) (srcPtr + srcOff);
                            var dstBP = (byte*) (dstPtr + dstOff);
                            CopyMemory(srcBP, dstBP, length);
                        }
                    }
                }
            } else {
#endif
                const int bcLimit = BufferBlockCopyThreshold / sizeof(long);
                if (length >= bcLimit) {
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
        public static ulong[] DeepCopy(this ulong[] data)
        {
            if (data == null) {
                return null;
            }
            var dst = new ulong[data.Length];
            data.DeepCopy(dst, data.Length);
            return dst;
        }

        /// <summary>
        ///     Copy values from <paramref name="src" /> array into <paramref name="dst" /> array.
        /// </summary>
        /// <param name="src">Array to copy from.</param>
        /// <param name="dst">Array to copy into.</param>
        /// <param name="length">Amount of values to copy.</param>
        /// <param name="srcOff">The offset in <paramref name="src" /> at which to copy from.</param>
        /// <param name="dstOff">The offset in <paramref name="dst" /> at which to copy into.</param>
        public static void DeepCopy(this ulong[] src, ulong[] dst, int length, int srcOff = 0, int dstOff = 0)
        {
            Shared.ThrowOnInvalidArgument(src, dst, length, srcOff, dstOff);
            DeepCopyInternal(src, srcOff, dst, dstOff, length);
        }

        /// <summary>
        ///     Copy values from <paramref name="src" /> array into <paramref name="dst" /> array.
        /// </summary>
        /// <param name="src">Array to copy from.</param>
        /// <param name="srcOff">The offset in <paramref name="src" /> at which to copy from.</param>
        /// <param name="dst">Array to copy into.</param>
        /// <param name="dstOff">The offset in <paramref name="dst" /> at which to copy into.</param>
        /// <param name="length">Amount of values to copy.</param>
        internal static void DeepCopyInternal(ulong[] src, int srcOff, ulong[] dst, int dstOff, int length)
        {
#if INCLUDE_UNSAFE
            const int umLimit = UnmanagedThreshold / sizeof(ulong);
            if (length >= umLimit) {
                unsafe {
                    fixed (ulong* srcPtr = src) {
                        fixed (ulong* dstPtr = dst) {
                            var srcBP = (byte*) (srcPtr + srcOff);
                            var dstBP = (byte*) (dstPtr + dstOff);
                            CopyMemory(srcBP, dstBP, length);
                        }
                    }
                }
            } else {
#endif
                const int bcLimit = BufferBlockCopyThreshold / sizeof(ulong);
                if (length >= bcLimit) {
                    Buffer.BlockCopy(src, srcOff, dst, dstOff, length);
                } else {
                    Array.Copy(src, srcOff, dst, dstOff, length);
                }
#if INCLUDE_UNSAFE
            }
#endif
        }


#if INCLUDE_UNSAFE
        /// <summary>
        ///     Copy data from <paramref name="src" /> into <paramref name="dst" />.
        /// </summary>
        /// <param name="src">Pointer to source of data.</param>
        /// <param name="dst">Pointer to destination for data.</param>
        /// <param name="length">Length of data to copy in bytes.</param>
        internal static unsafe void CopyMemory(byte* src, byte* dst, int length)
        {
            if (Shared.PlatformWordSize == sizeof(UInt32)) {
                while (length >= sizeof(UInt64)) {
                    *(UInt32*) dst = *(UInt32*) src;
                    dst += sizeof(UInt32);
                    src += sizeof(UInt32);
                    *(UInt32*) dst = *(UInt32*) src;
                    dst += sizeof(UInt32);
                    src += sizeof(UInt32);
                    length -= sizeof(UInt64);
                }
            } else if (Shared.PlatformWordSize == sizeof(UInt64)) {
                while (length >= sizeof(UInt64) * 2) {
                    *(UInt64*) dst = *(UInt64*) src;
                    dst += sizeof(UInt64);
                    src += sizeof(UInt64);
                    *(UInt64*) dst = *(UInt64*) src;
                    dst += sizeof(UInt64);
                    src += sizeof(UInt64);
                    length -= sizeof(UInt64) * 2;
                }

                if (length >= sizeof(UInt64)) {
                    *(UInt64*) dst = *(UInt64*) src;
                    dst += sizeof(UInt64);
                    src += sizeof(UInt64);
                    length -= sizeof(UInt64);
                }
            }

            if (length >= sizeof(UInt32)) {
                *(UInt32*) dst = *(UInt32*) src;
                dst += sizeof(UInt32);
                src += sizeof(UInt32);
                length -= sizeof(UInt32);
            }

            if (length >= sizeof(UInt16)) {
                *(UInt16*) dst = *(UInt16*) src;
                dst += sizeof(UInt16);
                src += sizeof(UInt16);
                length -= sizeof(UInt16);
            }

            if (length > 0) {
                *dst = *src;
            }
        }
#endif
    }
}
