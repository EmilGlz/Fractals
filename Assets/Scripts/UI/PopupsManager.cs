using System;
using System.Collections.Generic;

namespace Assets.Scripts.UI
{
    public class PopupsManager
    {
        private static PopupsManager _instance;

        private readonly List<PopupBase> _activePopups;
        private PopupsManager()
        {
            _activePopups = new List<PopupBase>();
        }
        public static PopupsManager instance => _instance ?? (_instance = new PopupsManager());

        public static bool IsShown<T>()
    where T : PopupBase
        {
            return instance.Any(p => p is T);
        }

        public void AddPopup(PopupBase popup)
        {
            _activePopups.Add(popup);
        }

        public void RemovePopup(PopupBase popup)
        {
            if (_activePopups.Contains(popup))
                _activePopups.Remove(popup);
        }

        public bool Any(Predicate<PopupBase> predicate)
        {
            CleanupDestroyedPopups();
            return _activePopups?.Exists(predicate) == true;
        }

        public T GetActive<T>() where T : PopupBase
        {
            CleanupDestroyedPopups();
            foreach (var popup in _activePopups)
                if (popup is T resultPopup)
                    return resultPopup;

            return default;
        }

        private void CleanupDestroyedPopups()
        {
            DisposeAll(popup => popup.Instance == null);
        }

        public void DisposeAll(Predicate<PopupBase> predicate = null)
        {
            var popups = _activePopups.ToArray();
            foreach (var popup in popups)
            {
                if (popup.Instance != null)
                {
                    if (predicate != null && !predicate(popup))
                        continue;
                }
                popup.Dispose();
                _activePopups.Remove(popup);
            }
        }

        public void DisposePopup<T>() where T : PopupBase
        {
            var activePopup = GetActive<T>();
            if (activePopup != null)
                activePopup.Dispose();
        }

        public void DestroyAll()
        {
            var popups = _activePopups.ToArray();
            foreach (var popup in popups)
                popup.DestroyPopup();
            _activePopups.Clear();
        }

        public void DestroyLast()
        {
            var lastPopup = GetActivePopup();
            if (lastPopup != null)
            {
                _activePopups.Remove(lastPopup);
                lastPopup.DestroyPopup();
            }
        }

        public PopupBase GetActivePopup()
        {
            CleanupDestroyedPopups();
            return _activePopups.Count > 0 ? _activePopups[^1] : null;
        }

    }
}
