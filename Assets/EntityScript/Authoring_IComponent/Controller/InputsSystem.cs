using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public partial class InputsSystem : SystemBase
{
    public PlayerInputs PlayerInputs = null;
    protected override void OnCreate()
    {
        base.OnCreate();
        PlayerInputs = new();
        PlayerInputs.Enable();
    }
    protected override void OnUpdate()
    {
        foreach (RefRW<InputsIComponentData> inputData in SystemAPI.Query<RefRW<InputsIComponentData>>())
        {
            inputData.ValueRW.MoveDirection = (float2)PlayerInputs.PlayerInput.Moving.ReadValue<Vector2>();
        }
    }
    protected override void OnDestroy()
    {
        PlayerInputs.Disable();
        PlayerInputs.Dispose();
        base.OnDestroy();
    }
}