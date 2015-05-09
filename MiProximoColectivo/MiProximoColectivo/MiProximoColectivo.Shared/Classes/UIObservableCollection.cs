using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace MiProximoColectivo.Classes
{
    public class UIObservableCollection<T> : ObservableCollection<T>
    {
        public UIObservableCollection() : base()
        { }
        public UIObservableCollection(ObservableCollection<T> collection) : base()
        {
            AssignObservableCollection(collection);
        }
        public bool AssignObservableCollection(ObservableCollection<T> collection)
        {
            bool resultado = true;

            try
            {
                Clear();

                if (collection != null)
                {
                    foreach (T item in collection)
                    {
                        if (item != null)
                            this.Add(item);
                    }
                }
            }
            catch (Exception ex)
            {
                resultado = false;
            }

            return resultado;
        }
    }
}
