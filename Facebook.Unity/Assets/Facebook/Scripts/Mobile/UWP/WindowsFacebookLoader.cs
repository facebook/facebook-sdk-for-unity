
namespace Facebook.Unity.Mobile.UWP
{
    using Facebook.Unity;
    using Facebook.Unity.Mobile;

    internal class WindowsFacebookLoader : FB.CompiledFacebookLoader
    {
        protected override FacebookGameObject FBGameObject
        {
            get
            {
                WindowsFacebookGameObject windowsFB = ComponentFactory.GetComponent<WindowsFacebookGameObject>();
                if (windowsFB.Facebook == null)
                {
                    windowsFB.Facebook = new WindowsFacebook();
                }

                return windowsFB;
            }
        }
    }
}
