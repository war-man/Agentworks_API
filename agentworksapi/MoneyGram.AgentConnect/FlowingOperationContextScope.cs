using System;
using System.ServiceModel;

namespace MoneyGram.AgentConnect
{
    public sealed class FlowingOperationContextScope : IDisposable
    {
        bool _inflight = false;
        bool _disposed;
        OperationContext _thisContext = null;
        OperationContext _originalContext = null;

        public FlowingOperationContextScope(IContextChannel channel) :
            this(new OperationContext(channel))
        {
        }

        public FlowingOperationContextScope(OperationContext context)
        {
            _originalContext = OperationContext.Current;
            OperationContext.Current = _thisContext = context;
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                if (_inflight || OperationContext.Current != _thisContext)
                    throw new InvalidOperationException();
                _disposed = true;
                OperationContext.Current = _originalContext;
                _thisContext = null;
                _originalContext = null;
            }
        }

        internal void BeforeAwait()
        {
            if (_inflight)
                return;
            _inflight = true;
            // leave _thisContext as the current context
        }

        internal void AfterAwait()
        {
            if (!_inflight)
                throw new InvalidOperationException();
            _inflight = false;
            // ignore the current context, restore _thisContext
            OperationContext.Current = _thisContext;
        }
    }
}