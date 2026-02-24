using System.Collections.Generic;
using UnityEngine;

namespace Between.View
{
    public class ViewManager : MonoBehaviour
    {
        [SerializeField] private Transform _canvas;

        private readonly Dictionary<BaseView, BaseView> _cache = new();

        public T CreateView<T>(T prefab) where T : BaseView
        {
            if (!_cache.TryGetValue(prefab, out BaseView view))
            {
                view = Instantiate(prefab, _canvas);
                _cache[prefab] = view;
            }

            view.Show();
            return (T)view;
        }

        public void DestroyView<T>(T prefab) where T : BaseView
        {
            if (!_cache.TryGetValue(prefab, out BaseView view))
                return;

            Destroy(view.gameObject);
            _cache.Remove(prefab);
        }

        public void DestroyAllView()
        {
            foreach (var view in _cache.Values)
            {
                if (view != null)
                    Destroy(view.gameObject);
            }

            _cache.Clear();
        }

        private void OnDestroy() => DestroyAllView();
    }
}
