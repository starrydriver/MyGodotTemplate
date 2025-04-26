using Godot;
using System;

public partial class MyBullet : RigidBody2D
{
    [Export] private float _floatForce = 100f;    // 漂浮力强度
    [Export] private float _stopThreshold = 10f;  // 修改为10像素/秒阈值
    private bool _isFloating = false;            // 是否已进入漂浮状态
    private float _speedCheckInterval = 0.2f;    // 速度检查间隔(秒)
    private float _timeSinceLastCheck = 0f;       // 计时器

    public override void _PhysicsProcess(double delta)
    {
        // 实时速度监控（降低检查频率优化性能）
        _timeSinceLastCheck += (float)delta;
        if (_timeSinceLastCheck >= _speedCheckInterval)
        {
            _timeSinceLastCheck = 0f;
            
            // 检测速度是否低于阈值且未进入漂浮状态
            if (!_isFloating && LinearVelocity.Length() < _stopThreshold)
            {
                StartFloating();
            }
        }
    }

    private void StartFloating()
    {
        _isFloating = true;
        GravityScale = 0f;       // 禁用重力
        LinearDamp = 0.2f;       // 设置线性阻尼
        AngularDamp = 0.5f;      // 添加旋转阻尼

        // 施加随机漂浮力（强度随剩余速度衰减）
        float speedRatio = LinearVelocity.Length() / _stopThreshold;
        Vector2 randomDir = new Vector2(
            (float)GD.RandRange(-1, 1),
            (float)GD.RandRange(-1, 1)
        ).Normalized();
        
        // 速度越接近阈值，力越小（平滑过渡）
        float randomStrength = (float)GD.RandRange(0.3f, _floatForce) * (1 - speedRatio);
        ApplyCentralImpulse(randomDir * randomStrength);

        // 随机旋转扭矩（根据剩余速度调整）
        ApplyTorqueImpulse((float)GD.RandRange(-0.3f, 0.3f) * (1 - speedRatio));
    }
}