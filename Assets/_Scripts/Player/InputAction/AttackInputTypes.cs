using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//半山腰太挤，你总得去山顶看看//
public class AttackInputTypes
{
    public bool isAttack { get; set; }

    public bool isInAir { get; set; }

    private bool _xInput;
    private bool _yInput;
    private bool _aInput;
    private bool _bInput;
    private bool _rbInput;

    // 公开属性，通过 set 访问器控制互斥逻辑
    public bool X_input
    {
        get => _xInput;
        set
        {
            if (value) ResetOtherButtons(nameof(X_input)); // 若设为 true，重置其他按钮
            _xInput = value;
        }
    }

    public bool Y_input
    {
        get => _yInput;
        set
        {
            if (value) ResetOtherButtons(nameof(Y_input));
            _yInput = value;
        }
    }

    public bool A_input
    {
        get => _aInput;
        set
        {
            if (value) ResetOtherButtons(nameof(A_input));
            _aInput = value;
        }
    }

    public bool B_input
    {
        get => _bInput;
        set
        {
            if (value) ResetOtherButtons(nameof(B_input));
            _bInput = value;
        }
    }

    public bool RB_input
    {
        get => _rbInput;
        set
        {
            if (value) ResetOtherButtons(nameof(RB_input));
            _rbInput = value;
        }
    }

    // 重置除当前按钮外的所有其他按钮为 false
    private void ResetOtherButtons(string currentButtonName)
    {
        if (currentButtonName != nameof(X_input)) _xInput = false;
        if (currentButtonName != nameof(Y_input)) _yInput = false;
        if (currentButtonName != nameof(A_input)) _aInput = false;
        if (currentButtonName != nameof(B_input)) _bInput = false;
        if (currentButtonName != nameof(RB_input)) _rbInput = false;
    }
    public void SetAllButtonFalse()
    {
        isAttack = false;
        X_input = false;
        Y_input = false;
        RB_input = false;
    }
}
