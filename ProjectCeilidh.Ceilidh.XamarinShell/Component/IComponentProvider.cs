using Xamarin.Forms;

namespace ProjectCeilidh.Ceilidh.XamarinShell.Component
{
    public interface IComponentProvider
    {
        VisualElement CreateComponent(string id, object serializationData);
    }
}
