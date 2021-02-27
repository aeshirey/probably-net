using System;
using System.Runtime.InteropServices;

namespace Probably.NET
{
    public class HyperLogLog : SafeHandle
    {
        private const string DLL_NAME = "probably_net";
        [DllImport(DLL_NAME)] private static extern IntPtr hll_new(double error_rate);
        [DllImport(DLL_NAME)] private static extern IntPtr hll_new_from_keys(double error_rate, ulong key0, ulong key1);

        [DllImport(DLL_NAME)] private static extern void hll_insert_u8(IntPtr hll, byte value);
        [DllImport(DLL_NAME)] private static extern void hll_insert_i8(IntPtr hll, sbyte value);
        [DllImport(DLL_NAME)] private static extern void hll_insert_i16(IntPtr hll, short value);
        [DllImport(DLL_NAME)] private static extern void hll_insert_u16(IntPtr hll, ushort value);
        [DllImport(DLL_NAME)] private static extern void hll_insert_i32(IntPtr hll, int value);
        [DllImport(DLL_NAME)] private static extern void hll_insert_u32(IntPtr hll, uint value);
        [DllImport(DLL_NAME)] private static extern void hll_insert_i64(IntPtr hll, long value);
        [DllImport(DLL_NAME)] private static extern void hll_insert_u64(IntPtr hll, ulong value);
        [DllImport(DLL_NAME)] private static extern void hll_insert_str(IntPtr hll, string value);
        [DllImport(DLL_NAME)] private static extern void hll_insert_bool(IntPtr hll, bool value);

        [DllImport(DLL_NAME)] private static extern double hll_len(IntPtr hll);
        [DllImport(DLL_NAME)] private static extern void hll_drop(IntPtr hll);


        public override bool IsInvalid => this.handle == IntPtr.Zero;

        protected override bool ReleaseHandle()
        {
            if (!this.IsInvalid)
            {
                hll_drop(handle);
            }

            return true;
        }


        public HyperLogLog(double error_rate) : base(IntPtr.Zero, true)
        {
            this.handle = hll_new(error_rate);
        }

        public HyperLogLog(double error_rate, ulong key0, ulong key1) : base(IntPtr.Zero, true)
        {
            this.handle = hll_new_from_keys(error_rate, key0, key1);
        }

        public void Insert(sbyte value) => hll_insert_i8(this.handle, value);
        public void Insert(byte value) => hll_insert_u8(this.handle, value);
        public void Insert(short value) => hll_insert_i16(this.handle, value);
        public void Insert(ushort value) => hll_insert_u16(this.handle, value);
        public void Insert(int value) => hll_insert_i32(this.handle, value);
        public void Insert(uint value) => hll_insert_u32(this.handle, value);
        public void Insert(long value) => hll_insert_i64(this.handle, value);
        public void Insert(ulong value) => hll_insert_u64(this.handle, value);
        public void Insert(bool value) => hll_insert_bool(this.handle, value);
        public void Insert(string value) => hll_insert_str(this.handle, value);


        /// <summary>Estimates the number of items seen in the set.</summary>
        public double Count() => hll_len(this.handle);
    }

}
