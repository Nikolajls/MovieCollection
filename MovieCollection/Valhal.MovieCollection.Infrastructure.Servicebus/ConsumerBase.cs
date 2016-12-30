using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Valhal.MovieCollection.Infrastructure.Servicebus
{
    public abstract class ConsumerBase<T>
    {
       
        protected ConsumerBase()
        {
        }

        public Func<T, Task> ProcessAsync()
        {
            return message => { return Task.Factory.StartNew(() => { Process()(message); }); };
        }

        public Action<T> Process()
        {
            return message =>
            {
                try
                {
                    Execute(message);
                }
                catch (Exception ex)
                {
                 
                }
            };
        }

        protected abstract void Execute(T message);
    }
}

