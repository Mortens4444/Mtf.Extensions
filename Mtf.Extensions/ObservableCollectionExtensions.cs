using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Mtf.Extensions
{
    public static class ObservableCollectionExtensions
    {
        /// <summary>
        /// Adds the items to the observable collection one by one.
        /// Simple and reliable; callers are responsible for making the call on the UI thread if needed.
        /// </summary>
        public static void AddRange<T>(this ObservableCollection<T> collection, IEnumerable<T> items)
        {
            if (collection == null)
            {
                throw new ArgumentNullException(nameof(collection));
            }

            if (items != null)
            {
                foreach (var item in items)
                {
                    collection.Add(item);
                }
            }
        }
    }

}
