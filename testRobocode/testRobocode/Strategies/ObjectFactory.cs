using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bot.Bots;

namespace Bot.Strategies
{
    public class StrategiesFactory
    {
        private Amaterasu self;
        private Dictionary<string, object> collection;

        public StrategiesFactory(Amaterasu self)
        {
            this.self = self;
            collection = new Dictionary<string, object>();
        }

        public T Get<T>() 
            where T: class
        {
            var type = typeof(T);
            object anObject;
            if (!this.collection.TryGetValue(type.FullName, out anObject))
            {
                anObject = New<T>();
                this.Add(anObject);
            }

            return anObject as T;
        }

        public void Add(object anObject)
        {
            var type = anObject.GetType();
            collection.Add(type.FullName, anObject);
        }
        
        public object New<T>()
            where T : class
        {
            var type = typeof(T);
            var constructor = type.GetConstructor(new Type[] { typeof(Amaterasu) });

            return constructor.Invoke(new object[] { self });
        }
    }
}
