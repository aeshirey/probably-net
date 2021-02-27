using System;
using System.Runtime.InteropServices;

namespace Probably.NET
{
    public class HyperLogLog
    {
        private const string DLL_NAME = "probably_net";
        [DllImport(DLL_NAME)] private static extern UIntPtr hll_new(double error_rate);
        [DllImport(DLL_NAME)] private static extern UIntPtr hll_new_from_keys(double error_rate, ulong key0, ulong key1);

        [DllImport(DLL_NAME)] private static extern void hll_insert_u8(UIntPtr hll, byte value);
        [DllImport(DLL_NAME)] private static extern void hll_insert_i8(UIntPtr hll, sbyte value);
        [DllImport(DLL_NAME)] private static extern void hll_insert_i16(UIntPtr hll, short value);
        [DllImport(DLL_NAME)] private static extern void hll_insert_u16(UIntPtr hll, ushort value);
        [DllImport(DLL_NAME)] private static extern void hll_insert_i32(UIntPtr hll, int value);
        [DllImport(DLL_NAME)] private static extern void hll_insert_u32(UIntPtr hll, uint value);
        [DllImport(DLL_NAME)] private static extern void hll_insert_i64(UIntPtr hll, long value);
        [DllImport(DLL_NAME)] private static extern void hll_insert_u64(UIntPtr hll, ulong value);
        [DllImport(DLL_NAME)] private static extern void hll_insert_str(UIntPtr hll, string value);
        [DllImport(DLL_NAME)] private static extern void hll_insert_bool(UIntPtr hll, bool value);

        [DllImport(DLL_NAME)] private static extern double hll_len(UIntPtr hll);
        [DllImport(DLL_NAME)] private static extern void hll_drop(UIntPtr hll);


        private readonly UIntPtr ptr;

        public HyperLogLog(double error_rate)
        {
            this.ptr = hll_new(error_rate);
        }

        public HyperLogLog(double error_rate, ulong key0, ulong key1)
        {
            this.ptr = hll_new_from_keys(error_rate, key0, key1);
        }

        public void Insert(sbyte value) => hll_insert_i8(this.ptr, value);
        public void Insert(byte value) => hll_insert_u8(this.ptr, value);
        public void Insert(short value) => hll_insert_i16(this.ptr, value);
        public void Insert(ushort value) => hll_insert_u16(this.ptr, value);
        public void Insert(int value) => hll_insert_i32(this.ptr, value);
        public void Insert(uint value) => hll_insert_u32(this.ptr, value);
        public void Insert(long value) => hll_insert_i64(this.ptr, value);
        public void Insert(ulong value) => hll_insert_u64(this.ptr, value);
        public void Insert(bool value) => hll_insert_bool(this.ptr, value);
        public void Insert(string value) => hll_insert_str(this.ptr, value);


        /// <summary>Estimates the number of items seen in the set.</summary>
        public double Count() => hll_len(this.ptr);


        ~HyperLogLog()
        {
            hll_drop(this.ptr);
        }
    }

}
