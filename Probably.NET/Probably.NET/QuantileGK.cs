using System;
using System.Runtime.InteropServices;

namespace Probably.NET
{
    public class QuantileGK : SafeHandle
    {
        [DllImport(FFIUtil.DLL_NAME)] private static extern IntPtr quantile_gk_new(double epsilon);
        [DllImport(FFIUtil.DLL_NAME)] private static extern IntPtr quantile_gk_new_from_bytes(byte[] bytes, uint len);
        [DllImport(FFIUtil.DLL_NAME)] private static extern void quantile_gk_insert(IntPtr gk, double value);
        [DllImport(FFIUtil.DLL_NAME)] private static extern double quantile_gk_quantile(IntPtr gk, double quantile);
        [DllImport(FFIUtil.DLL_NAME)] private static extern void quantile_gk_drop(IntPtr gk);
        [DllImport(FFIUtil.DLL_NAME)] private static extern void quantile_gk_merge(IntPtr gk1, IntPtr gk2);
        [DllImport(FFIUtil.DLL_NAME)] private static extern IntPtr quantile_gk_get_bytes(IntPtr gk, ref uint length, ref uint capacity);
        [DllImport(FFIUtil.DLL_NAME)] private static extern void quantile_gk_free_bytes(IntPtr bytesPtr, uint length, uint capacity);


        public override bool IsInvalid => this.handle == IntPtr.Zero;

        public QuantileGK(double epsilon) : base(IntPtr.Zero, true) => this.handle = quantile_gk_new(epsilon);

        public QuantileGK(byte[] bytes) : base(IntPtr.Zero, true) => this.handle = quantile_gk_new_from_bytes(bytes, (uint)bytes.Length);
        public byte[] GetBytes() => FFIUtil.GetBytes(this.handle, quantile_gk_get_bytes, quantile_gk_free_bytes);

        public void Insert(double value) => quantile_gk_insert(this.handle, value);

        public double Quantile(double quantile) => (quantile < 0 || quantile > 1)
            ? throw new ArgumentOutOfRangeException(nameof(quantile))
            : quantile_gk_quantile(this.handle, quantile);

        public void Merge(QuantileGK other) => quantile_gk_merge(this.handle, other.handle);

        protected override bool ReleaseHandle()
        {
            if (!this.IsInvalid)
            {
                quantile_gk_drop(this.handle);
            }

            return true;
        }
    }
}
