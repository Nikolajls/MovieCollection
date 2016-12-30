namespace FoxTales.Infrastructure.CommandFramework
{
    public class SharedInstance<T>
    {
        public T Instance { get; set; }

        public SharedInstance(T instance)
        {
            Instance = instance;
        }

        //{

        //public static implicit operator T(SharedInstance<T> me)
        //    return me.Instance;
        //}

        //public static implicit operator SharedInstance<T>(T me)
        //{
        //    return new SharedInstance<T>(me);
        //}
    }
}