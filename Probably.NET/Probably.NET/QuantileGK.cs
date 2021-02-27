using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Probably.NET
{
    public class QuantileGK : SafeHandle
    {
        private const string DLL_NAME = "probably_net";
        [DllImport(DLL_NAME)] private static extern IntPtr quantile_gk_new(double epsilon);
        [DllImport(DLL_NAME)] private static extern void quantile_gk_insert(IntPtr gk, double value);
        [DllImport(DLL_NAME)] private static extern double quantile_gk_quantile(IntPtr gk, double quantile);
        [DllImport(DLL_NAME)] private static extern void quantile_gk_drop(IntPtr gk);


        public override bool IsInvalid => this.handle == IntPtr.Zero;

        public QuantileGK(double epsilon) : base(IntPtr.Zero, true) => this.handle = quantile_gk_new(epsilon);

        public void Insert(double value) => quantile_gk_insert(this.handle, value);

        public double Quantile(double quantile) => (quantile < 0 || quantile > 1)
            ? throw new ArgumentOutOfRangeException(nameof(quantile))
            : quantile_gk_quantile(this.handle, quantile);

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
