//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.11.1
//     from Assets/Input/User Input.inputactions
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public partial class @UserInput: IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @UserInput()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""User Input"",
    ""maps"": [
        {
            ""name"": ""MobileTouch"",
            ""id"": ""a0d3bd60-4d5d-43f2-b2fc-a9707a3eb385"",
            ""actions"": [
                {
                    ""name"": ""Tap"",
                    ""type"": ""Button"",
                    ""id"": ""f1d19048-ed92-45de-9014-b81608a6fbb5"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""efef492b-2cca-4fee-becf-5ec5eab7c3df"",
                    ""path"": ""<Touchscreen>/primaryTouch/tap"",
                    ""interactions"": ""Tap"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Tap"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""3147c87a-4bfb-46f8-8fca-edd5e9661b69"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": ""Tap"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Tap"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // MobileTouch
        m_MobileTouch = asset.FindActionMap("MobileTouch", throwIfNotFound: true);
        m_MobileTouch_Tap = m_MobileTouch.FindAction("Tap", throwIfNotFound: true);
    }

    ~@UserInput()
    {
        UnityEngine.Debug.Assert(!m_MobileTouch.enabled, "This will cause a leak and performance issues, UserInput.MobileTouch.Disable() has not been called.");
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }

    public IEnumerable<InputBinding> bindings => asset.bindings;

    public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
    {
        return asset.FindAction(actionNameOrId, throwIfNotFound);
    }

    public int FindBinding(InputBinding bindingMask, out InputAction action)
    {
        return asset.FindBinding(bindingMask, out action);
    }

    // MobileTouch
    private readonly InputActionMap m_MobileTouch;
    private List<IMobileTouchActions> m_MobileTouchActionsCallbackInterfaces = new List<IMobileTouchActions>();
    private readonly InputAction m_MobileTouch_Tap;
    public struct MobileTouchActions
    {
        private @UserInput m_Wrapper;
        public MobileTouchActions(@UserInput wrapper) { m_Wrapper = wrapper; }
        public InputAction @Tap => m_Wrapper.m_MobileTouch_Tap;
        public InputActionMap Get() { return m_Wrapper.m_MobileTouch; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(MobileTouchActions set) { return set.Get(); }
        public void AddCallbacks(IMobileTouchActions instance)
        {
            if (instance == null || m_Wrapper.m_MobileTouchActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_MobileTouchActionsCallbackInterfaces.Add(instance);
            @Tap.started += instance.OnTap;
            @Tap.performed += instance.OnTap;
            @Tap.canceled += instance.OnTap;
        }

        private void UnregisterCallbacks(IMobileTouchActions instance)
        {
            @Tap.started -= instance.OnTap;
            @Tap.performed -= instance.OnTap;
            @Tap.canceled -= instance.OnTap;
        }

        public void RemoveCallbacks(IMobileTouchActions instance)
        {
            if (m_Wrapper.m_MobileTouchActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IMobileTouchActions instance)
        {
            foreach (var item in m_Wrapper.m_MobileTouchActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_MobileTouchActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public MobileTouchActions @MobileTouch => new MobileTouchActions(this);
    public interface IMobileTouchActions
    {
        void OnTap(InputAction.CallbackContext context);
    }
}
