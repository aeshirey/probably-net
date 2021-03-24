using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Probably.NET
{
    /// <summary>
    /// Internal FFI utility functions
    /// </summary>
    internal static class FFIUtil
    {
        public const string DLL_NAME = "probably_net";

        /// <summary>
        /// A delegate type for getting the serialized data structure from unmanaged code.
        /// 
        /// Thie concrete function is specific to, for example, HyperLogLog (hll_get_bytes).
        /// </summary>
        /// <returns>A pointer to a vector of bytes owned by unmanaged code.</returns>
        public delegate IntPtr ByteGetter(IntPtr handle, ref uint length, ref uint capacity);

        /// <summary>
        /// Calls the unmanaged code to free the previously allocated memory.
        /// </summary>
        public delegate void ByteFreer(IntPtr bytePtr, uint length, uint capacity);

        /// <summary>
        /// Convenience function to call the unmanaged getter, copy it to a CLR byte[], then free the memory.
        /// </summary>
        /// <param name="handle">The handle to the underlying data structure.</param>
        /// <param name="getter">A P/Invoke for getting a pointer to a vector of bytes.</param>
        /// <param name="freer">A P/Invoke for releasing the memory occupied by the bytes.</param>
        /// <returns></returns>
        public static byte[] GetBytes(IntPtr handle, ByteGetter getter, ByteFreer freer)
        {
            // Get the bytes from unmanaged code. Note that with Vec::from_raw_parts*, we need to keep the
            // length _and capacity_ of the buffer so memory can be properly freed.
            // * https://doc.rust-lang.org/std/vec/struct.Vec.html#method.from_raw_parts
            uint len = 0, cap = 0;
            IntPtr bytePtr = getter(handle, ref len, ref cap);

            // bytePtr is a pointer to where the memory reside. Allocate a buffer and copy the data over to managed memomry.
            byte[] bytes = new byte[len];
            Marshal.Copy(bytePtr, bytes, 0, (int)len);

            // Release the unmanaged memory
            freer(bytePtr, len, cap);

            return bytes;
        }
    }
}
