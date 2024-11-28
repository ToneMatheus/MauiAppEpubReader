#if ANDROID
using Android.Views;
using AndroidX.AppCompat.Widget;
using static Android.Views.View;
#endif
using Microsoft.Maui.Handlers;


namespace MauiAppEpubReader;

public class CustomEditorHandler : EditorHandler
{
#if ANDROID
    protected override AppCompatEditText CreatePlatformView()
    {
        var platfromView = base.CreatePlatformView();
        platfromView.SetOnTouchListener(new CustomTouchListener());
        return platfromView;
    }

    class CustomTouchListener : Java.Lang.Object, IOnTouchListener
    {
        public bool OnTouch(Android.Views.View? v, MotionEvent? e)
        {
            v?.Parent?.RequestDisallowInterceptTouchEvent(true);
            if ((e.Action & MotionEventActions.Up) != 0 &&
                (e.ActionMasked & MotionEventActions.Up) != 0)
            {
                v?.Parent?.RequestDisallowInterceptTouchEvent(false);
            }

            return false;
        }
    }
#endif
}