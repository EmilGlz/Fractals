using UnityEngine;

namespace Assets.Scripts
{
    public static class Device
    {
        public static bool IsAndroid => Application.platform == RuntimePlatform.Android;
        public static bool IsIOS => Application.platform == RuntimePlatform.IPhonePlayer;

        public static bool IsMobile => IsIOS || IsAndroid;
        public static int TargetFrameRate
        {
            get
            {
                if (!IsMobile)
                    return 60;
                return 45;
            }
        }

        public static float Width;
        public static float Height;
        public static int TopOffset;
        public static int BottomOffset;
        public static int RightOffset;
        public static int LeftOffset;
    }
}
