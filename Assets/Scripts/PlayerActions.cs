//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.4.4
//     from Assets/Settings/PlayerActions.inputactions
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

public partial class @PlayerActions : IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @PlayerActions()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""PlayerActions"",
    ""maps"": [
        {
            ""name"": ""Default_PlayerActions"",
            ""id"": ""f0cff418-85af-4d0e-b16c-dc0d41f2fb47"",
            ""actions"": [
                {
                    ""name"": ""Move"",
                    ""type"": ""Value"",
                    ""id"": ""97c6c1c9-859b-4d48-b695-d3f7dbaec194"",
                    ""expectedControlType"": ""Stick"",
                    ""processors"": ""StickDeadzone"",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Aim"",
                    ""type"": ""Value"",
                    ""id"": ""0927c105-0bce-4051-af1c-a8425fedc92c"",
                    ""expectedControlType"": ""Stick"",
                    ""processors"": ""StickDeadzone"",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Shoot"",
                    ""type"": ""Button"",
                    ""id"": ""722de3b6-2b24-467e-a29d-708b1e2a404b"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""418d99dd-d61b-4e53-84d9-88c4270f0de0"",
                    ""path"": ""<Gamepad>/leftStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""8340a0ba-b8e3-4d6e-8cb4-1428e0fe559c"",
                    ""path"": ""<Gamepad>/rightTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Shoot"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""9672cad4-051e-44b5-9514-cd0920091675"",
                    ""path"": ""<Gamepad>/buttonWest"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Shoot"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""897e9c88-50be-46aa-a681-5559876a2d68"",
                    ""path"": ""<Gamepad>/rightStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Aim"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // Default_PlayerActions
        m_Default_PlayerActions = asset.FindActionMap("Default_PlayerActions", throwIfNotFound: true);
        m_Default_PlayerActions_Move = m_Default_PlayerActions.FindAction("Move", throwIfNotFound: true);
        m_Default_PlayerActions_Aim = m_Default_PlayerActions.FindAction("Aim", throwIfNotFound: true);
        m_Default_PlayerActions_Shoot = m_Default_PlayerActions.FindAction("Shoot", throwIfNotFound: true);
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

    // Default_PlayerActions
    private readonly InputActionMap m_Default_PlayerActions;
    private IDefault_PlayerActionsActions m_Default_PlayerActionsActionsCallbackInterface;
    private readonly InputAction m_Default_PlayerActions_Move;
    private readonly InputAction m_Default_PlayerActions_Aim;
    private readonly InputAction m_Default_PlayerActions_Shoot;
    public struct Default_PlayerActionsActions
    {
        private @PlayerActions m_Wrapper;
        public Default_PlayerActionsActions(@PlayerActions wrapper) { m_Wrapper = wrapper; }
        public InputAction @Move => m_Wrapper.m_Default_PlayerActions_Move;
        public InputAction @Aim => m_Wrapper.m_Default_PlayerActions_Aim;
        public InputAction @Shoot => m_Wrapper.m_Default_PlayerActions_Shoot;
        public InputActionMap Get() { return m_Wrapper.m_Default_PlayerActions; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(Default_PlayerActionsActions set) { return set.Get(); }
        public void SetCallbacks(IDefault_PlayerActionsActions instance)
        {
            if (m_Wrapper.m_Default_PlayerActionsActionsCallbackInterface != null)
            {
                @Move.started -= m_Wrapper.m_Default_PlayerActionsActionsCallbackInterface.OnMove;
                @Move.performed -= m_Wrapper.m_Default_PlayerActionsActionsCallbackInterface.OnMove;
                @Move.canceled -= m_Wrapper.m_Default_PlayerActionsActionsCallbackInterface.OnMove;
                @Aim.started -= m_Wrapper.m_Default_PlayerActionsActionsCallbackInterface.OnAim;
                @Aim.performed -= m_Wrapper.m_Default_PlayerActionsActionsCallbackInterface.OnAim;
                @Aim.canceled -= m_Wrapper.m_Default_PlayerActionsActionsCallbackInterface.OnAim;
                @Shoot.started -= m_Wrapper.m_Default_PlayerActionsActionsCallbackInterface.OnShoot;
                @Shoot.performed -= m_Wrapper.m_Default_PlayerActionsActionsCallbackInterface.OnShoot;
                @Shoot.canceled -= m_Wrapper.m_Default_PlayerActionsActionsCallbackInterface.OnShoot;
            }
            m_Wrapper.m_Default_PlayerActionsActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Move.started += instance.OnMove;
                @Move.performed += instance.OnMove;
                @Move.canceled += instance.OnMove;
                @Aim.started += instance.OnAim;
                @Aim.performed += instance.OnAim;
                @Aim.canceled += instance.OnAim;
                @Shoot.started += instance.OnShoot;
                @Shoot.performed += instance.OnShoot;
                @Shoot.canceled += instance.OnShoot;
            }
        }
    }
    public Default_PlayerActionsActions @Default_PlayerActions => new Default_PlayerActionsActions(this);
    public interface IDefault_PlayerActionsActions
    {
        void OnMove(InputAction.CallbackContext context);
        void OnAim(InputAction.CallbackContext context);
        void OnShoot(InputAction.CallbackContext context);
    }
}
