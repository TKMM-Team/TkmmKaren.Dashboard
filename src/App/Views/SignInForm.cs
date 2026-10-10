using System.Runtime.InteropServices.JavaScript;
using Avalonia.Browser;
using Avalonia.Controls;
using Avalonia.Platform;

namespace TkmmKaren.Dashboard.Views;

public partial class SignInForm : NativeControlHost
{
    private static SignInForm? _current;
    private JSObject? _form;

    public SignInForm()
    {
        _current = this;
        SetSubmit(OnSubmit);
    }

    public event Action? Submitted;

    public string Username => _form is null ? "" : FieldValue(_form, "username");

    public string Password => _form is null ? "" : FieldValue(_form, "password");

    public void ShowError(string? message)
    {
        if (_form is not null) {
            SetError(_form, message ?? "");
        }
    }

    public void ShowBusy(bool busy)
    {
        if (_form is not null) {
            SetBusy(_form, busy);
        }
    }

    protected override IPlatformHandle CreateNativeControlCore(IPlatformHandle parent)
    {
        _current = this;
        _form = CreateForm();
        return new JSObjectControlHandle(_form);
    }

    protected override void DestroyNativeControlCore(IPlatformHandle control)
    {
        ((JSObjectControlHandle)control).Destroy();
        _form = null;
        base.DestroyNativeControlCore(control);
    }

    private static void OnSubmit() => _current?.Submitted?.Invoke();

    [JSImport("globalThis.tkmmkarenSetSignInSubmit")]
    private static partial void SetSubmit([JSMarshalAs<JSType.Function>] Action submit);

    [JSImport("globalThis.tkmmkarenCreateSignInForm")]
    private static partial JSObject CreateForm();

    [JSImport("globalThis.tkmmkarenFieldValue")]
    private static partial string FieldValue(JSObject form, string name);

    [JSImport("globalThis.tkmmkarenSetSignInError")]
    private static partial void SetError(JSObject form, string message);

    [JSImport("globalThis.tkmmkarenSetSignInBusy")]
    private static partial void SetBusy(JSObject form, bool busy);
}
