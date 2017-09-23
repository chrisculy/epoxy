using System;
using System.Runtime.InteropServices;

namespace Epoxy
{
    // Owning safe handle
    public class SafeEpoxyHandle : SafeHandle
    {

        public SafeEpoxyHandle(IntPtr nativePointer, Action<IntPtr> destructor)
            : base(IntPtr.Zero, true)
        {
            handle = nativePointer;
            this.destructor = destructor;
        }

        public override bool IsInvalid => handle == IntPtr.Zero || destructor == null;

        protected override bool ReleaseHandle()
        {
            destructor(handle);
            return true;
        }

        private Action<IntPtr> destructor;
    }
}